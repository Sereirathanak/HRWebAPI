using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ccf_booking_api.Models;
using ccf_re_seller_api.Modals;
using CCFReSeller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterUserController : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IConfiguration _configuration;
        private readonly ReSellerAPIContext _context;

        public InterUserController(IConfiguration config, ReSellerAPIContext context)
        {
            _configuration = config;
            _context = context;
        }
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<CcfuserRe>>> GetAll(CustomerFilter filter)
        {
            if (filter.level == 4 || filter.level == 5)
            {
                var listReferalCustomer = _context.CcfuserRes
               .Include(rf => rf.ccfreferalRe)
               .AsQueryable();
                if ((filter.sdate != null && filter.sdate != "") && (filter.edate != null && filter.edate != ""))
                {
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(filter.edate.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.datecreate >= dateFrom && la.datecreate <= dateTo);
                }
                else if (filter.sdate != null && filter.sdate != "")
                {
                    var strDateTo = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    DateTime dateFrom = DateTime.Parse(filter.sdate.ToString());
                    DateTime dateTo = DateTime.Parse(strDateTo.ToString());
                    listReferalCustomer = listReferalCustomer.Where(la => la.datecreate >= dateFrom && la.datecreate <= dateTo);
                }
                if (filter.status != null && filter.status != "")
                {
                    listReferalCustomer = listReferalCustomer.Where(lr => lr.verifystatus == filter.status.ToString());
                }
                int totalListReferalCustomer = listReferalCustomer.Count();
                var listReferalsCustomer = listReferalCustomer
                    .OrderByDescending(lr => lr.datecreate)
                    .AsQueryable()
                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                    .Take(filter.pageSize)
                    .OrderBy(x => x.verifystatus == "R")
                    .ToList();
                return listReferalsCustomer;
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<CcfuserRe>> CreateUser(CcfuserRe ccfuserRe)
        {
            bool exsitingUserLogCreate = false;
            exsitingUserLogCreate = _context.CcfuserRes.Any(e => e.staffid == ccfuserRe.staffid);
            if (_context.CcfuserRes.Any(e => e.staffid == null))
            {
                exsitingUserLogCreate = false;
            }
            else if (exsitingUserLogCreate == true)
            {
                exsitingUserLogCreate = true;
            }
            //Phone Table no to be null but internal user need only
            //staff Id, passwrod, branch code only.


            if (ccfuserRe.staffid == null || ccfuserRe.staffid == "")
            {
                return BadRequest($"The staff id is require.");
            };

            if (ccfuserRe.brcode == null || ccfuserRe.brcode == "")
            {
                return BadRequest($"The Branch is require.");
            };

            if (ccfuserRe.pwd == null || ccfuserRe.pwd == "")
            {
                return BadRequest($"The Passwrod is require.");
            };

            if (ccfuserRe.level == null)
            {
                return BadRequest($"Sometime wrong with level user.");
            };
            //
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            try
            {
                //
                if (exsitingUserLogCreate == true)
                {
                    var listByPhone = _context.CcfuserRes.Where(u => u.staffid == ccfuserRe.staffid);
                    return Ok(listByPhone);
                }
                else
                {
                    //create user
                    ccfuserRe.uid = await GetNextID();
                    ccfuserRe.uno = int.Parse(await GetNextID());

                    var phoneUserInternal = "0";
                    if (ccfuserRe.phone != "")
                    {
                        phoneUserInternal = ccfuserRe.phone;
                    }
                    if (ccfuserRe.phone != null)
                    {
                        phoneUserInternal = ccfuserRe.phone;
                    }
                    if (ccfuserRe.phone == null)
                    {
                        phoneUserInternal = "0";
                    };
                    if (ccfuserRe.phone == "")
                    {
                        phoneUserInternal = "0";
                    };
                    ccfuserRe.datecreate = DOI;
                    ccfuserRe.ustatus = Constant.ACTIVE;
                    ccfuserRe.u5 = "N";
                    ccfuserRe.phone = phoneUserInternal;
                    _context.CcfuserRes.Add(ccfuserRe);

                    // create user referer
                    CcfreferalRe user = new CcfreferalRe();

                    user.refcode = await GetNextIDReferal();
                    user.regdate = DOI;
                    user.status = Constant.ACTIVE;
                    user.refname = ccfuserRe.uname;
                    user.refphone = ccfuserRe.phone;
                    user.uid = ccfuserRe.uid;
                    user.u1 = ccfuserRe.utype;
                    user.u5 = "N";
                    _context.CcfreferalRes.Add(user);


                    await _context.SaveChangesAsync();

                    var listReferer = _context.CcfuserRes.Include(el => el.ccfreferalRe)
                        .Where(el => el.uid == ccfuserRe.uid);

                    return Ok(ccfuserRe);

                }
            }
            catch (DbUpdateException)
            {
                if (CcfuserReExists(ccfuserRe.uid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool CcfuserReExists(string id)
        {
            return _context.CcfuserRes.Any(e => e.uid == id);
        }

        //Next ID Table Referal
        public async Task<string> GetNextIDReferal()
        {
            var id = await _context.CcfreferalRes.OrderByDescending(u => u.refcode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "300000";
            }
            var nextId = int.Parse(id.refcode) + 1;
            return nextId.ToString();
        }

        //Next ID
        public async Task<string> GetNextID()
        {
            var id = await _context.CcfuserRes.OrderByDescending(u => u.uid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "100000";
            }
            var nextId = int.Parse(id.uid) + 1;
            return nextId.ToString();
        }
        //internal login

        [HttpPost("{id}/loginInternal")]
        public async Task<IActionResult> Post(CcfuserRe _userData)
        {
            try
            {
                if (_userData != null && _userData.pwd != null)
                {
                    var user = GetUser(_userData.staffid, _userData.pwd);
                    //
                    Console.WriteLine(_userData.staffid);
                    Console.WriteLine(_userData.pwd);
                    Console.WriteLine(user);
                    //
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
                            new Claim("Phone", user.uid),
                            new Claim("Uno", user.uno.ToString()),
                            //new Claim("UserLevel", user.utype.ToString()),
                            //new Claim("UserBranch", user.uotpcode)
                       };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1000), signingCredentials: signIn);
                        var registerToken = new JwtSecurityTokenHandler().WriteToken(token);

                        // End UserLog

                        List<Authentication> result = new List<Authentication>
                        {
                            new Authentication {
                                uname   = user.uname,
                                uid   = user.uid,
                                token   = registerToken,
                                email   = user.u1,
                                phone   = user.phone,
                                otp = user.uotpcode,
                                pwe = user.pwd,
                                ustatus = user.ustatus,
                                ufacebook = user.ufacebook,
                                level = user.level,
                                staffid = user.staffid,
                                staffposition = user.staffposition,
                                brcode = user.brcode,
                                u5 = user.u5
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
                    return BadRequest("The data is invalid.");
                }
                //return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        private CcfuserRe GetUser(string staffid, string password)
        {
            return _context.CcfuserRes.FirstOrDefault(u => u.staffid == staffid && u.pwd == password);
        }

    }
}
