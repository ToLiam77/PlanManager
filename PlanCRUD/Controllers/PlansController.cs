using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlanCRUD.Data;
using PlanCRUD.Models;

namespace PlanCRUD.Controllers
{
    public class PlansController : Controller
    {
        private readonly PlanCRUDContext _context;

        public PlansController(PlanCRUDContext context)
        {
            _context = context;
        }

        // GET: Plans
        public async Task<IActionResult> Index(string searchString, string Day, int minPrice = int.MinValue, int maxPrice = int.MaxValue, int minDuration = 0, int maxDuration = 15)
        {
            if (_context.Plan == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var activities = from a in _context.Plan
                         select a;

            //Title
            if (!String.IsNullOrEmpty(searchString))
            {
                activities = activities.Where(s => s.Activity!.ToUpper().Contains(searchString.ToUpper()));
            }

            //Day
            if (!String.IsNullOrEmpty(Day))
            {
                activities = activities.Where(s => s.Days!.ToUpper().Contains(Day.ToUpper()) || s.Days!.Contains("All Days"));
            }

            //Price
            activities = activities.Where(s => s.Price >= minPrice && s.Price <= (maxPrice == 50 ? int.MaxValue : maxPrice));

            //Duration
            string[] timeRanges =
            {
                "15m",        // Index 0
                "30m",        // Index 1
                "45m",        // Index 2
                "1h",         // Index 3
                "1h 15m",     // Index 4
                "1h 30m",     // Index 5
                "1h 45m",     // Index 6
                "2h",         // Index 7
                "2h 15m",     // Index 8
                "2h 30m",     // Index 9
                "2h 45m",     // Index 10
                "3h",         // Index 11
                "3h 15m",     // Index 12
                "3h 30m",     // Index 13
                "3h 45m",     // Index 14
                "4h"          // Index 15
            };
            activities = activities.Where(s => s.Duration >= ConvertDurationStringToTimeSpan(timeRanges[minDuration]) 
                                            && s.Duration <= (maxDuration == 15 ? new TimeSpan(23, 59, 59) : ConvertDurationStringToTimeSpan(timeRanges[maxDuration])));



            //Pass Filters back to view
            ViewBag.SearchString = searchString;
            ViewBag.Day = Day;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.MinDuration = minDuration;
            ViewBag.MaxDuration = maxDuration;

            return View(await activities.ToListAsync());
        }

        public static TimeSpan ConvertDurationStringToTimeSpan(string duration)
        {
            int hours = 0;
            int minutes = 0;

            Regex hourRegex = new Regex(@"(\d+)h");
            Match hourMatch = hourRegex.Match(duration);

            Regex minuteRegex = new Regex(@"(\d+)m");
            Match minuteMatch = minuteRegex.Match(duration);

            if (hourMatch.Success)
            {
                hours = int.Parse(hourMatch.Groups[1].Value);
            }

            if (minuteMatch.Success)
            {
                minutes = int.Parse(minuteMatch.Groups[1].Value);
            }

            return new TimeSpan(hours, minutes, 0);
        }


        // GET: Plans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Plan == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Activity,Days,StartTime,EndTime,Duration,Price,Currency,Details")] Plan plan, int DurationHours, int DurationMinutes, string[] Days)
        {
            if (ModelState.IsValid)
            {
                plan.Duration = new TimeSpan(DurationHours, DurationMinutes, 0);

                // Handle days of the week
                if (Days != null && Days.Length > 0)
                {
                    if (Days.Contains("AllDays") || Days.Length == 7)
                    {
                        plan.Days = "All Days";
                    }
                    else
                    {
                        plan.Days = string.Join(",", Days);
                    }
                }
                else
                {
                    plan.Days = "None";
                }

                // Add the plan to the database
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Plan == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activity,Days,StartTime,EndTime,Duration,Price,Details")] Plan plan, int DurationHours, int DurationMinutes, string[] Days)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Create a TimeSpan from the selected hours and minutes
                    plan.Duration = new TimeSpan(DurationHours, DurationMinutes, 0);

                    // Handle days of the week
                    if (Days != null && Days.Length > 0)
                    {
                        if (Days.Contains("AllDays") || Days.Length == 7)
                        {
                            plan.Days = "All Days";
                        }
                        else
                        {
                            plan.Days = string.Join(",", Days);
                        }
                    }
                    else
                    {
                        plan.Days = "None";
                    }

                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
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
            return View(plan);
        }

        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Plan == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Plan == null)
            {
                return Problem("Entity set 'PlanCRUDContext.Plan'  is null.");
            }
            var plan = await _context.Plan.FindAsync(id);
            if (plan != null)
            {
                _context.Plan.Remove(plan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
          return (_context.Plan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
