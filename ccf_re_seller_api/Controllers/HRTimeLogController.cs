using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ccf_re_seller_api.Modals;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRTimeLogController : Controller
    {

        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRTimeLogController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<HRTimeLogClass>>> GetAll(HRClockInOut filter)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd", CultureInfo.GetCultureInfo("en-GB"));
            var employeees = _context.timeLogClass
                                    .OrderByDescending(x => x.tim)
                                    .AsQueryable();

            if (filter.level != null && filter.level <= 3)
            {
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    employeees = employeees.Where(cu => cu.tdate >= dateFrom && cu.tdate <= dateTo);
                }
                else 
                {
                    employeees = employeees.Where(d => d.tdate == DOI);
                }

                if (filter.branchClock != null && filter.branchClock != "")
                {
                    employeees = employeees.Where(cu => cu.braid == filter.branchClock.ToString());
                }

                if (filter.level != null && filter.level < 5)
                {
                    employeees = employeees.Where(lr => lr.eid == filter.eid);
                }
                else
                {
                    employeees = employeees.Where(lr => lr.tim == filter.eid);
                }
            }
            else
            {
                //employeees = _context.timeLogClass.AsQueryable();
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    employeees = employeees.Where(cu => cu.tdate >= dateFrom && cu.tdate <= dateTo);
                }
                else
                {
                    employeees = employeees.Where(d => d.tdate == DOI);
                }

                if (filter.branchClock != null && filter.branchClock != "")
                {
                    employeees = employeees.Where(cu => cu.braid == filter.branchClock.ToString());
                }

                if (filter.eid != null)
                {
                    employeees = employeees.Where(lr => lr.eid == filter.eid);
                }

            }


            return employeees.ToList();
           
        }

        //Get All List Referer
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<HRTimeLogClass>>> GetTimeClock(HRClockInOut filter)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd", CultureInfo.GetCultureInfo("en-GB"));
            var listEmployeeClock = _context.timeLogClass
                                    .Include(e => e.ccfpinfo)
                                    .AsQueryable();          

            int totallistEmployee = listEmployeeClock.Count();
            var listEmployess = listEmployeeClock
                                               .OrderByDescending(lr => lr.tdate)
                                               
                                               .AsQueryable()
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                               .Take(filter.pageSize)
                                               .ToList();

            return listEmployess;

        }

        [HttpPost("posttimelog")]
        public async Task<IActionResult> PostTimeClock(HRTimeLogClass timeLog)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd", CultureInfo.GetCultureInfo("en-GB"));
            var datetimeLog = DateTime.Now.ToString("HH:mm:ss");

            try
            {
                bool exsitingEmployee = false;
                // timelog eid == ecard
                exsitingEmployee = _context.employee.Any(e => e.ecard == timeLog.eid);
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
                    if (timeLog.braid != null &&
                        timeLog.eid != null && timeLog.cty != null)
                    {
                        var employee = _context.employee.FirstOrDefault(e => e.ecard == timeLog.eid);
                        timeLog.timid = GetLogNextID();
                        timeLog.eid = employee.eid;
                        timeLog.tdate = DOI;
                        timeLog.tim = datetimeLog.ToString();

                        _context.timeLogClass.Add(timeLog);
                        await _context.SaveChangesAsync();

                        return Ok(timeLog);
                    }
                    else
                    {
                        return BadRequest("Request Param.");
                    }
                }
                else
                {
                    return BadRequest("Employee don't have in our company.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        //
        public string GetLogNextID()
        {
            var userLog = _context.timeLogClass.OrderByDescending(u => u.timid).FirstOrDefault();

            if (userLog == null)
            {
                return "100000";
            }
            var nextId = int.Parse(userLog.timid) + 1;
            return nextId.ToString();
        }
        //
    }
}
