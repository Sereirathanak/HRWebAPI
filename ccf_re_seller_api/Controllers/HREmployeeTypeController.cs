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
    public class HREmployeeTypeController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HREmployeeTypeController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HREmployeeType>>> GetCcfreferalCus()
        {
            return await _context.employeeType.ToListAsync();
        }
        //
        [HttpPost("createemployeetype")]
        public async Task<IActionResult> Create(HREmployeeType _employeeType)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_employeeType.typ != null)
                {
                    _employeeType.empid = GetLogNextID();

                    _context.employeeType.Add(_employeeType);
                    await _context.SaveChangesAsync();

                    return Ok(_employeeType);
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
        [HttpPut("editemployeetype/{id}")]
        public async Task<IActionResult> Edit(string id, HREmployeeType _employeeType)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (id != _employeeType.empid)
            {
                return BadRequest();
            }

            _context.Entry(_employeeType).State = EntityState.Modified;

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

            return Ok(_employeeType);
        }
        //
        private bool CcflogReExists(string id)
        {
            return _context.employeeType.Any(e => e.empid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.employeeType.OrderByDescending(u => u.empid).FirstOrDefault();

            if (userLog == null)
            {
                return "300000";
            }
            var nextId = int.Parse(userLog.empid) + 1;
            return nextId.ToString();
        }
        //
    }
}
