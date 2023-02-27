using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Data;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Models;
using ccf_re_seller_api.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRLeaveRequestApprovalController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;
        private readonly HRRepository _userRepository;


        public HRLeaveRequestApprovalController(IConfiguration config, HRContext context, IWebHostEnvironment env)
        {
            _configuration = config;
            _context = context;
            _userRepository = new HRRepository(_context, env);

        }

        // List Approver
        [HttpPost("approver/{id}")]
        public async Task<ActionResult<IEnumerable<HRleaveApprovalRequest>>> GetListApprove(string id,HRClockInOut filter)
        {
             //HRCustomerFilter filter
           var listLeave = _context.leaveApprovalRequest.AsQueryable()
               .Include(e => e.ccflre)
               .Include(e => e.ccfpinfo);
            

            var leave = await listLeave.Where(lr => lr.lreid == id)
                                      
                                     // .Where(e => e.apstatu==2)
                                      .OrderByDescending(lr => lr.applev).Reverse()
                                      .AsQueryable()
                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                      .Take(filter.pageSize)
                                      .ToListAsync();

            if (filter.search != ""  && filter.search!=null)
            {

                List<Object> termsList = new List<Object>();


                var checkLeaveRequest = _context.leaveRequest.Where(cur => cur.eid == filter.search);

                foreach (var i in checkLeaveRequest)
                {
                    var leaverequest =  leave.Where(lr => lr.lreid == i.lreid)
                                           //.Where(e => e.apstatu == 2)
                                           .OrderByDescending(lr => lr.applev).Reverse()
                                           .AsQueryable()
                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                           .Take(filter.pageSize)
                                           .ToList();


                    foreach (var listEmployee in leaverequest)
                    {
                        termsList.Add(listEmployee);
                    }
                }
                return Ok(termsList);
            }
            return Ok(leave);
        }

        [HttpGet("a")]
       public async Task<ActionResult> getall()
        {
            return Ok(await _context.leaveApprovalRequest.ToListAsync());
        }



        // List leave that need to Approve.
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<HRleaveApprovalRequest>>> GetLeaveRequest(string id, HRCustomerFilter filter)
        {
            var listLeave = _context.leaveApprovalRequest.AsQueryable();//.Where(e => e.eid==id)
                //.Include(e => e.ccflre)
                //.Include(e => e.ccfpinfo);



            var leave =await listLeave  .Where(lr => lr.eid == id)
                                        .Where(e => e.apstatu==2).OrderByDescending(e =>e.ccflre.frdat)
                                       // .OrderByDescending(lr => lr.applev).Reverse()
                                        .AsQueryable()
                                        .Skip((filter.pageNumber - 1) * filter.pageSize)
                                        .Take(filter.pageSize)
                                        .Include(e => e.ccflre)
                                        .Include(e => e.ccfpinfo)
                                        .ToListAsync();

            if(filter.search != "" && filter.search != null)
            {
                    leave = await listLeave.Where(lr => lr.ccflre.eid == filter.search).OrderByDescending(e => e.ccflre.frdat)
                                           //  .OrderByDescending(lr => lr.applev).Reverse()
                                             .AsQueryable()
                                             .Skip((filter.pageNumber - 1) * filter.pageSize)
                                             .Take(filter.pageSize)
                                             .Include(e => e.ccfpinfo).Include(e => e.ccflre)
                                             .ToListAsync();

                    return leave.Where(e => e.lreid!= null).ToList();


            }
            return leave;
        }

        //
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HRleaveApprovalRequest>>> GetAll(HRCustomerFilter filter)
        {
            var listEmployee =await _context.leaveApprovalRequest.AsQueryable().ToListAsync();


            int totallistEmployee = listEmployee.Count();
            var listEmployess = listEmployee.Where(lr => lr.eid == filter.eid)
                                               .OrderByDescending(lr => lr.applev).Reverse()
                                               .AsQueryable()
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                               .Take(filter.pageSize)
                                               .ToList();
            return listEmployess;

        }
        //
        [HttpPost("createleaveapproval")]

        public async Task<IActionResult> Create(HRleaveApprovalRequest _leaveApproval)
        {
            try
            {
                if (_leaveApproval.eid != null &&
                    _leaveApproval.lreid != null &&
                    _leaveApproval.applev != 0 &&
                    _leaveApproval.prio != 0 &&
                    _leaveApproval.apstatu != 0
                   )
                {
                    _leaveApproval.lredid = GetLogNextID();

                    _context.leaveApprovalRequest.Add(_leaveApproval);
                    await _context.SaveChangesAsync();
                    //
                    var requestLeave = _context.leaveRequest.SingleOrDefault(e => e.lreid == _leaveApproval.lreid);
                    var user = _context.ccfUserClass.SingleOrDefault(e => e.uid == requestLeave.eid);
                    await _userRepository.SendNotificationUser(_leaveApproval.eid, user.bcode, "CCF HR System App", $"Leave request from {user.uname}.", requestLeave.eid, requestLeave.lreid, "L");
                    return Ok(_leaveApproval);
                }
                else
                {
                    return BadRequest("Request Param.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        //
        //
        [HttpPut("editleaveapproval/{id}")]
        public async Task<IActionResult> Edit(string id, HRleaveApprovalRequest _leaveApproval)
        {

            if (id != _leaveApproval.lredid)
            {
                return BadRequest();
            }
            _context.Entry(_leaveApproval).State = EntityState.Modified;
            //await _context.SaveChangesAsync();

            var listApproveRequest = await _context.leaveApprovalRequest.Where(e => e.lreid == _leaveApproval.lreid).ToListAsync();
            var _leavereq = await _context.leaveRequest.SingleOrDefaultAsync(e => e.lreid == _leaveApproval.lreid);
            var statusRequtest = 1;
            var LeaveBalance = await _context.leaveEnrollment.SingleOrDefaultAsync(e => e.eid == _leavereq.eid);

            if (_leaveApproval.applev == listApproveRequest.Count()-1)
            {
                _context.Entry(_leavereq).State = EntityState.Modified;

                _context.Entry(LeaveBalance).State = EntityState.Modified;
                //LeaveBalance.releav = LeaveBalance.releav - Convert.ToInt16(_leavereq.numleav);

                statusRequtest = _leaveApproval.apstatu;
                _leavereq.statu = statusRequtest.ToString();

            }
            //await _context.SaveChangesAsync();

          

            var statusEdit = "pending";

            if (_leaveApproval.apstatu == 0)
            {
                statusEdit = "reject";
            }

            if (_leaveApproval.apstatu == 1)
            {
                statusEdit = "approved";
            }

            if (_leaveApproval.apstatu == 2)
            {
                statusEdit = "pending";
            }

            if (_leaveApproval.apstatu == 3)
            {
                statusEdit = "return";
            }

            var user = await _context.ccfUserClass.SingleOrDefaultAsync(e => e.uid == _leaveApproval.eid);

            await _userRepository.SendNotificationUser(_leavereq.eid, user.bcode, "CCF HR System App", $"Leave request have been {statusEdit} from {user.uname}.", _leavereq.eid, _leavereq.lreid, "L");
            var userApproval = _context.leaveApprovalRequest.Where(e => e.lreid == _leaveApproval.lreid)
                                .Where(e => e.eid != _leaveApproval.eid).ToList();
            //for (var i = 0; i < userApproval.Count; i++)
            //{
            //    System.Diagnostics.Debug.WriteLine(userApproval[i]);

            //}
            foreach (var userApprove in userApproval)
            {
                await _userRepository.SendNotificationUser(userApprove.eid, user.bcode, "CCF HR System App", $"Leave request have been {statusEdit} from {user.uname}.", _leavereq.eid, _leavereq.lreid, "L");

            }

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
            return Ok(_leaveApproval);
        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.leaveApprovalRequest.Any(e => e.lredid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.leaveApprovalRequest.OrderByDescending(u => u.lredid).FirstOrDefault();

            if (userLog == null)
            {
                return "700000";
            }
            var nextId = int.Parse(userLog.lredid) + 1;
            return nextId.ToString();
        }
        [HttpGet("count/{eid}")]
        public async Task<ActionResult> GetCount(string eid)
        {
            var pending = await _context.leaveApprovalRequest.Where(e => e.eid == eid && e.apstatu==2).ToListAsync();
            var approve = await _context.leaveApprovalRequest.Where(e => e.eid == eid && e.apstatu == 1).ToListAsync();
            if (pending.Count != 0 || approve.Count!=0)
            {
                Count count = new Count();
                count.Approved = approve.Count;
                count.Pending = pending.Count;
                return Ok(count);
            }
            else
            {
                return NotFound("User Not fount");
            }
            

        }
    }
}
