using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileUp.Models;

namespace MobileUp.Controllers
{
    [Produces("application/json")]
    [Route("api/ListingsAPI")]
    public class ListingsAPIController : Controller
    {
        private readonly MobileUpContext _context;

        public ListingsAPIController(MobileUpContext context)
        {
            _context = context;
        }

        // GET: api/ListingsAPI
        [HttpGet]
        public IEnumerable<Listings> GetListings()
        {
            return _context.Listings;
        }

        // GET: api/ListingsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listings = await _context.Listings.SingleOrDefaultAsync(m => m.ListingId == id);

            if (listings == null)
            {
                return NotFound();
            }

            return Ok(listings);
        }

        // PUT: api/ListingsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListings([FromRoute] int id, [FromBody] Listings listings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != listings.ListingId)
            {
                return BadRequest();
            }

            _context.Entry(listings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListingsExists(id))
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

        // POST: api/ListingsAPI
        [HttpPost]
        public async Task<IActionResult> PostListings([FromBody] Listings listings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Listings.Add(listings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListings", new { id = listings.ListingId }, listings);
        }

        // DELETE: api/ListingsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listings = await _context.Listings.SingleOrDefaultAsync(m => m.ListingId == id);
            if (listings == null)
            {
                return NotFound();
            }

            _context.Listings.Remove(listings);
            await _context.SaveChangesAsync();

            return Ok(listings);
        }

        private bool ListingsExists(int id)
        {
            return _context.Listings.Any(e => e.ListingId == id);
        }
    }
}