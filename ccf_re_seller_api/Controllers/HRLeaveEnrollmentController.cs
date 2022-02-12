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
        //
        [HttpPost("createleaveenrollment")]
        public async Task<IActionResult> CreatePostion(HRleaveEnrollment _leaveenrollment)
        {
            try
            {
                if (
                    _leaveenrollment.orgid != null &&
                    _leaveenrollment.eid != null &&
                    _leaveenrollment.accruyear != 0 &&
                    _leaveenrollment.accrunum != 0 &&
                    _leaveenrollment.releav != 0 &&
                    _leaveenrollment.usleav != 0 &&
                    _leaveenrollment.acruleav != 0 
                   )

                {
                    _leaveenrollment.lerid = GetLogNextID();
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
