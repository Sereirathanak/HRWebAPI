using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRLeaveEnrollmentController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRLeaveEnrollmentController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRleaveEnrollment>>> Get()
        {
            return await _context.leaveEnrollment.ToListAsync();
        }

        //run srip
        [HttpPut("runleaveenrollment")]
        public async Task<IActionResult> RunIncreaseLeave()
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            
            var checkEmployeeMoreThen = _context.employeeJoinInfo.AsNoTracking().AsQueryable()
                .Where(e => e.status == "New");

            if (checkEmployeeMoreThen.Count() >= 0)
            {
                var forWorkOver30 = checkEmployeeMoreThen.Where(e => (DOI - e.edate).Days >= 30).ToList();

                var forWorkLessThen20 = checkEmployeeMoreThen.Where(e => (DOI - e.edate).Days <= 20).ToList();


                HRleaveEnrollment leaveEnrollment = new HRleaveEnrollment();

                if (forWorkOver30.Count() >= 0)
                {
                    for (int i = 0; i < forWorkOver30.Count(); i++)
                    {

                        var employeeMoreThen30 = _context.leaveEnrollment.AsNoTracking().AsQueryable()
                            .Where(e => e.eid == forWorkOver30[i].eid).ToList();

                        for (var index = 0; index < employeeMoreThen30.Count(); index++)
                        {


                            leaveEnrollment.lerid = employeeMoreThen30[index].lerid;
                            leaveEnrollment.orgid = employeeMoreThen30[index].orgid;
                            leaveEnrollment.eid = employeeMoreThen30[index].eid;
                            leaveEnrollment.accruyear = employeeMoreThen30[index].accruyear;
                            leaveEnrollment.accrunum = employeeMoreThen30[index].accrunum + 1.5;
                            leaveEnrollment.releav = employeeMoreThen30[index].releav;
                            leaveEnrollment.usleav = employeeMoreThen30[index].usleav;
                            leaveEnrollment.acruleav = employeeMoreThen30[index].acruleav;
                            leaveEnrollment.remark = employeeMoreThen30[index].remark;

                            _context.Entry(leaveEnrollment).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }

                    }
                }
                //

                if (forWorkLessThen20.Count() >= 0)
                {
                    for (int i = 0; i < forWorkLessThen20.Count(); i++)
                    {
                        var employeeLessThen20 = _context.leaveEnrollment.AsNoTracking().AsQueryable()
                          .Where(e => e.eid == forWorkLessThen20[i].eid).ToList();

                        for (var index = 0; index < employeeLessThen20.Count(); index++)
                        {
                            leaveEnrollment.lerid = employeeLessThen20[index].lerid;
                            leaveEnrollment.orgid = employeeLessThen20[index].orgid;
                            leaveEnrollment.eid = employeeLessThen20[index].eid;
                            leaveEnrollment.accruyear = employeeLessThen20[index].accruyear;
                            leaveEnrollment.accrunum = employeeLessThen20[index].accrunum + 1;
                            leaveEnrollment.releav = employeeLessThen20[index].releav;
                            leaveEnrollment.usleav = employeeLessThen20[index].usleav;
                            leaveEnrollment.acruleav = employeeLessThen20[index].acruleav;
                            leaveEnrollment.remark = employeeLessThen20[index].remark;

                            _context.Entry(leaveEnrollment).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            var employeeEntrollment = await _context.leaveEnrollment.ToListAsync();
            return Ok(employeeEntrollment);

        }
        //
        [HttpPost("createleaveenrollment")]
        public async Task<IActionResult> CreatePostion(HRleaveEnrollment _leaveenrollment)
        {
            try
            {
                if (
                    _leaveenrollment.orgid != null &&
                    _leaveenrollment.eid != null 
                   )

                {
                    _leaveenrollment.lerid = GetLogNextID();

                    var checkEmployee = _context.employeeJoinInfo.SingleOrDefault(e => e.eid == _leaveenrollment.eid);
                    var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
                    //
                    var day =   (DOI - checkEmployee.edate ).Days;

                    var statusEmployee = checkEmployee.status;
                    if(statusEmployee == "New")
                    {
                        if(day >= 30)
                        {
                            _leaveenrollment.accrunum = 1.5;
                        }

                        if (day >= 20)
                        {
                            _leaveenrollment.accrunum = 1;
                        }

                        if (day >= 10)
                        {
                            _leaveenrollment.accrunum = 0.5;
                        }

                        if (day < 10)
                        {
                            _leaveenrollment.accrunum = 0;
                        }
                    }
                    else
                    {
                        if(_leaveenrollment.accrunum == null)
                        {
                            _leaveenrollment.accrunum = 18;
                        }
                    }


                    _context.leaveEnrollment.Add(_leaveenrollment);
                    await _context.SaveChangesAsync();

                    return Ok(_leaveenrollment);
                }
                else
                {
                    return BadRequest("Request Param.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        //
        [HttpPut("editleaveenrollment/{id}")]
        public async Task<IActionResult> Edit(string id, HRleaveEnrollment _leaveenrollment)
        {

            if (id != _leaveenrollment.lerid)
            {
                return BadRequest();
            }

            _context.Entry(_leaveenrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcflogReExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_leaveenrollment);
        }
        //
        private bool CcflogReExists(string id)
        {
            return _context.leaveEnrollment.Any(e => e.lerid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.leaveEnrollment.OrderByDescending(u => u.lerid).FirstOrDefault();

            if (userLog == null)
            {
                return "900000";
            }
            var nextId = int.Parse(userLog.lerid) + 1;
            return nextId.ToString();
        }
        //
    }
}
