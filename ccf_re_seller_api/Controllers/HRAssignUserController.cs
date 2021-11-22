using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
//{
//    public class AssignUserController : Controller
//    {
//        // GET: /<controller>/
//        //public IActionResult Index()
//        //{
//        //    return View();
//        //}
//    }
//}

{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]

    public class HRAssignUserController : ControllerBase
    {
        private readonly HRContext _context;

        private readonly UserRepository _userRepository;

        public HRAssignUserController(HRContext context, IWebHostEnvironment env)
        {
            _context = context;
            _userRepository = new HRUserRepository(_context, env);
        }

        // GET: api/CcfcustAsigs
        // Assign User to process
        [HttpPost("hr/{idBranch}/{levelUser}")]
        public async Task<ActionResult<HRCcfassign>> AssignUserToProcessing(string idBranch, string levelUser, HRCcfassign CcfcustAsig)
        {

            //var level = int.Parse(levelUser);
            //if (level == 4)
            //{
            //    var list = _context.CcfUserClass.Where(lv => lv.ulevel == 3)
            //                 .Where(br => br.bcode == idBranch);
            //    return Ok(list);
            //};
            //if (level == 3)
            //{
            //    var list = _context.CcfUserClass.Where(lv => lv.ulevel == 2)
            //                .Where(br => br.brcode == idBranch);
            //    return Ok(list);
            //};
            //if (level == 2)
            //{
            //    var list = _context.CcfUserClass.Where(lv => lv.ulevel == 1)
            //                .Where(br => br.brcode == idBranch);
            //    return Ok(list);
            //};
            //if (level == 1)
            //{
            //    var list = _context.CcfUserClass.Where(lv => lv.ulevel == 3)
            //                .Where(br => br.brcode == idBranch);
            //    return Ok(list);
            //};
            return Ok();

            //return await _context.CcfcustAsigs.ToListAsync();
        }


        // GET: api/CcfcustAsigs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRCcfassign>>> GetCcfcustAsigs()
        {
            return await _context.ccfassign.ToListAsync();
        }

        // GET: api/CcfcustAsigs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HRCcfassign>> GetCcfcustAsig(string id)
        {
            //var signalLoan = _context.CcfUserClass.SingleOrDefault(u => u.cid == id);

            //var ccfcustAsig = await _context.ccfassignRe
            //                        .Include(u => u.CcfuserReFu)
            //                        .Include(u => u.CcfuserReTu)
            //                        .Include(c => c.CcfreferalCu)
            //                        //.Include(i => i.cid == signalLoan.cid)
            //                        .Where(cid => cid.cid == id)
            //                        .AsQueryable()
            //                        .ToListAsync(); ;

            //if (ccfcustAsig == null)
            //{
            //    return NotFound();
            //}

            //return Ok(ccfcustAsig);
            return Ok();

        }

        // PUT: api/CcfcustAsigs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfcustAsig(string id, HRCcfassign ccfcustAsig)
        {
            if (id != ccfcustAsig.ucode)
            {
                return BadRequest();
            }

            _context.Entry(ccfcustAsig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!CcfcustAsigExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //}
                throw;

            }

            return NoContent();
        }

        // POST: api/CcfcustAsigs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("hr/assign")]
        public async Task<ActionResult<HRCcfassign>> PostCcfcustAsig(HRValidateUserRoles _ccfcustAsig)
        {
            //bool exitAssign = false;
            //bool exitLoan = false;
            //exitLoan = _context.CcfcustAsigs.Any(el => el.cid == ccfcustAsig.cid);
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                if (_ccfcustAsig.ucode != null)
                {
                    // Delete Old Records
                    var oldObjs = _context.ccfassign.Where(a => a.ucode == _ccfcustAsig.ucode);
                    if (oldObjs != null)
                    {
                        foreach (var role in oldObjs)
                        {
                            _context.ccfassign.Remove(role);
                        }
                        _context.SaveChanges();
                    }
                }

                if (_ccfcustAsig.rcode != null)
                {
                    // Insert New Records
                    for (int i = 0; i < _ccfcustAsig.rcode.Count(); i++)
                    {
                        HRCcfassign userRoles = new HRCcfassign();
                        userRoles.id = GetNextID();
                        userRoles.ucode = _ccfcustAsig.ucode;
                        userRoles.rcode = _ccfcustAsig.rcode[i];
                        userRoles.adate = _ccfcustAsig.adate;
                        userRoles.aby = _ccfcustAsig.aby;
                        userRoles.rdes = _ccfcustAsig.rdes;

                        _context.ccfassign.Add(userRoles);
                        await _context.SaveChangesAsync();
                    }

                }
                else
                {
                    return Ok("This user was successfully revoked the roles.");
                }
                //_context.ccfassign.Add(_ccfcustAsig);
                //await _context.SaveChangesAsync();

                return Ok("The user was successfully assigned the roles.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        public int GetNextID()
        {
            var assignedRole = _context.ccfassign.OrderByDescending(u => u.id).FirstOrDefault();

            if (assignedRole == null)
            {
                return 1;
            }
            var nextId = assignedRole.id + 1;
            return nextId;
        }


        // DELETE: api/CcfcustAsigs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HRCcfassign>> DeleteCcfcustAsig(string id)
        {
            var ccfcustAsig = await _context.ccfassign.FindAsync(id);
            if (ccfcustAsig == null)
            {
                return NotFound();
            }

            _context.ccfassign.Remove(ccfcustAsig);
            await _context.SaveChangesAsync();

            return ccfcustAsig;
        }
       
    }
}
