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
    public class HRPostionController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRPostionController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRPosition>>> GetCcfreferalCus()
        {
            return await _context.position.ToListAsync();
        }
        //

        [HttpPost("createpostion")]
        public async Task<IActionResult> CreatePostion(HRPosition position)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (position.pos != null)
                {
                    position.posid = GetLogNextID();

                    _context.position.Add(position);
                    await _context.SaveChangesAsync();

                    return Ok(position);
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
        [HttpPut("editposition/{id}")]
        public async Task<IActionResult> Edit(string id, HRPosition position)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (id != position.posid)
            {
                return BadRequest();
            }

            _context.Entry(position).State = EntityState.Modified;

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

            return Ok(position);
        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.position.Any(e => e.posid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.position.OrderByDescending(u => u.posid).FirstOrDefault();

            if (userLog == null)
            {
                return "200000";
            }
            var nextId = int.Parse(userLog.posid) + 1;
            return nextId.ToString();
        }
        //
    }
}
