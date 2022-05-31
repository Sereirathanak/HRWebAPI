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
    public class HRGroupOverTimeApprovalController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;
        private readonly HRRepository _userRepository;


        public HRGroupOverTimeApprovalController(IConfiguration config, HRContext context, IWebHostEnvironment env)
        {
            _configuration = config;
            _context = context;
            _userRepository = new HRRepository(_context, env);

        }
        //
        //id = groupmisappid
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupOverTimeApprove>>> OnApproval(string id, HRCustomerFilter filter)
        {
            try
            {
                var getUser = _context.groupOverTimeApprove.SingleOrDefault(e => e.groupoveapp == id);
                getUser.apstatu = int.Parse(filter.status);


                var listApproveRequest = _context.groupOverTimeApprove.Where(e => e.groupoveid == getUser.groupoveid);

                var statusRequtest = 1;

                if (getUser.applev != listApproveRequest.Count() - 2)
                {
                    if (getUser.apstatu != 3)
                    {
                        statusRequtest = 2;
                    }
                }

                if (getUser.applev == 99)
                {
                    statusRequtest = int.Parse(filter.status);
                }

                if (getUser.applev == 98)
                {
                    statusRequtest = int.Parse(filter.status);
                }


                var _missionreq = _context.groupOverTimeRequest.SingleOrDefault(e => e.groupoveid == getUser.groupoveid);
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
                var requestDetail = _context.groupOverTimeDetail.Where(e => e.groupoveid == getUser.groupoveid).ToList();
                var user = _context.employee.SingleOrDefault(e => e.eid == getUser.eid);

                foreach (var userRequestMission in requestDetail)
                {
                    await _userRepository.SendNotificationUser(userRequestMission.eid, "", "CCF HR System App", $"Group Mission request have been {statusEdit} from {user.dname}.", userRequestMission.eid, userRequestMission.groupoveid, "O");
                }
                var userApproval = _context.groupOverTimeApprove.Where(e => e.groupoveid == getUser.groupoveid)
                               .Where(e => e.eid != getUser.eid).ToList();
                foreach (var userApprove in userApproval)
                {
                    await _userRepository.SendNotificationUser(userApprove.eid, "", "CCF HR System App", $"Mission request have been {statusEdit} from {user.dname}.", getUser.eid, getUser.groupoveid, "O");

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
        private bool CcflogReExists(string id)
        {
            return _context.groupOverTimeApprove.Any(e => e.groupoveid == id);
        }
        //
        [HttpPost("detailapproval/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupOverTimeApprove>>> GetDetilApp(string id, HRCustomerFilter filter)
        {
            var reasechReason = _context.groupOverTimeApprove.Where(e => e.groupoveid == filter.ucode)
                                        .Include(e => e.ccfpinfo)
                                        .Include(e => e.ccfgroupovereq)
                                        .OrderByDescending(lr => lr.applev).Reverse()
                                        .ToList();
            return reasechReason;
        }
        //
        [HttpPost("approver/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupOverTimeApprove>>> GetListApprove(string id, HRCustomerFilter filter)
        {

            var listMission = _context.groupOverTimeApprove.Include(e => e.ccfgroupovereq.groupOverTimeDetail)
                                            .AsQueryable();

            var mission = listMission.Where(mis => mis.eid == id)
                                           .OrderByDescending(lr => lr.applev).Reverse()
                                           .AsQueryable()
                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                           .Take(filter.pageSize)
                                           .ToList();


            if (filter.search != "" && filter.search != null)
            {
                var reasechReason = _context.groupOverTimeRequest.SingleOrDefault(e => e.reason.ToLower().Contains(filter.search.ToLower()));


                if (reasechReason != null)
                {
                    mission = mission.Where(gm => gm.groupoveid == reasechReason.groupoveid)
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
                return Ok(mission);
            }
            return BadRequest();

        }
        //
        [HttpPost("creategroupovertimeapproval")]
        public async Task<ActionResult> Post(HRGroupOverTimeApprove _overtimereq)
        {
            try
            {
                var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
                //
                if (_overtimereq.groupoveid != null && _overtimereq.eid != null)
                {
                    _overtimereq.groupoveapp = GetNextIDGroupMissionApprove().ToString();
                    _overtimereq.createdate = DOI;
                    //// Add to Transaction DB
                    _context.groupOverTimeApprove.Add(_overtimereq);
                    await _context.SaveChangesAsync();
                    return Ok(_overtimereq);
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
            var groupMissionDetail = _context.groupOverTimeApprove.OrderByDescending(u => u.groupoveapp).FirstOrDefault();

            if (groupMissionDetail == null)
            {
                return 600000;
            }
            var nextId = int.Parse(groupMissionDetail.groupoveapp) + 1;
            return nextId;
        }
        //
    }
}
