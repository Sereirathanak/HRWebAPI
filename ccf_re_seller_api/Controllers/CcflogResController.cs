using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CcflogResController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcflogResController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcflogRes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcflogRe>>> GetCcflogRes()
        {
            return await _context.CcflogRes.ToListAsync();
        }

        // GET: api/CcflogRes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CcflogRe>> GetCcflogRe(string id)
        {
            var ccflogRe = await _context.CcflogRes.FindAsync(id);

            if (ccflogRe == null)
            {
                return NotFound();
            }

            return ccflogRe;
        }

        // PUT: api/CcflogRes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcflogRe(string id, CcflogRe ccflogRe)
        {
            if (id != ccflogRe.id)
            {
                return BadRequest();
            }

            _context.Entry(ccflogRe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcflogReExists(id))
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

        // POST: api/CcflogRes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcflogRe>> PostCcflogRe(CcflogRe ccflogRe)
        {
            ccflogRe.id = await GetNextID();
            _context.CcflogRes.Add(ccflogRe);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcflogReExists(ccflogRe.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcflogRe", new { id = ccflogRe.id }, ccflogRe);
        }

        //Next ID
        public async Task<string> GetNextID()
        {
            var id = await _context.CcflogRes.OrderByDescending(u => u.id).FirstOrDefaultAsync();

            if (id == null)
            {
                return "700000";
            }
            var nextId = int.Parse(id.id) + 1;
            return nextId.ToString();
        }

        // DELETE: api/CcflogRes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcflogRe>> DeleteCcflogRe(string id)
        {
            var ccflogRe = await _context.CcflogRes.FindAsync(id);
            if (ccflogRe == null)
            {
                return NotFound();
            }

            _context.CcflogRes.Remove(ccflogRe);
            await _context.SaveChangesAsync();

            return ccflogRe;
        }

        private bool CcflogReExists(string id)
        {
            return _context.CcflogRes.Any(e => e.id == id);
        }
    }
}
