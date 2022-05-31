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
    public class HRGroupOverTimeRequestController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRGroupOverTimeRequestController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet("detailrequest/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupOverTimeRequest>>> GetRequestDetial(string id)
        {
            var reasechReason = _context.groupOverTimeRequest.Where(e => e.groupoveid == id)
                                .Include(e => e.groupOverTimeDetail)
                                .Include(e => e.groupOverTimeApprove)
                                .AsQueryable();
            return Ok(reasechReason);
        }
        //
        [HttpPost("detail/{id}")]
        public async Task<ActionResult<IEnumerable<HRGroupOverTimeRequest>>> GetDetilApp(string id, HRCustomerFilter filter)
        {
            var reasechReason = _context.groupOverTimeRequest.Where(e => e.groupoveid == filter.ucode)
                                        .Include(e => e.groupOverTimeDetail)
                                        .Include(e => e.groupOverTimeApprove)
                                        .Skip((filter.pageNumber - 1) * filter.pageSize)
                                        .Take(filter.pageSize)
                                        .AsQueryable()
                                        .ToList();
            return Ok(reasechReason);
        }
        //


        //
        [HttpPost("creategroupovertimereqeust")]
        public async Task<ActionResult> Post([FromForm] ValidateGroupOverDocument _overTimeRequest)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_overTimeRequest.orgid != null &&
                    _overTimeRequest.ovtyid != null &&
                    _overTimeRequest.timin != null &&
                    _overTimeRequest.timout != null &&
                    _overTimeRequest.overper != null &&
                    _overTimeRequest.reason != null)
                {
                    var _idMissionRequest = "";
                    if (HttpContext.Request.Form.Files.Count() > 0)
                    {


                        string allowExtensions = ".jpg|.jpeg|.png|.gif";
                        string fileEx = "";
                        string mineType = "";
                        string fileName = "";
                        string errEduId = "";
                        string errEduIdBank = "";
                        string errEduIdselfie = "";

                        var id = _context.overTimeRequest.Max(c => c.oveid);
                        int convertInt = 0;
                        if (id == null)
                        {
                            convertInt = 800000;
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
                                    //_overTimeRequest.oveid = GetLogNextID();
                                    //_overTimeRequest.file = memoryStream.ToArray();
                                    //_context.overTimeRequest.Add(_overTimeRequest);
                                    var overtimereq = new HRGroupOverTimeRequest(_context)
                                    {
                                        groupoveid = GenerateID.ToString(),
                                        orgid = _overTimeRequest.orgid,
                                        ovtyid = _overTimeRequest.ovtyid,
                                        frdat = _overTimeRequest.frdate,
                                        todat = _overTimeRequest.todate,
                                        timin = _overTimeRequest.timin,
                                        timout = _overTimeRequest.timout,
                                        overper = _overTimeRequest.overper,
                                        reason = _overTimeRequest.reason,
                                        remark = _overTimeRequest.remark,
                                        createdate = DOI,
                                        statu = "2"
                                    };
                                    _context.groupOverTimeRequest.Add(overtimereq);
                                    await _context.SaveChangesAsync();

                                    var idDocument = _context.overtimeRequestDocument.Max(c => c.overtimereqdocid);
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
                                    var overtimereqDcument = new HRGroupOverTimeDocument()
                                    {
                                        groupovertimereqdocid = GenerateIDDcument.ToString(),
                                        overtimerequestid = overtimereq.groupoveid,
                                        file = memoryStream.ToArray(),
                                    };
                                    _context.groupOverTimeDocument.Add(overtimereqDcument);
                                    await _context.SaveChangesAsync();
                                    _idMissionRequest = overtimereq.groupoveid;
                                }

                                memoryStream.Close();
                                memoryStream.Dispose();
                            }
                        }
                    }
                    else
                    {
                        //_overTimeRequest.oveid = GetLogNextID();
                        var overtimereq = new HRGroupOverTimeRequest(_context)
                        {
                            groupoveid = GetLogNextID(),
                            orgid = _overTimeRequest.orgid,
                            ovtyid = _overTimeRequest.ovtyid,
                            frdat = _overTimeRequest.frdate,
                            todat = _overTimeRequest.todate,
                            timin = _overTimeRequest.timin,
                            timout = _overTimeRequest.timout,
                            overper = _overTimeRequest.overper,
                            reason = _overTimeRequest.reason,
                            remark = _overTimeRequest.remark,
                            createdate = DOI,
                            statu = "2"
                        };

                        _context.groupOverTimeRequest.Add(overtimereq);
                        _idMissionRequest = overtimereq.groupoveid;
                    }

                    await _context.SaveChangesAsync();
                    var requestOvertime = _context.groupOverTimeRequest.SingleOrDefault(e => e.groupoveid == _idMissionRequest);

                    return Ok(requestOvertime);
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
            var userLog = _context.groupOverTimeRequest.OrderByDescending(u => u.groupoveid).FirstOrDefault();

            if (userLog == null)
            {
                return "800000";
            }
            var nextId = int.Parse(userLog.groupoveid) + 1;
            return nextId.ToString();
        }
        //
    }
}
