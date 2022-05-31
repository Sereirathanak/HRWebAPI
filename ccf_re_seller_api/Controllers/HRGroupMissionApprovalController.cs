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
    public class HRGroupMissionApprovalController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;
        private readonly HRRepository _userRepository;


        public HRGroupMissionApprovalController(IConfiguration config, HRContext context, IWebHostEnvironment env)
        {
            _configuration = config;
            _context = context;
            _userRepository = new HRRepository(_context, env);

        }
        //id = groupmisappid
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupMissionApproveClass>>> OnApproval(string id, HRCustomerFilter filter)
        {
            try
            {
                var getUser = _context.groupMissionApprove.SingleOrDefault(e => e.groupmisappid == id);
                getUser.apstatu = int.Parse(filter.status);


                var listApproveRequest = _context.groupMissionApprove.Where(e => e.gmid == getUser.gmid);

                var statusRequtest = 1;

                if (getUser.applev != listApproveRequest.Count() - 2)
                {

                    if (getUser.apstatu != 3)
                    {
                        statusRequtest = 2;
                    }
                }

                if (getUser.applev == 99) {
                    statusRequtest = int.Parse(filter.status);
                }

                if (getUser.applev == 98)
                {
                    statusRequtest = int.Parse(filter.status);
                }

                var _missionreq = _context.groupMissionRequest.SingleOrDefault(e => e.gmid == getUser.gmid);
                _missionreq.statu = statusRequtest.ToString();

                //
                var statusEdit = "pedding";

                if (int.Parse(filter.status) == 0)
                {
                    statusEdit = "reject";
                }

                if (int.Parse(filter.status) == 1)
                {
                    statusEdit = "approved";
                }

                if (int.Parse(filter.status) == 2)
                {
                    statusEdit = "pedding";
                }

                if (int.Parse(filter.status) == 3)
                {
                    statusEdit = "return";
                }
                var requestDetail = _context.groupMissionDetailClass.Where(e => e.gmid == getUser.gmid).ToList();
                var user = _context.employee.SingleOrDefault(e => e.eid == getUser.eid);

                foreach (var userRequestMission in requestDetail)
                {
                    await _userRepository.SendNotificationUser(userRequestMission.eid, "", "CCF HR System App", $"Group Mission request have been {statusEdit} from {user.dname}.", userRequestMission.eid, userRequestMission.gmid, "M");
                }
                var userApproval = _context.groupMissionApprove.Where(e => e.gmid == getUser.gmid)
                               .Where(e => e.eid != getUser.eid).ToList();
                foreach (var userApprove in userApproval)
                {
                    await _userRepository.SendNotificationUser(userApprove.eid, "", "CCF HR System App", $"Mission request have been {statusEdit} from {user.dname}.", getUser.eid, getUser.gmid, "M");

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
                return Ok(getUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.groupMissionApprove.Any(e => e.groupmisappid == id);
        }
        //
        [HttpPost("detailapproval/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupMissionApproveClass>>> GetDetilApp(string id, HRCustomerFilter filter)
        {
            var reasechReason = _context.groupMissionApprove.Where(e => e.gmid == filter.ucode)
                                        .Include(e => e.ccfpinfo)
                                        .Include(e => e.ccfgroupMissionRequest)
                                        .OrderByDescending(lr => lr.applev).Reverse()
                                        .ToList();
            return reasechReason;
        }
        //
        [HttpPost("approver/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupMissionApproveClass>>> GetListApprove(string id, HRCustomerFilter filter)
        {

            var listMission = _context.groupMissionApprove.Include(e => e.ccfgroupMissionRequest.groupMissionDetail)
                                            .AsQueryable();

            var mission = listMission.Where(mis => mis.eid == id)
                                           .OrderByDescending(lr => lr.applev).Reverse()
                                           .AsQueryable()
                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                           .Take(filter.pageSize)
                                           .ToList();


            if (filter.search != "" && filter.search != null)
            {
                var reasechReason = _context.groupMissionRequest.SingleOrDefault(e => e.reason.ToLower().Contains(filter.search.ToLower()));

                var reasechMissionName = _context.groupMissionRequest.Where(e => e.missu.ToLower().Contains(filter.search.ToLower()));

                if (reasechReason != null)
                {
                    mission = mission.Where(gm => gm.gmid == reasechReason.gmid)
                                          .Where(mis => mis.eid == id)
                                          .AsQueryable()
                                          .OrderByDescending(lr => lr.createdate).Reverse()
                                          .AsQueryable()
                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                          .Take(filter.pageSize)
                                          .ToList();
                    return Ok(mission);

                }

                if (reasechMissionName != null)
                {
                    mission = mission.Where(gm => gm.gmid == reasechReason.gmid)
                                          .Where(mis => mis.eid == id)
                                          .AsQueryable()
                                          .OrderByDescending(lr => lr.createdate).Reverse()
                                          .AsQueryable()
                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                          .Take(filter.pageSize)
                                          .ToList();
                    return Ok(mission);

                }
            }
            else
            {
                int totalListReferalCus = mission.Count();
                return Ok(mission);
            }
            return BadRequest();

        }
        //
        [HttpPost("creategroupmissionapproval")]
        public async Task<ActionResult> Post(HRGroupMissionApproveClass _missionreq)
        {

            try
            {
                //
                if (_missionreq.gmid != null && _missionreq.eid != null)
                {

                    var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
                    _missionreq.groupmisappid = GetNextIDGroupMissionApprove().ToString();

                    _missionreq.createdate = DOI;

                    //// Add to Transaction DB
                    _context.groupMissionApprove.Add(_missionreq);
                    await _context.SaveChangesAsync();
                    return Ok(_missionreq);
                }
                else
                {
                    return BadRequest();

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        //

        public int GetNextIDGroupMissionApprove()
        {
            var groupMissionDetail = _context.groupMissionApprove.OrderByDescending(u => u.groupmisappid).FirstOrDefault();

            if (groupMissionDetail == null)
            {
                return 600000;
            }
            var nextId = int.Parse(groupMissionDetail.groupmisappid) + 1;
            return nextId;
        }
    }
}
