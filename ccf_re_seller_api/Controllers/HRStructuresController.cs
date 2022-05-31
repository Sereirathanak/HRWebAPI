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
    public class HRStructuresController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRStructuresController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnHRStructures>>> GetCcfreferalCus()
        {
            var list = _context.structures.AsQueryable();
            var listEmploy = _context.employeeJoinInfo.AsQueryable();

            //for (int i = 0; i < listEmploy.Count(); i++)
            //{
                
            //    var recordLists = _context.structures.Where(e => e.children == listEmploy)
            //                            .AsQueryable()
            //                            .ToList();

            //    ReturnHRStructures resultLists = new ReturnHRStructures()
            //    {
            //        listStructures = recordLists
            //    };

            //    return Ok(i);
            //}

            foreach (var item in _context.employeeJoinInfo.Select((value, i) => new { i, value }))
            {
                var values = item.value;
                var index = item.i;

                var recordLists = _context.structures.Where(e => e.children == values.sup)
                                     .AsQueryable()
                                     .ToList();
                //ReturnHRStructures resultLists = new ReturnHRStructures()
                //{
                //    listStructures = recordLists
                //};

                return Ok(recordLists);
            }

            return BadRequest();

        }
        //
    }
}
