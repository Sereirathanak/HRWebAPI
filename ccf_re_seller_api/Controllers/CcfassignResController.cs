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

    public class CcfassignResController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcfassignResController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcfassignRes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfassignRe>>> GetCcfassignRes()
        {
            return await _context.CcfassignRes.ToListAsync();
        }

        // GET: api/CcfassignRes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CcfassignRe>> GetCcfassignRe(string id)
        {
            var ccfassignRe = await _context.CcfassignRes.FindAsync(id);

            if (ccfassignRe == null)
            {
                return NotFound();
            }

            return ccfassignRe;
        }

        // PUT: api/CcfassignRes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfassignRe(string id, CcfassignRe ccfassignRe)
        {
            if (id != ccfassignRe.id)
            {
                return BadRequest();
            }

            _context.Entry(ccfassignRe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfassignReExists(id))
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

        // POST: api/CcfassignRes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfassignRe>> PostCcfassignRe(CcfassignRe ccfassignRe)
        {

            ccfassignRe.id = await GetNextID();
            ccfassignRe.adate = DateTime.Now;
            _context.CcfassignRes.Add(ccfassignRe);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfassignReExists(ccfassignRe.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfassignRe", new { id = ccfassignRe.id }, ccfassignRe);
        }
        //Next ID
        public async Task<string> GetNextID()
        {
            var id = await _context.CcfassignRes.OrderByDescending(u => u.id).FirstOrDefaultAsync();

            if (id == null)
            {
                return "800000";
            }
            var nextId = int.Parse(id.id) + 1;
            return nextId.ToString();
        }

        // DELETE: api/CcfassignRes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfassignRe>> DeleteCcfassignRe(string id)
        {
            var ccfassignRe = await _context.CcfassignRes.FindAsync(id);
            if (ccfassignRe == null)
            {
                return NotFound();
            }

            _context.CcfassignRes.Remove(ccfassignRe);
            await _context.SaveChangesAsync();

            return ccfassignRe;
        }

        private bool CcfassignReExists(string id)
        {
            return _context.CcfassignRes.Any(e => e.id == id);
        }
    }
}
