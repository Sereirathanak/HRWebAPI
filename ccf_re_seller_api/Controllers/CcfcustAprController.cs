using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Repositories;
using CCFReSeller;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CcfcustAprController : Controller
    {
        private readonly ReSellerAPIContext _context;
        private readonly UserRepository _userRepository;

        public CcfcustAprController(ReSellerAPIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _userRepository = new UserRepository(_context, env);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CcfcustAsig>> GetCcfcustApr(string id)
        {
            var ccfcustAsig = await _context.CcfcustAsigs.FindAsync(id);

            if (ccfcustAsig == null)
            {
                return NotFound();
            }

            return ccfcustAsig;
        }

        //Get All List Approve
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<CcfcustApr>>> GetAll(CustomerFilter filter)
        {
            var listCcfcustAprs = _context.CcfcustAprs
                .Include(ul => ul.CcfuserRe)
                .Include(u => u.CcfreferalCu)
                .AsQueryable();

            int totalCcfcustAprs = listCcfcustAprs.Count();
            var listCcfcustApr = listCcfcustAprs.Where(lr => lr.status == Constant.APPROVE)
                                               .OrderByDescending(lr => lr.date)
                                               .AsQueryable()
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                               .Take(filter.pageSize)
                                               .ToList();

            return listCcfcustApr;

        }

        [HttpPost]
        public async Task<ActionResult<CcfcustApr>> PostCcfcustApr(CcfcustApr ccfcustApr)
        {
            

            //return Ok(userReferer);

            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            try
            {
                var user = _context.CcfuserRes.SingleOrDefault(ul => ul.uid == ccfcustApr.uid);
                if (ccfcustApr.status == Constant.APPROVE)
                {
                    ccfcustApr.aprcode = await GetNextID();
                    ccfcustApr.status = Constant.APPROVE;
                    ccfcustApr.date = DOI;
                    _context.CcfcustAprs.Add(ccfcustApr);

                    //

                    CcfcustAsig updateCcfcustAsigs = new CcfcustAsig(_context);
                    updateCcfcustAsigs.status = Constant.APPROVE;
                    updateCcfcustAsigs.u1 = user.uname;
                    updateCcfcustAsigs.date = DOI;
                    updateCcfcustAsigs.fuid = ccfcustApr.uid;
                    updateCcfcustAsigs.tuid = ccfcustApr.uid;
                    updateCcfcustAsigs.cid = ccfcustApr.cid;
                    updateCcfcustAsigs.ascode = await GetNextIDCcfcustAsigs();
                    updateCcfcustAsigs.remark = ccfcustApr.remark;
                    updateCcfcustAsigs.u1 = ccfcustApr.u1;
                    updateCcfcustAsigs.u2 = ccfcustApr.u2;
                    updateCcfcustAsigs.u3 = ccfcustApr.u3;
                    updateCcfcustAsigs.u4 = ccfcustApr.u4;
                    updateCcfcustAsigs.u5 = ccfcustApr.u5;
                    _context.CcfcustAsigs.Add(updateCcfcustAsigs);
                    //

                    var referalCustomer = _context.CcfreferalCus.SingleOrDefault(el => el.cid == ccfcustApr.cid);

                    referalCustomer.status = Constant.APPROVE;
                    referalCustomer.u1 = user.uname;
                    _context.Entry(referalCustomer).State = EntityState.Modified;


                    var exchangeRate = _context.ExchangeRate.SingleOrDefault(el => el.curcode == ccfcustApr.u4);
                    //Calculator money for referer
                    int amount = 0;
                    if (int.Parse(ccfcustApr.u4) == 100)
                    {
                        int khmerAmount = int.Parse(ccfcustApr.u3);
                        int finalAmount = khmerAmount / int.Parse(exchangeRate.exchangeamount);


                        if (finalAmount < 10000)
                        {
                            amount = 5;
                        }

                        if (finalAmount >= 10000 && finalAmount <= 30000)
                        {
                            amount = 15;
                        }

                        if (finalAmount >= 30000 && finalAmount <= 100000)
                        {
                            amount = 20;
                        }

                    }
                    else
                    {
                        if (int.Parse(ccfcustApr.u3) < 10000)
                        {
                            amount = 5;
                        }

                        if (int.Parse(ccfcustApr.u3) >= 10000 && int.Parse(ccfcustApr.u3) <= 30000)
                        {
                            amount = 15;
                        }

                        if (int.Parse(ccfcustApr.u3) >= 30000 && int.Parse(ccfcustApr.u3) <= 100000)
                        {
                            amount = 20;
                        }
                    }

                    var referalCusUp = _context.CcfreferalCusUps.SingleOrDefault(el => el.cid == ccfcustApr.cid);
                    var referalCus = _context.CcfreferalCus.SingleOrDefault(el => el.cid == referalCusUp.cid);


                    var userReferer = _context.CcfreferalRes.SingleOrDefault(us => us.refcode == referalCus.refcode);

                    int amountUser = 0;
                    if (userReferer.bal == null)
                    {
                        amountUser = amount;
                    }
                    else
                    {
                        amountUser = ((int)(userReferer.bal + amount));
                    }

                    //deposit to referrer account into Transition Table


                    var id = _context.Transition.Max(c => c.tid);
                    int convertInt = 0;
                    if (id == null)
                    {
                        convertInt = 40000;
                    }
                    else
                    {
                        convertInt = int.Parse(id) + 1;

                    }


                    Transition updateTransition = new Transition(_context);
                    updateTransition.tid = convertInt.ToString();
                    updateTransition.cid = ccfcustApr.cid;
                    updateTransition.refcode = userReferer.refcode;
                    updateTransition.transitiontype = "Deposit";
                    updateTransition.approvecode = ccfcustApr.aprcode;
                    updateTransition.datecreate = DOI;
                    updateTransition.status = ccfcustApr.status;
                    updateTransition.amount = amount;
                    updateTransition.currencycode = ccfcustApr.u4;

                    _context.Transition.Add(updateTransition);

                    //if transfer widthrawal


                    userReferer.bal = amountUser;
                    _context.Entry(userReferer).State = EntityState.Modified;


                    var referalCustomerUpdate = _context.CcfreferalCusUps.SingleOrDefault(el => el.cid == ccfcustApr.cid);

                    referalCustomerUpdate.status = Constant.APPROVE;
                    referalCustomerUpdate.u1 = user.uname;
                    referalCustomerUpdate.u1 = user.uname;
                    referalCustomerUpdate.curcode = ccfcustApr.u4;
                    referalCustomerUpdate.lamount = int.Parse(ccfcustApr.u3);
                    referalCustomerUpdate.refbalance = amount.ToString();
                    referalCustomerUpdate.loanid = ccfcustApr.u1;

                    _context.Entry(referalCustomerUpdate).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    //
                    var referer = _context.CcfreferalRes.SingleOrDefault(rn => rn.refcode == referalCustomer.refcode);
                    //
                    await _userRepository.SendNotificationAssignedUser("CCF ReSeller App", $"New customer have been approved by {user.uname}", user.uid, ccfcustApr.cid, referalCustomer.cname, referalCustomerUpdate.status, ccfcustApr.date, referer.refcode, referalCustomerUpdate.phone);
                }
                else
                {

                    ccfcustApr.aprcode = await GetNextID();
                    ccfcustApr.status = Constant.DISAPPROVE;
                    ccfcustApr.date = DOI;
                    _context.CcfcustAprs.Add(ccfcustApr);
                    //

                    CcfcustAsig updateCcfcustAsigs = new CcfcustAsig(_context);
                    updateCcfcustAsigs.status = Constant.DISAPPROVE;
                    updateCcfcustAsigs.u1 = user.uname;
                    updateCcfcustAsigs.date = DOI;
                    updateCcfcustAsigs.fuid = ccfcustApr.uid;
                    updateCcfcustAsigs.tuid = ccfcustApr.uid;
                    updateCcfcustAsigs.cid = ccfcustApr.cid;
                    updateCcfcustAsigs.ascode = await GetNextIDCcfcustAsigs();
                    updateCcfcustAsigs.remark = ccfcustApr.remark;
                    updateCcfcustAsigs.u1 = ccfcustApr.u1;
                    updateCcfcustAsigs.u2 = ccfcustApr.u2;
                    _context.CcfcustAsigs.Add(updateCcfcustAsigs);

                    //
                    var referalCustomer = _context.CcfreferalCus.SingleOrDefault(el => el.cid == ccfcustApr.cid);

                    referalCustomer.status = Constant.DISAPPROVE;
                    referalCustomer.u1 = user.uname;
                    _context.Entry(referalCustomer).State = EntityState.Modified;


                    var referalCustomerUpdate = _context.CcfreferalCusUps.SingleOrDefault(el => el.cid == ccfcustApr.cid);

                    referalCustomerUpdate.status = Constant.DISAPPROVE;
                    referalCustomerUpdate.u1 = user.uname;
                    _context.Entry(referalCustomerUpdate).State = EntityState.Modified;


                    await _context.SaveChangesAsync();

                    var referer = _context.CcfreferalRes.SingleOrDefault(rn => rn.refcode == referalCustomer.refcode);
                    //
                    await _userRepository.SendNotificationAssignedUser("CCF ReSeller App", $"Customer have been disaaproved by {user.uname}", user.uid, ccfcustApr.cid, referalCustomer.cname, referalCustomerUpdate.status, ccfcustApr.date, referer.refcode, referalCustomerUpdate.phone);

                };
            }
            catch (DbUpdateException)
            {
                if (CcfcustAsigExists(ccfcustApr.aprcode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfcustApr", new { id = ccfcustApr.aprcode }, ccfcustApr);
        }

        public async Task<string> GetNextIDTransition()
        {
            var id = await _context.Transition.OrderByDescending(u => u.tid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "900000";
            }
            var nextId = int.Parse(id.tid + 1);
            return nextId.ToString();
        }

        public async Task<string> GetNextIDCcfcustAsigs()
        {
            var id = await _context.CcfcustAsigs.OrderByDescending(u => u.ascode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "600000";
            }
            var nextId = int.Parse(id.ascode) + 1;
            return nextId.ToString();
        }

        //Next ID
        public async Task<string> GetNextID()
        {
            var id = await _context.CcfcustAprs.OrderByDescending(u => u.aprcode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "900000";
            }
            var nextId = int.Parse(id.aprcode) + 1;
            return nextId.ToString();
        }

        //Next ID
        //public async Task<int> ExitingAmount()
        //{
        //    var bal = await _context.CcfreferalRes.OrderByDescending(u => u.refcode).FirstOrDefaultAsync();

        //    if (bal == null)
        //    {
        //        return 0;
        //    }
        //    var nextId = int.Parse(bal.aprcode) + 1;
        //    return nextId.ToString();
        //}

        private bool CcfcustAsigExists(string id)
        {
            return _context.CcfcustAprs.Any(e => e.aprcode == id);
        }

    }
}
