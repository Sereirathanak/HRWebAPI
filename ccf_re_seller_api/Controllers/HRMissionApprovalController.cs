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
    public class HRMissionApprovalController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;
        private readonly HRRepository _userRepository;

        public HRMissionApprovalController(IConfiguration config, HRContext context, IWebHostEnvironment env)
        {
            _configuration = config;
            _context = context;
            _userRepository = new HRRepository(_context, env);
        }

        [HttpGet("approver/{id}")]
        public async Task<ActionResult<IEnumerable<HRMissionApproval>>> GetListApprove(string id)
        {

            var listMission = _context.missionApproval.AsQueryable();
            var mission = listMission.Where(mis => mis.eid == id)
                                           .OrderByDescending(lr => lr.applev).Reverse()
                                           .AsQueryable()
                                           .ToList();

            return Ok(mission);
        }


        //
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HRMissionApproval>>> GetMissionRequest(string id)
        {
            var listMission= _context.missionApproval.AsQueryable()
                .Include(e => e.ccfmissionreq)
                .Include(e => e.ccfpinfo);
            var mission = listMission.Where(mis => mis.misnid == id)
                                           .OrderByDescending(lr => lr.applev).Reverse()
                                           .AsQueryable()
                                           .ToList();
            return mission;
        }
        //
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HRMissionApproval>>> GetAll(HRCustomerFilter filter)
        {
            var listEmployee = _context.missionApproval.AsQueryable();
                //.Include(e =>e.ccfmissionreq)
                //.Include(e => e.ccfpinfo);

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
        [HttpPost("createmissionapproval")]

        public async Task<IActionResult> Create(HRMissionApproval _missionApproval)
        {
            try
            {
                if (_missionApproval.eid != null &&
                    _missionApproval.misnid != null &&
                    _missionApproval.applev != 0 &&
                    _missionApproval.prio != 0 &&
                    _missionApproval.apstatu != 0 
                   )
                {
                    _missionApproval.misdid = GetLogNextID();

                    _context.missionApproval.Add(_missionApproval);
                    await _context.SaveChangesAsync();
                    //
                    var requestMission = _context.missionreq.SingleOrDefault(e => e.misnid == _missionApproval.misnid);
                    var user = _context.ccfUserClass.SingleOrDefault(e => e.uid == requestMission.eid);
                    await _userRepository.SendNotificationUser(_missionApproval.eid, user.bcode, "CCF HR System App", $"Misson reqest from {user.uname}.", requestMission.eid, _missionApproval.misnid, "M");
                    //
                    return Ok(_missionApproval);
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
        [HttpPut("editmissionApproval/{id}")]
        public async Task<IActionResult> Edit(string id, HRMissionApproval _missionApproval)
        {

            if (id != _missionApproval.misdid)
            {
                return BadRequest();
            }

            var _missionreq = _context.missionreq.SingleOrDefault(e => e.misnid == _missionApproval.misnid);

            _missionreq.statu = _missionApproval.apstatu.ToString();

            await _context.SaveChangesAsync();

            _context.Entry(_missionApproval).State = EntityState.Modified;

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
            //
            var statusEdit = "pedding";

            if (_missionApproval.apstatu == 0)
            {
                statusEdit = "reject";
            }

            if (_missionApproval.apstatu == 1)
            {
                statusEdit = "approved";
            }

            if (_missionApproval.apstatu == 2)
            {
                statusEdit = "pedding";
            }

            if (_missionApproval.apstatu == 3)
            {
                statusEdit = "return";
            }

            var requestLeave = _context.missionreq.SingleOrDefault(e => e.misnid == _missionApproval.misnid);
            var user = _context.ccfUserClass.SingleOrDefault(e => e.uid == _missionApproval.eid);
            await _userRepository.SendNotificationUser(requestLeave.eid, user.bcode, "CCF HR System App", $"Mission reqest have been {statusEdit} from {user.uname}.", requestLeave.eid, requestLeave.misnid, "M");
            var userApproval = _context.missionApproval.Where(e => e.misnid == _missionApproval.misnid)
                                .Where(e => e.eid != _missionApproval.eid).AsQueryable();

            foreach (var userApprove in userApproval)
            {
                await _userRepository.SendNotificationUser(userApprove.eid, user.bcode, "CCF HR System App", $"Mission reqest have been {statusEdit} from {user.uname}.", requestLeave.eid, requestLeave.misnid, "M");

            }
            //

            return Ok(_missionApproval);
        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.missionApproval.Any(e => e.misdid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.missionApproval.OrderByDescending(u => u.misdid).FirstOrDefault();

            if (userLog == null)
            {
                return "200000";
            }
            var nextId = int.Parse(userLog.misdid) + 1;
            return nextId.ToString();
        }
        //
    }
}
