using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRGroupDetailController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;
        
        public HRGroupDetailController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        
        //
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupMissionDetailClass>>> GetDetil(string id, HRCustomerFilter filter)
        {


            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (filter.search != "" && filter.search != null)
            {

                var reasechReason =  _context.groupMissionRequest.SingleOrDefault(e => e.reason.ToLower().Contains(filter.search.ToLower()));

               
                var reasechMissionName = _context.groupMissionRequest.Where(e => e.missu.ToLower().Contains(filter.search))
                                           .OrderByDescending(lr => lr.createdate).Reverse()
                                          .AsQueryable()
                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                          .Take(filter.pageSize)
                                          .ToList();

                if (reasechReason != null)
                {

                    var employeeGroupMission = _context.groupMissionDetailClass
                                                      .OrderByDescending(lr => lr.createdate).Reverse()
                                                      .Where(e => e.gmid == reasechReason.gmid)
                                                      .Where(e => e.eid == id)
                                                      .Include(e => e.groupMissionRequest)
                                                      .AsQueryable()
                                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                      .Take(filter.pageSize)
                                                      .ToList();
                    return Ok(employeeGroupMission);
                }

                if (reasechMissionName != null && reasechMissionName.Count() > 0)
                {
                    return Ok(reasechMissionName);
                }
            }
            else
            {
                var employeeGroupMission = _context.groupMissionDetailClass
                                                      .OrderByDescending(lr => lr.createdate).Reverse()
                                                      .Where(e => e.eid == id)
                                                      .Include(e => e.groupMissionRequest)
                                                      .AsQueryable()
                                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                      .Take(filter.pageSize)
                                                      .ToList();
                return Ok(employeeGroupMission);

            }
            return BadRequest();

        }
        //
        [HttpPost("creategroupmissiondetail")]
        public async Task<ActionResult> Post( HRGroupMissionDetailClass _missionreq)
        {

            try
            {
                if (_missionreq.gmid != null && _missionreq.eid != null)
                {
                    var NextId = GetNextIDGroupMissionDetail();
                    int intNextId = NextId;
                    var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
                    _missionreq.createdate = DOI;
                    _missionreq.gdid = GetNextIDGroupMissionDetail().ToString();
                 

                    //// Add to Transaction DB
                    _context.groupMissionDetailClass.Add(_missionreq);
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
        //
        public int GetNextIDGroupMissionDetail()
        {
            var groupMissionDetail = _context.groupMissionDetailClass.OrderByDescending(u => u.gdid).FirstOrDefault();

            if (groupMissionDetail == null)
            {
                return 70000;
            }
            var nextId = int.Parse(groupMissionDetail.gdid) + 1;
            return nextId;
        }
        //

    }
}
