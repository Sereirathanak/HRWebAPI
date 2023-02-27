using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Repositories;
using System.Web.Http.Cors;

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*","*")]

    public class CcfmessagesResController : ControllerBase
    {
        private readonly ReSellerAPIContext _context;
        private UserRepository _userRepository;

        public CcfmessagesResController(ReSellerAPIContext context)
        {
            _context = context;
        }

        // GET: api/CcfmessagesRes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CcfmessagesRe>>> GetCcfmessagesRes()
        {
            return await _context.CcfmessagesRes.ToListAsync();
        }

        // POST: api/CcfmessagesRes/5
        [HttpPost("ByUser")]
        public async Task<ActionResult<IEnumerable<ReturnMessage>>> GetCcfmessagesRe(CustomerFilter filter)
        {
            try
            {
                var messages = _context.CcfmessagesRes
                                       .Where(m => m.ucode == filter.uid)
                                       .AsQueryable();

                var recordLists = messages.OrderByDescending(m => m.date)

                                       .OrderBy(m => m.mstatus)
                                     // .ThenByDescending(m => m.id)
                                      .AsQueryable()
                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                      .Take(filter.pageSize)
                                      .ToList();

                int totalMessage = messages.Count();
                int totalRead = messages.Where(m => m.mstatus == 1).Count();
                int totalUnRead = messages.Where(m => m.mstatus == 0).Count();

                List<ReturnMessage> resultLists = new List<ReturnMessage>() {
                    new ReturnMessage()
                    {
                        totalMessage    = totalMessage,
                        totalRead       = totalRead,
                        totalUnread     = totalUnRead,
                        listMessages    = recordLists
                    }
                };

                return resultLists;
            }
            catch (Exception ex)
            {
                return BadRequest(new KeyValuePair<string, string>("999", ex.Message.ToString()));
            }
        }

        // POST api/<MessagesController>/5
        [HttpPost("Read/{id}")]
        public async Task<IActionResult> Read(string id)
        {
            try
            {
                var messageObj = _context.CcfmessagesRes.SingleOrDefault(m => m.id == id);

                if (messageObj == null)
                {
                    return BadRequest(new KeyValuePair<string, string>("000", $"The message {id} not found."));
                }

                messageObj.mstatus = 1;
                _context.SaveChanges();

                return Ok(new KeyValuePair<string, string>("200", "The message was successfully read."));
            }
            catch (Exception ex)
            {
                return BadRequest(new KeyValuePair<string, string>("999", ex.Message.ToString()));
            }
        }

        // PUT: api/CcfmessagesRes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCcfmessagesRe(string id, CcfmessagesRe ccfmessagesRe)
        {
            if (id != ccfmessagesRe.id)
            {
                return BadRequest();
            }

            _context.Entry(ccfmessagesRe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CcfmessagesReExists(id))
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

        // POST: api/CcfmessagesRes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CcfmessagesRe>> PostCcfmessagesRe(CcfmessagesRe ccfmessagesRe)
        {
            _context.CcfmessagesRes.Add(ccfmessagesRe);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CcfmessagesReExists(ccfmessagesRe.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCcfmessagesRe", new { id = ccfmessagesRe.id }, ccfmessagesRe);
        }

        // DELETE: api/CcfmessagesRes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CcfmessagesRe>> DeleteCcfmessagesRe(string id)
        {
            var ccfmessagesRe = await _context.CcfmessagesRes.FindAsync(id);
            if (ccfmessagesRe == null)
            {
                return NotFound();
            }

            _context.CcfmessagesRes.Remove(ccfmessagesRe);
            await _context.SaveChangesAsync();

            return ccfmessagesRe;
        }

        private bool CcfmessagesReExists(string id)
        {
            return _context.CcfmessagesRes.Any(e => e.id == id);
        }
    }
}
