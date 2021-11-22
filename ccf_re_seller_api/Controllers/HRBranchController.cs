using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
//using ccf_re_seller_api.Modals;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<HRBranchClass>> FetchBranchByUser()
        {
            var listReferalCus = _context.branchClass
               .AsQueryable();
            return Ok(listReferalCus);
        }
    }
}
