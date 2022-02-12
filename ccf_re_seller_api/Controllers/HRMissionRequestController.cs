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
    public class HRMissionRequestController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRMissionRequestController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HRMissionreq>>> GetDetil(string id)
        {
            var employee = _context.employee.SingleOrDefault(e => e.eid == id);
            var listMission = _context.missionreq.AsQueryable();
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            if (employee.elevel >=4)
            {
                listMission.Include(e => e.ccfpinfo)
               .Include(e => e.ccforg)
               .Include(e => e.ccfmis).Where(mis => mis.todate == DOI);
            }
        
            var mission = listMission.AsQueryable()
                                           .ToList();
            return Ok(mission);
        }
        //
        [HttpPost("createmissionreqeust")]
        public async Task<ActionResult> Post([FromForm] ValidateMiossionDocument _missionreq)
        {
            //return Ok(_missionreq);

            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_missionreq.orgid != null &&
                    _missionreq.eid != null &&
                    _missionreq.misid != null &&
                    _missionreq.missu != null &&
                    _missionreq.dep != null &&
                    _missionreq.arr != null &&
                    _missionreq.mfor != null &&
                    _missionreq.lnot != null &&
                    _missionreq.overper != null &&
                    _missionreq.reason != null)
                {
                    var _idMissionRequest = "";
                    if (HttpContext.Request.Form.Files.Count() > 0)
                    {


                        var employeeRequest = _context.employee.SingleOrDefault(l => l.eid == _missionreq.eid);
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

                        var id = _context.missionreq.Max(c => c.misnid);
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
                                    var missionreq = new HRMissionreq(_context)
                                    {
                                        misnid = GenerateID.ToString(),
                                        orgid = _missionreq.orgid,
                                        eid = _missionreq.eid,
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

                                    _context.missionreq.Add(missionreq);
                                    await _context.SaveChangesAsync();

                                    var idDocument = _context.missionRequestDocument.Max(c => c.misreqdocid);
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

                                    var missionreqDcument = new HRMissionRequestDocument()
                                    {
                                        misreqdocid = GenerateIDDcument.ToString(),
                                        eid = _missionreq.eid,
                                        missionrequestid = missionreq.misnid,
                                        file = memoryStream.ToArray(),
                                    };

                                    _context.missionRequestDocument.Add(missionreqDcument);
                                    await _context.SaveChangesAsync();
                                    _idMissionRequest = missionreq.misnid;

                                }

                                memoryStream.Close();
                                memoryStream.Dispose();
                            }
                        }
                    }
                    else
                    {
                        var missionreq = new HRMissionreq(_context)
                        {
                            //
                            misnid = GetLogNextID(),
                            orgid = _missionreq.orgid,
                            eid = _missionreq.eid,
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

                        _context.missionreq.Add(missionreq);
                        _idMissionRequest = missionreq.misnid;

                    }

                    await _context.SaveChangesAsync();
                    var requestMission =  _context.missionreq.SingleOrDefault(e => e.misnid == _idMissionRequest);
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
        [HttpPut("editmissionreq/{id}")]
        public async Task<IActionResult> Edit(string id, HRMissionreq _missionreq)
        {

            if (id != _missionreq.misnid)
            {
                return BadRequest();
            }

            _context.Entry(_missionreq).State = EntityState.Modified;

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

            return Ok(_missionreq);
        }
        //
        //
        private bool CcflogReExists(string id)
        {
            return _context.missionreq.Any(e => e.misnid == id);
        }
        //
        public string GetLogNextID()
        {
            var userLog = _context.missionreq.OrderByDescending(u => u.misnid).FirstOrDefault();

            if (userLog == null)
            {
                return "200000";
            }
            var nextId = int.Parse(userLog.misnid) + 1;
            return nextId.ToString();
        }
        //
    }
}
