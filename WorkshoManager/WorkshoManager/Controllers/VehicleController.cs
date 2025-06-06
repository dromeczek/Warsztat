using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkshoManager.Data;
using WorkshoManager.Models;
using System.Linq;

namespace WorkshoManager.Controllers
{
    [Authorize(Roles = "Recepcjonista,Admin")]
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicle/Create?customerId=1
        public IActionResult Create(int customerId)
        {
            var vehicle = new Vehicle
            {
                CustomerId = customerId
            };

            return View(vehicle);
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["Errors"] = string.Join(" | ", errors);

                return View(vehicle);
            }

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            return RedirectToAction("Details", "Customer", new { id = vehicle.CustomerId });
        }
    }
}
