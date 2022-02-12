using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRBranchController : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IConfiguration _configuration;
        private readonly HRContext _context;


        public HRBranchController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        // GET: api/CcfreferalCus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRBranchClass>>> GetCcfreferalCus()
        {
            return await _context.hrBranchClass.ToListAsync();
        }


        [HttpPost]
        public async Task<IActionResult> PostBranch(HRBranchClass _hrBranch)
        {
            try
            {
                if (_hrBranch.orgid != null &&
                    _hrBranch.braname != null &&
                    _hrBranch.typ != null &&
                    _hrBranch.braadd != null
                    )
                {

                    _context.hrBranchClass.Add(_hrBranch);
                    await _context.SaveChangesAsync();

                    return Ok(_hrBranch);
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
        public string GetLogNextID()
        {
            var userLog = _context.hrBranchClass.OrderByDescending(u => u.braid).FirstOrDefault();

            if (userLog == null)
            {
                return "100000";
            }
            var nextId = int.Parse(userLog.braid) + 1;
            return nextId.ToString();
        }
        //
    }
}
