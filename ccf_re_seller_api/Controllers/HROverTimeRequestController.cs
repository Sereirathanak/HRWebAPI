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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HROverTimeRequestController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HROverTimeRequestController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HROverTimeRequest>>> Get(string id)
        {
            var listOverTime = _context.overTimeRequest.AsQueryable()
                  .Include(e => e.ccfpinfo)
                  .Include(e => e.ccforg)
                  .Include(e => e.ccfovty);
            ;
            var overtime = listOverTime.Where(mis => mis.eid == id)
                                           .AsQueryable()
                                           .ToList();
            return Ok(overtime);
        }

        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HROverTimeRequest>>> GetCcfreferalCus()
        {
            return await _context.overTimeRequest.ToListAsync();
        }
        //
        [HttpPost("createovertimerequest")]
        public async Task<ActionResult> Post([FromForm] ValidateOverDocument _overTimeRequest)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_overTimeRequest.orgid != null &&
                    _overTimeRequest.eid != null &&
                    _overTimeRequest.ovtyid != null &&
                    _overTimeRequest.timin != null &&
                    _overTimeRequest.timout != null &&
                    _overTimeRequest.overper != null &&
                    _overTimeRequest.reason != null)
                {
                    var _idMissionRequest = "";
                    if (HttpContext.Request.Form.Files.Count() > 0)
                    {


                        var employeeRequest = _context.employee.SingleOrDefault(l => l.eid == _overTimeRequest.eid);
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
                                    var overtimereq = new HROverTimeRequest(_context)
                                    {
                                        oveid = GenerateID.ToString(),
                                        orgid = _overTimeRequest.orgid,
                                        eid = _overTimeRequest.eid,
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
                                    _context.overTimeRequest.Add(overtimereq);
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
                                    var overtimereqDcument = new HROvertimeRequestDocument()
                                    {
                                        overtimereqdocid = GenerateIDDcument.ToString(),
                                        eid = _overTimeRequest.eid,
                                        overtimerequestid = overtimereq.oveid,
                                        file = memoryStream.ToArray(),
                                    };
                                    _context.overtimeRequestDocument.Add(overtimereqDcument);
                                    await _context.SaveChangesAsync();
                                    _idMissionRequest = overtimereq.oveid;
                                }

                                memoryStream.Close();
                                memoryStream.Dispose();
                            }
                        }
                    }
                    else
                    {
                        //_overTimeRequest.oveid = GetLogNextID();
                        var overtimereq = new HROverTimeRequest(_context)
                        {
                            oveid = GetLogNextID(),
                            orgid = _overTimeRequest.orgid,
                            eid = _overTimeRequest.eid,
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

                        _context.overTimeRequest.Add(overtimereq);
                        _idMissionRequest = overtimereq.oveid;
                    }

                    await _context.SaveChangesAsync();
                    var requestOvertime = _context.overTimeRequest.SingleOrDefault(e => e.oveid == _idMissionRequest);

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
        [HttpPut("editovertimerequest/{id}")]
        public async Task<IActionResult> Edit(string id, HROverTimeRequest _overTimeRequest)
        {

            if (id != _overTimeRequest.oveid)
            {
                return BadRequest();
            }

            _context.Entry(_overTimeRequest).State = EntityState.Modified;

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

            return Ok(_overTimeRequest);
        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.overTimeRequest.Any(e => e.oveid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.overTimeRequest.OrderByDescending(u => u.oveid).FirstOrDefault();

            if (userLog == null)
            {
                return "800000";
            }
            var nextId = int.Parse(userLog.oveid) + 1;
            return nextId.ToString();
        }
        //
    }
}
