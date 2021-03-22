using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Modals;
using CCFReSeller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CcfcustAprController : Controller
    {
        private readonly ReSellerAPIContext _context;

        public CcfcustAprController(ReSellerAPIContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CcfcustAsig>> GetCcfcustApr(string id)
        {
            var ccfcustAsig = await _context.CcfcustAsigs.FindAsync(id);

            if (ccfcustAsig == null)
            {
                return NotFound();
            }

            return ccfcustAsig;
        }

        //Get All List Approve
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<CcfcustApr>>> GetAll(CustomerFilter filter)
        {
            var listCcfcustAprs = _context.CcfcustAprs
                .Include(ul => ul.CcfuserRe)
                .Include(u=> u.CcfreferalCu)
                .AsQueryable();

            int totalCcfcustAprs = listCcfcustAprs.Count();
            var listCcfcustApr = listCcfcustAprs.Where(lr => lr.status == Constant.APPROVE)
                                               .OrderByDescending(lr => lr.date)
                                               .AsQueryable()
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                               .Take(filter.pageSize)
                                               .ToList();

            return listCcfcustApr;

        }

        [HttpPost]
        public async Task<ActionResult<CcfcustApr>> PostCcfcustApr(CcfcustApr ccfcustApr)
        {
            ccfcustApr.aprcode = await GetNextID();
            ccfcustApr.status = Constant.APPROVE;
            ccfcustApr.date = DateTime.Now;
            _context.CcfcustAprs.Add(ccfcustApr);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfcustAsigExists(ccfcustApr.aprcode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfcustApr", new { id = ccfcustApr.aprcode }, ccfcustApr);
        }

        //Next ID
        public async Task<string> GetNextID()
        {
            var id = await _context.CcfcustAprs.OrderByDescending(u => u.aprcode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "900000";
            }
            var nextId = int.Parse(id.aprcode) + 1;
            return nextId.ToString();
        }

        private bool CcfcustAsigExists(string id)
        {
            return _context.CcfcustAprs.Any(e => e.aprcode == id);
        }

    }
}
