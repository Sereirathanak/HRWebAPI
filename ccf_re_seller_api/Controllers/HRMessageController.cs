using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Modals;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRMessageController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRMessageController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }

        //get

        [HttpPost("ByUser")]
        public async Task<ActionResult<IEnumerable<ReturnHRMessage>>> GetByUser(string ucode, CustomerFilter filter)
        {
            try
            {
                //uid == eid from employee infor
                var messages = _context.hrccfmessages
                                       .Where(m => m.ucode == filter.uid )
                                       .AsQueryable();

                var recordLists = messages
                                        .Where(m => m.mstatus==0)
                                       .OrderBy(m => m.mstatus)
                                       
                                      .ThenByDescending(m => m.id)
                                      .AsQueryable()
                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                      .Take(filter.pageSize)
                                      .ToList();

                int totalMessage = messages.Count();
                int totalRead = messages.Where(m => m.mstatus == 1).Count();
                int totalUnRead = messages.Where(m => m.mstatus == 0).Count();

                List<ReturnHRMessage> resultLists = new List<ReturnHRMessage>() {
                    new ReturnHRMessage()
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
                var messageObj = _context.hrccfmessages.SingleOrDefault(m => m.id == id);

                if (messageObj == null)
                {
                    return BadRequest(new KeyValuePair<string, string>("000", $"The message {id} not found."));
                }
                _context.Entry(messageObj).State = EntityState.Modified;
                messageObj.mstatus = 1;
                _context.SaveChanges();

                return Ok(new KeyValuePair<string, string>("200", "The message was successfully read."));
            }
            catch (Exception ex)
            {
                return BadRequest(new KeyValuePair<string, string>("999", ex.Message.ToString()));
            }
        }
    }
}
