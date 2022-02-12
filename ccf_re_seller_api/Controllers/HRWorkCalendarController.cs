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
using System.Data;
using ccf_re_seller_api.Modals;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRWorkCalendarController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRWorkCalendarController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRWorkCalendar>>> GetCcfreferalCus()
        {
            return await _context.workCalendar.ToListAsync();
        }
        //
        [HttpPost("createworkcalendar")]
        public async Task<IActionResult> Create(HRWorkCalendar _workCalendar)
        {
            try
            {
                if (_workCalendar.stim != null &&
                    _workCalendar.etim != null &&
                    _workCalendar.braid != null &&
                    _workCalendar.orgid != null 
                    )
                {
                    var exitingWordCalendar = _context.workCalendar.FirstOrDefault(e => e.braid == _workCalendar.braid);
                    //if(_workCalendar.braid )
                    if (exitingWordCalendar == null) {
                        _workCalendar.wkcid = GetLogNextID();

                        _context.workCalendar.Add(_workCalendar);
                        await _context.SaveChangesAsync();

                        return Ok(_workCalendar);
                    }
                    else
                    {
                        return BadRequest("Branch already create working of the day.");

                    }

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
        [HttpPut("editworkcalendar/{id}")]
        public async Task<IActionResult> Edit(string id, HRWorkCalendar _workCalendar)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (id != _workCalendar.wkcid)
            {
                return BadRequest();
            }

            _context.Entry(_workCalendar).State = EntityState.Modified;

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

            return Ok(_workCalendar);
        }
        //
        private bool CcflogReExists(string id)
        {
            return _context.workCalendar.Any(e => e.wkcid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.workCalendar.OrderByDescending(u => u.wkcid).FirstOrDefault();

            if (userLog == null)
            {
                return "400000";
            }
            var nextId = int.Parse(userLog.wkcid) + 1;
            return nextId.ToString();
        }
        //
    }
}
