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
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            if (id != ccfreferalCusUp.id)
            {
                return BadRequest();
            }
            var status = "";
            if (ccfreferalCusUp.status == "Request Disbursement")
            {
                status = "Request Disbursement";
            }
            else
            {
                status = Constant.PROCESS;
            }

            ccfreferalCusUp.refdate = DOI;
            _context.Entry(ccfreferalCusUp).State = EntityState.Modified;

            try
            {
                var listCus = _context.CcfreferalCus.SingleOrDefault(e => e.cid == ccfreferalCusUp.cid);
                listCus.status = Constant.PROCESS;
                listCus.province = ccfreferalCusUp.province;
                listCus.commune = ccfreferalCusUp.commune;
                listCus.district = ccfreferalCusUp.district;
                listCus.village = ccfreferalCusUp.village;
                listCus.curcode = ccfreferalCusUp.curcode;
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

            if (filter.level == 0)
            {
                var listReferalCustomer = _context.CcfreferalCusUps
               .Include(rf => rf.CcfreferalCu)
               .Include(ul => ul.CcfuserRe)
               .AsQueryable();
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.refdate >= dateFrom && la.refdate <= dateTo);
                }
                else if (filter.sdate != null && filter.sdate != "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.refdate >= dateFrom && la.refdate <= dateTo);
                }
                if (filter.status != null && filter.status != "")
                {
                    listReferalCustomer = listReferalCustomer.Where(lr => lr.status == filter.status.ToString());
                }
                int totalListReferalCustomer = listReferalCustomer.Count();
                var listReferalsCustomer = listReferalCustomer
                    .Where(us => us.uid == filter.uid)
                    .OrderByDescending(lr => lr.refdate)
                    .AsQueryable()
                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                    .Take(filter.pageSize)
                    .OrderBy(x => x.status == Constant.PEDDING ? 1 : x.status == Constant.PROCESS ? 2 : x.status == "FINAL APPROVE" ? 3: x.status == "Request Disbursement" ? 4 : x.status == "D" ? 5 : x.status == "A" ? 6 : 7)
                    .ToList();

                return listReferalsCustomer;
            }
            else
            {

                var listReferalCustomer = _context.CcfreferalCusUps
                    .Include(rf => rf.CcfreferalCu)
                    .Include(ul => ul.CcfuserRe)
                    .AsQueryable();
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.refdate >= dateFrom && la.refdate <= dateTo);
                }
                else if (filter.sdate != null && filter.sdate != "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.refdate >= dateFrom && la.refdate <= dateTo);
                }
                if (filter.status != null && filter.status != "")
                {
                    listReferalCustomer = listReferalCustomer.Where(lr => lr.status == filter.status.ToString());
                }
                int totalListReferalCustomer = listReferalCustomer.Count();
                var listReferalsCustomer = listReferalCustomer
                    .OrderByDescending(lr => lr.refdate)
                    .AsQueryable()
                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                    .Take(filter.pageSize)
                    .OrderBy(x => x.status == Constant.PEDDING ? 1 : x.status == Constant.PROCESS ? 2 : x.status == "FINAL APPROVE" ? 3 : x.status == "Request Disbursement" ? 4 : x.status == "D" ? 5 : x.status == "A" ? 6 : 7)
                    .ToList();
                return listReferalsCustomer;
            };


            //return listReferalsCustomer;

        }

        

            // POST: api/CcfreferalCusUps
            // To protect from overposting attacks, enable the specific properties you want to bind to, for
            // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
            [HttpPost]
        public async Task<ActionResult<CcfreferalCusUp>> PostCcfreferalCusUp(CcfreferalCusUp ccfreferalCusUp)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            ccfreferalCusUp.id = await GetNextID();
            ccfreferalCusUp.refdate = DOI;
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
