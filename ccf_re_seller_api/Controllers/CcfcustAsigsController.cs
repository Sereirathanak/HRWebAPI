using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;
using CCFReSeller;
using ccf_re_seller_api.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CcfcustAsigsController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;
        private readonly UserRepository _userRepository;

        public CcfcustAsigsController(ReSellerAPIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _userRepository = new UserRepository(_context, env);
        }

        // GET: api/CcfcustAsigs
        // Assign User to process
        [HttpPost("{idBranch}/{levelUser}")]
        public async Task<ActionResult<CcfcustAsig>> AssignUserToProcessing(string idBranch, string levelUser, CcfcustAsig CcfcustAsig)
        {

            var level = int.Parse(levelUser);
            if (level == 4)
            {
                var list = _context.CcfuserRes.Where(lv => lv.level == 3)
                             .Where(br => br.brcode == idBranch);
                return Ok(list);
            };
            if (level == 3)
            {
                var list = _context.CcfuserRes.Where(lv => lv.level == 2)
                            .Where(br => br.brcode == idBranch);
                return Ok(list);
            };
            if (level == 2)
            {
                var list = _context.CcfuserRes.Where(lv => lv.level == 1)
                            .Where(br => br.brcode == idBranch);
                return Ok(list);
            };
            if (level == 1)
            {
                var list = _context.CcfuserRes.Where(lv => lv.level == 3)
                            .Where(br => br.brcode == idBranch);
                return Ok(list);
            };
            return Ok();

            //return await _context.CcfcustAsigs.ToListAsync();
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
            var signalLoan = _context.CcfreferalCusUps.SingleOrDefault(u => u.cid == id);

            var ccfcustAsig = await _context.CcfcustAsigs
                                    .Include(u => u.CcfuserReFu)
                                    .Include(u => u.CcfuserReTu)
                                    .Include(c => c.CcfreferalCu)
                                    //.Include(i => i.cid == signalLoan.cid)
                                    .Where(cid => cid.cid == id)
                                    .AsQueryable()
                                    .ToListAsync(); ;

            if (ccfcustAsig == null)
            {
                return NotFound();
            }

            return Ok(ccfcustAsig);
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
            //bool exitAssign = false;
            //bool exitLoan = false;
            //exitLoan = _context.CcfcustAsigs.Any(el => el.cid == ccfcustAsig.cid);
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            var user = _context.CcfuserRes.SingleOrDefault(ul => ul.uid == ccfcustAsig.tuid);
            Console.WriteLine(ccfcustAsig.status);

            if(ccfcustAsig.status == "Request Disbursement")
            {
                ccfcustAsig.ascode = await GetNextID();
                ccfcustAsig.date = DOI;
                _context.CcfcustAsigs.Add(ccfcustAsig);

                var referalCustomer = _context.CcfreferalCus.SingleOrDefault(e => e.cid == ccfcustAsig.cid);

                referalCustomer.status = ccfcustAsig.status;
                referalCustomer.u1 = user.uname;
                referalCustomer.u5 = user.u5;


                _context.Entry(referalCustomer).State = EntityState.Modified;
                var referalCustomerUpdate = _context.CcfreferalCusUps.SingleOrDefault(e => e.cid == ccfcustAsig.cid);

                referalCustomerUpdate.status = ccfcustAsig.status;
                referalCustomerUpdate.u1 = user.uname;
                referalCustomer.u5 = user.u5;
                _context.Entry(referalCustomerUpdate).State = EntityState.Modified;
                //
                var referer = _context.CcfreferalRes.SingleOrDefault(rn => rn.refcode == referalCustomer.refcode);
                //
                await _userRepository.SendNotificationAssignedUser("CCF ReSeller App", $"Loan Application is request disbursement {user.uname}", user.uid, ccfcustAsig.cid, referalCustomer.cname, referalCustomerUpdate.status, ccfcustAsig.date, referer.refcode, referalCustomerUpdate.phone);
            }

            if (user.level == 3 && ccfcustAsig.status == "FINAL APPROVE")
            {
                ccfcustAsig.ascode = await GetNextID();
                ccfcustAsig.date = DOI;
                _context.CcfcustAsigs.Add(ccfcustAsig);

                var referalCustomer = _context.CcfreferalCus.SingleOrDefault(e => e.cid == ccfcustAsig.cid);

                referalCustomer.status = ccfcustAsig.status;
                referalCustomer.u1 = user.uname;
                referalCustomer.u5 = user.u5;


                _context.Entry(referalCustomer).State = EntityState.Modified;
                var referalCustomerUpdate = _context.CcfreferalCusUps.SingleOrDefault(e => e.cid == ccfcustAsig.cid);

                referalCustomerUpdate.status = ccfcustAsig.status;
                referalCustomerUpdate.u1 = user.uname;
                referalCustomer.u5 = user.u5;
                _context.Entry(referalCustomerUpdate).State = EntityState.Modified;
                //
                var referer = _context.CcfreferalRes.SingleOrDefault(rn => rn.refcode == referalCustomer.refcode);
                //
                await _userRepository.SendNotificationAssignedUser("CCF ReSeller App", $"New customer have been assigned to {user.uname}", user.uid, ccfcustAsig.cid, referalCustomer.cname, referalCustomerUpdate.status, ccfcustAsig.date, referer.refcode, referalCustomerUpdate.phone);
            }
            else
            {
                ccfcustAsig.ascode = await GetNextID();
                ccfcustAsig.date = DOI;

                _context.CcfcustAsigs.Add(ccfcustAsig);

                var referalCustomer = _context.CcfreferalCus.SingleOrDefault(e => e.cid == ccfcustAsig.cid);
                referalCustomer.status = ccfcustAsig.status;
                referalCustomer.u1 = user.uname;
                referalCustomer.u5 = user.u5;
                _context.Entry(referalCustomer).State = EntityState.Modified;


                var referalCustomerUpdate = _context.CcfreferalCusUps.SingleOrDefault(s => s.cid == ccfcustAsig.cid);
                referalCustomerUpdate.status = ccfcustAsig.status;
                referalCustomerUpdate.u1 = user.uname;
                referalCustomer.u5 = user.u5;
                _context.Entry(referalCustomerUpdate).State = EntityState.Modified;
                //
                var referer = _context.CcfreferalRes.SingleOrDefault(rn => rn.refcode == referalCustomer.refcode);
                //
                await _userRepository.SendNotificationAssignedUser("CCF ReSeller App", $"New customer have been assigned to {user.uname}", user.uid, ccfcustAsig.cid, referalCustomer.cname, referalCustomerUpdate.status, ccfcustAsig.date, referer.refcode, referalCustomerUpdate.phone);
            };

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
