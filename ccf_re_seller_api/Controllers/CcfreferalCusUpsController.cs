using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;
using CCFReSeller;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CcfreferalCusUpsController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcfreferalCusUpsController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcfreferalCusUps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfreferalCusUp>>> GetCcfreferalCusUps()
        {
            return await _context.CcfreferalCusUps.ToListAsync();
        }

        // GET: api/CcfreferalCusUps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CcfreferalCusUp>> GetCcfreferalCusUp(string id)
        {
            var ccfreferalCusUp = await _context.CcfreferalCusUps.FindAsync(id);

            if (ccfreferalCusUp == null)
            {
                return NotFound();
            }

            return ccfreferalCusUp;
        }

        // PUT: api/CcfreferalCusUps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfreferalCusUp(string id, CcfreferalCusUp ccfreferalCusUp)
        {
            if (id != ccfreferalCusUp.id)
            {
                return BadRequest();
            }

            _context.Entry(ccfreferalCusUp).State = EntityState.Modified;

            try
            {
                //var list = CcfreferalCu();
                // Update Loan Request Status To Group Loan
                var listCus = _context.CcfreferalCus.SingleOrDefault(e => e.cid == ccfreferalCusUp.cid);
                listCus.status = Constant.PROCESS;
                _context.SaveChanges();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfreferalCusUpExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(ccfreferalCusUp);
        }

        // Get List Referral Customer update
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<CcfreferalCusUp>>> GetAll(CustomerFilter filter)
        {
            var listReferalCustomer = _context.CcfreferalCusUps
                .Include(rf => rf.CcfreferalCu)
                .Include(ul => ul.CcfuserRe)
                .AsQueryable();

            int totalListReferalCustomer = listReferalCustomer.Count();
            var listReferalsCustomer = listReferalCustomer.Where(lr => lr.status == Constant.PROCESS)
                                               .OrderByDescending(lr => lr.refdate)
                                               .AsQueryable()
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                               .Take(filter.pageSize)
                                               .ToList();

            return listReferalsCustomer;

        }

        // POST: api/CcfreferalCusUps
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfreferalCusUp>> PostCcfreferalCusUp(CcfreferalCusUp ccfreferalCusUp)
        {
            ccfreferalCusUp.id = await GetNextID();
            ccfreferalCusUp.refdate = DateTime.Now;
            ccfreferalCusUp.status = Constant.PROCESS;
            _context.CcfreferalCusUps.Add(ccfreferalCusUp);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfreferalCusUpExists(ccfreferalCusUp.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfreferalCusUp", new { id = ccfreferalCusUp.id }, ccfreferalCusUp);
        }

        public async Task<string> GetNextID()
        {
            var id = await _context.CcfreferalCusUps.OrderByDescending(u => u.id).FirstOrDefaultAsync();

            if (id == null)
            {
                return "500000";
            }
            var nextId = int.Parse(id.id) + 1;
            return nextId.ToString();
        }

        // DELETE: api/CcfreferalCusUps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfreferalCusUp>> DeleteCcfreferalCusUp(string id)
        {
            var ccfreferalCusUp = await _context.CcfreferalCusUps.FindAsync(id);
            if (ccfreferalCusUp == null)
            {
                return NotFound();
            }

            _context.CcfreferalCusUps.Remove(ccfreferalCusUp);
            await _context.SaveChangesAsync();

            return ccfreferalCusUp;
        }

        private bool CcfreferalCusUpExists(string id)
        {
            return _context.CcfreferalCusUps.Any(e => e.id == id);
        }
    }
}
