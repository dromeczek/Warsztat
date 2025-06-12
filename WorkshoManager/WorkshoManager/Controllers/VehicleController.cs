using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkshoManager.Data;
using WorkshoManager.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Create(int? customerId)
        {
            var vehicle = new Vehicle();

            if (customerId.HasValue)
            {
                vehicle.CustomerId = customerId.Value;
            }
            else
            {
                ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "LastName");
            }

            return View(vehicle);
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                var customerExists = await _context.Customers.AnyAsync(c => c.Id == vehicle.CustomerId);
                if (!customerExists)
                {
                    ModelState.AddModelError("CustomerId", "Wybrany klient nie istnieje.");
                    ViewBag.Customers = new SelectList(_context.Customers.ToList(), "Id", "LastName");
                    return View(vehicle);
                }

                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    vehicle.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _context.Vehicles.Add(vehicle);
                _context.SaveChanges();
                return RedirectToAction("Details", "Customer", new { id = vehicle.CustomerId });
            }

            ViewBag.Customers = new SelectList(_context.Customers.ToList(), "Id", "LastName");
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null)
                return NotFound();

            int customerId = vehicle.CustomerId;

            // Usuń plik zdjęcia, jeśli istnieje
            if (!string.IsNullOrEmpty(vehicle.ImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", vehicle.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();

            return RedirectToAction("Details", "Customer", new { id = customerId });
        }


    }
}
