using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*","*")]

    public class AddressesController : Controller
    {

        private readonly ReSellerAPIContext _context;

        public AddressesController(ReSellerAPIContext context)
        {
            _context = context;
        }
        // GET: api/<AddressesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> Get()
        {
            var addresses = _context.Addresses
                                    .OrderBy(a => a.procode)
                                    .AsQueryable();

            return await addresses.ToListAsync();
        }

        // GET: api/AddressesController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(string id)
        {
            var address = await _context.Addresses.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        // POST: api/Addresses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            try
            {
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAddress", new { id = address.acode }, address);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Key (acode)") && ex.ToString().Contains("already exists"))
                    return BadRequest($"The address code {address.acode} is already exists.");

                return BadRequest(ex.Message.ToString());
            }
        }

        // PUT: api/Addresses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(string id, Address address)
        {
            if (id != address.acode)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    if (ex.ToString().Contains("Key (acode)") && ex.ToString().Contains("already exists"))
                        return BadRequest($"The address code {address.acode} is already exists.");

                    return BadRequest(ex.Message.ToString());
                }
            }

            return NoContent();
        }

        private bool AddressExists(string id)
        {
            return _context.Addresses.Any(e => e.acode == id);
        }

        // GET: api/<AddressesController>
        [HttpGet("Provinces/{id?}")]
        public async Task<ActionResult<IEnumerable<ReturnProvince>>> GetProvince(string id)
        {
            try
            {
                return LoadProvince(id);
            }
            catch
            {
                throw;
            }

        }

        // GET: api/<AddressesController>
        [HttpGet("Districts/{id}/{discode?}")]
        public async Task<ActionResult<IEnumerable<ReturnDistrict>>> GetDisctrict(string id, string discode)
        {
            try
            {
                return LoadDistrict(id, discode);
            }
            catch
            {
                throw;
            }
        }

        // GET: api/<AddressesController>
        [HttpGet("aDistricts/{id}/{discode?}")]
        public async Task<dynamic> GetAjaxDistrict(string id, string discode)
        {
            try
            {
                return JsonConvert.SerializeObject(LoadDistrict(id, discode));
            }
            catch
            {
                throw;
            }
        }

        // GET: api/<AddressesController>
        [HttpGet("Communes/{id}/{comcode?}")]
        public async Task<ActionResult<IEnumerable<ReturnCommune>>> GetCommune(string id, string comcode)
        {
            try
            {
                return LoadCommune(id, comcode);
            }
            catch
            {
                throw;
            }
        }

        // GET: api/<AddressesController>
        [HttpGet("Villages/{id}/{vilcode?}")]
        public async Task<ActionResult<IEnumerable<ReturnVillage>>> GetVillage(string id, string vilcode)
        {
            try
            {
                return LoadVillage(id, vilcode);
            }
            catch
            {
                throw;
            }
        }

        // GET: api/<AddressesController>
        [HttpGet("ByVillages/{id}")]
        public async Task<ActionResult<Address>> GetAddressByVillage(string id)
        {
            try
            {
                var addressObjectList = await _context.Addresses
                                                      .Where(a => a.vilcode == id.ToString())
                                                      .FirstOrDefaultAsync();

                if (addressObjectList == null)
                {
                    return null;
                }

                return addressObjectList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ReturnProvince> LoadProvince(string procode)
        {
            try
            {
                var addressObjectList = _context.Addresses.Select(a => new { a.procode, a.prodes, a.u1 });

                if (procode != null && procode != "")
                {
                    addressObjectList = addressObjectList.Where(a => a.procode == procode.ToString());
                }

                addressObjectList = addressObjectList.Distinct()
                                                     .OrderBy(a => a.u1)
                                                     .ThenBy(a => a.prodes)
                                                     .AsQueryable();

                if (addressObjectList == null)
                {
                    return null;
                }

                var results = addressObjectList.Select(r => new ReturnProvince()
                {
                    procode = r.procode,
                    prodes = r.prodes
                })
                .ToList();

                return results;
            }
            catch
            {
                throw;
            }
        }

        public List<ReturnDistrict> LoadDistrict(string procode, string discode)
        {
            try
            {
                var addressObjectList = _context.Addresses.Where(r => r.procode == procode.ToString())
                                                          .Select(a => new { a.discode, a.disdes });

                if (discode != null && discode != "")
                {
                    addressObjectList = addressObjectList.Where(a => a.discode == discode.ToString());
                }

                addressObjectList = addressObjectList
                                             .Distinct()
                                             .OrderBy(a => a.discode)
                                             .AsQueryable();

                if (addressObjectList == null)
                {
                    return null;
                }

                var results = addressObjectList.Select(r => new ReturnDistrict()
                {
                    discode = r.discode,
                    disdes = r.disdes
                })
                .ToList();

                return results;
            }
            catch
            {
                throw;
            }
        }

        public List<ReturnCommune> LoadCommune(string discode, string comcode)
        {
            try
            {
                var addressObjectList = _context.Addresses
                                             .Where(r => r.discode == discode.ToString())
                                             .Select(a => new { a.comcode, a.comdes });

                if (comcode != null && comcode != "")
                {
                    addressObjectList = addressObjectList.Where(r => r.comcode == comcode.ToString());
                }

                addressObjectList = addressObjectList.Distinct()
                                                     .OrderBy(a => a.comcode)
                                                     .AsQueryable();

                if (addressObjectList == null)
                {
                    return null;
                }

                var results = addressObjectList.Select(r => new ReturnCommune()
                {
                    comcode = r.comcode,
                    comdes = r.comdes
                })
                .ToList();

                return results;
            }
            catch
            {
                throw;
            }
        }

        public List<ReturnVillage> LoadVillage(string comcode, string vilcode)
        {
            try
            {
                var addressObjectList = _context.Addresses
                                             .Where(r => r.comcode == comcode.ToString())
                                             .Select(a => new { a.vilcode, a.vildes });

                if (vilcode != null && vilcode != "")
                {
                    addressObjectList = addressObjectList.Where(ad => ad.vilcode == vilcode.ToString());
                }

                addressObjectList = addressObjectList.Distinct()
                                                     .OrderBy(a => a.vilcode)
                                                     .AsQueryable();

                if (addressObjectList == null)
                {
                    return null;
                }

                var results = addressObjectList.Select(r => new ReturnVillage()
                {
                    vilcode = r.vilcode,
                    vildes = r.vildes
                })
                .ToList();

                return results;
            }
            catch
            {
                throw;
            }
        }
    }
}
