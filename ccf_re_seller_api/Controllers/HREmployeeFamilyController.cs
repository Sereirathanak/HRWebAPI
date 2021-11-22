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

        [HttpPost("hr/createEmployeeFamily")]
        public async Task<IActionResult> CreateUser(HREmployeeFamily _employeeFamily)

        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            //Ex-date
            var year = DateTime.Now.ToString("yyyy");
            int plusYear = int.Parse(year) + 1;
            var MM = DateTime.Now.ToString("MM");
            var dd = DateTime.Now.ToString("dd");
            var HH = DateTime.Now.ToString("HH");
            var mm = DateTime.Now.ToString("mm");
            var ss = DateTime.Now.ToString("ss");
            String plusString = $"{plusYear}{"-"}{MM}{"-"}{dd} {HH}{":"}{mm}{":"}{ss}";
            DateTime DOIEx = DateTime.ParseExact((plusString).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

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
                    var employee = _context.employee.SingleOrDefault(e => e.eid == _employeeFamily.eid);

                    if (_employeeFamily.fname != null
                         && _employeeFamily.rtype != null
                         && _employeeFamily.famstatus != null
                         && employee.eid !=null)
                    {


                        //Employee Family

                        _employeeFamily.famid = await GetNextIDEmployeeFamily();
                        _employeeFamily.eid = employee.eid;
                        _employeeFamily.fname = _employeeFamily.fname;
                        _employeeFamily.rtype = _employeeFamily.rtype;
                        _employeeFamily.famstatus = _employeeFamily.famstatus;
                        _employeeFamily.photo = _employeeFamily.photo;
                        _employeeFamily.rmark = _employeeFamily.rmark;
                        _context.employeeFamily.Add(_employeeFamily);

                        await _context.SaveChangesAsync();

                        return Ok(_employeeFamily);
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
