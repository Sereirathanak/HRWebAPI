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
    public class HRGroupOverTimeDetailController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRGroupOverTimeDetailController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        //
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupOverTimeDetail>>> GetDetil(string id, HRCustomerFilter filter)
        {


            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (filter.search != "" && filter.search != null)
            {

                var reasechReason = _context.groupOverTimeRequest.SingleOrDefault(e => e.reason.ToLower().Contains(filter.search.ToLower()));



                if (reasechReason != null)
                {

                    var employeeGroupMission = _context.groupOverTimeDetail
                                                      .OrderByDescending(lr => lr.createdate).Reverse()
                                                      .Where(e => e.groupoveid == reasechReason.groupoveid)
                                                      .Where(e => e.eid == id)
                                                      .Include(e => e.groupOverTimeRequest)
                                                      .AsQueryable()
                                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                      .Take(filter.pageSize)
                                                      .ToList();
                    return Ok(employeeGroupMission);
                }

               
            }
            else
            {
                var employeeGroupMission = _context.groupOverTimeDetail
                                                      .OrderByDescending(lr => lr.createdate).Reverse()
                                                      .Where(e => e.eid == id)
                                                      .Include(e => e.groupOverTimeRequest)
                                                      .AsQueryable()
                                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                      .Take(filter.pageSize)
                                                      .ToList();
                return Ok(employeeGroupMission);

            }
            return BadRequest();

        }

        //
        [HttpPost("creategroupovertimedetail")]
        public async Task<ActionResult> Post(HRGroupOverTimeDetail _overtimereq)
        {
            try
            {
                if (_overtimereq.groupoveid != null && _overtimereq.eid != null)
                {
                    var NextId = GetNextIDGroupOverTimeDetail();
                    int intNextId = NextId;
                    var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

                    _overtimereq.gdovertimedetailid = GetNextIDGroupOverTimeDetail().ToString();
                    _overtimereq.createdate = DOI;


                    //// Add to Transaction DB
                    _context.groupOverTimeDetail.Add(_overtimereq);
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
        public int GetNextIDGroupOverTimeDetail()
        {
            var groupOverTimeDetail = _context.groupOverTimeDetail.OrderByDescending(u => u.gdovertimedetailid).FirstOrDefault();

            if (groupOverTimeDetail == null)
            {
                return 70000;
            }
            var nextId = int.Parse(groupOverTimeDetail.gdovertimedetailid) + 1;
            return nextId;
        }
        //
    }
}
