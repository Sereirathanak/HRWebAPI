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
using System.IO;
using ccf_re_seller_api.Modals;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRDepartmentController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRDepartmentController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRDepartment>>> GetAll()
        {
            return await _context.department.ToListAsync();
        }
        //
        [HttpPost("createdepartment")]
        public async Task<IActionResult> CreatePostion(HRDepartment _department)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_department.depname != null)
                {
                    _department.depid = GetLogNextID();

                    _context.department.Add(_department);
                    await _context.SaveChangesAsync();

                    return Ok(_department);
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
        [HttpPut("editdepartment/{id}")]
        public async Task<IActionResult> Edit(string id, HRDepartment _department)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (id != _department.depid)
            {
                return BadRequest();
            }

            _context.Entry(_department).State = EntityState.Modified;

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

            return Ok(_department);
        }
        //
        private bool CcflogReExists(string id)
        {
            return _context.position.Any(e => e.posid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.department.OrderByDescending(u => u.depid).FirstOrDefault();

            if (userLog == null)
            {
                return "300000";
            }
            var nextId = int.Parse(userLog.depid) + 1;
            return nextId.ToString();
        }
        //
    }
}
