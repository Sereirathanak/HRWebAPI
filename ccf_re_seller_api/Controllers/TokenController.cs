using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ccf_booking_api.Models;
using ccf_re_seller_api.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    //return View();
        //}
       
            public IConfiguration _configuration;
            private readonly ReSellerAPIContext _context;

            public TokenController(IConfiguration config, ReSellerAPIContext context)
            {
                _configuration = config;
                _context = context;
            }

            [HttpPost]
            public async Task<IActionResult> Post(CcfuserRe _userData)
            {
                try
                {
                    if (_userData != null && _userData.pwd != null)
                    {
                        var user = GetUser(_userData.phone, _userData.pwd);
                    //
                    Console.WriteLine(_userData.phone);
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

            private CcfuserRe GetUser(string phone, string password)
            {
                return _context.CcfuserRes.FirstOrDefault(u => u.phone == phone && u.pwd == password);
            }
    }

}
