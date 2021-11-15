using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class CurrentAmountController : Controller
    {
        private readonly ReSellerAPIContext _context;

        public CurrentAmountController(ReSellerAPIContext context)
        {
            _context = context;
        }


        [HttpPost("all")]
        public async Task<ActionResult<UserCurrentAmount>> getAll(CustomerFilter filter)
        {

            if (filter.level == 4 || filter.level == 5)
            {
                var listReferrer = _context.CcfcurrentAmount
              .Include(rf => rf.CcfuserRe)
              .AsQueryable();

                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    listReferrer = listReferrer.Where(la => la.createdate >= dateFrom && la.createdate <= dateTo);
                }
                else if (filter.sdate != null && filter.sdate != "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    listReferrer = listReferrer.Where(la => la.createdate >= dateFrom && la.createdate <= dateTo);
                }
                if (filter.currentamout != null && filter.currentamout != "")
                {
                    listReferrer = listReferrer.Where(lr => lr.currentamount > 0);
                }
                if (filter.search != null && filter.search != "")
                {
                    listReferrer = listReferrer.Where(lr => lr.uid.ToLower().Contains(filter.search.ToLower()));
                }

                int totalListReferalCustomer = listReferrer.Count();
                var listReferalsCustomer = listReferrer
                    .Where(lr => lr.uid != null)
                    .Where(al => al.currentamount > 0)
                    .OrderByDescending(lr => lr.createdate)
                    .AsQueryable()
                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                    .Take(filter.pageSize)
                    //.OrderBy(x => x.verifystatus == "")
                    .ToList();
                return Ok(listReferalsCustomer);
            }
            return BadRequest();
        }



        [HttpGet("currentamount/{id}")]
        public async Task<ActionResult<UserCurrentAmount>> getCurrentAmount(string id)
        {

            var referer = _context.CcfreferalRes.SingleOrDefault(rn => rn.uid == id);

            var currentAmount = _context.CcfcurrentAmount.SingleOrDefault(u => u.uid == id);

            if (currentAmount.uid == null)
            {
                return BadRequest(new KeyValuePair<string, string>("999", "Referrer don't have commission"));
            }
            else
            {
                List<UserCurrentAmount> results = new List<UserCurrentAmount>()
            {
                new UserCurrentAmount()
                {
                    uid = referer.uid,
                    currentamount = currentAmount.currentamount.ToString(),
                    uname = referer.refname,
                    userStatus = referer.verifystatus,
                    phone = referer.refphone,
                    status = currentAmount.status
                }
             };

                return Ok(results);
            }
        }
        //Withdrew Money to Referrer

        [HttpPut("widthdraw/{id}")]
        public async Task<IActionResult> PostWidthDraw(string id, UserCurrentAmount userCurrentAmount)
        {
            var currentAmountUser = _context.CcfcurrentAmount.SingleOrDefault(u => u.uid == id);
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            if (currentAmountUser.caid == null)
            {
                return BadRequest(new KeyValuePair<string, string>("999", "Referrer don't have commission"));
            }
            else
            {
                var referer = _context.CcfreferalRes.SingleOrDefault(rn => rn.uid == id);




                int minusAmountReferrer = int.Parse(currentAmountUser.currentamount.ToString()) - int.Parse(userCurrentAmount.currentamount);

                currentAmountUser.uid = userCurrentAmount.uid;
                currentAmountUser.currentamount = minusAmountReferrer;
                currentAmountUser.status = "Withdraw";
                currentAmountUser.createdate = DOI;
                currentAmountUser.caid = currentAmountUser.caid;

                await PostTransitionWidthrawal(int.Parse(userCurrentAmount.currentamount), referer.refcode);

                //await _context.SaveChangesAsync();

                _context.Entry(currentAmountUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                return Ok(currentAmountUser);
            }

        }

        //if transfer widthrawal
        //api/transition
        public async Task<dynamic> PostTransitionWidthrawal(int transitionAmount,string referrerParam)
        {


            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            var referrer = _context.CcfreferalRes.SingleOrDefault(e => e.refcode == referrerParam);
            var currentAmountUser = _context.CcfcurrentAmount.SingleOrDefault(u => u.uid == referrer.uid);


            var transitions = new Transition(_context);
            transitions.tid = await GetNextIDWithdrawer();
            transitions.datecreate = DOI;
            transitions.transitiontype = "Withdrawal";
            transitions.refcode = referrerParam;
            transitions.currencycode = "USD";
            transitions.amount = transitionAmount;
            transitions.typeaccountbank = referrer.typeaccountbank;
            transitions.accountbank = referrer.typeaccountnumber;
            transitions.status = "W";
            _context.Transition.Add(transitions);

            referrer.bal = currentAmountUser.currentamount;
            //Calculator withdrawal money from referer
            await _context.SaveChangesAsync();
            return Ok(transitions);

        }



        public async Task<string> GetNextIDWithdrawer()
        {
            var id = await _context.Transition.OrderByDescending(u => u.tid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "900000";
            }
            var nextId = int.Parse(id.tid) + 1;
            return nextId.ToString();
        }

    }
}
