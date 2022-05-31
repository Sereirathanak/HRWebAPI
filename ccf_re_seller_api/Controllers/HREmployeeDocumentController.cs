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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HREmployeeDocumentController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HREmployeeDocumentController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HREmployeeDocument>>> GetAll()
        {
            return await _context.employeeDocument.ToListAsync();
        }

        //
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HREmployeeDocument>>> GetByDocument(string id)
        {
            var detail = await _context.employeeDocument.Where(u => u.eid == id)
              .AsQueryable()
              .ToListAsync();
            if (detail == null)
            {
                return NotFound();
            }
            var results = _context.employeeDocument.Where(u => u.eid == id);
            return results.ToList();
        }
        //
        [HttpPost("hr/createEmployeeDocument")]
        public async Task<ActionResult> Post([FromForm]  HRValidateEmployeeDocument _employeeDocument)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employee.Any(e => e.eid == _employeeDocument.eid);
                if (_context.employee.Any(e => e.ecard == null))
                {
                    exsitingEmployee = false;
                }
                else if (exsitingEmployee == true)
                {
                    exsitingEmployee = true;
                }

                if (exsitingEmployee == true)
                {
                    var employee = _context.employee.SingleOrDefault(e => e.eid == _employeeDocument.eid);

                    if (_employeeDocument.doctype != null
                        && _employeeDocument.docnum != null
                        && _employeeDocument.edate != null
                        && _employeeDocument.eid != null
                        )
                    {
                        if (HttpContext.Request.Form.Files.Count() > 0)
                        {


                            var employeeDocument = _context.employee.SingleOrDefault(l => l.eid == _employeeDocument.eid);
                            if (employeeDocument == null)
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

                            var id = _context.employeeDocument.Max(c => c.docid);
                            int convertInt = 0;
                            if (id == null)
                            {
                                convertInt = 40000;
                            }
                            else
                            {
                                convertInt = int.Parse(id) + 1;

                            }
                            var GenerateID = convertInt.ToString();

                            if (HttpContext.Request.Form.Files["edu[101]"] != null)
                            {

                                fileName = HttpContext.Request.Form.Files["edu[101]"].FileName;
                                mineType = HttpContext.Request.Form.Files["edu[101]"].ContentType;
                                fileEx = Path.GetExtension(fileName);
                                //
                                using (var memoryStream = new MemoryStream())
                                {

                                    await HttpContext.Request.Form.Files["edu[101]"].CopyToAsync(memoryStream);

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

                                        //var oldDocument101 = _context.employeeDocument.FirstOrDefault(ld => ld.eid == _employeeDocument.eid);

                                        //if (id == null)
                                        //{
                                            var document101 = new HREmployeeDocument()
                                            {
                                                docid = GenerateID.ToString(),
                                                docnum = _employeeDocument.docnum,
                                                edate = _employeeDocument.edate,
                                                doctype = _employeeDocument.doctype,
                                                docatt = memoryStream.ToArray(),
                                                eid = _employeeDocument.eid
                                            };
                                            _context.employeeDocument.Add(document101);
                                        //}
                                        //else
                                        //{
                                        //    oldDocument101.docnum = _employeeDocument.docnum;
                                        //    oldDocument101.docatt = memoryStream.ToArray();
                                        //    oldDocument101.edate = _employeeDocument.edate;
                                        //}

                                    }

                                    memoryStream.Close();
                                    memoryStream.Dispose();
                                }
                            }
                            else
                            {
                                errEduId = "The Family Document is required.";
                            }
                            

                            //
                            await _context.SaveChangesAsync();

                            return Ok();
                        }
                        IDictionary<string, string> errDocument = new Dictionary<string, string>();
                        errDocument.Add(new KeyValuePair<string, string>("000", $"The document is required."));
                        return BadRequest(errDocument);

                    }
                    else
                    {

                        return BadRequest("The credential is invalid.");

                    }
                }
                else
                {
                    return BadRequest("The credential is invalid.");

                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }
        //

        //Edit Employee
        [HttpPut("hr/editEmployeeDocument/{eid}")]
        public async Task<IActionResult> EditEmployeeDocument(string eid, HREmployeeDocument employeeDocument)

        {
            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employeeDocument.Any(e => e.eid == eid);


                if (_context.employee.Any(e => e.ecard == null))
                {
                    exsitingEmployee = false;
                }
                else if (exsitingEmployee == true)
                {
                    exsitingEmployee = true;
                }

                if (exsitingEmployee == true)
                {
                    if (employeeDocument.doctype != null
                      && employeeDocument.docnum != null
                      && employeeDocument.edate != null
                      && employeeDocument.eid != null
                      )
                    {
                        var users = _context.employeeDocument.SingleOrDefault(e => e.docid == employeeDocument.docid);

                        users.docid = employeeDocument.docid;
                        users.eid = employeeDocument.eid;
                        users.doctype = employeeDocument.doctype;
                        users.docnum = employeeDocument.docnum;
                        users.edate = employeeDocument.edate;
                        users.docatt = employeeDocument.docatt;
                        users.rmark = employeeDocument.rmark;

                        _context.Entry(employeeDocument).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        return Ok(employeeDocument);
                    }
                    else
                    {
                        return BadRequest("The credential is invalid.");
                    }

                }
                else
                {
                    return BadRequest("The credential is invalid.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }


        //Get Next ID Employee Docment ID
        public async Task<string> GetNextIDEmployeeDocment()
        {
            var id = await _context.employeeDocument.OrderByDescending(u => u.docid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "400000";
            }
            var nextId = int.Parse(id.docid) + 1;
            return nextId.ToString();
        }
    }
}
