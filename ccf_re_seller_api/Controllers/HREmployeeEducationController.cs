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

        [HttpPost("hr/createEmployeeEducation")]
        public async Task<IActionResult> CreateUserEducation(HREmployeeEducation _employeeEducation)

        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

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
                        && _employeeEducation.sdate != null
                        && _employeeEducation.edate != null
                        && _employeeEducation.certyfi != null
                        && employee.eid != null
                        )
                    {

                        //Employee Education
                        _employeeEducation.eduid = await GetNextIDEmployeeEducation();
                        _employeeEducation.eid = employee.eid;
                        _employeeEducation.inst = _employeeEducation.inst;
                        _employeeEducation.sub = _employeeEducation.sub;
                        _employeeEducation.sdate = _employeeEducation.sdate;
                        _employeeEducation.edate = _employeeEducation.edate;
                        _employeeEducation.certyfi = _employeeEducation.certyfi;
                        _context.employeeEducation.Add(_employeeEducation);

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
                     && employeeEducation.certyfi != null
                     && eid != null
                     )
                    {
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
