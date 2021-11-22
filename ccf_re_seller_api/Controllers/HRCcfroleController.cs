using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    public class HRCcfroleController : Controller
    {
        private readonly HRContext _context;

        public HRCcfroleController(HRContext context)
        {
            _context = context;
        }

        //[HttpGet("hr/userrole")]
        //// GET: UserLogController
        //public async Task<ActionResult<IEnumerable<HRCcfrole>>> GetUserLogs()
        //{
        //    var userLogs = _context.ccfrole.OrderByDescending(l => l.cdate).AsQueryable();
        //    return await userLogs.ToListAsync();
        //}

        [HttpPost("hr/createrole/{uid}")]
        public async Task<IActionResult> CreateUserRole(string uid,HRCcfrole _role)

        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_role.rname != null)
                {
                    //Employee Document
                    _role.rcode =  GetNextID();
                    _role.rname = _role.rname;
                    _role.rdes = _role.rdes;
                    _role.cdate = DOI;
                    _role.cby = uid;
                  
                    _context.ccfrole.Add(_role);

                    await _context.SaveChangesAsync();
                    return Ok(_role);
                    
                }
                else
                {
                    return BadRequest("The credential is invalid.");

                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        //next id
        public string GetNextID()
        {
            var userrole = _context.ccfrole.OrderByDescending(u => u.rcode).FirstOrDefault();

            if (userrole == null)
            {
                return "200";
            }
            var nextId = int.Parse(userrole.rcode) + 1;
            return nextId.ToString();
        }

    }
}
