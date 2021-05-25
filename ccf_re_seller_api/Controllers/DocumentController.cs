using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : Controller
    {
        private readonly ReSellerAPIContext _context;
        private readonly UserRepository _userRepository;

        public DocumentController(ReSellerAPIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _userRepository = new UserRepository(_context, env);
        }

        [HttpGet("ByLoan/{uid}")]
        public async Task<ActionResult<IEnumerable<ReturnDocumentByLoan>>> GetByLoan(string uid)
        {
            var loanDocuments = await _context.Document
                                            .Where(ld => ld.ucode == uid)
                                            .OrderBy(ld => ld.type)
                                            .AsQueryable()
                                            .ToListAsync();


            if (loanDocuments == null)
            {
                return NotFound();
            }

            var results = loanDocuments.Select(r => new ReturnDocumentByLoan()
            {
                dcode = r.dcode,
                type = r.type,
                uid = r.ucode,
                description = r.description,
                filepath = r.base64FilePath
            })
            .ToList();

            return results;
            
        }


        // POST api/<LoanDocumentsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ValidateUserDocument loanDocument)
        {

            try
            {
                var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

                if (HttpContext.Request.Form.Files.Count() > 0)
                {

                    var loan = _context.CcfuserRes.SingleOrDefault(l => l.uid == loanDocument.ucode.ToString());
                    if (loan == null)
                    {
                        IDictionary<string, string> errNotFound = new Dictionary<string, string>();
                        errNotFound.Add(new KeyValuePair<string, string>("000", $"The loan code {loanDocument.ucode} is not found."));
                        return BadRequest(errNotFound);
                    }

                    string occupation = "";
                    double lamt = 0;

                    //if (loan.curcode == "100")
                    //{
                    //    lamt = (loan.lamt / 4000);
                    //}

                    string allowExtensions = ".jpg|.jpeg|.png|.gif";
                    string fileEx = "";
                    string mineType = "";
                    string fileName = "";
                    string errKycNidfront = "";
                    string errKycNidfrontBank = "";
                    string errKycNidfrontselfie = "";
                    


                    var id = _context.Document.Max(c => c.dcode);
                    int convertInt = 0;
                    if (id == null)
                    {
                        convertInt = 40000;
                    }
                    else
                    {
                        convertInt = int.Parse(id) + 1;

                    }

                    var GenerateID =  convertInt.ToString();

                    if (HttpContext.Request.Form.Files["kyc[101]"] != null)
                    {

                        fileName = HttpContext.Request.Form.Files["kyc[101]"].FileName;
                        mineType = HttpContext.Request.Form.Files["kyc[101]"].ContentType;
                        fileEx = Path.GetExtension(fileName);

                        using (var memoryStream = new MemoryStream())
                        {

                            await HttpContext.Request.Form.Files["kyc[101]"].CopyToAsync(memoryStream);

                            // Validate File Size 10M
                            if (memoryStream.Length > 10485760)
                            {
                                errKycNidfront = "The document size cannot bigger than 10M.";
                            }

                            if (!allowExtensions.Contains(fileEx))
                            {
                                errKycNidfront = "The document type is not allow.";
                            }

                            if (errKycNidfront == "")
                            {

                                var oldDocument101 = _context.Document.SingleOrDefault(ld => ld.ucode == loanDocument.ucode && ld.type == "101");

                                if (oldDocument101 == null)
                                {
                                    var document101 = new Document()
                                    {
                                        dcode = GenerateID.ToString(),
                                        description = "National ID Front",
                                        datecreate = DOI,
                                        minetype = mineType,
                                        type = "101",
                                        filepath = memoryStream.ToArray(),
                                        ucode = loanDocument.ucode
                                    };


                                    _context.Document.Add(document101);
                                }
                                else
                                {
                                    oldDocument101.description = "National ID Front";
                                    oldDocument101.filepath = memoryStream.ToArray();
                                }

                            }

                            memoryStream.Close();
                            memoryStream.Dispose();
                        }

                    }
                    else
                    {
                        errKycNidfront = "The NID is required.";
                    }

                    if (HttpContext.Request.Form.Files["kyc[102]"] != null)
                    {
                        fileName = HttpContext.Request.Form.Files["kyc[102]"].FileName;
                        mineType = HttpContext.Request.Form.Files["kyc[102]"].ContentType;
                        fileEx = Path.GetExtension(fileName);

                        using (var memoryStream = new MemoryStream())
                        {
                            await HttpContext.Request.Form.Files["kyc[102]"].CopyToAsync(memoryStream);

                            // Validate File Size 10M
                            if (memoryStream.Length > 10485760)
                            {
                                errKycNidfrontBank = "The document size cannot bigger than 10M.";
                            }

                            if (!allowExtensions.Contains(fileEx))
                            {
                                errKycNidfrontBank = "The document type is not allow.";
                            }

                            if (errKycNidfrontBank == "")
                            {
                                var NextID = int.Parse(GenerateID) + 1;
                                GenerateID = NextID.ToString();
                                var oldDocument = _context.Document.SingleOrDefault(ld => ld.ucode == loanDocument.ucode && ld.type == "102");
                                if (oldDocument == null)
                                {
                                    var document = new Document()
                                    {
                                        dcode = NextID.ToString(),
                                        minetype = mineType,
                                        type = "102",
                                        filepath = memoryStream.ToArray(),
                                        description = "National ID Back",
                                        ucode = loanDocument.ucode
                                    };
                                    _context.Document.Add(document);
                                }
                                else
                                {
                                    oldDocument.description = "National ID Back";
                                    oldDocument.filepath = memoryStream.ToArray();
                                }
                            }

                            memoryStream.Close();
                            memoryStream.Dispose();
                        }
                    }
                    else
                    {
                        errKycNidfrontBank = "The Family Book is required.";
                    }

                    //Selfie with National ID
                    if (HttpContext.Request.Form.Files["kyc[103]"] != null)
                    {
                        fileName = HttpContext.Request.Form.Files["kyc[103]"].FileName;
                        mineType = HttpContext.Request.Form.Files["kyc[103]"].ContentType;
                        fileEx = Path.GetExtension(fileName);

                        using (var memoryStream = new MemoryStream())
                        {
                            await HttpContext.Request.Form.Files["kyc[103]"].CopyToAsync(memoryStream);

                            // Validate File Size 10M
                            if (memoryStream.Length > 10485760)
                            {
                                errKycNidfrontselfie = "The document size cannot bigger than 10M.";
                            }

                            if (!allowExtensions.Contains(fileEx))
                            {
                                errKycNidfrontselfie = "The document type is not allow.";
                            }

                            if (errKycNidfrontselfie == "")
                            {
                                var NextID = int.Parse(GenerateID) + 1;
                                GenerateID = NextID.ToString();
                                var oldDocument = _context.Document.SingleOrDefault(ld => ld.ucode == loanDocument.ucode && ld.type == "103");
                                if (oldDocument == null)
                                {
                                    var document = new Document()
                                    {
                                        dcode = NextID.ToString(),
                                        minetype = mineType,
                                        type = "103",
                                        filepath = memoryStream.ToArray(),
                                        description = "Resident Book",
                                        ucode = loanDocument.ucode
                                    };
                                    _context.Document.Add(document);
                                }
                                else
                                {
                                    oldDocument.description = "Resident Book";
                                    oldDocument.filepath = memoryStream.ToArray();
                                }
                            }

                            memoryStream.Close();
                            memoryStream.Dispose();
                        }
                    }



                    // Final Validation
                    IDictionary<string, string> errList = new Dictionary<string, string>();

                    if (errKycNidfront != "")
                        errList.Add(new KeyValuePair<string, string>("101", errKycNidfront));

                    if (errKycNidfrontBank != "")
                        errList.Add(new KeyValuePair<string, string>("102", errKycNidfrontBank));

                    if (errKycNidfrontselfie != "")
                        errList.Add(new KeyValuePair<string, string>("103", errKycNidfrontselfie));


                    if (errList.Count() > 0)
                    {
                        return BadRequest(errList.ToArray());
                    }
                    else
                    {
                        if(loanDocument.typeaccountbank == null || loanDocument.typeaccountbank == "")
                        {
                            return BadRequest(new KeyValuePair<string, string>("999", "Type Account Bank is required."));
                        }

                        if (loanDocument.typeaccountnumber == null || loanDocument.typeaccountnumber == "")
                        {
                            return BadRequest(new KeyValuePair<string, string>("999", "Account Bank is required."));
                        }

                        if (loanDocument.idtype == null || loanDocument.idtype == "")
                        {
                            return BadRequest(new KeyValuePair<string, string>("999", "ID type is required."));
                        }

                        if (loanDocument.idnumber == null || loanDocument.idnumber == "")
                        {
                            return BadRequest(new KeyValuePair<string, string>("999", "ID number is required."));
                        }

                        if (loanDocument.dob == null || loanDocument.dob == "")
                        {
                            return BadRequest(new KeyValuePair<string, string>("999", "Date of birth number is required."));
                        }

                        if (loanDocument.phone == 0.ToString() || loanDocument.phone == "" || loanDocument.phone == null)
                        {
                            return BadRequest(new KeyValuePair<string, string>("999", "Phone number is required."));
                        }


                        if (loanDocument.username == "" || loanDocument.username == null)
                        {
                            return BadRequest(new KeyValuePair<string, string>("999", "Full name is required."));
                        }

                        if (loanDocument.gender == "" || loanDocument.gender == null)
                        {
                            return BadRequest(new KeyValuePair<string, string>("999", "Gender is required."));
                        }

                        var status = "";
                        if (loanDocument.status != "" || loanDocument.status != null) {
                            status = loanDocument.status;
                        }
                        else
                        {
                            status = "R";
                        }

                         // Save/Update Referrer 
                         var userReferrer = _context.CcfreferalRes.SingleOrDefault(lr => lr.uid == loanDocument.ucode);
                        if (userReferrer != null)
                        {
                            userReferrer.regdate = DOI;
                            userReferrer.verifystatus = "R";
                            userReferrer.typeaccountbank = loanDocument.typeaccountbank;
                            userReferrer.typeaccountnumber = loanDocument.typeaccountnumber;
                            userReferrer.idtype = loanDocument.idtype;
                            userReferrer.idnumber = loanDocument.idnumber;
                            userReferrer.dob = loanDocument.dob;
                            userReferrer.refphone = loanDocument.phone;
                            userReferrer.refname = loanDocument.username;
                            userReferrer.gender = loanDocument.gender;

                        }
                        else
                        {
                           
                            return BadRequest(new KeyValuePair<string, string>("999", "A Referrer don't have in list."));

                        }

                        // Save/Update User Table 
                        var refererName = _context.CcfuserRes.SingleOrDefault(rn => rn.uid == loanDocument.ucode);

                        if (refererName != null)
                        {
                            refererName.verifystatus = status;
                            refererName.banktype = loanDocument.typeaccountbank;
                            refererName.banknumber = loanDocument.typeaccountnumber;
                            refererName.idtype = loanDocument.idtype;
                            refererName.idnumber = loanDocument.idnumber;
                            refererName.dob = loanDocument.dob;
                            refererName.phone = loanDocument.phone;
                            refererName.uname = loanDocument.username;
                            refererName.gender = loanDocument.gender;

                        }
                        else
                        {

                            return BadRequest(new KeyValuePair<string, string>("999", "A Referrer don't have in list."));

                        }

                        // Save Commit All Data Into Database
                        await _context.SaveChangesAsync();
                        await _userRepository.SendNotificationCreateReferer("CCF ReSeller App", $"An Referrer {refererName.uname} have been referer by {refererName.uname}", refererName.uid, GenerateID, refererName.uname, "", refererName.datecreate, refererName.phone);

                        return Ok(new KeyValuePair<string, string>("200", $"An Referrer {loanDocument.ucode} was successfully submitted."));
                    }
                    //

                }
                IDictionary<string, string> errDocument = new Dictionary<string, string>();
                errDocument.Add(new KeyValuePair<string, string>("000", $"The document is required."));
                return BadRequest(errDocument);
            }
            catch (Exception ex)
            {
                return BadRequest(new KeyValuePair<string, string>("999", ex.Message.ToString()));
            }
        }
            //
    }
}
