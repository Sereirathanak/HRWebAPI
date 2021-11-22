using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ccf_re_seller_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HRUserLogsController : Controller
    {
        private readonly HRContext _context;

        public HRUserLogsController(HRContext context)
        {
            _context = context;
        }

        [HttpGet("hr/")]
        // GET: UserLogController
        public async Task<ActionResult<IEnumerable<HRCcfulog>>> GetUserLogs()
        {
            var userLogs = _context.ccfulog.OrderByDescending(l => l.ldate).AsQueryable();
            return await userLogs.ToListAsync();
        }

        //post user log in
        [HttpPost("hr/userlog")]
        public async Task<IActionResult> PostUserLog(HRCcfulog userlog)

        {
            try
            {

                userlog.ucode = userlog.ucode;
                userlog.ldate = userlog.ldate;
                userlog.odate = userlog.odate;
                userlog.fdevice = userlog.fdevice;
                userlog.iostatus = userlog.iostatus;


                _context.ccfulog.Add(userlog);


                await _context.SaveChangesAsync();

                return Ok(userlog);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

    }
}
