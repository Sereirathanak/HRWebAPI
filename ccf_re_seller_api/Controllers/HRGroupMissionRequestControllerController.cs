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
    public class HRGroupMissionRequestController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRGroupMissionRequestController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet("detailrequest/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupMissionClass>>> GetRequestDetial(string id)
        {
            var reasechReason = _context.groupMissionRequest.Where(e => e.gmid == id)
                                .Include(e => e.groupMissionDetail)
                                .Include(e => e.groupMissionApprove)
                                .AsQueryable();
            return Ok(reasechReason);
        }
        //
        [HttpPost("detail/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupMissionApproveClass>>> GetDetilApp(string id, HRCustomerFilter filter)
        {
            var reasechReason = _context.groupMissionRequest.Where(e => e.gmid == filter.ucode)
                                        .Include(e => e.groupMissionDetail)
                                        .Include(e => e.groupMissionApprove)
                                        .Skip((filter.pageNumber - 1) * filter.pageSize)
                                        .Take(filter.pageSize)
                                        .AsQueryable()
                                        .ToList();
            return Ok(reasechReason);
        }
        //
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupMissionClass>>> GetDetil(string id, HRCustomerFilter filter)
        {
            var employee = _context.employee.SingleOrDefault(e => e.eid == id);

            var listGroupMission = _context.groupMissionRequest.AsQueryable();

            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            if (employee.elevel == 1)
            {
                listGroupMission.Include(e => e.groupMissionDetail)
               .Include(e => e.groupMissionApprove)
               .Include(e => e.ccforg)
               .Include(e => e.ccfmis).Where(mis => mis.todate == DOI);
            }

            if (employee.elevel == 2)
            {
                listGroupMission.Include(e => e.groupMissionDetail)
                .Include(e => e.groupMissionApprove)
                .Include(e => e.ccforg)
                .Include(e => e.ccfmis).Where(mis => mis.todate == DOI);
            }

            if (employee.elevel == 3)
            {
                listGroupMission.Include(e => e.groupMissionDetail)
                .Include(e => e.groupMissionApprove)
                .Include(e => e.ccforg)
                .Include(e => e.ccfmis).Where(mis => mis.todate == DOI);
            }
            var listGroupMissionDetail = _context.groupMissionDetailClass
                                           .AsQueryable()
                                           .Where(e => e.eid == id)
                                           .Include(e => e.groupMissionRequest)
                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                           .Take(filter.pageSize)
                                           .ToList();

            //

            if (filter.search != "" && filter.search != null)
            {


                var reasechReason = _context.groupMissionRequest.Where(e => e.reason.ToLower().Contains(filter.search));
                var reasechMissionName = _context.groupMissionRequest.Where(e => e.missu.ToLower().Contains(filter.search));

                //if (reasechReason != null && reasechReason.Count() > 0)
                //{
                //    listGroupMissionDetail = _context.groupMissionDetailClass.Where(mis => mis.eid == id)
                //                          .AsQueryable()
                //                          .Where(e => e.groupMissionRequest.reason.ToLower().Contains(filter.search.ToLower()))
                //                          .OrderByDescending(lr => lr.groupMissionRequest.createdate).Reverse()
                //                          .AsQueryable()
                //                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                //                          .Take(filter.pageSize)
                //                          .ToList();
                //    return Ok(listGroupMissionDetail);

                //}

                //if (reasechMissionName != null && reasechMissionName.Count() > 0)
                //{
                //    listGroupMissionDetail = _context.groupMissionDetailClass.Where(mis => mis.eid == id)
                //                          .AsQueryable()
                //                          .Where(e => e.groupMissionRequest.missu.ToLower().Contains(filter.search.ToLower()))
                //                          .OrderByDescending(lr => lr.groupMissionRequest.createdate).Reverse()
                //                          .AsQueryable()
                //                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                //                          .Take(filter.pageSize)
                //                          .ToList();
                //    return Ok(listGroupMissionDetail);

                //}
            }

            return Ok(listGroupMissionDetail);
        }




        //
        [HttpPost("creategroupmissionreqeust")]
        public async Task<ActionResult> Post([FromForm] ValidateGroupMiossionDocument _missionreq)
        {

            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_missionreq.orgid != null &&
                    _missionreq.misid != null &&
                    _missionreq.missu != null &&
                    _missionreq.dep != null &&
                    _missionreq.arr != null &&
                    _missionreq.mfor != null &&
                    _missionreq.lnot != null &&
                    _missionreq.overper != null &&
                    _missionreq.reason != null)
                {
                    //
                    var _idMissionRequest = "";
                    var id = _context.groupMissionRequest.Max(c => c.gmid);
                    int convertInt = 0;
                    if (id == null)
                    {
                        convertInt = 200000;
                    }
                    else
                    {
                        convertInt = int.Parse(id) + 1;

                    }

                    var GenerateID = convertInt.ToString();

                    //
                    if (HttpContext.Request.Form.Files.Count() > 0)
                    {


                        //var employeeRequest = _context.employee.SingleOrDefault(l => l.eid == _missionreq.eid);
                        //if (employeeRequest == null)
                        //{
                        //    IDictionary<string, string> errNotFound = new Dictionary<string, string>();
                        //    errNotFound.Add(new KeyValuePair<string, string>("000", $"Employee is not found."));
                        //    return BadRequest(errNotFound);
                        //}

                        string allowExtensions = ".jpg|.jpeg|.png|.gif";
                        string fileEx = "";
                        string mineType = "";
                        string fileName = "";
                        string errEduId = "";
                        string errEduIdBank = "";
                        string errEduIdselfie = "";



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
                                    var missionreq = new HRGroupMissionClass(_context)
                                    {
                                        gmid = GenerateID,
                                        orgid = _missionreq.orgid,
                                        misid = _missionreq.misid,
                                        missu = _missionreq.missu,
                                        frdate = _missionreq.frdate,
                                        todate = _missionreq.todate,
                                        dep = _missionreq.dep,
                                        arr = _missionreq.arr,
                                        mfor = _missionreq.mfor,
                                        lnot = _missionreq.lnot,
                                        overper = _missionreq.overper,
                                        reason = _missionreq.reason,
                                        remark = _missionreq.remark,
                                        createdate = DOI,
                                        statu = "2"
                                        
                                    };

                                    _context.groupMissionRequest.Add(missionreq);
                                    await _context.SaveChangesAsync();
                   

                                    if (_missionreq.groupMissionApprove != null && _missionreq.groupMissionApprove.Count() > 0)
                                    {
                                        var NextId = GetNextIDGroupMissionApprove();
                                        int intNextId = NextId;
                                        foreach (var detail in _missionreq.groupMissionApprove)
                                        {

                                            //// Create New Obj
                                            HRGroupMissionApproveClass groupApp = new HRGroupMissionApproveClass(_context);

                                            //// Initial Value
                                            groupApp.groupmisappid = GetNextIDGroupMissionApprove().ToString();
                                            groupApp.gmid = missionreq.gmid;
                                            groupApp.eid = detail.eid;
                                            groupApp.applev = detail.applev;
                                            groupApp.apstatu = detail.apstatu;
                                            groupApp.com = detail.com;
                                            groupApp.remark = detail.remark;
                                            groupApp.prio = detail.prio;



                                            groupApp.com = detail.com;
                                            groupApp.remark = detail.remark;

                                            //// Add to Transaction DB
                                            _context.groupMissionApprove.Add(groupApp);
                                            await _context.SaveChangesAsync();
                                        }
                                    }

                                    var idDocument = _context.groupmMssionRequestDocumentClassClass.Max(c => c.groupmisreqdocid);
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

                                    var missionreqDcument = new HRGroupmMssionRequestDocumentClassClass()
                                    {
                                        groupmisreqdocid = GenerateIDDcument.ToString(),
                                        groupmissionrequestid = missionreq.gmid,
                                        file = memoryStream.ToArray(),
                                    };

                                    _context.groupmMssionRequestDocumentClassClass.Add(missionreqDcument);
                                    await _context.SaveChangesAsync();
                                    _idMissionRequest = missionreq.gmid;


                                }

                                memoryStream.Close();
                                memoryStream.Dispose();
                            }
                        }

                    }
                    else
                    {
                        var missionreq = new HRGroupMissionClass(_context)
                        {
                            gmid = GenerateID.ToString(),
                            orgid = _missionreq.orgid,
                            misid = _missionreq.misid,
                            missu = _missionreq.missu,
                            frdate = _missionreq.frdate,
                            todate = _missionreq.todate,
                            dep = _missionreq.dep,
                            arr = _missionreq.arr,
                            mfor = _missionreq.mfor,
                            lnot = _missionreq.lnot,
                            overper = _missionreq.overper,
                            reason = _missionreq.reason,
                            remark = _missionreq.remark,
                            createdate = DOI,
                            statu = "2"
                        };

                        _context.groupMissionRequest.Add(missionreq);
                        await _context.SaveChangesAsync();
                        _idMissionRequest = missionreq.gmid;
                        //
                        if (_missionreq.groupMissionApprove != null && _missionreq.groupMissionApprove.Count() > 0)
                        {
                            var NextId = GetNextIDGroupMissionApprove();
                            int intNextId = NextId;
                            foreach (var detail in _missionreq.groupMissionApprove)
                            {

                                //// Create New Obj
                                HRGroupMissionApproveClass groupApp = new HRGroupMissionApproveClass(_context);

                                //// Initial Value
                                groupApp.groupmisappid = GetNextIDGroupMissionApprove().ToString();
                                groupApp.gmid = missionreq.gmid;
                                groupApp.eid = detail.eid;
                                groupApp.applev = detail.applev;
                                groupApp.apstatu = detail.apstatu;
                                groupApp.com = detail.com;
                                groupApp.remark = detail.remark;
                                groupApp.prio = detail.prio;

                                //// Add to Transaction DB
                                _context.groupMissionApprove.Add(groupApp);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    var groupRequestMission = _context.groupMissionRequest.SingleOrDefault(e => e.gmid == _idMissionRequest);
                    return Ok(groupRequestMission);
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
       

        public int GetNextIDGroupMissionApprove()
        {
            var groupMissionDetail = _context.groupMissionApprove.OrderByDescending(u => u.groupmisappid).FirstOrDefault();

            if (groupMissionDetail == null)
            {
                return 60000;
            }
            var nextId = int.Parse(groupMissionDetail.groupmisappid) + 1;
            return nextId;
        }

    }
}
