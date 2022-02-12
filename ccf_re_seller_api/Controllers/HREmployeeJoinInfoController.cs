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
    public class HREmployeeJoinInfoController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HREmployeeJoinInfoController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HREmployeeJoinInfo>>> GetDetailEmployeeJoin(string id)
        {
            var results = _context.employeeJoinInfo.Where(u => u.eid == id);

            if (results == null)
            {
                return NotFound();
            }
            return results.ToList();
        }
    }
}
