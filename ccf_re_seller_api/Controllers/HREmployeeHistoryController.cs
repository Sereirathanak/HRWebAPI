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
    public class HREmployeeHistoryController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HREmployeeHistoryController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpPost("hr/createEmployeeHistory")]
        public async Task<IActionResult> CreateUserEmployeeHistory(HREmployeeHistory employeeHistory)

        {
            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employee.Any(e => e.eid == employeeHistory.eid);
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
                    var employee = _context.employee.FirstOrDefault(e => e.eid == employeeHistory.eid);

                    if (employeeHistory.com != null
                        && employeeHistory.pos != null
                        && employeeHistory.res != null
                        && employeeHistory.sdate != null
                        && employeeHistory.edate != null
                        && employeeHistory.lres != null
                        && employeeHistory.eid != null
                        )
                    {


                        employeeHistory.ecom = await GetNextIDEmployeeHistoryWork();
                        employeeHistory.eid = employee.eid;
                        employeeHistory.com = employeeHistory.com;
                        employeeHistory.pos = employeeHistory.pos;
                        employeeHistory.res = employeeHistory.res;
                        employeeHistory.sdate = employeeHistory.sdate;
                        employeeHistory.edate = employeeHistory.edate;
                        employeeHistory.lres = employeeHistory.lres;
                        employeeHistory.remark = employeeHistory.remark;

                        _context.employeeHistory.Add(employeeHistory);


                        await _context.SaveChangesAsync();

                        return Ok(employeeHistory);
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
        //Edit Employee History
        [HttpPut("hr/editEmployeeHistory/{eid}")]
        public async Task<IActionResult> EditEmployeeDocument(string eid, HREmployeeHistory employeeHistory)

        {
            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employeeHistory.Any(e => e.eid == eid);


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
                    if (employeeHistory.com != null
                         && employeeHistory.pos != null
                         && employeeHistory.res != null
                         && employeeHistory.lres != null
                         && eid != null
                         )
                    {
                        _context.Entry(employeeHistory).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        return Ok(employeeHistory);
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
        //Get Next ID Employee History Worid ID
        public async Task<string> GetNextIDEmployeeHistoryWork()
        {
            var id = await _context.employeeHistory.OrderByDescending(u => u.ecom).FirstOrDefaultAsync();

            if (id == null)
            {
                return "600000";
            }
            var nextId = int.Parse(id.ecom) + 1;
            return nextId.ToString();
        }
    }
}
