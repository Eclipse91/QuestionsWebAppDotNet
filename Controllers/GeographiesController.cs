using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestionsWebApp.Data;
using QuestionsWebApp.Models;
using static System.Formats.Asn1.AsnWriter;

namespace QuestionsWebApp.Controllers
{
    public class GeographiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GeographiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Geographies
        public async Task<IActionResult> Index(int score = 0)
        {
            var randomCountry = await _context.Geography
                .OrderBy(r => Guid.NewGuid())  // Shuffle the records to get a random one
                .FirstOrDefaultAsync();

            // Pass the score and country to the view using ViewData
            ViewData["Score"] = score;
            ViewData["Country"] = randomCountry;

            return View();  // No need to pass a model, ViewData will carry the data
        }

        [HttpPost]
        public async Task<IActionResult> ShowAnswer(string country, string capital, int score)
        {
            var randomCountry = await _context.Geography
                .OrderBy(r => Guid.NewGuid())
                .FirstOrDefaultAsync();

            var isCorrect = await _context.Geography
                .Where(j => j.Country.ToLower() == country.ToLower() && j.Capital.ToLower() == capital.ToLower())
                .FirstOrDefaultAsync();

            if (isCorrect != null)
            {
                score++;  // Increment the score if the answer is correct
            }

            // Pass the updated score and random country back to the view
            ViewData["Score"] = score;
            ViewData["Country"] = randomCountry;

            return View("Index");  // Return the updated view
        }


        // GET: Geographies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geography = await _context.Geography
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geography == null)
            {
                return NotFound();
            }

            return View(geography);
        }

        // GET: Geographies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Geographies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Country,Capital,Continent")] Geography geography)
        {
            if (ModelState.IsValid)
            {
                _context.Add(geography);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(geography);
        }

        // GET: Geographies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geography = await _context.Geography.FindAsync(id);
            if (geography == null)
            {
                return NotFound();
            }
            return View(geography);
        }

        // POST: Geographies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Country,Capital,Continent")] Geography geography)
        {
            if (id != geography.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(geography);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeographyExists(geography.Id))
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
            return View(geography);
        }

        // GET: Geographies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geography = await _context.Geography
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geography == null)
            {
                return NotFound();
            }

            return View(geography);
        }

        // POST: Geographies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var geography = await _context.Geography.FindAsync(id);
            if (geography != null)
            {
                _context.Geography.Remove(geography);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeographyExists(int id)
        {
            return _context.Geography.Any(e => e.Id == id);
        }
    }
}
