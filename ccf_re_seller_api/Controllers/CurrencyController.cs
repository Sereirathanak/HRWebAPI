using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        private readonly ReSellerAPIContext _context;

        public CurrencyController(ReSellerAPIContext context)
        {
            _context = context;
        }

        [HttpGet("Currencies")]
        public async Task<List<ReturnCurrency>> getCurrency()
        {
            var loanProduct = _context.Currency.Select(lp => new { lp.curcode, lp.curname }).AsQueryable();

            if (loanProduct == null)
            {
                return null;
            }

            var results = loanProduct.Select(r => new ReturnCurrency()
            {
                curcode = r.curcode,
                curname = r.curname
            })
            .ToListAsync();

            return await results;
        }
    }
}
