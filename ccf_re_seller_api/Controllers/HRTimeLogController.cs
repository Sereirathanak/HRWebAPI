using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ccf_re_seller_api.Modals;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Data;
using System.Diagnostics;
using Npgsql;
using System.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRTimeLogController : Controller
    {

        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRTimeLogController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        public DateTime ConverterDateTimeString(string convertdatetime)
        {
            DateTime DT = DateTime.Parse(convertdatetime.ToString());
            DateTime DTT = DateTime.Parse(DT.ToString("yyyy-MM-dd"));

            return DTT;
        }


        [HttpPost("autoclockin")]
        public async Task<ActionResult<IEnumerable<HRTimeLogClass>>> PostAutoClockIN(HRClockInOut filter)
        {


            var userLog = _context.timeLogClass
                                    .Where(e => e.tdate == DateTime.Parse(filter.sdate) && e.sta == "In")
                                    .OrderByDescending(lr => lr.tdate)
                                    .OrderByDescending(lr => lr.tim)
                                    .OrderByDescending(lr => lr.eid)
                                    .ToList();

            var datetimeLog = DateTime.Now.ToString("HH:mm:ss");

            //if (filter.timeClock != "" && filter.timeClock != null)
            //{
            //    datetimeLog = filter.timeClock;
            //}

            List<string> termsList = new List<string>();
            var employeees = _context.employee.ToList();
            foreach (var item1 in userLog)
            {
                employeees = employeees.Where(e => e.eid != item1.eid).ToList();
            }
            foreach (var insert in employeees)
            {
                termsList.Add(insert.eid);

            }
            foreach (var insert in termsList)
            {
                var joiningInfor = _context.employeeJoinInfo.SingleOrDefault(e => e.eid == insert);
                try
                {

                    HRTimeLogClass timeLog = new HRTimeLogClass(_context);
                    timeLog.timid = GetLogNextID();
                    timeLog.eid = insert;
                    timeLog.tdate = DateTime.Parse(filter.sdate);
                    timeLog.tim = datetimeLog.ToString();
                    timeLog.braid = joiningInfor.site;
                    timeLog.sta = "Auto Clock In";
                    timeLog.cty = "Auto Clock In";
                    _context.timeLogClass.Add(timeLog);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message.ToString());
                }
            }
            return Ok(termsList);
        }


        [HttpPost("autoclockout")]
        public async Task<ActionResult<IEnumerable<HRTimeLogClass>>> PostAutoClock(HRClockInOut filter)
        {

            var userLog =  _context.timeLogClass
                                    .Where(e => e.tdate == DateTime.Parse(filter.sdate) && e.sta == "Out")
                                    .OrderByDescending(lr => lr.tdate)
                                    .OrderByDescending(lr => lr.tim)
                                    .OrderByDescending(lr => lr.eid)
                                    .ToList();
          
            var datetimeLog = DateTime.Now.ToString("HH:mm:ss");
            //if (filter.timeClock != "" && filter.timeClock != null)
            //{
            //    datetimeLog = filter.timeClock;
            //}
            List<string> termsList = new List<string>();
            var employeees = _context.employee.ToList();
            foreach (var item1 in userLog)
            {
                employeees = employeees.Where(e => e.eid != item1.eid).ToList();
            }
            foreach (var insert in employeees)
            {
                termsList.Add(insert.eid);

            }
            foreach (var insert in termsList)
            {
                var joiningInfor = _context.employeeJoinInfo.SingleOrDefault(e => e.eid == insert);
                try
                {

                    HRTimeLogClass timeLog = new HRTimeLogClass(_context);
                    timeLog.timid = GetLogNextID();
                    timeLog.eid = insert;
                    timeLog.tdate = DateTime.Parse(filter.sdate);
                    timeLog.tim = datetimeLog.ToString();
                    timeLog.braid = joiningInfor.site;
                    timeLog.sta = "Auto Clock Out";
                    timeLog.cty = "Auto Clock Out";
                    _context.timeLogClass.Add(timeLog);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message.ToString());
                }
            }
            return Ok(termsList);
        }

        [HttpPost("report")]
        public async Task<ActionResult<IEnumerable<HRTimeLogClass>>> FetchAllReport(HRClockInOut filter)
        {
            var employeees = _context.timeLogClass
                                 .OrderByDescending(x => x.tdate).Reverse()
                                 .OrderByDescending(e => e.eid)
                                 .OrderByDescending(e => e.tim)
                                 .Include(e => e.ccfpinfo)
                                 .AsQueryable();

            if (filter.status != null && filter.status != "")
            {
                employeees = employeees.Where(e => e.sta == filter.status);
            }

            if (filter.eid != null && filter.eid != "")
            {
                employeees = employeees.Where(e => e.eid == filter.eid);
            }


            if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
            {
                DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                employeees = employeees.Where(la => la.tdate >= dateFrom && la.tdate <= dateTo);
            }
            else if (filter.sdate != null && filter.sdate != "")
            {
                var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                employeees = employeees.Where(la => la.tdate >= dateFrom && la.tdate <= dateTo);
            }


            return Ok(employeees);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<HRTimeLogClass>>> GetAll(HRClockInOut filter)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd", CultureInfo.GetCultureInfo("en-GB"));
            var employeees = _context.timeLogClass
                                    .OrderByDescending(x => x.tim)
                                    .AsQueryable();

            if (filter.level != null && filter.level <= 3)
            {
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    employeees = employeees.Where(cu => cu.tdate >= dateFrom && cu.tdate <= dateTo);
                }
                else 
                {
                    employeees = _context.timeLogClass
                                    .OrderByDescending(x => x.tim)
                                    .AsQueryable();
                }

                if (filter.branchClock != null && filter.branchClock != "")
                {
                    employeees = employeees.Where(cu => cu.braid == filter.branchClock.ToString());
                }

                if (filter.level != null && filter.level < 5)
                {
                    employeees = employeees.Where(lr => lr.eid == filter.eid);
                }
                else
                {
                    employeees = employeees.Where(lr => lr.tim == filter.eid);
                }
            }
            else
            {
                //employeees = _context.timeLogClass.AsQueryable();
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    employeees = employeees.Where(cu => cu.tdate >= dateFrom && cu.tdate <= dateTo);
                }
                else
                {
                    employeees = _context.timeLogClass
                                    .OrderByDescending(x => x.tim)
                                    .AsQueryable();
                }

                if (filter.branchClock != null && filter.branchClock != "")
                {
                    employeees = employeees.Where(cu => cu.braid == filter.branchClock.ToString());
                }

                if (filter.eid != null)
                {
                    employeees = employeees.Where(lr => lr.eid == filter.eid);
                }

            }

            return employeees.ToList();
           
        }

        //Get All List Referer
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<HRTimeLogClass>>> GetTimeClock(HRClockInOut filter)
        {
            var listEmployeeClock = _context.timeLogClass
                                    .Include(e => e.ccfpinfo)
                                    .AsQueryable()
                                    .ToList();

            var employeeEcard = _context.employee.SingleOrDefault(e => e.ecard == filter.ecard);
            var employeeName = _context.employee.Where(e => e.eid == filter.search);
            var employeeBranch = listEmployeeClock.Where(e => e.braid == filter.branchClock);

            if (filter.ecard != "")
            {

                List<Object> termsList = new List<Object>();

                var datetime = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime DT = DateTime.Parse(datetime.ToString());
                DateTime DTT = DateTime.Parse(DT.ToString("yyyy-MM-dd"));
                var checkLeaveRequest = _context.leaveRequest.Where(cur => cur.eid == filter.search);

                var newEcardSearch = listEmployeeClock
                            .Where(e => e.eid == employeeEcard.eid)
                            .Where(e => e.tdate == DTT)
                            .OrderByDescending(lr => lr.tim)
                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                            .Take(filter.pageSize)
                            .ToList();


                return Ok(newEcardSearch);
            }

            if (employeeBranch.Count() > 0 && filter.branchClock != "" && filter.sdate != "" && filter.edate != "")
            {
                DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                if (filter.listall == true)
                {
                    if (employeeBranch.Count() > 0)
                    {
                        var listClocked = listEmployeeClock
                                      .Where(e => e.braid == filter.branchClock)
                                      .Where(e => e.tdate >= dateFrom && e.tdate <= dateTo)
                                      .OrderByDescending(lr => lr.tdate)
                                      .ToList();

                        return Ok(listClocked);
                    }
                }

                if (filter.listall == false || filter.listall == null)
                {
                    if (employeeBranch.Count() > 0)
                    {
                        var listClocked = listEmployeeClock
                                      .Where(e => e.braid == filter.branchClock)
                                      .Where(e => e.tdate >= dateFrom && e.tdate <= dateTo)
                                      .OrderByDescending(lr => lr.tdate)
                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                      .Take(filter.pageSize)
                                      .ToList();

                        return Ok(listClocked);
                    }
                }
            }

            if (employeeName.Count() > 0 && filter.search != "" && filter.sdate != "" && filter.edate != "")
            {
                DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                List<Object> termsList = new List<Object>();

                if (employeeName.Count() > 0)
                {
                    if (filter.listall == true)
                    {
                        var listClocked = listEmployeeClock
                                                         .Where(e => e.eid == filter.search)
                                                         .Where(e => e.tdate >= dateFrom && e.tdate <= dateTo)
                                                         .OrderByDescending(lr => lr.tdate)
                                                         .ToList();
                        return Ok(listClocked);

                    }

                    if (filter.listall == false || filter.listall == null)
                    {
                        var listClocked = listEmployeeClock
                                                         .Where(e => e.eid == filter.search)
                                                         .Where(e => e.tdate >= dateFrom && e.tdate <= dateTo)
                                                         .OrderByDescending(lr => lr.tdate)
                                                         .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                         .Take(filter.pageSize)
                                                         .ToList();
                        return Ok(listClocked);

                    }
                }
            }

            if (filter.search == null)
            {
                var datetime = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime DT = DateTime.Parse(datetime.ToString());
                DateTime DTT = DateTime.Parse(DT.ToString("yyyy-MM-dd"));

                var newList = listEmployeeClock
                                        .Where(e => e.tdate == DTT)
                                        .OrderByDescending(lr => lr.tim)
                                        .Skip((filter.pageNumber - 1) * filter.pageSize)
                                        .Take(filter.pageSize)
                                        .ToList();

                return Ok(newList);
            }
            if (filter.listall == true)
            {
                if ((filter.sdate != "") && (filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    var newList = listEmployeeClock
                                       .Where(e => e.tdate >= dateFrom && e.tdate <= dateTo)
                                       .OrderByDescending(lr => lr.tdate)
                                       .ToList();

                    return Ok(newList);
                }
            }
            if (filter.listall == false || filter.listall == null)
            {
                if ((filter.sdate != "") && (filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    var newList = listEmployeeClock
                                       .Where(e => e.tdate >= dateFrom && e.tdate <= dateTo)
                                       .OrderByDescending(lr => lr.tdate)
                                       .Skip((filter.pageNumber - 1) * filter.pageSize)
                                       .Take(filter.pageSize)
                                       .ToList();

                    return Ok(newList);
                }
            }

            if (filter.listall == true)
            {
                if (filter.edate == "" && filter.sdate != "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    var newList = listEmployeeClock
                                      .Where(e => e.tdate >= dateFrom && e.tdate <= dateTo)
                                      .OrderByDescending(lr => lr.tdate)
                                      .ToList();

                    return Ok(newList);
                }
            }
            if (filter.listall == false || filter.listall == null)
            {
                if (filter.edate == "" && filter.sdate != "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    var newList = listEmployeeClock
                                      .Where(e => e.tdate >= dateFrom && e.tdate <= dateTo)
                                      .OrderByDescending(lr => lr.tdate)
                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                      .Take(filter.pageSize)
                                      .ToList();

                    return Ok(newList);


                }
            }
            if (filter.listall == true)
            {
                if (filter.branchClock != "")
                {
                    if (employeeBranch != null && employeeBranch.Count() > 0)
                    {

                        var newList = listEmployeeClock
                                            .Where(e => e.braid == filter.branchClock)
                                            .OrderByDescending(lr => lr.tdate)
                                            .ToList();
                        return Ok(newList);



                    }
                }
            }

            if (filter.listall == false || filter.listall == null)
            {
                if (filter.branchClock != "")
                {
                    if (employeeBranch != null && employeeBranch.Count() > 0)
                    {

                        var newList = listEmployeeClock
                                            .Where(e => e.braid == filter.branchClock)
                                            .OrderByDescending(lr => lr.tdate)
                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                            .Take(filter.pageSize)
                                            .ToList();
                        return Ok(newList);



                    }
                }
            }

            if (filter.listall == true)
            {
                if (filter.search != "")
                {
                    if (employeeName != null && employeeName.Count() > 0 && filter.edate == "" && filter.sdate == "")
                    {
                        var newList =  listEmployeeClock.Where(e => e.eid == filter.search)
                                        .OrderByDescending(lr => lr.tdate)
                                        .ToList();

                        return Ok(newList);
                    }
                }
            }

            if (filter.listall == false || filter.listall == null)
            {
               
                if (filter.search != "" && filter.edate == "" && filter.sdate == "")
                {

                    var newList = listEmployeeClock.Where(e => e.eid.Contains(filter.search))
                                    .OrderByDescending(lr => lr.tdate)
                                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                                    .Take(filter.pageSize)
                                    .ToList();

                    return Ok(newList);
                }
            }


            if (filter.search == "" && filter.sdate == "" && filter.edate == "")
            {
                var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                var newList = _context.timeLogClass
                                 .Where(e => e.tdate == dateTo)
                                 .Include(e => e.ccfpinfo)
                                 .AsQueryable()
                                 .OrderByDescending(lr => lr.tdate)
                                 .Skip((filter.pageNumber - 1) * filter.pageSize)
                                 .Take(filter.pageSize)
                                 .ToList();
                return Ok(newList);
            }

            return BadRequest();
        }

        [HttpPost("adjust")]
        public async Task<ActionResult> AdjustTime(string eid,TimeLogDTO timeLog)
        {
            HRTimeLogClass hRTimeLog = new HRTimeLogClass(_context);

            hRTimeLog.braid = timeLog.bcode;
            hRTimeLog.timid = GetLogNextID();
            hRTimeLog.eid = timeLog.eid;
            if (timeLog.Status == "In")
            {
                hRTimeLog.tim = "8:00:00";
            }
            else if (timeLog.Status == "Out")
            {
                hRTimeLog.tim = "17:00:00";
            }
            hRTimeLog.cty = "Forget Scan";
            hRTimeLog.tdate =Convert.ToDateTime(timeLog.ClockDate);
            hRTimeLog.sta = timeLog.Status;
            await _context.timeLogClass.AddAsync(hRTimeLog);
            await _context.SaveChangesAsync();
            return Ok(hRTimeLog);
        }



        [HttpPost("posttimelog")]
        public async Task<IActionResult> PostTimeClock(HRTimeLogClass timeLog)
        {
            //return BadRequest(timeLog);

            var datetime = DateTime.Now.ToString("yyyy-MM-dd");
             DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd", CultureInfo.GetCultureInfo("en-GB"));
            //DateTime DOI = DateTime.Now;

            var datetimeLog = DateTime.Now.ToString("HH:mm:ss");
            DateTime convertingdatetime = DateTime.ParseExact(datetime, "yyyy-MM-dd", null);

            
            try
            {
                bool exsitingEmployee = false;
                // timelog eid == ecard
                exsitingEmployee = _context.employee.Any(e => e.ecard == timeLog.eid);
                 
                if (_context.employee.Any(e => e.ecard == null))
                {
                    exsitingEmployee = false;
                }

              
                if (exsitingEmployee == true)
                {

                    if (timeLog.braid != null &&
                        timeLog.eid != null && timeLog.cty != null)
                    {

                        var employee = await _context.employee.FirstOrDefaultAsync(e => e.ecard == timeLog.eid);
                        var branchID = await _context.employeeJoinInfo.FirstOrDefaultAsync(e => e.eid == employee.eid);

                        timeLog.timid = GetLogNextID();
                        timeLog.braid = branchID.site;
                        timeLog.eid = employee.eid;
                        timeLog.tdate = DOI;
                        timeLog.tim = datetimeLog.ToString();
                       


                        await _context.timeLogClass.AddAsync(timeLog);
                        await _context.SaveChangesAsync();

                        return Ok(timeLog);
                    }
                    else
                    {
                        return BadRequest("Request Param.");
                    }
                }
                else
                {
                    return BadRequest("Employee don't have in our company.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost("count")]
        public IActionResult GetCount(Filter filter)
        {
            NpgsqlConnection con;
            NpgsqlCommand cmd;
            NpgsqlDataAdapter da;
            DataTable dt;
            string sql = "";
            if(filter.StartDate=="" && filter.EndDate == "")
            {
                sql = "select distinct(ar.eid),(select count(*) from attandance_report a where a.eid ='200302' and a.timein is null and a.branchname is not null ) as \"Forget In\",(select count(*) from attandance_report b where b.eid = '200302' and b.timeout is null  and b.branchname is not null ) as \"Forget out\",(select count(distinct(l.\"KEY\")) from v_rpt_leave l where l.\"Staff ID\" = (select p.ecard  from ccfpinfo  p where p.eid='200302') )--and to_date(\"From Date\",'YYYY-MM-dd')>=date_trunc('month',now())  )as \"Number Leave\"  from attandance_report ar where ar.branchname is not  null and ar.eid ='200302' and ar.date_report >= to_char(date_trunc('month',now()),'YYYY-MM-dd')::DATE and ar.date_report<=to_char(now(),'YYYY-MM-dd')::DATE ";
               
            }

            return BadRequest("NO Data");
        }



        [HttpPost("attendance")]
        public IActionResult getAttendance(Filter filter)
        {
            NpgsqlConnection con;
            NpgsqlCommand cmd;
            NpgsqlDataAdapter da;
            DataTable dt;
            string sql="";
            if (filter.StartDate=="" && filter.EndDate=="")
            {
                sql = "select * from (select distinct concat(to_char(c.tdate::timestamp,'yyyy-MM-dd'), i.ecard) as \"key\",to_char(c.tdate::timestamp,'DD-MM-yyyy') \"date_report\",i.eid,i.ecard,i.fname ,i.lname ,i.fullname ,i.department,i.jobposition,i.estatus,b.braname branchname,(select min(tt.tim) from ccfhrmanagement.ccftim tt where tt.sta='In' and tt.tdate=c.tdate and tt.eid= c.eid  limit 1) as \"timein\",(select max(tt.tim) from ccfhrmanagement.ccftim tt where tt.sta='Out' and tt.tdate =c.tdate and tt.eid=c.eid  limit 1) as \"timeout\", to_char('080000'::time,'HH24:MI:SS') \"inworkinghour\", to_char('170000'::time,'HH24:MI:SS') \"outworkinghour\" from ccfhrmanagement.ccftim c  inner join ccfhrmanagement.ccfbranch b on b.braid = c.braid right join ccfhrmanagement.v_employee i on c.eid=i.eid ) t where t.eid='" + filter.Eid + "' and t.\"date_report\"='"+DateTime.Now.ToString("dd-MM-yyyy")+ "'  order by t.\"date_report\"";
              //  return Ok(sql);
            }
            else
            {
                sql = "select * from (select distinct concat(to_char(c.tdate::timestamp,'yyyy-MM-dd'), i.ecard) as \"key\",to_char(c.tdate::timestamp,'DD-MM-yyyy') \"date_report\",i.eid,i.ecard,i.fname ,i.lname ,i.fullname ,i.department,i.jobposition,i.estatus,b.braname branchname,(select min(tt.tim) from ccfhrmanagement.ccftim tt where tt.sta='In' and tt.tdate=c.tdate and tt.eid= c.eid  limit 1) as \"timein\",(select max(tt.tim) from ccfhrmanagement.ccftim tt where tt.sta='Out' and tt.tdate =c.tdate and tt.eid=c.eid  limit 1) as \"timeout\", to_char('080000'::time,'HH24:MI:SS') \"inworkinghour\", to_char('170000'::time,'HH24:MI:SS') \"outworkinghour\" from ccfhrmanagement.ccftim c  inner join ccfhrmanagement.ccfbranch b on b.braid = c.braid right join ccfhrmanagement.v_employee i on c.eid=i.eid and c.tdate::timestamp>='"+filter.StartDate+"' and c.tdate::timestamp<='"+filter.EndDate+"') t where t.eid='" + filter.Eid + "' order by t.\"date_report\""; ;
                //return Ok(sql);
            }
            //string sql = "select * from (select distinct concat(to_char(c.tdate::timestamp,'DD-MM-YYYY'), i.ecard) as \"key\",to_char(c.tdate::timestamp,'DD-MM-YYYY') \"date_report\",i.eid,i.ecard,i.fname ,i.lname ,i.fullname ,i.department,i.jobposition,i.estatus,b.braname branchname,(select min(tt.tim) from ccfhrmanagement.ccftim tt where tt.sta='In' and tt.tdate=c.tdate and tt.eid= c.eid  limit 1) as \"timein\",(select max(tt.tim) from ccfhrmanagement.ccftim tt where tt.sta='Out' and tt.tdate =c.tdate and tt.eid=c.eid  limit 1) as \"timeout\", to_char('080000'::time,'HH24:MI:SS') \"inworkinghour\", to_char('170000'::time,'HH24:MI:SS') \"outworkinghour\" from ccfhrmanagement.ccftim c  inner join ccfhrmanagement.ccfbranch b on b.braid = c.braid right join ccfhrmanagement.v_employee i on c.eid=i.eid) t where t.eid='"+ eid+"' and t.\"date_report\">='"+startDate+"' and t.\"date_report\"<='"+ endDate+"' order by t.\"date_report\""; ;
           // string sql = "select * from ccfhrmanagement.v_attendance limit 10";
            try
            {
                con = new NpgsqlConnection(_configuration.GetConnectionString("ProductionConnection"));
                con.Open();
                cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                da = new NpgsqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return Ok(dt);
                }
                else
                {
                    return BadRequest(" No Data Found");
                }
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok();
        }


        public string GetLogNextID()
        {
            var userLog =_context.timeLogClass.OrderByDescending(u => u.timid).FirstOrDefault();

            Debug.WriteLine("My second error message.", userLog);
            if (userLog == null)
            {
                return "1";
            }
            var nextId = Int32.Parse(userLog.timid) + 1;
            return nextId.ToString();

        }

    }
}
