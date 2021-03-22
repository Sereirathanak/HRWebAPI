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
    public class CcfmessagesResController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcfmessagesResController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcfmessagesRes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfmessagesRe>>> GetCcfmessagesRes()
        {
            return await _context.CcfmessagesRes.ToListAsync();
        }

        // GET: api/CcfmessagesRes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CcfmessagesRe>> GetCcfmessagesRe(string id)
        {
            var ccfmessagesRe = await _context.CcfmessagesRes.FindAsync(id);

            if (ccfmessagesRe == null)
            {
                return NotFound();
            }

            return ccfmessagesRe;
        }

        // PUT: api/CcfmessagesRes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfmessagesRe(string id, CcfmessagesRe ccfmessagesRe)
        {
            if (id != ccfmessagesRe.id)
            {
                return BadRequest();
            }

            _context.Entry(ccfmessagesRe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfmessagesReExists(id))
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

        // POST: api/CcfmessagesRes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfmessagesRe>> PostCcfmessagesRe(CcfmessagesRe ccfmessagesRe)
        {
            _context.CcfmessagesRes.Add(ccfmessagesRe);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfmessagesReExists(ccfmessagesRe.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfmessagesRe", new { id = ccfmessagesRe.id }, ccfmessagesRe);
        }

        // DELETE: api/CcfmessagesRes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfmessagesRe>> DeleteCcfmessagesRe(string id)
        {
            var ccfmessagesRe = await _context.CcfmessagesRes.FindAsync(id);
            if (ccfmessagesRe == null)
            {
                return NotFound();
            }

            _context.CcfmessagesRes.Remove(ccfmessagesRe);
            await _context.SaveChangesAsync();

            return ccfmessagesRe;
        }

        private bool CcfmessagesReExists(string id)
        {
            return _context.CcfmessagesRes.Any(e => e.id == id);
        }
    }
}
