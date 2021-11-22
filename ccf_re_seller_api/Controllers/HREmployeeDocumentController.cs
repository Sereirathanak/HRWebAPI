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

        [HttpPost("hr/createEmployeeDocument")]
        public async Task<IActionResult> CreateUser(HREmployeeDocument _employeeDocument)

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
                        && employee.eid !=null
                        )
                    {
                        //Employee Document
                        _employeeDocument.docid = await GetNextIDEmployeeDocment();
                        _employeeDocument.eid = employee.eid;
                        _employeeDocument.doctype = _employeeDocument.doctype;
                        _employeeDocument.docnum = _employeeDocument.docnum;
                        _employeeDocument.edate = _employeeDocument.edate;
                        _employeeDocument.docatt = _employeeDocument.docatt;
                        _employeeDocument.rmark = _employeeDocument.rmark;
                        _context.employeeDocument.Add(_employeeDocument);

                        await _context.SaveChangesAsync();

                        return Ok(employee);
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
