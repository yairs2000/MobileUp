using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileUp.Models;
using System.IO;

namespace MobileUp.Controllers
{
    public class ListingsController : Controller
    {
        private readonly MobileUpContext _context;

        public ListingsController(MobileUpContext context)
        {
            _context = context;
        }

        // GET: Listings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Listings.ToListAsync());
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile( )
        {
            if( Request.Form.Files?.Count > 0 && Request.Form.Files["DataFile"].Length > 0 && Request.Form.Files["DataFile"].FileName.EndsWith(".csv") )
            {
                using ( var reader = new StreamReader( Request.Form.Files["DataFile"].OpenReadStream()) )
                {
                    bool bFirslLine = true;
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(!bFirslLine )
                        {
                            var values = line.Split( ',' );
                            Listings listings = new Listings();
                            //listings.ListingId = Convert.ToInt32(values[0]);
                            listings.Address = values[1];
                            listings.Description = values[2];
                            double dPrice = 0;
                            Double.TryParse( values[3], out dPrice );
                            listings.Price = dPrice;
                            _context.Add( listings );
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            bFirslLine = false;
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listings = await _context.Listings
                .SingleOrDefaultAsync(m => m.ListingId == id);
            if (listings == null)
            {
                return NotFound();
            }

            return View(listings);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListingId,Address,Description,Price")] Listings listings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(listings);
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listings = await _context.Listings.SingleOrDefaultAsync(m => m.ListingId == id);
            if (listings == null)
            {
                return NotFound();
            }
            return View(listings);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ListingId,Address,Description,Price")] Listings listings)
        {
            if (id != listings.ListingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListingsExists(listings.ListingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(listings);
        }

        // GET: Listings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listings = await _context.Listings
                .SingleOrDefaultAsync(m => m.ListingId == id);
            if (listings == null)
            {
                return NotFound();
            }

            return View(listings);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listings = await _context.Listings.SingleOrDefaultAsync(m => m.ListingId == id);
            _context.Listings.Remove(listings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListingsExists(int id)
        {
            return _context.Listings.Any(e => e.ListingId == id);
        }


    }
}
