using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;
using System.Globalization;
using System.Web.Http.Cors;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*","*")]

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
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            if (id != ccflogRe.id)
            {
                return BadRequest();
            }
            ccflogRe.odate = DOI;
            ccflogRe.u1 = "LogOut";

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
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));


            try
            {
                var id = _context.CcflogRes.Max(c => c.id);
                int convertInt = 0;
                if (id == null)
                {
                    convertInt = 40000;
                }
                else
                {
                    convertInt = int.Parse(id) + 1;

                }
                ccflogRe.id = convertInt.ToString();
                if(ccflogRe.u1 == "L")
                {
                    ccflogRe.ldate = DOI;

                }
                else
                {
                    ccflogRe.odate = DOI;
                }
                _context.CcflogRes.Add(ccflogRe);
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
            return Ok(ccflogRe);

        }

        private  string  genNextId()
        {
            var id = _context.CcflogRes.Max(c => c.id) + 1;

            return id.ToString();
        }




        public async Task<dynamic> GetNextIDLog()
        {
            var ids = await _context.CcflogRes.OrderByDescending(u => u.id).FirstOrDefaultAsync();

            if (ids == null)
            {
                return "400000";
            }
            var nextId = int.Parse(ids.id) + 1;
            return nextId;
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
