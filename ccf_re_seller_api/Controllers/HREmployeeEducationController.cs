using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HREmployeeEducationController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HREmployeeEducationController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HREmployeeEducation>>> GetCcfreferalCus()
        {
            return await _context.employeeEducation.ToListAsync();
        }
        //

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HREmployeeEducation>>> GetByDocumentEducation(string id)
        {
            var detail = await _context.employeeEducation.Where(u => u.eid == id)
              .AsQueryable()
              .ToListAsync();
            if (detail == null)
            {
                return NotFound();
            }
            var results = _context.employeeEducation.Where(u => u.eid == id);
            return results.ToList();
        }


        [HttpPost("hr/createEmployeeEducation")]
         public async Task<ActionResult> Post([FromForm] HRValidateEmployeeEducation _employeeEducation)
        {
            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employee.Any(e => e.eid == _employeeEducation.eid);

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
                    var employee = _context.employee.SingleOrDefault(e => e.eid == _employeeEducation.eid);



                    if (_employeeEducation.inst != null
                        && _employeeEducation.sub != null
                        && _employeeEducation.eid != null
                        )
                    {
                        var employeeDocument = _context.employee.SingleOrDefault(l => l.eid == _employeeEducation.eid);
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

                        var id = _context.employeeEducation.Max(c => c.eduid);
                        int convertInt = 0;
                        if (id == null)
                        {
                            convertInt = 60000;
                        }
                        else
                        {
                            convertInt = int.Parse(id) + 1;

                        }
                        var GenerateID = convertInt.ToString();


                        if (HttpContext.Request.Form.Files["eduuid[101]"] != null)
                        {

                            fileName = HttpContext.Request.Form.Files["eduuid[101]"].FileName;
                            mineType = HttpContext.Request.Form.Files["eduuid[101]"].ContentType;
                            fileEx = Path.GetExtension(fileName);


                            //
                            using (var memoryStream = new MemoryStream())
                            {

                                await HttpContext.Request.Form.Files["eduuid[101]"].CopyToAsync(memoryStream);

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

                                    var document101 = new HREmployeeEducation()
                                    {
                                        eduid = GenerateID.ToString(),
                                        eid = _employeeEducation.eid,
                                        inst = _employeeEducation.inst,
                                        sub = _employeeEducation.sub,
                                        sdate = _employeeEducation.sdate,
                                        edate = _employeeEducation.edate,
                                        certyfi = memoryStream.ToArray(),
                                        remark = _employeeEducation.remark,

                                    };

                                    _context.employeeEducation.Add(document101);
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

                        return Ok(_employeeEducation);
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
        //Edit Employee Education
        [HttpPut("hr/editEmployeeEducation/{eid}")]
        public async Task<IActionResult> EditEmployeeDocument(string eid, HREmployeeEducation employeeEducation)

        {
            try
            {

                bool exsitingEmployee = false;

                exsitingEmployee = _context.employeeEducation.Any(e => e.eid == eid);


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
                    if (employeeEducation.inst != null
                     && employeeEducation.sub != null
                     && eid != null
                     )
                    {

                        var users = _context.employeeEducation.SingleOrDefault(e => e.eduid == employeeEducation.eduid);

                        users.eduid = employeeEducation.eduid;
                        users.eid = employeeEducation.eid;
                        users.inst = employeeEducation.inst;
                        users.sub = employeeEducation.sub;
                        users.sdate = employeeEducation.sdate;
                        users.edate = employeeEducation.edate;
                        users.remark = employeeEducation.remark;
                        users.certyfi = employeeEducation.certyfi;


                        _context.Entry(users).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        return Ok(employeeEducation);




                        _context.Entry(employeeEducation).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        return Ok(employeeEducation);
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
        //Get Next ID Employee Education ID
        public async Task<string> GetNextIDEmployeeEducation()
        {
            var id = await _context.employeeEducation.OrderByDescending(u => u.eduid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "700000";
            }
            var nextId = int.Parse(id.eduid) + 1;
            return nextId.ToString();
        }
    }
}
