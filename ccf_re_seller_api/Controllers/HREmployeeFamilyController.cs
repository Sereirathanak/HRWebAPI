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
    public class HREmployeeFamilyController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HREmployeeFamilyController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HREmployeeFamily>>> GetCcfreferalCus()
        {
            return await _context.employeeFamily.ToListAsync();
        }
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HREmployeeFamily>>> GetByDocumentFamily(string id)
        {
            var detail = await _context.employeeFamily.Where(u => u.eid == id)
              .AsQueryable()
              .ToListAsync();
            if (detail == null)
            {
                return NotFound();
            }
            var results = _context.employeeFamily.Where(u => u.eid == id);
            return results.ToList();
        }
        //
        [HttpPost("hr/createEmployeeFamily")]
        public async Task<ActionResult> Post([FromForm] HRValidateEmployeeFamily _employeeFamily)

        {

            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employee.Any(e => e.eid == _employeeFamily.eid);
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
                    var employee = _context.employee.FirstOrDefault(e => e.eid == _employeeFamily.eid);

                    if (_employeeFamily.fname != null
                         && _employeeFamily.rtype != null
                         && _employeeFamily.famstatus != null
                         && _employeeFamily.eid !=null)
                    {


                        ////Employee Family

                        //_employeeFamily.famid = await GetNextIDEmployeeFamily();
                        //_employeeFamily.eid = employee.eid;
                        //_employeeFamily.fname = _employeeFamily.fname;
                        //_employeeFamily.rtype = _employeeFamily.rtype;
                        //_employeeFamily.famstatus = _employeeFamily.famstatus;
                        //_employeeFamily.photo = _employeeFamily.photo;
                        //_employeeFamily.rmark = _employeeFamily.rmark;
                        //_context.employeeFamily.Add(_employeeFamily);

                        //await _context.SaveChangesAsync();

                        //return Ok(_employeeFamily);
                        if (HttpContext.Request.Form.Files.Count() > 0)
                        {


                            var employeeDocument = _context.employee.FirstOrDefault(l => l.eid == _employeeFamily.eid);
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

                            var id = _context.employeeFamily.Max(c => c.famid);
                            int convertInt = 0;
                            if (id == null)
                            {
                                convertInt = 50000;
                            }
                            else
                            {
                                convertInt = int.Parse(id) + 1;

                            }
                            var GenerateID = convertInt.ToString();

                            if (HttpContext.Request.Form.Files["efamily[101]"] != null)
                            {

                                fileName = HttpContext.Request.Form.Files["efamily[101]"].FileName;
                                mineType = HttpContext.Request.Form.Files["efamily[101]"].ContentType;
                                fileEx = Path.GetExtension(fileName);
                                //
                                using (var memoryStream = new MemoryStream())
                                {

                                    await HttpContext.Request.Form.Files["efamily[101]"].CopyToAsync(memoryStream);

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

                                        //var oldDocument101 = _context.employeeFamily.SingleOrDefault(ld => ld.eid == _employeeFamily.eid );

                                        //if (oldDocument101 == null)
                                        //{
                                            var document101 = new HREmployeeFamily()
                                            {
                                                famid = GenerateID.ToString(),
                                                eid = _employeeFamily.eid,
                                                fname = _employeeFamily.fname,
                                                rtype = _employeeFamily.rtype,
                                                famstatus = _employeeFamily.famstatus,
                                                photo = memoryStream.ToArray(),
                                                rmark = _employeeFamily.rmark
                                            };


                                            _context.employeeFamily.Add(document101);
                                        //}
                                        //else
                                        //{
                                           
                                        //    oldDocument101.fname = _employeeFamily.fname;
                                        //    oldDocument101.rtype = _employeeFamily.rtype;
                                        //    oldDocument101.famstatus = _employeeFamily.famstatus;
                                        //    oldDocument101.photo = memoryStream.ToArray();
                                        //    oldDocument101.rmark = _employeeFamily.rmark;
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
        //Edit Employee Family
        [HttpPut("hr/editEmployeeFamily/{eid}")]
        public async Task<IActionResult> EditEmployeeDocument(string eid, HREmployeeFamily employeeFamily)

        {
            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employeeFamily.Any(e => e.eid == eid);


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
                    if (employeeFamily.fname != null
                         && employeeFamily.rtype != null
                         && employeeFamily.famstatus != null
                         && eid != null)
                    {
                        _context.Entry(employeeFamily).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        return Ok(employeeFamily);
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

        //Get Next ID Employee Fmaily ID
        public async Task<string> GetNextIDEmployeeFamily()
        {
            var id = await _context.employeeFamily.OrderByDescending(u => u.famid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "500000";
            }
            var nextId = int.Parse(id.famid) + 1;
            return nextId.ToString();
        }
        //

    }
}
