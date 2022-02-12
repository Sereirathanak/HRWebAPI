using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
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

        [HttpGet("approver/{id}")]
        public async Task<ActionResult<IEnumerable<HRleaveApprovalRequest>>> GetListApprove(string id)
        {

            var listLeave = _context.leaveApprovalRequest.AsQueryable()
                .Include(e => e.ccflre)
                .Include(e => e.ccfpinfo);
            var leave = listLeave.Where(mis => mis.eid == id)
                                           .OrderByDescending(lr => lr.applev).Reverse()
                                           .AsQueryable()
                                           .ToList();
            return leave;
        }


        //
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HRleaveApprovalRequest>>> GetLeaveRequest(string id)
        {

            var listLeave = _context.leaveApprovalRequest.AsQueryable()
                .Include(e => e.ccflre)
                .Include(e => e.ccfpinfo);
            var leave = listLeave.Where(mis => mis.lreid == id)
                                           .OrderByDescending(lr => lr.applev).Reverse()
                                           .AsQueryable()
                                           .ToList();
            return leave;
        }

        //
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HRleaveApprovalRequest>>> GetAll(HRCustomerFilter filter)
        {
            var listEmployee = _context.leaveApprovalRequest.AsQueryable();


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
                    await _userRepository.SendNotificationUser(_leaveApproval.eid, user.bcode, "CCF HR System App", $"Leave reqest from {user.uname}.", requestLeave.eid, requestLeave.lreid, "L");
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
            var _leavereq = _context.leaveRequest.SingleOrDefault(e => e.lreid == _leaveApproval.lreid);

            _leavereq.statu = _leaveApproval.apstatu.ToString();

            await _context.SaveChangesAsync();

            _context.Entry(_leaveApproval).State = EntityState.Modified;

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

            var statusEdit = "pedding";

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
                statusEdit = "pedding";
            }

            if (_leaveApproval.apstatu == 3)
            {
                statusEdit = "return";
            }

            var requestLeave = _context.leaveRequest.SingleOrDefault(e => e.lreid == _leaveApproval.lreid);
            var user = _context.ccfUserClass.SingleOrDefault(e => e.uid == _leaveApproval.eid);
            await _userRepository.SendNotificationUser(requestLeave.eid, user.bcode, "CCF HR System App", $"Leave reqest have been {statusEdit} from {user.uname}.", requestLeave.eid, requestLeave.lreid, "L");
            var userApproval = _context.leaveApprovalRequest.Where(e => e.lreid == _leaveApproval.lreid)
                                .Where(e => e.eid != _leaveApproval.eid).AsQueryable();

            foreach (var userApprove in userApproval)
            {
                await _userRepository.SendNotificationUser(userApprove.eid, user.bcode, "CCF HR System App", $"Leave reqest have been {statusEdit} from {user.uname}.", requestLeave.eid, requestLeave.lreid, "L");

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
        //
    }
}
