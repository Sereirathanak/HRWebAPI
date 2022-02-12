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
    public class HRMissionTypeController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRMissionTypeController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRMissionType>>> Get()
        {
            return await _context.mssionType.ToListAsync();
        }
        //
        [HttpPost("createmissiontype")]
        public async Task<IActionResult> Create(HRMissionType _missionType)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_missionType.mistp != null
                    )
                {
                    _missionType.misid = GetLogNextID();

                    _context.mssionType.Add(_missionType);
                    await _context.SaveChangesAsync();

                    return Ok(_missionType);
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
        [HttpPut("editmissiontype/{id}")]
        public async Task<IActionResult> Edit(string id, HRMissionType _missionType)
        {
            
            if (id != _missionType.misid)
            {
                return BadRequest();
            }

            _context.Entry(_missionType).State = EntityState.Modified;

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

            return Ok(_missionType);
        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.mssionType.Any(e => e.misid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.mssionType.OrderByDescending(u => u.misid).FirstOrDefault();

            if (userLog == null)
            {
                return "200000";
            }
            var nextId = int.Parse(userLog.misid) + 1;
            return nextId.ToString();
        }
        //
    }
}
