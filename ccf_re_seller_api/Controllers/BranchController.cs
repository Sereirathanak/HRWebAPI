using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class BranchController : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IConfiguration _configuration;
        private readonly ReSellerAPIContext _context;

        public BranchController(IConfiguration config, ReSellerAPIContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<BranchClass>> FetchBranchByUser()
        {
            var listReferalCus = _context.BranchClass
               .AsQueryable();
            return Ok(listReferalCus);
        }
    }
}
