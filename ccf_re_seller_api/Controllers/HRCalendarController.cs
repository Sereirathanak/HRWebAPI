
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ccf_re_seller_api.Models;
using ccf_re_seller_api.Modals;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRCalendarController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRCalendarController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRCalendar>>> GetAll()
        {
            return await _context.calendar.ToListAsync();
        }
        //
        [HttpPost("createcalendar")]
        public async Task<IActionResult> CreatePostion(HRCalendar _calendar)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                var exitingOrganization = _context.calendar.FirstOrDefault(e => e.orgid == _calendar.orgid);
                if(exitingOrganization == null)
                {
                    if (_calendar.mon != null &&
                        _calendar.tue != null &&
                        _calendar.wen != null &&
                        _calendar.thu != null &&
                        _calendar.fri != null &&
                        _calendar.sat != null &&
                        _calendar.sun != null)

                    {
                        _calendar.calid = GetLogNextID();

                        _context.calendar.Add(_calendar);
                        await _context.SaveChangesAsync();

                        return Ok(_calendar);
                    }
                    else
                    {
                        return BadRequest("Request Param.");
                    }
                }
                else
                {
                    return BadRequest("The Organization already create calendar.");

                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        //
        //
        [HttpPut("editcalendar/{id}")]
        public async Task<IActionResult> Edit(string id, HRCalendar _calendar)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (id != _calendar.calid)
            {
                return BadRequest();
            }

            _context.Entry(_calendar).State = EntityState.Modified;

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

            return Ok(_calendar);
        }
        //
        private bool CcflogReExists(string id)
        {
            return _context.calendar.Any(e => e.calid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.calendar.OrderByDescending(u => u.calid).FirstOrDefault();

            if (userLog == null)
            {
                return "600000";
            }
            var nextId = int.Parse(userLog.calid) + 1;
            return nextId.ToString();
        }
        //
    }
}
