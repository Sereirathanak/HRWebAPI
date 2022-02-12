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
    public class HROrganizationController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HROrganizationController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HROrganizationClass>>> GetCcfreferalCus()
        {
            return await _context.organizationClass.ToListAsync();
        }

        //
        [HttpPost("createOrganization")]
        public async Task<IActionResult> CreateOrganization(HROrganizationClass organization)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (organization.orgname != null &&
                    organization.regno != null &&
                    organization.vat != null &&
                    organization.ind != null &&
                    organization.add != null &&
                    organization.cp != null &&
                    organization.dis != null &&
                    organization.com != null &&
                    organization.vil != null &&
                    organization.roa != null &&
                    organization.no != null &&
                    organization.con != null &&
                    organization.ema != null &&
                    organization.web != null 
                    )
                {
                    organization.orgid = GetLogNextID();

                    _context.organizationClass.Add(organization);
                    await _context.SaveChangesAsync();

                    return Ok(organization);
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
        [HttpPut("editOrganization/{id}")]
        public async Task<IActionResult> EditOrganization(string id, HROrganizationClass organization)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (id != organization.orgid)
            {
                return BadRequest();
            }
          
            _context.Entry(organization).State = EntityState.Modified;

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

            return Ok(organization);
        }
        //
        private bool CcflogReExists(string id)
        {
            return _context.organizationClass.Any(e => e.orgid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.organizationClass.OrderByDescending(u => u.orgid).FirstOrDefault();

            if (userLog == null)
            {
                return "100000";
            }
            var nextId = int.Parse(userLog.orgid) + 1;
            return nextId.ToString();
        }
        //
    }
}
