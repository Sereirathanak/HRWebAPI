using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using ccf_re_seller_api.Modals;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class ProfileImageController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public ProfileImageController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }


        //
        [HttpGet("{id}")]
        public async Task<ActionResult<HRImageProfile>> GetDetil(string id)
        {
            var listImageProfileDocument = _context.imageProfile.SingleOrDefault(e => e.userid == id);
            return listImageProfileDocument;
        }
        //Upload Image Profile
        //
        [HttpPost()]
        public async Task<ActionResult> Post([FromForm] HRImageProfile _mageProfile)
        {

            try
            {
                var _idEmployeeID = "";
                bool checkExitingImageProfile = false;

                checkExitingImageProfile = _context.imageProfile.Any(e => e.userid == _mageProfile.userid);

                if (_context.imageProfile.Any(e => e.userid == null))
                {
                    checkExitingImageProfile = false;
                }
                else if (checkExitingImageProfile == true)
                {
                    checkExitingImageProfile = true;
                }

                if (checkExitingImageProfile == false) {

                    if (_mageProfile.userid != null)
                    {

                        if (HttpContext.Request.Form.Files.Count() > 0)
                        {


                            var employeeRequest = _context.employee.SingleOrDefault(l => l.eid == _mageProfile.userid);
                            if (employeeRequest == null)
                            {
                                IDictionary<string, string> errNotFound = new Dictionary<string, string>();
                                errNotFound.Add(new KeyValuePair<string, string>("000", $"Employee is not found."));
                                return BadRequest(errNotFound);
                            }

                            string allowExtensions = ".jpg|.jpeg|.png|.gif";
                            string fileEx = "";
                            string mineType = "";
                            string fileName = "";
                            string errEduId = "";
                            string errEduIdBank = "";
                            string errEduIdselfie = "";

                            var id = _context.imageProfile.Max(c => c.pid);
                            int convertInt = 0;
                            if (id == null)
                            {
                                convertInt = 900000;
                            }
                            else
                            {
                                convertInt = int.Parse(id) + 1;

                            }

                            var GenerateID = convertInt.ToString();

                            if (HttpContext.Request.Form.Files["file[101]"] != null)
                            {

                                fileName = HttpContext.Request.Form.Files["file[101]"].FileName;
                                mineType = HttpContext.Request.Form.Files["file[101]"].ContentType;
                                fileEx = Path.GetExtension(fileName);
                                //
                                using (var memoryStream = new MemoryStream())
                                {

                                    await HttpContext.Request.Form.Files["file[101]"].CopyToAsync(memoryStream);

                                    // Validate File Size 10M
                                    if (memoryStream.Length > 10485760)
                                    {
                                        errEduId = "The document size cannot bigger than 10M.";
                                    }

                                    if (!allowExtensions.Contains(fileEx))
                                    {
                                        errEduId = "The document type is not allow.";
                                    }

                                    if (errEduId == "")
                                    {

                                        var idDocument = _context.imageProfile.Max(c => c.pid);
                                        int convertIntDocument = 0;
                                        if (idDocument == null)
                                        {
                                            convertIntDocument = 200000;
                                        }
                                        else
                                        {
                                            convertIntDocument = int.Parse(idDocument) + 1;

                                        }

                                        var GenerateIDDcument = convertIntDocument.ToString();

                                        var profileImage = new HRImageProfile()
                                        {
                                            pid = GenerateIDDcument.ToString(),
                                            userid = _mageProfile.userid,
                                            file = memoryStream.ToArray(),
                                        };

                                        _context.imageProfile.Add(profileImage);
                                        await _context.SaveChangesAsync();
                                        _idEmployeeID = profileImage.pid;

                                    }

                                    memoryStream.Close();
                                    memoryStream.Dispose();
                                }
                            }
                        }
                    }
                    else
                    {
                        return BadRequest("Request Param.");
                    }
                    var requestImage = _context.imageProfile.SingleOrDefault(e => e.pid == _idEmployeeID);
                    return Ok(requestImage);
                }
                else
                {
                    //edit profile
                    if (HttpContext.Request.Form.Files.Count() > 0)
                    {


                        var employeeRequest = _context.employee.SingleOrDefault(l => l.eid == _mageProfile.userid);
                        if (employeeRequest == null)
                        {
                            IDictionary<string, string> errNotFound = new Dictionary<string, string>();
                            errNotFound.Add(new KeyValuePair<string, string>("000", $"Employee is not found."));
                            return BadRequest(errNotFound);
                        }
                        string allowExtensions = ".jpg|.jpeg|.png|.gif";
                        string fileEx = "";
                        string mineType = "";
                        string fileName = "";
                        string errEduId = "";
                        string errEduIdBank = "";
                        string errEduIdselfie = "";

                        if (HttpContext.Request.Form.Files["file[101]"] != null)
                        {

                            fileName = HttpContext.Request.Form.Files["file[101]"].FileName;
                            mineType = HttpContext.Request.Form.Files["file[101]"].ContentType;
                            fileEx = Path.GetExtension(fileName);
                            //
                            using (var memoryStream = new MemoryStream())
                            {

                                await HttpContext.Request.Form.Files["file[101]"].CopyToAsync(memoryStream);

                                // Validate File Size 10M
                                if (memoryStream.Length > 10485760)
                                {
                                    errEduId = "The document size cannot bigger than 10M.";
                                }

                                if (!allowExtensions.Contains(fileEx))
                                {
                                    errEduId = "The document type is not allow.";
                                }

                                if (errEduId == "")
                                {
                                    var exitingImageProfile = _context.imageProfile.SingleOrDefault(e => e.userid == _mageProfile.userid);

                                    var profileImage = new HRImageProfile()
                                    {
                                        pid = exitingImageProfile.pid,
                                        userid = exitingImageProfile.userid,
                                        file = memoryStream.ToArray(),
                                    };

                                    _context.Entry(exitingImageProfile).CurrentValues.SetValues(profileImage);
                                    await _context.SaveChangesAsync();
                                    _idEmployeeID = exitingImageProfile.pid;

                                }

                                memoryStream.Close();
                                memoryStream.Dispose();
                            }
                        }
                    }
                    var requestImage = _context.imageProfile.SingleOrDefault(e => e.pid == _idEmployeeID);
                    return Ok(requestImage);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
