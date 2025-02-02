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
        public async Task<IActionResult> Index(int score = 0, List<string> selectedContinents = null)
        {
            // If no continents are selected, show all countries by default
            if (selectedContinents == null || selectedContinents.Count == 0)
            {
                selectedContinents = new List<string>
                {
                    "Africa", "Asia", "Europe", "Oceania", "North America", "South America"
                };
            }

            // Get a random country based on the selected continents
            var randomCountry = await GetRandomCountryByContinents(selectedContinents);

            // Pass the selected continents, country, and score to the view
            ViewData["Score"] = score;
            ViewData["Country"] = randomCountry;
            ViewBag.Continents = new[] { "Africa", "Asia", "Europe", "Oceania", "North America", "South America" };
            ViewBag.SelectedContinents = selectedContinents; // Ensure selected continents are passed back

            return View();
        }

        public async Task<IActionResult> ShowAnswer(string country, string capital, int score, List<string> selectedContinents)
        {

            // Get a new random country based on selected continents
            var randomCountry = await GetRandomCountryByContinents(selectedContinents);
            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(capital))
            {
                // Pass the updated score, random country, and selected continents back to the view
                ViewData["Score"] = score;
                ViewData["Country"] = randomCountry;
                ViewBag.Continents = new[] { "Africa", "Asia", "Europe", "Oceania", "North America", "South America" };
                ViewBag.SelectedContinents = selectedContinents; // Ensure selected continents are passed back
                return View("Index"); // Return to Index if country or capital is null or empty
            }

            // Check if the country and capital match
            var isCorrect = await _context.Geography
                .Where(j => j.Country.ToLower() == country.ToLower() && j.Capital.ToLower() == capital.ToLower())
                .FirstOrDefaultAsync();

            if (isCorrect != null)
            {
                score++; // Increment the score if the answer is correct
            }
            else
            {
                // Store the correct answer (capital) in ViewData to show the alert in the view
                var correctAnswer = await _context.Geography
                    .Where(j => j.Country.ToLower() == country.ToLower())
                    .FirstOrDefaultAsync();

                ViewData["CorrectAnswer"] = correctAnswer?.Capital;
            }

            // Pass the updated score, random country, and selected continents back to the view
            ViewData["Score"] = score;
            ViewData["Country"] = randomCountry;
            ViewBag.Continents = new[] { "Africa", "Asia", "Europe", "Oceania", "North America", "South America" };
            ViewBag.SelectedContinents = selectedContinents; // Ensure selected continents are passed back

            return View("Index");
        }

        private async Task<Geography> GetRandomCountryByContinents(List<string> continents)
        {
            var countries = await _context.Geography
                .Where(g => continents.Contains(g.Continent))
                .ToListAsync();

            var random = new Random();
            return countries.OrderBy(c => random.Next()).FirstOrDefault();
        }

        // Helper method to get a random country based on the selected continent
        private async Task<Geography> GetRandomCountryByContinent(string continent)
        {
            var countries = await _context.Geography
                .Where(g => g.Continent.ToLower() == continent.ToLower())
                .ToListAsync();

            var random = new Random();
            return countries.OrderBy(c => random.Next()).FirstOrDefault();  // Return a random country from the list
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

        public Task<Geography?> GetRandomCountry(string country = "")
        {
            var randomCountry = _context.Geography
                .Where(g => g.Country != country) // Exclude the passed country
                .OrderBy(r => Guid.NewGuid()) // Shuffle the results
                .FirstOrDefaultAsync();

            return randomCountry;
        }
    }
}
