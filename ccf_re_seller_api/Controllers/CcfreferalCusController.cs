using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Repositories;
using CCFReSeller;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class CcfreferalCusController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;
        private readonly UserRepository _userRepository;

        public CcfreferalCusController(ReSellerAPIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _userRepository = new UserRepository(_context, env);
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
            var listReferalsCus = listReferalCus.Where(lr => lr.status == Constant.REQUEST)
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
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));


            try
            {
                ccfreferalCu.cid = await GetNextID();
                ccfreferalCu.refcode = ccfreferalCu.refcode;
                ccfreferalCu.refdate = DOI;
                ccfreferalCu.status = Constant.PEDDING;
                ccfreferalCu.cid = ccfreferalCu.cid;
                ccfreferalCu.uid = ccfreferalCu.uid;
                ccfreferalCu.cname = ccfreferalCu.cname;
                ccfreferalCu.phone = ccfreferalCu.phone;
                ccfreferalCu.lamount = (int)ccfreferalCu.lamount;
                ccfreferalCu.lpourpose = ccfreferalCu.lpourpose;
                ccfreferalCu.u4 =
                     ccfreferalCu.u4;

                ccfreferalCu.province = ccfreferalCu.province;
                ccfreferalCu.district = ccfreferalCu.district;
                ccfreferalCu.commune = ccfreferalCu.commune;
                ccfreferalCu.village = ccfreferalCu.village;
                ccfreferalCu.curcode = ccfreferalCu.curcode;



                //

                CcfreferalCusUp ccfreferalCusUp = new CcfreferalCusUp(_context);
                ccfreferalCusUp.id = await GetNextIDCusterUpdate();
                ccfreferalCusUp.refdate = DOI;
                ccfreferalCusUp.status = Constant.PEDDING;
                ccfreferalCusUp.cid = ccfreferalCu.cid;
                ccfreferalCusUp.uid = ccfreferalCu.uid;
                ccfreferalCusUp.cname = ccfreferalCu.cname;
                ccfreferalCusUp.phone = ccfreferalCu.phone;
                ccfreferalCusUp.lamount = (int)ccfreferalCu.lamount;
                ccfreferalCusUp.lpourpose = ccfreferalCu.lpourpose;
                ccfreferalCusUp.address =
                     ccfreferalCu.u4;
                ccfreferalCusUp.job = "";
                ccfreferalCusUp.bm = "";
                ccfreferalCusUp.btl = "";
                ccfreferalCusUp.co = "";
                ccfreferalCusUp.br = "";
                ccfreferalCusUp.province = ccfreferalCu.province;
                ccfreferalCusUp.district = ccfreferalCu.district;
                ccfreferalCusUp.commune = ccfreferalCu.commune;
                ccfreferalCusUp.village = ccfreferalCu.village;
                ccfreferalCusUp.curcode = ccfreferalCu.curcode;



                //insert to Referal Customer table
                _context.CcfreferalCus.Add(ccfreferalCu);

                //insert to Referal Customer Update table
                _context.CcfreferalCusUps.Add(ccfreferalCusUp);


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
            var refererName = _context.CcfreferalRes.SingleOrDefault(rn => rn.refcode == ccfreferalCu.refcode);
            await _userRepository.SendNotificationCreateReferer("CCF ReSeller App", $"New customer {ccfreferalCu.cname} have been referer by {refererName.refname}", ccfreferalCu.uid, ccfreferalCu.cid, ccfreferalCu.cname, ccfreferalCu.lamount.ToString(), ccfreferalCu.refdate, ccfreferalCu.phone, "");
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
