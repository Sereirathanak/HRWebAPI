using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;
using System.Web.Http.Cors;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*","*")]

    public class CcfroleResController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcfroleResController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcfroleRes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfroleRe>>> GetCcfroleRes()
        {
            return await _context.CcfroleRes.ToListAsync();
        }

        // GET: api/CcfroleRes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CcfroleRe>> GetCcfroleRe(string id)
        {
            var ccfroleRe = await _context.CcfroleRes.FindAsync(id);

            if (ccfroleRe == null)
            {
                return NotFound();
            }

            return ccfroleRe;
        }

        // PUT: api/CcfroleRes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfroleRe(string id, CcfroleRe ccfroleRe)
        {
            if (id != ccfroleRe.rcode)
            {
                return BadRequest();
            }

            _context.Entry(ccfroleRe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfroleReExists(id))
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

        // POST: api/CcfroleRes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfroleRe>> PostCcfroleRe(CcfroleRe ccfroleRe)
        {
            _context.CcfroleRes.Add(ccfroleRe);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfroleReExists(ccfroleRe.rcode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfroleRe", new { id = ccfroleRe.rcode }, ccfroleRe);
        }

        //Next ID
        public async Task<string> GetNextID()
        {
            var id = await _context.CcfroleRes.OrderByDescending(u => u.rcode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "200000";
            }
            var nextId = int.Parse(id.rcode) + 1;
            return nextId.ToString();
        }

        // DELETE: api/CcfroleRes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfroleRe>> DeleteCcfroleRe(string id)
        {
            var ccfroleRe = await _context.CcfroleRes.FindAsync(id);
            if (ccfroleRe == null)
            {
                return NotFound();
            }

            _context.CcfroleRes.Remove(ccfroleRe);
            await _context.SaveChangesAsync();

            return ccfroleRe;
        }

        private bool CcfroleReExists(string id)
        {
            return _context.CcfroleRes.Any(e => e.rcode == id);
        }
    }
}
