using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;
using CCFReSeller;

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

            _context.Entry(ccfuserRe).State = EntityState.Modified;

            try
            {
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

            return NoContent();
        }

        // POST: api/CcfuserRes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfuserRe>> PostCcfuserRe(CcfuserRe ccfuserRe)
        {
            bool exsitingUserLogCreate = false;
            exsitingUserLogCreate = _context.CcfuserRes.Any(e => e.phone == ccfuserRe.phone);
            if (_context.CcfuserRes.Any(e => e.phone == null))
            {
                exsitingUserLogCreate = false;
            }
            else if (exsitingUserLogCreate == true)
            {
                exsitingUserLogCreate = true;
            }


            bool exsitingUserLogCreateByFacebook = false;
            exsitingUserLogCreateByFacebook = _context.CcfuserRes.Any(e => e.ufacebook == ccfuserRe.ufacebook);
            if (_context.CcfuserRes.Any(e => e.ufacebook == null))
            {
                exsitingUserLogCreateByFacebook = false;
            }
            else if(exsitingUserLogCreateByFacebook == true)
            {
                exsitingUserLogCreateByFacebook = true;
            }
           
            Console.WriteLine(exsitingUserLogCreateByFacebook);

            try
            {
               
                //
                Console.WriteLine(exsitingUserLogCreate);
                //
                Console.WriteLine(exsitingUserLogCreateByFacebook);
                //

                if (exsitingUserLogCreateByFacebook == true)
                {
                    var list = _context.CcfuserRes.Where(u => u.ufacebook == ccfuserRe.ufacebook);

                    return Ok(list);
                }

                if (exsitingUserLogCreate == true)
                {
                    var listByPhone = _context.CcfuserRes.Where(u => u.phone == ccfuserRe.phone);
                    return Ok(listByPhone);
                }
                else 
                {
                    // create user 
                    ccfuserRe.uid = await GetNextID();
                    ccfuserRe.uno = int.Parse(await GetNextID());
                    ccfuserRe.datecreate = DateTime.Now;
                    ccfuserRe.ustatus = Constant.ACTIVE;
                    ccfuserRe.utype = Constant.CUSTOMER;
                    _context.CcfuserRes.Add(ccfuserRe);


                    // create user referer

                    CcfreferalRe user = new CcfreferalRe();

                    user.refcode = await GetNextIDReferal();
                    user.regdate = DateTime.Now;
                    user.status = Constant.ACTIVE;
                    user.refname = ccfuserRe.uname;
                    user.refphone = ccfuserRe.phone;
                    user.uid = ccfuserRe.uid;



                    _context.CcfreferalRes.Add(user);

                    await _context.SaveChangesAsync();

                    var listReferer = _context.CcfuserRes.Include(el => el.ccfreferalRe)
                        .Where(el=>el.uid == ccfuserRe.uid);
                       
                    return Ok(listReferer);
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
                var user = await _context.CcfuserRes.FindAsync(id);
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
