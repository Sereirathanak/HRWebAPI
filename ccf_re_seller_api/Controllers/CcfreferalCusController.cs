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
    public class CcfreferalCusController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcfreferalCusController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcfreferalCus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfreferalCu>>> GetCcfreferalCus()
        {
            return await _context.CcfreferalCus.ToListAsync();
        }

        // GET: api/CcfreferalCus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CcfreferalCu>> GetCcfreferalCu(string id)
        {
            var ccfreferalCu = await _context.CcfreferalCus
                .FindAsync(id);
               

            if (ccfreferalCu == null)
            {
                return NotFound();
            }

            return ccfreferalCu;
        }

        //Get All List Referer
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<CcfreferalCu>>> GetAll(CustomerFilter filter)
        {
            var listReferalCus = _context.CcfreferalCus
                .Include(ul => ul.CcfuserRe)
                .AsQueryable();

            int totalListReferalCus = listReferalCus.Count();
            var listReferalsCus= listReferalCus.Where(lr => lr.status == Constant.REQUEST)
                                               .OrderByDescending(lr => lr.refdate)
                                               .AsQueryable()
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                               .Take(filter.pageSize)
                                               .ToList();

            return listReferalsCus;

        }

        // PUT: api/CcfreferalCus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfreferalCu(string id, CcfreferalCu ccfreferalCu)
        {
            if (id != ccfreferalCu.cid)
            {
                return BadRequest();
            }

            _context.Entry(ccfreferalCu).State = EntityState.Modified;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfreferalCuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(ccfreferalCu);
        }

        // POST: api/CcfreferalCus
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfreferalCu>> PostCcfreferalCu(CcfreferalCu ccfreferalCu)
        {
            ccfreferalCu.cid = await GetNextID();
            ccfreferalCu.refdate = DateTime.Now;
            ccfreferalCu.status = Constant.REQUEST;
            //

            CcfreferalCusUp ccfreferalCusUp = new CcfreferalCusUp();
            ccfreferalCusUp.id = await GetNextIDCusterUpdate();
            ccfreferalCusUp.refdate = DateTime.Now;
            ccfreferalCusUp.status = Constant.PROCESS;
            ccfreferalCusUp.cid = ccfreferalCu.cid;
            ccfreferalCusUp.uid = ccfreferalCu.uid;
            ccfreferalCusUp.cname = ccfreferalCu.cname;
            ccfreferalCusUp.phone = ccfreferalCu.phone;
            ccfreferalCusUp.lamount = int.Parse(ccfreferalCu.lamount.ToString());
            ccfreferalCusUp.lpourpose = ccfreferalCu.lpourpose;
            ccfreferalCusUp.address = ccfreferalCu.u1;
            ccfreferalCusUp.job = "";
            ccfreferalCusUp.bm = "";
            ccfreferalCusUp.btl = "";
            ccfreferalCusUp.co = "";
            ccfreferalCusUp.br = "";

            //insert to Referal Customer Update table
            _context.CcfreferalCusUps.Add(ccfreferalCusUp);

            //insert to Referal Customer table
            _context.CcfreferalCus.Add(ccfreferalCu);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfreferalCuExists(ccfreferalCu.cid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(ccfreferalCu);
        }


        public async Task<string> GetNextIDCusterUpdate()
        {
            var id = await _context.CcfreferalCusUps.OrderByDescending(u => u.id).FirstOrDefaultAsync();

            if (id == null)
            {
                return "500000";
            }
            var nextId = int.Parse(id.id) + 1;
            return nextId.ToString();
        }

        public async Task<string> GetNextID()
        {
            var id = await _context.CcfreferalCus.OrderByDescending(u => u.cid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "400000";
            }
            var nextId = int.Parse(id.cid) + 1;
            return nextId.ToString();
        }

        // DELETE: api/CcfreferalCus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfreferalCu>> DeleteCcfreferalCu(string id)
        {
            var ccfreferalCu = await _context.CcfreferalCus.FindAsync(id);
            if (ccfreferalCu == null)
            {
                return NotFound();
            }

            _context.CcfreferalCus.Remove(ccfreferalCu);
            await _context.SaveChangesAsync();

            return ccfreferalCu;
        }

        private bool CcfreferalCuExists(string id)
        {
            return _context.CcfreferalCus.Any(e => e.cid == id);
        }
    }
}
