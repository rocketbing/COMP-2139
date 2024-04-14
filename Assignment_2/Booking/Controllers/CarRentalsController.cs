using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CarRentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarRentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarRentals
        public async Task<IActionResult> Index()
        {
              return _context.CarRentals != null ? 
                          View(await _context.CarRentals.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.CarRentals'  is null.");
        }

        // GET: CarRentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CarRentals == null)
            {
                return NotFound();
            }

            var carRental = await _context.CarRentals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carRental == null)
            {
                return NotFound();
            }

            return View(carRental);
        }

        // GET: CarRentals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarRentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarRental carRental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carRental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carRental);
        }

        // GET: CarRentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CarRentals == null)
            {
                return NotFound();
            }

            var carRental = await _context.CarRentals.FindAsync(id);
            if (carRental == null)
            {
                return NotFound();
            }
            return View(carRental);
        }

        // POST: CarRentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarRental carRental)
        {
            if (id != carRental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carRental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarRentalExists(carRental.Id))
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
            return View(carRental);
        }

        // GET: CarRentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CarRentals == null)
            {
                return NotFound();
            }

            var carRental = await _context.CarRentals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carRental == null)
            {
                return NotFound();
            }

            return View(carRental);
        }

        // POST: CarRentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CarRentals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CarRentals'  is null.");
            }
            var carRental = await _context.CarRentals.FindAsync(id);
            if (carRental != null)
            {
                _context.CarRentals.Remove(carRental);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarRentalExists(int id)
        {
          return (_context.CarRentals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
