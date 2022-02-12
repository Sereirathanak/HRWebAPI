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
    public class HROvertimeTypeController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HROvertimeTypeController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HROverTimeType>>> Get()
        {
            return await _context.overTimeType.ToListAsync();
        }
        //
        [HttpPost("creatovertimetype")]
        public async Task<IActionResult> Create(HROverTimeType _overTimeType)
        {
            try
            {
                if (_overTimeType.vtyp != null
                    )
                {
                    _overTimeType.ovtyid = GetLogNextID();

                    _context.overTimeType.Add(_overTimeType);
                    await _context.SaveChangesAsync();

                    return Ok(_overTimeType);
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
        [HttpPut("editovertimetype/{id}")]
        public async Task<IActionResult> Edit(string id, HROverTimeType _overTimeType)
        {

            if (id != _overTimeType.ovtyid)
            {
                return BadRequest();
            }

            _context.Entry(_overTimeType).State = EntityState.Modified;

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

            return Ok(_overTimeType);
        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.overTimeType.Any(e => e.ovtyid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.overTimeType.OrderByDescending(u => u.ovtyid).FirstOrDefault();

            if (userLog == null)
            {
                return "900000";
            }
            var nextId = int.Parse(userLog.ovtyid) + 1;
            return nextId.ToString();
        }
        //
    }
}
