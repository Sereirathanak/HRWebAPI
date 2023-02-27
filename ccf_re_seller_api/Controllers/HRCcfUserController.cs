
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ccf_re_seller_api.Models;
using ccf_re_seller_api.Repositories;
using Microsoft.AspNetCore.Hosting;
using ccf_re_seller_api.Modals;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRCcfUserController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;
     

        public HRCcfUserController(HRContext context)
        {
            _context = context;
        }

        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRCcfUserClass>>> GetAll()
        {
            return await _context.ccfUserClass.ToListAsync();
        }


        [HttpGet("zone/{eid}")]
        public async Task<ActionResult> GetBranchZone(string eid)
        {
            var Branch =await _context.ccfUserClass.FirstOrDefaultAsync(c => c.uid == eid);
            if(Branch == null)
            {
                return NotFound("User not found");
            }
            //return Ok(Branch);
            var Zone =await _context.mapZoneClass.Include(e => e.ccfbranch).


                FirstOrDefaultAsync(z => z.braid == Branch.bcode);
            return Ok(Zone);

        }

        // POST: api/Users/5/UpdateMobileToken
        [HttpPost("{id}/mtoken")]
        public async Task<ActionResult<HRCcfUserClass>> UpdateMobileToken(string id, [Bind("mtoken")] HRCcfUserClass userForm)
        {
            try
            {
                var user = _context.ccfUserClass.FirstOrDefault(e => e.ucode == id);
                if (user == null)
                {
                    return NotFound();
                }

                user.mtoken = userForm.mtoken;
                await _context.SaveChangesAsync();

                return Ok(new KeyValuePair<string, string>("200", "The mobile token was successfully saved."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("hr/createuser/{userId}")]
        public async Task<IActionResult> CreateUser(string userId, HRCcfUserClass _user)

        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            var year = DateTime.Now.ToString("yyyy");
            int plusYear = int.Parse(year) + 1;
            var MM = DateTime.Now.ToString("MM");
            var dd = DateTime.Now.ToString("dd");
            var HH = DateTime.Now.ToString("HH");
            var mm = DateTime.Now.ToString("mm");
            var ss = DateTime.Now.ToString("ss");
            String plusString = $"{plusYear}{"-"}{MM}{"-"}{dd} {HH}{":"}{mm}{":"}{ss}";
            DateTime DOIEx = DateTime.ParseExact((plusString).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            bool exsitingUser = false;

            exsitingUser = _context.ccfUserClass.Any(e => e.uid == _user.uid);
            exsitingUser = _context.ccfUserClass.Any(e => e.uid == _user.uid);

            if (_context.employee.Any(e => e.ecard == null))
            {
                exsitingUser = false;
            }
            else if (exsitingUser == true)
            {
                exsitingUser = true;
            }

            if (exsitingUser == true)
            {
                var listUser = _context.employee.Where(u => u.ecard == _user.uid);
                return Ok(listUser);
            }
            else
            {
                try
                {
                    if (_user.uid != null && _user.upassword != null && _user.bcode != null && _user.uname != null)
                    {
                        _user.ucode = await GetNextID();
                        _user.uid = _user.uid;
                        _user.upassword = _user.upassword;
                        _user.ulevel = _user.ulevel;
                        _user.bcode = _user.bcode;
                        _user.datecreate = DOI;
                        _user.isapprover = _user.isapprover;
                        _user.ustatus = "A";
                        _user.uname = _user.uname;
                        _user.exdate = DOIEx;
                        _context.ccfUserClass.Add(_user);
                        await _context.SaveChangesAsync();

                        // assign user

                        var role = _context.ccfrole.SingleOrDefault();
                        var roleUser = "";
                        if (role == null)
                        {
                            roleUser = "";
                        }
                        else
                        {
                            roleUser = role.rcode;
                        }
                        HRCcfassign _ccfcustAsig = new HRCcfassign();

                        _ccfcustAsig.ucode = _user.ucode;
                        _ccfcustAsig.rcode = role.rcode;
                        _ccfcustAsig.adate = DOI;
                        _ccfcustAsig.aby = userId;


                        _context.ccfassign.Add(_ccfcustAsig);



                        //
                        await _context.SaveChangesAsync();
                        return Ok(_user);
                    }
                    else
                    {

                        return BadRequest("The credential is invalid.");

                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message.ToString());
                }
            }
        }

        public List<string> GetRoleListByUser(string id)
        {
            try
            {
                List<string> roleList = new List<string>();
                var assignUserRole = _context.ccfassign
                                             .Where(r => r.ucode == id.ToString())
                                             .AsQueryable();

                if (assignUserRole == null)
                {
                    roleList.Add("0");
                    return roleList;
                }

                var results = assignUserRole.Select(r => new ReturnAssign()
                {
                    rcode = r.rcode
                })
                .ToList();

                for (int i = 0; i < results.Count; i++)
                {
                    roleList.Add(results[i].rcode);
                }

                return roleList;
            }
            catch
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfreferalCu(string id, HRCcfUserClass _user)
        {
            if (id != _user.ucode)
            {
                return BadRequest();
            }

            _context.Entry(_user).State = EntityState.Modified;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var employeeEcard = _context.employee.FirstOrDefault(u => u.eid == _user.uid);
            var userJoinInfo = _context.employeeJoinInfo.FirstOrDefault(u => u.eid == employeeEcard.eid);

            var user = _context.ccfUserClass.FirstOrDefault(u => u.uid == employeeEcard.eid);

            List<HRAuthentication> result = new List<HRAuthentication>
            {
                new HRAuthentication {
                    ucode   = user.ucode,
                    uname   = employeeEcard.dname,
                    uid   = employeeEcard.ecard,
                    phone   = employeeEcard.pnum,
                    pwe = user.upassword,
                    email = employeeEcard.email,
                    ustatus = employeeEcard.estatus,
                    level = employeeEcard.elevel,
                    roles   = GetRoleListByUser(user.ucode),
                    changePassword = user.changepassword,
                    token   = "",
                    brcode = userJoinInfo.site,
                    eid = employeeEcard.eid,
                    datecreate = user.datecreate,
                    isapprover = user.isapprover,
                    exdate = user.exdate,

                }
            };

            return Ok(result);
        }

        private bool CcfUserExists(string id)
        {
            return _context.ccfUserClass.Any(e => e.uid == id);
        }

        //Next ID
        public async Task<string> GetNextID()
        {
            var id = await _context.ccfUserClass.OrderByDescending(u => u.ucode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "100000";
            }
            var nextId = int.Parse(id.ucode) + 1;
            return nextId.ToString();
        }
        


        [HttpPost("login")]
        public async Task<ActionResult<HRCcfUserClass>> Login(HRCcfUserClass _userData)
        {
            try
            {
                if (_userData != null && _userData.upassword != null)
                {

                    bool _checkUserLogin = false;
                    bool _checkUserEmployeeLogin = false;

                    var emp =await _context.employee.FirstOrDefaultAsync(e => e.ecard == _userData.uid);

                    _checkUserLogin = _context.ccfUserClass.Any(u => u.upassword == _userData.upassword && u.uid== emp.eid);
                    _checkUserEmployeeLogin = _context.employee.Any(u => u.ecard == _userData.uid);

                    //return Ok(_checkUserLogin);

                    if (_checkUserLogin == true)
                    {
                        var employeeEcard = _context.employee.FirstOrDefault(u => u.ecard == _userData.uid);

                        if (employeeEcard != null)
                        {
                            // Check Lock
                            if (employeeEcard.estatus == "L")
                            {
                                return BadRequest("The user is locked by Administrator.");
                            }

                            // Check Inactive
                            if (employeeEcard.estatus == "I")
                            {
                                return BadRequest("The user is inactived, please contact to Administrator.");
                            }

                            //create claims details based on the user information
                            //     var claims = new[] {
                            //     new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            //     new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            //     new Claim("Phones", employeeEcard.pnum),
                            //     new Claim("Unoe", employeeEcard.rdate.ToString()),
                            //};

                            //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            //     var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            //     var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1000), signingCredentials: signIn);
                            //     var registerToken = new JwtSecurityTokenHandler().WriteToken(token);


                            var userJoinInfo= _context.employeeJoinInfo.FirstOrDefault(u => u.eid == employeeEcard.eid);

                            var user = _context.ccfUserClass.FirstOrDefault(u => u.uid == employeeEcard.eid);

                            // check for leave remain

                            var LeaveBalnce = _context.leaveEnrollment.FirstOrDefault(l => l.eid == employeeEcard.eid);

                            List<HRAuthentication> result = new List<HRAuthentication>
                            {
                                new HRAuthentication {
                                    ucode   = user.ucode,
                                    uname   = employeeEcard.dname,
                                    uid   = employeeEcard.ecard,
                                    phone   = employeeEcard.pnum,
                                    pwe = user.upassword,
                                    email = employeeEcard.email,
                                    ustatus = employeeEcard.estatus,
                                    level = employeeEcard.elevel,
                                    roles   = GetRoleListByUser(user.ucode),
                                    changePassword = user.changepassword,
                                    token   = "",
                                    brcode = userJoinInfo.site,
                                    eid = employeeEcard.eid,
                                    datecreate = user.datecreate,
                                    isapprover = user.isapprover,
                                    exdate = user.exdate,
                                    leaveRemain=LeaveBalnce.releav,

                                }
                            };
                            return Ok(result);

                        }
                        else
                        {
                            return BadRequest("The credential is invalid.");
                        }
                    }
                    else
                    {
                        return BadRequest("The credential is invalid.");

                    }

                }
                else
                {
                    return BadRequest("The data is invalid.");
                }
                //return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        [HttpPost("userbranch")]
        public async Task<ActionResult> AddUserBranch(HRCcfUserBranch userBranch)
        {
            var existed = await _context.UserBranches.Where(e => e.eid == userBranch.eid && e.branch == userBranch.branch).ToListAsync();
                //.Where(e => e.branch==userBranch.branch);
            if(existed.Count>0)
            {
                return BadRequest("User with eid " + userBranch.eid + " and Branch Code " + userBranch.branch + " already existed in system");
            }
            _context.UserBranches.Add(userBranch);
            await _context.SaveChangesAsync();
            return Ok(userBranch);
        }

        public string GetLogNextID()
        {
            var userLog = _context.ccfulog.OrderByDescending(u => u.id).FirstOrDefault();

            if (userLog == null)
            {
                return "1";
            }
            var nextId = int.Parse(userLog.id) + 1;
            return nextId.ToString();
        }

        [HttpPut("login/{id}")]
        public async Task<IActionResult> EditUserLogin(string id, HRCcfUserClass _user)
        {
            if (id != _user.uid)
            {
                return BadRequest();
            }

            _context.Entry(_user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_user);
        }

    }
}
