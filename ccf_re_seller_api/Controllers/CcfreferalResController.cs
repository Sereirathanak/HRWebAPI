using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;
using CCFReSeller;
using System.Globalization;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CcfreferalResController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcfreferalResController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcfreferalRes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfreferalRe>>> GetCcfreferalRes()
        {
            return await _context.CcfreferalRes.ToListAsync();
        }

        // GET: api/CcfreferalRes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TotalRequest>>> GetCcfreferalRe(string id)
        {
            //var ccfreferalRe = await _context.CcfreferalRes.FindAsync(id);
            //
            var list = _context.CcfreferalRes
                        .SingleOrDefault(re => re.uid == id);

            var listCustomer = _context.CcfreferalCusUps.AsQueryable();

            int totalCustomer = listCustomer.Where(u => u.uid == id)
                                    .Count();
            int totalPaddingCustomer = listCustomer.Where(u => u.uid == id)
                                       .Where(u => u.status == Constant.ASSIGN).Count();
            int totalLoanCustomer = listCustomer.Where(u => u.uid == id)
                           .Where(u => u.status == Constant.APPROVE).Count();

            //var 

            if (listCustomer == null)
            {
                return NotFound();
            }

            List<TotalRequest> results = new List<TotalRequest>()
            {
                new TotalRequest()
                {
                    totalCustomer = totalCustomer,
                    totalPaddingCustomer = totalPaddingCustomer,
                     totalLoanCustomer = totalLoanCustomer,
                     CcfreferalRe = list,
                }
            };
            return results;
        }
        //Get List Referrer
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<CcfreferalRe>>> GetAll(CustomerFilter filter)
        {
            var listReferal = _context.CcfreferalRes
                .Include(ul => ul.CcfuserRe)
                .AsQueryable();

            int totalListReferal = listReferal.Count();
            var listReferals = listReferal.Where(lr => lr.status == Constant.REQUEST)
                                               .OrderByDescending(lr => lr.regdate)
                                               .AsQueryable()
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                               .Take(filter.pageSize)
                                               .ToList();

            return listReferals;

        }

        // PUT: api/CcfreferalRes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfreferalRe(string id, CcfreferalRe ccfreferalRe)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            ccfreferalRe.status = Constant.PROCESS;
            ccfreferalRe.regdate = DOI;

            if (id != ccfreferalRe.refcode)
            {
                return BadRequest();
            }

            _context.Entry(ccfreferalRe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfreferalReExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(ccfreferalRe);
        }

        // POST: api/CcfreferalRes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfreferalRe>> PostCcfreferalRe(CcfreferalRe ccfreferalRe)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            ccfreferalRe.refcode = await GetNextID();
            ccfreferalRe.regdate = DOI;
            _context.CcfreferalRes.Add(ccfreferalRe);
            ccfreferalRe.status = Constant.REQUEST;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfreferalReExists(ccfreferalRe.refcode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfreferalRe", new { id = ccfreferalRe.refcode }, ccfreferalRe);
        }
        //Next ID
        public async Task<string> GetNextID()
        {
            var id = await _context.CcfreferalRes.OrderByDescending(u => u.refcode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "300000";
            }
            var nextId = int.Parse(id.refcode) + 1;
            return nextId.ToString();
        }

        // DELETE: api/CcfreferalRes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfreferalRe>> DeleteCcfreferalRe(string id)
        {
            var ccfreferalRe = await _context.CcfreferalRes.FindAsync(id);
            if (ccfreferalRe == null)
            {
                return NotFound();
            }

            _context.CcfreferalRes.Remove(ccfreferalRe);
            await _context.SaveChangesAsync();

            return ccfreferalRe;
        }

        private bool CcfreferalReExists(string id)
        {
            return _context.CcfreferalRes.Any(e => e.refcode == id);
        }
    }
}
