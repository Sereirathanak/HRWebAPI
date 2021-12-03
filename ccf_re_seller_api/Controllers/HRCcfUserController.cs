
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

        public HRCcfUserController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost("hr/createuser/{userId}")]
        public async Task<IActionResult>CreateUser(string userId, HRCcfUserClass _user)

        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            var year = DateTime.Now.ToString("yyyy");
            int plusYear = int.Parse(year)+1;
            var MM = DateTime.Now.ToString("MM");
            var dd = DateTime.Now.ToString("dd");
            var HH = DateTime.Now.ToString("HH");
            var mm = DateTime.Now.ToString("mm");
            var ss = DateTime.Now.ToString("ss");
            String plusString = $"{plusYear}{"-"}{MM}{"-"}{dd} {HH}{":"}{mm}{":"}{ss}";
            DateTime DOIEx = DateTime.ParseExact((plusString).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            bool exsitingUser = false;

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
                        if(role == null)
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
        //funtion user login

        [HttpPost("login")]
        public async Task<IActionResult> Login(HRCcfUserClass _userData)
        {
            try
            {
                if (_userData != null && _userData.upassword != null)
                {

                    //var user = GetUser(_userData.uid, _userData.upassword);
                    bool _checkUserLogin = false;
                    bool _checkUserEmployeeLogin = false;

                    _checkUserLogin =  _context.ccfUserClass.Any(u => u.upassword == _userData.upassword);
                    _checkUserEmployeeLogin = _context.employee.Any(u => u.ecard == _userData.uid);

                    if (_checkUserLogin == true && _checkUserEmployeeLogin == true)
                    {
                        var user = _context.ccfUserClass.FirstOrDefault(u => u.upassword == _userData.upassword);
                        if (user != null)
                        {
                            // Check Lock
                            if (user.ustatus == "L")
                            {
                                return BadRequest("The user is locked by Administrator.");
                            }

                            // Check Inactive
                            if (user.ustatus == "I")
                            {
                                return BadRequest("The user is inactived, please contact to Administrator.");
                            }

                            //create claims details based on the user information
                            var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("Phones", user.uid),
                            new Claim("Unoe", user.datecreate.ToString()),
                       };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1000), signingCredentials: signIn);
                            var registerToken = new JwtSecurityTokenHandler().WriteToken(token);


                            var employeeEcard = _context.employee.FirstOrDefault(u => u.ecard == _userData.uid);
                            List<HRAuthentication> result = new List<HRAuthentication>
                        {
                            new HRAuthentication {
                                uname   = user.uname,
                                uid   = employeeEcard.ecard,
                                phone   = employeeEcard.pnum,
                                pwe = user.upassword,
                                ustatus = user.ustatus,
                                level = user.ulevel,
                                roles   = GetRoleListByUser(user.ucode),
                                changePassword = user.changepassword,
                                token   = registerToken,
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


        //private HRCcfUserClass GetUser(string uid, string password)
        //{
        //    bool _checkUserLogin = false;
        //    bool _checkUserEmployeeLogin = false;

        //     _checkUserLogin = _context.ccfUserClass.Any(u => u.upassword == password);
        //     _checkUserEmployeeLogin = _context.employee.Any(u => u.ecard == uid);

        //    if(_checkUserLogin == true && _checkUserEmployeeLogin == true)
        //    {
        //        return _context.ccfUserClass.FirstOrDefault(u => u.uid == uid);

        //    }
        //}

    }
}
