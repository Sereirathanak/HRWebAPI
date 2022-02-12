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
    public class HRLeaveController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRLeaveController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRLeaveType>>> GetCcfreferalCus()
        {
            return await _context.leaveType.ToListAsync();
        }
        //
        [HttpPost("createleavetype")]
        public async Task<IActionResult> CreatePostion(HRLeaveType _leaveType)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_leaveType.ltyp != null )

                {
                    _leaveType.leaid = GetLogNextID();

                    _context.leaveType.Add(_leaveType);
                    await _context.SaveChangesAsync();

                    return Ok(_leaveType);
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
        //
        [HttpPut("editleavetype/{id}")]
        public async Task<IActionResult> Edit(string id, HRLeaveType _leaveType)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (id != _leaveType.leaid)
            {
                return BadRequest();
            }

            _context.Entry(_leaveType).State = EntityState.Modified;

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

            return Ok(_leaveType);
        }
        //
        private bool CcflogReExists(string id)
        {
            return _context.leaveType.Any(e => e.leaid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.leaveType.OrderByDescending(u => u.leaid).FirstOrDefault();

            if (userLog == null)
            {
                return "700000";
            }
            var nextId = int.Parse(userLog.leaid) + 1;
            return nextId.ToString();
        }
        //
    }
}
