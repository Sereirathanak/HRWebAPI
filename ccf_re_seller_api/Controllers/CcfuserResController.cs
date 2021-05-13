using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;
using CCFReSeller;
using System.Globalization;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CcfuserResController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;

        public CcfuserResController(ReSellerAPIContext context)
        {
            _context = context;
        }





        // GET: api/CcfuserRes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfuserRe>>> GetCcfuserRes()
        {
            return await _context.CcfuserRes.ToListAsync();
            //return Ok();
        }

        //check exiting phone
        [HttpGet("{phone}/phone")]
        public async Task<ActionResult<CcfuserRe>> GetPhone(string phone)
        {
            if (phone.Length < 8)
            {
                return BadRequest(new KeyValuePair<string, string>("999", "Your phone number is invalid"));

            }

            var exitingPhone = _context.CcfuserRes.SingleOrDefault(ul => ul.phone == phone);
            //var listCus = _context.CcfreferalCus.SingleOrDefault(e => e.phone == phone);

           


            if (phone != exitingPhone.phone)
            {
                return BadRequest(new KeyValuePair<string, string>("999", "Your phone number is invalid"));
            }
            else
            {
                return Ok(exitingPhone);

            }
        }

        // GET: api/CcfuserRes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CcfuserRe>> GetCcfuserRe(string id)
        {
            var ccfuserRe = await _context.CcfuserRes.FindAsync(id);

            if (ccfuserRe == null)
            {
                return NotFound();
            }

            return ccfuserRe;
        }

        // PUT: api/CcfuserRes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfuserRe(string id, CcfuserRe ccfuserRe)
        {
            if (id != ccfuserRe.uid)
            {
                return BadRequest();
            }

            var users = _context.CcfuserRes.SingleOrDefault(e => e.uid == id);
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
            try
            {

                users.uid = id;
                users.uno = int.Parse(id);
                users.pwd = ccfuserRe.pwd;
                users.uname = ccfuserRe.uname;
                users.phone = ccfuserRe.phone;
                users.email = ccfuserRe.email;
                users.address = ccfuserRe.address;
                users.job = ccfuserRe.job;
                users.u4 = ccfuserRe.u4;
                users.datecreate = DOI;
                users.ustatus = Constant.ACTIVE;
                users.utype = Constant.CUSTOMER;
                users.u5 = "N";
                users.level = 0;
                users.staffposition = Constant.CUSTOMER;

                users.dob = ccfuserRe.dob;
                users.idtype = ccfuserRe.idtype;
                users.idnumber = ccfuserRe.idnumber;
                users.banktype = ccfuserRe.banktype;
                users.banknumber = ccfuserRe.banknumber;
                users.verifystatus = ccfuserRe.verifystatus;
                users.gender = ccfuserRe.gender;

                var user = _context.CcfreferalRes.SingleOrDefault(e => e.uid == id);
                if (id == user.uid)
                {
                    user.regdate = DOI;
                    user.status = Constant.ACTIVE;
                    user.refname = ccfuserRe.uname;
                    user.refphone = ccfuserRe.phone;
                    user.uid = ccfuserRe.uid;
                    user.u5 = "N";
                    user.u1 = Constant.CUSTOMER;
                    user.email = ccfuserRe.email;
                    user.address = ccfuserRe.address;
                    user.job = ccfuserRe.job;
                    user.nid = ccfuserRe.u4;

                    user.dob = ccfuserRe.dob;
                    user.idtype = ccfuserRe.idtype;
                    user.idnumber = ccfuserRe.idnumber;
                    user.typeaccountbank = ccfuserRe.banktype;
                    user.typeaccountnumber = ccfuserRe.banknumber;
                    user.verifystatus = ccfuserRe.verifystatus;
                    user.gender = ccfuserRe.gender;


                    _context.Entry(user).State = EntityState.Modified;

                }
                _context.Entry(users).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfuserReExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(users);
        }

        // POST: api/CcfuserRes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfuserRe>> PostCcfuserRe(CcfuserRe ccfuserRe)
        {

            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                bool exsitingUserLogCreate = false;
                exsitingUserLogCreate = _context.CcfuserRes.Any(e => e.phone == ccfuserRe.phone);

                if (exsitingUserLogCreate == true)
                {
                    var listByPhone = _context.CcfuserRes.Include(u => u.ccfreferalRe).SingleOrDefault(u => u.phone == ccfuserRe.phone);
                    return Ok(listByPhone);
                }

                else
                {
                    //create user
                    ccfuserRe.uid = await GetNextID();
                    ccfuserRe.uno = int.Parse(await GetNextID());
                    ccfuserRe.datecreate = DOI;
                    ccfuserRe.ustatus = Constant.ACTIVE;
                    ccfuserRe.utype = Constant.CUSTOMER;
                    ccfuserRe.u5 = "N";
                    ccfuserRe.level = 0;
                    ccfuserRe.staffposition = Constant.CUSTOMER;
                    ccfuserRe.verifystatus = "Please Verify Account";


                    // create user referer

                    CcfreferalRe user = new CcfreferalRe();

                    user.refcode = await GetNextIDReferal();
                    user.regdate = DOI;
                    user.status = Constant.ACTIVE;
                    user.refname = ccfuserRe.uname;
                    user.refphone = ccfuserRe.phone;
                    user.uid = ccfuserRe.uid;
                    user.u5 = "N";
                    user.u1 = Constant.CUSTOMER;
                    user.verify = "N";
                    user.verifystatus = "Please Verify Account";

                    _context.CcfuserRes.Add(ccfuserRe);
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

        //
        [HttpPost("facebook")]
        public async Task<ActionResult<CcfuserRe>> PostCcfuserReFacebook(CcfuserRe ccfuserRe)
        {

            try
            {
                var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
                bool exsitingUserLogCreateByFacebook = false;
                exsitingUserLogCreateByFacebook = _context.CcfuserRes.Any(e => e.ufacebook == ccfuserRe.ufacebook);
                Console.WriteLine(exsitingUserLogCreateByFacebook);
                if (exsitingUserLogCreateByFacebook == true)
                {
                    var list = _context.CcfuserRes.Include(u => u.ccfreferalRe)
                        .SingleOrDefault(u => u.ufacebook == ccfuserRe.ufacebook);

                    Console.WriteLine(list);

                    return Ok(list);
                }
                else
                {
                    // create user 
                    ccfuserRe.uid = await GetNextID();
                    ccfuserRe.uno = int.Parse(await GetNextID());
                    ccfuserRe.datecreate = DOI;
                    ccfuserRe.ustatus = Constant.ACTIVE;
                    ccfuserRe.utype = Constant.CUSTOMER;
                    ccfuserRe.u5 = "N";
                    ccfuserRe.level = 0;
                    ccfuserRe.staffposition = Constant.CUSTOMER;


                    //// create user referer

                    CcfreferalRe user = new CcfreferalRe();

                    user.refcode = await GetNextIDReferal();
                    user.regdate = DOI;
                    user.status = Constant.ACTIVE;
                    user.refname = ccfuserRe.uname;
                    user.refphone = ccfuserRe.phone;
                    user.uid = ccfuserRe.uid;
                    user.u5 = "N";
                    user.u1 = Constant.CUSTOMER;

                    _context.CcfuserRes.Add(ccfuserRe);
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

        //
        //Referal Next ID
        //Next ID
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

        //Post Token Notification
        [HttpPost("{id}/mtoken")]
        public async Task<ActionResult<CcfuserRe>> UpdateMobileToken(string id, [Bind("mtoken")] CcfuserRe userForm)
        {
            try
            {
                var user = _context.CcfuserRes.SingleOrDefault(ul => ul.uid == id);
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

        // DELETE: api/CcfuserRes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfuserRe>> DeleteCcfuserRe(string id)
        {
            var ccfuserRe = await _context.CcfuserRes.FindAsync(id);
            if (ccfuserRe == null)
            {
                return NotFound();
            }

            _context.CcfuserRes.Remove(ccfuserRe);
            await _context.SaveChangesAsync();

            return ccfuserRe;
        }

        private bool CcfuserReExists(string id)
        {
            return _context.CcfuserRes.Any(e => e.uid == id);
        }
    }
}
