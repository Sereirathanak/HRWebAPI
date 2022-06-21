using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Repositories;
using Microsoft.AspNetCore.Hosting;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRLeaveRequestController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;


        public HRLeaveRequestController(IConfiguration config, HRContext context, IWebHostEnvironment env)
        {
            _configuration = config;
            _context = context;

        }
        //
        [HttpPost("report")]
        public async Task<ActionResult<IEnumerable<HRLeaveRequest>>> FetchAllReportLeave(HRClockInOut filter)
        {
            var datetime = DateTime.Now.ToString("HH:mm");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "HH:mm", CultureInfo.GetCultureInfo("en-GB"));
            var employeees = _context.leaveRequest
                                 .OrderByDescending(x => x.frdat)
                                 .OrderByDescending(e => e.eid)
                                 .Include(e => e.ccfpinfo)
                                 .Include(e => e.ccflea)
                                 .Include(e => e.ccfpinfo.ccfemployeeJoinInfo)
                                 .AsQueryable()
                                 .ToList(); 

            var employeeName = employeees.Where(e => e.eid == filter.search);
            var employeeBranch = _context.employeeJoinInfo.Where(e => e.site == filter.branchClock);


            //filter branch request by page size unlimit
            if (filter.listall == true)
            {
                if (filter.branchClock != "" && employeeBranch.Count() > 0 && filter.sdate == "" && filter.edate == "")
                {
                    if (employeeBranch.Count() > 0)
                    {
                        var newEmployeees = employeees.Where(e => e.ccfpinfo.ccfemployeeJoinInfo.site == filter.branchClock)
                                                .ToList();
                        return Ok(newEmployeees);

                    }
                }

                if (filter.branchClock != "" && employeeBranch.Count() > 0 && filter.sdate != "" && filter.edate != "")
                {
                    if (employeeBranch.Count() > 0)
                    {
                        DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                        DateTime dateTo = DateTime.Parse(filter.edate.ToString());

                        var newEmployeees = employeees.Where(la => la.frdat >= dateFrom && la.todat <= dateTo)
                                                .Where(e => e.ccfpinfo.ccfemployeeJoinInfo.site == filter.branchClock)
                                                .ToList();
                        return Ok(newEmployeees);
                    }
                }

            }

            //filter branch request by page size limit
            if (filter.listall == false || filter.listall == null)
            {
                if (filter.branchClock != "" && employeeBranch.Count() > 0 && filter.sdate == "" && filter.edate == "")
                {
                    if (employeeBranch.Count() > 0)
                    {
                        var newEmployeees = employeees.Where(e => e.ccfpinfo.ccfemployeeJoinInfo.site == filter.branchClock)
                                               .Skip((filter.pageNumber - 1) * filter.pageSize)
                                              .Take(filter.pageSize)
                                              .ToList();

                        return Ok(newEmployeees);

                    }
                }

                if (filter.branchClock != "" && employeeBranch.Count() > 0 && filter.sdate != "" && filter.edate != "")
                {
                    if (employeeBranch.Count() > 0)
                    {
                        DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                        DateTime dateTo = DateTime.Parse(filter.edate.ToString());

                       var newEmployeees = employeees.Where(la => la.frdat >= dateFrom && la.todat <= dateTo)
                                                .Where(e => e.ccfpinfo.ccfemployeeJoinInfo.site == filter.branchClock)
                                                .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                .Take(filter.pageSize)
                                                .ToList();
                    
                        return Ok(newEmployeees);
                    }
                }

            }

            //filter by user request by page size unlimit
            if (filter.listall == true)
            {
                if (filter.search != "" && employeeName.Count() > 0 && filter.sdate != "" && filter.edate != "")
                {
                    if (employeeName.Count() > 0)
                    {
                        DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                        DateTime dateTo = DateTime.Parse(filter.edate.ToString());

                       var newEmployeees = employeees.Where(e => e.eid == filter.search)
                                                .Where(la => la.frdat >= dateFrom && la.todat <= dateTo)
                                                .ToList();

                        return Ok(newEmployeees);

                    }
                }

                if (filter.search != "" && employeeName.Count() > 0 && filter.sdate == "" && filter.edate == "")
                {
                    if (employeeName.Count() > 0)
                    {
                        var newEmployeees = employeees.Where(la => la.eid == filter.search)
                                                .ToList();

                        return Ok(newEmployeees);

                    }
                }
            }

            if (filter.listall == false || filter.listall == null)
            {
                if (filter.search != "" && employeeName.Count() > 0 && filter.sdate != "" && filter.edate != "")
                {
                    if (employeeName.Count() > 0)
                    {
                        DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                        DateTime dateTo = DateTime.Parse(filter.edate.ToString());

                        var newEmployeees = employeees.Where(e => e.eid == filter.search)
                                                .Where(la => la.frdat >= dateFrom && la.todat <= dateTo)
                                                .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                .Take(filter.pageSize)
                                                .ToList();

                        return Ok(newEmployeees);

                    }
                }

                if (filter.search != "" && employeeName.Count() > 0 && filter.sdate == "" && filter.edate == "")
                {
                    if (employeeName.Count() > 0)
                    {
                        var newEmployeees = employeees.Where(la => la.eid == filter.search)
                                                .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                .Take(filter.pageSize)
                                                .ToList();

                        return Ok(newEmployeees);

                    }
                }

            }


            if (filter.listall == true)
            {
                if (filter.sdate != "" && filter.edate != "" && filter.search == "")
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    var newEmployeees = employeees.Where(la => la.frdat >= dateFrom && la.todat <= dateTo)
                                            .ToList();
                    return Ok(newEmployeees);

                }
            }
            if (filter.listall == false || filter.listall == null)
            {
                if (filter.sdate != "" && filter.edate != "" && filter.search == "")
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    var newEmployeees = employeees.Where(la => la.frdat >= dateFrom && la.todat <= dateTo)
                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                            .Take(filter.pageSize)
                                            .ToList();
                    return Ok(newEmployeees);

                }
            }
            if (filter.listall == true)
            {
                if (filter.status != "")
                {
                    var newEmployeees = employeees.Where(e => e.statu == filter.status)
                                            .ToList();
                    return Ok(newEmployeees);

                }
            }


            if (filter.listall == false || filter.listall == null)
            {
                if (filter.status != "")
                {
                    var newEmployeees = employeees.Where(e => e.statu == filter.status)
                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                            .Take(filter.pageSize)
                                            .ToList();
                    return Ok(newEmployeees);

                }
            }


            if (filter.listall == true)
            {
                if (filter.sdate != "" && filter.search == "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    var newEmployeees = employeees.Where(la => la.frdat >= dateFrom && la.frdat <= dateTo)
                                            .ToList();
                    return Ok(newEmployeees);

                }
            }

            if (filter.listall == false || filter.listall == null)
            {
                if (filter.sdate != "" && filter.search == "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    var newEmployeees = employeees.Where(la => la.frdat >= dateFrom && la.frdat <= dateTo)
                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                            .Take(filter.pageSize)
                                            .ToList();
                    return Ok(newEmployeees);

                }
            }

            return BadRequest();
        }
        //
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<HRLeaveRequest>>> Get(string id, HRCustomerFilter filter)
        {
            var listLeave = _context.leaveRequest.AsQueryable()
                  .Include(e => e.ccfpinfo)
                  .Include(e => e.ccforg)
                  .Include(e => e.ccflea);
            ;
            var leave = listLeave.Where(mis => mis.eid == id)
                                           .AsQueryable()
                                           .OrderByDescending(lr => lr.createdate).Reverse()
                                           .AsQueryable()
                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                           .Take(filter.pageSize)
                                           .ToList();

            if (filter.search != "" && filter.search != null)
            {
                leave = listLeave.Where(mis => mis.eid == id)
                                           .AsQueryable()
                                           .Where(e => e.reason.ToLower().Contains(filter.search.ToLower()))
                                           .OrderByDescending(lr => lr.createdate).Reverse()
                                           .AsQueryable()
                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                           .Take(filter.pageSize)
                                           .ToList();
                return Ok(leave);

            }
            return Ok(leave);
        }
        //
        [HttpPost("createleaverequest")]
        public async Task<ActionResult> Post([FromForm] ValidateLeaveDocument _leaveRequest)
        {
            try
            {
                var datetime = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd", CultureInfo.GetCultureInfo("en-GB"));
                if (
                    _leaveRequest.numleav != null &&
                    _leaveRequest.lfor != null &&
                    _leaveRequest.lnot != null &&
                    _leaveRequest.reason != null &&
                    _leaveRequest.eid != null &&
                    _leaveRequest.orgid != null)

                {
                    var _idMissionRequest = "";

                    if (HttpContext.Request.Form.Files.Count() > 0)
                    {
                        var employeeRequest = _context.employee.SingleOrDefault(l => l.eid == _leaveRequest.eid);
                        if (employeeRequest == null)
                        {
                            IDictionary<string, string> errNotFound = new Dictionary<string, string>();
                            errNotFound.Add(new KeyValuePair<string, string>("000", $"Employee is not found."));
                            return BadRequest(errNotFound);
                        }
                        string allowExtensions = ".jpg|.jpeg|.png|.gif";
                        string fileEx = "";
                        string mineType = "";
                        string fileName = "";
                        string errEduId = "";
                        string errEduIdBank = "";
                        string errEduIdselfie = "";

                        var id = _context.leaveRequest.Max(c => c.lreid);
                        int convertInt = 0;
                        if (id == null)
                        {
                            convertInt = 600000;
                        }
                        else
                        {
                            convertInt = int.Parse(id) + 1;

                        }

                        var GenerateID = convertInt.ToString();
                        if (HttpContext.Request.Form.Files["file[101]"] != null)
                        {

                            fileName = HttpContext.Request.Form.Files["file[101]"].FileName;
                            mineType = HttpContext.Request.Form.Files["file[101]"].ContentType;
                            fileEx = Path.GetExtension(fileName);
                            //
                            using (var memoryStream = new MemoryStream())
                            {

                                await HttpContext.Request.Form.Files["file[101]"].CopyToAsync(memoryStream);

                                // Validate File Size 10M
                                if (memoryStream.Length > 10485760)
                                {
                                    errEduId = "The document size cannot bigger than 10M.";
                                }

                                if (!allowExtensions.Contains(fileEx))
                                {
                                    errEduId = "The document type is not allow.";
                                }

                                if (errEduId == "")
                                {
                                    //_leaveRequest.lreid = GetLogNextID();
                                    //_leaveRequest.file = memoryStream.ToArray();
                                    //_context.leaveRequest.Add(_leaveRequest);
                                    var leavereq = new HRLeaveRequest(_context)
                                    {
                                        lreid = GenerateID.ToString(),
                                        orgid = _leaveRequest.orgid,
                                        eid = _leaveRequest.eid,
                                        leaid = _leaveRequest.leaid,
                                        frdat = _leaveRequest.frdat,
                                        todat = _leaveRequest.todat,
                                        numleav = _leaveRequest.numleav,
                                        lfor = _leaveRequest.lfor,
                                        lnot = _leaveRequest.lnot,
                                        reason = _leaveRequest.reason,
                                        remark = _leaveRequest.remark,
                                        createdate = DOI,
                                        statu = "2"
                                    };

                                    _context.leaveRequest.Add(leavereq);
                                    await _context.SaveChangesAsync();

                                    var idDocument = _context.leaveRequestDocument.Max(c => c.leavereqdocid);
                                    int convertIntDocument = 0;
                                    if (idDocument == null)
                                    {
                                        convertIntDocument = 200000;
                                    }
                                    else
                                    {
                                        convertIntDocument = int.Parse(idDocument) + 1;

                                    }
                                    var GenerateIDDcument = convertIntDocument.ToString();
                                    var leavereqDcument = new HRLeaveRequestDocument()
                                    {
                                        leavereqdocid = GenerateIDDcument.ToString(),
                                        eid = leavereq.eid,
                                        leaverequestid = leavereq.lreid,
                                        file = memoryStream.ToArray(),
                                    };
                                    _context.leaveRequestDocument.Add(leavereqDcument);
                                    await _context.SaveChangesAsync();
                                    _idMissionRequest = leavereq.lreid;

                                }

                                memoryStream.Close();
                                memoryStream.Dispose();
                            }
                        }
                    }
                    else
                    {
                        var leavereq = new HRLeaveRequest(_context)
                        {
                            lreid = GetLogNextID(),
                            orgid = _leaveRequest.orgid,
                            eid = _leaveRequest.eid,
                            leaid = _leaveRequest.leaid,
                            frdat = _leaveRequest.frdat,
                            todat = _leaveRequest.todat,
                            numleav = _leaveRequest.numleav,
                            lfor = _leaveRequest.lfor,
                            lnot = _leaveRequest.lnot,
                            reason = _leaveRequest.reason,
                            remark = _leaveRequest.remark,
                            createdate = DOI,
                            statu = "2"
                        };

                        //_leaveRequest.lreid = GetLogNextID();
                        _context.leaveRequest.Add(leavereq);
                        _idMissionRequest = leavereq.lreid;


                    }

                    await _context.SaveChangesAsync();
                    var requestMission = _context.leaveRequest.SingleOrDefault(e => e.lreid == _idMissionRequest);
                  
                   
                    return Ok(requestMission);
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
        public string GetLogNextID()
        {
            var userLog = _context.leaveRequest.OrderByDescending(u => u.lreid).FirstOrDefault();

            if (userLog == null)
            {
                return "600000";
            }
            var nextId = int.Parse(userLog.lreid) + 1;
            return nextId.ToString();
        }
        //
    }
}
