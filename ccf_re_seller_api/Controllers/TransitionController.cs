using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransitionController : Controller
    {
        private readonly ReSellerAPIContext _context;
        private readonly UserRepository _userRepository;

        public TransitionController(ReSellerAPIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _userRepository = new UserRepository(_context, env);
        }

        //api/transition/all
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<Transition>>> GetAll(CustomerFilter filter)
        {
            var listReferalCustomer = _context.Transition
                .AsQueryable();
          
            if (filter.level == 0)
            {
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.datecreate >= dateFrom && la.datecreate <= dateTo);
                }
                else if (filter.sdate != null && filter.sdate != "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.datecreate >= dateFrom && la.datecreate <= dateTo);
                }
                var list = listReferalCustomer
                    .OrderByDescending(lr => lr.datecreate)
                    .Where(re => re.refcode == filter.refcode)
                    .AsQueryable()
                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                    .Take(filter.pageSize)
                    .ToList();
                return list;

            }

            if(filter.level == 4)
            {
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.datecreate >= dateFrom && la.datecreate <= dateTo);
                }
                else if (filter.sdate != null && filter.sdate != "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.datecreate >= dateFrom && la.datecreate <= dateTo);
                }
                var list = listReferalCustomer
                   .OrderByDescending(lr => lr.datecreate)
                   .AsQueryable()
                   .Skip((filter.pageNumber - 1) * filter.pageSize)
                   .Take(filter.pageSize)
                   .ToList();
                return list;

            }
            return Ok();

        }

        //if transfer widthrawal
        //api/transition
        [HttpPost]
        public async Task<ActionResult<Transition>> PostTransitionWidthrawal(Transition transition)
        {


            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            var referrer = _context.CcfreferalRes.SingleOrDefault(e => e.refcode == transition.refcode);

            transition.tid = await GetNextID();
            transition.datecreate = DOI;
            transition.transitiontype = "Withdrawal";
            //transition.typeaccountbank = referrer.

            //status W = Withdrawal
            transition.status = "W";
            //insert to Transition Table for Withdrawal
            _context.Transition.Add(transition);

            //Calculator withdrawal money from referer
            var amountUser = ((int)(referrer.bal - transition.amount));
            referrer.bal = amountUser;
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            return Ok(transition);

        }



        public async Task<string> GetNextID()
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
