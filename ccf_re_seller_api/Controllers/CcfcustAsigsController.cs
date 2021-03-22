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
    public class CcfcustAsigsController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcfcustAsigsController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcfcustAsigs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfcustAsig>>> GetCcfcustAsigs()
        {
            return await _context.CcfcustAsigs.ToListAsync();
        }

        // GET: api/CcfcustAsigs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CcfcustAsig>> GetCcfcustAsig(string id)
        {
            var ccfcustAsig = await _context.CcfcustAsigs.FindAsync(id);

            if (ccfcustAsig == null)
            {
                return NotFound();
            }

            return ccfcustAsig;
        }

        // PUT: api/CcfcustAsigs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfcustAsig(string id, CcfcustAsig ccfcustAsig)
        {
            if (id != ccfcustAsig.ascode)
            {
                return BadRequest();
            }

            _context.Entry(ccfcustAsig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfcustAsigExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CcfcustAsigs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfcustAsig>> PostCcfcustAsig(CcfcustAsig ccfcustAsig)
        {
            ccfcustAsig.ascode = await GetNextID();
            ccfcustAsig.status = Constant.PROCESS;
            ccfcustAsig.date = DateTime.Now;
            _context.CcfcustAsigs.Add(ccfcustAsig);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfcustAsigExists(ccfcustAsig.ascode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfcustAsig", new { id = ccfcustAsig.ascode }, ccfcustAsig);
        }

        public async Task<string> GetNextID()
        {
            var id = await _context.CcfcustAsigs.OrderByDescending(u => u.ascode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "600000";
            }
            var nextId = int.Parse(id.ascode) + 1;
            return nextId.ToString();
        }


        // DELETE: api/CcfcustAsigs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfcustAsig>> DeleteCcfcustAsig(string id)
        {
            var ccfcustAsig = await _context.CcfcustAsigs.FindAsync(id);
            if (ccfcustAsig == null)
            {
                return NotFound();
            }

            _context.CcfcustAsigs.Remove(ccfcustAsig);
            await _context.SaveChangesAsync();

            return ccfcustAsig;
        }

        private bool CcfcustAsigExists(string id)
        {
            return _context.CcfcustAsigs.Any(e => e.ascode == id);
        }
    }
}
