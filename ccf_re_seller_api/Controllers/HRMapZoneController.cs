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
    public class HRMapZoneController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRMapZoneController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HRMapZoneClass>>> Get(string id)
        {
            var listZone = _context.mapZoneClass.AsQueryable()
                  .Include(e => e.ccfbranch);
            ;
            var zone = listZone.Where(z => z.braid == id)
                                           .AsQueryable()
                                           .ToList();
            return Ok(zone);
        }

        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRMapZoneClass>>> GetCcfreferalCus()
        {
            return await _context.mapZoneClass.ToListAsync();
        }
        //
        [HttpPost]
        public async Task<IActionResult> CreatePostion(HRMapZoneClass _mapZoneClass)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_mapZoneClass.braid != null &&
                    _mapZoneClass.latitude !=null &&
                    _mapZoneClass.longitude !=null)

                {
                    _mapZoneClass.zoneid = GetLogNextID();
                    _mapZoneClass.datecreate = DOI;

                    _context.mapZoneClass.Add(_mapZoneClass);
                    await _context.SaveChangesAsync();

                    return Ok(_mapZoneClass);
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
        [HttpPut("editmapzone/{id}")]
        public async Task<IActionResult> Edit(string id, HRMapZoneClass _mapZoneClass)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (id != _mapZoneClass.zoneid)
            {
                return BadRequest();
            }

            _context.Entry(_mapZoneClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfMapZoneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_mapZoneClass);
        }
        //
        private bool CcfMapZoneExists(string id)
        {
            return _context.mapZoneClass.Any(e => e.zoneid == id);
        }
        //
        public string GetLogNextID()
        {
            var mapzone = _context.mapZoneClass.OrderByDescending(u => u.zoneid).FirstOrDefault();

            if (mapzone == null)
            {
                return "700000";
            }
            var nextId = int.Parse(mapzone.zoneid) + 1;
            return nextId.ToString();
        }
    }
}
