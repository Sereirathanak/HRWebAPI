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
    public class HROverTimeApprovalController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;
        private readonly HRRepository _userRepository;

        public HROverTimeApprovalController(IConfiguration config, HRContext context, IWebHostEnvironment env)
        {
            _configuration = config;
            _context = context;
            _userRepository = new HRRepository(_context, env);
        }

        [HttpPost("approver/{id}")]
        public async Task<ActionResult<IEnumerable<HROverTimeApproval>>> GetListApprove(string id, HRClockInOut filter)
        {

            var listOT = _context.overTimeApproval.AsQueryable()
                .Include(e => e.ccfove)
                .Include(e => e.ccfpinfo);
            var overtime = listOT.Where(mis => mis.eid == id)
                                           .OrderByDescending(lr => lr.applev)
                                           .AsQueryable()
                                           .ToList();
            if (filter.search != "")
            {

                List<Object> termsList = new List<Object>();


                var checkOTRequest = _context.overTimeRequest.Where(cur => cur.eid == filter.search);

                foreach (var i in checkOTRequest)
                {
                    var OTRequest = overtime.Where(lr => lr.oveid == i.oveid)
                                           .OrderByDescending(lr => lr.applev)
                                           .AsQueryable()
                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                           .Take(filter.pageSize)
                                           .ToList();


                    foreach (var listEmployee in OTRequest)
                    {
                        termsList.Add(listEmployee);
                    }
                }
                return Ok(termsList);
            }
            return overtime;
        }

        //id from over time request
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HROverTimeApproval>>> GetDetail(string id)
        {
            var listLeave = _context.overTimeApproval.AsQueryable()
                .Include(e => e.ccfove)
                .Include(e => e.ccfpinfo);
            var leave = listLeave.Where(mis => mis.oveid == id)
                                           .OrderByDescending(lr => lr.applev).Reverse()
                                           .AsQueryable()
                                           .ToList();
            return leave;
        }
        //
        //
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HROverTimeApproval>>> GetAll(HRCustomerFilter filter)
        {
            var listEmployee = _context.overTimeApproval.AsQueryable();
            //.Include(e =>e.ccfmissionreq)
            //.Include(e => e.ccfpinfo);

            int totallistEmployee = listEmployee.Count();
            var listEmployess = listEmployee.Where(lr => lr.eid == filter.eid)
                                               .OrderByDescending(lr => lr.applev)
                                               .AsQueryable()
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                               .Take(filter.pageSize)
                                               .ToList();

            return listEmployess;

        }
        //
        [HttpPost("createovertimeapproval")]

        public async Task<IActionResult> Create(HROverTimeApproval _overtimeApproval)
        {
            try
            {
                if (_overtimeApproval.eid != null &&
                    _overtimeApproval.oveid != null &&
                    _overtimeApproval.applev != 0 &&
                    _overtimeApproval.prio != 0 &&
                    _overtimeApproval.apstatu != 0
                   )
                {
                    _overtimeApproval.ovedid = GetLogNextID();

                    _context.overTimeApproval.Add(_overtimeApproval);
                    await _context.SaveChangesAsync();

                    //
                    var requestMission = _context.overTimeRequest.SingleOrDefault(e => e.oveid == _overtimeApproval.oveid);
                    var user = _context.ccfUserClass.SingleOrDefault(e => e.uid == requestMission.eid);
                    await _userRepository.SendNotificationUser(requestMission.eid, user.bcode, "CCF HR System App", $"Over-Time request from {user.uname}.", requestMission.eid, _overtimeApproval.oveid, "O");
                    //

                    return Ok(_overtimeApproval);
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
        [HttpPut("editovertimeapproval/{id}")]
        public async Task<IActionResult> Edit(string id, HROverTimeApproval _overtimeApproval)
        {

            if (id != _overtimeApproval.ovedid)
            {
                return BadRequest();
            }

            _context.Entry(_overtimeApproval).State = EntityState.Modified;

            var listApproveRequest = _context.overTimeApproval.Where(e => e.oveid == _overtimeApproval.oveid);


            var statusRequtest = 1;

            if (_overtimeApproval.applev != listApproveRequest.Count() - 2)
            {
                if (_overtimeApproval.apstatu != 3)
                {
                    statusRequtest = 2;
                }
            }

            if (_overtimeApproval.applev == 99)
            {
                statusRequtest = _overtimeApproval.apstatu;
            }

            if (_overtimeApproval.applev == 98)
            {
                statusRequtest = _overtimeApproval.apstatu;
            }


            var _overtimereq = _context.overTimeRequest.SingleOrDefault(e => e.oveid == _overtimeApproval.oveid);

            _overtimereq.statu = statusRequtest.ToString();

            //
            var statusEdit = "pedding";

            if (_overtimeApproval.apstatu == 0)
            {
                statusEdit = "reject";
            }

            if (_overtimeApproval.apstatu == 1)
            {
                statusEdit = "approved";
            }

            if (_overtimeApproval.apstatu == 2)
            {
                statusEdit = "pedding";
            }

            if (_overtimeApproval.apstatu == 3)
            {
                statusEdit = "return";
            }

            var requestLeave = _context.overTimeRequest.SingleOrDefault(e => e.oveid == _overtimeApproval.oveid);
            var user = _context.ccfUserClass.SingleOrDefault(e => e.uid == _overtimeApproval.eid);
            await _userRepository.SendNotificationUser(requestLeave.eid, user.bcode, "CCF HR System App", $"Over-Time request have been {statusEdit} from {user.uname}.", requestLeave.eid, requestLeave.oveid, "O");
            var userApproval = _context.overTimeApproval.Where(e => e.oveid == _overtimeApproval.oveid)
                                .Where(e => e.eid != _overtimeApproval.eid).ToList();

            foreach (var userApprove in userApproval)
            {
                await _userRepository.SendNotificationUser(userApprove.eid, user.bcode, "CCF HR System App", $"Over-Time request have been {statusEdit} from {user.uname}.", requestLeave.eid, requestLeave.oveid, "O");

            }
            //

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


            return Ok(_overtimeApproval);
        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.overTimeApproval.Any(e => e.ovedid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.overTimeApproval.OrderByDescending(u => u.ovedid).FirstOrDefault();

            if (userLog == null)
            {
                return "400000";
            }
            var nextId = int.Parse(userLog.ovedid) + 1;
            return nextId.ToString();
        }
        //
    }
}
