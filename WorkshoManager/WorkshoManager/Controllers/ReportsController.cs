using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using WorkshoManager.Data;
using WorkshoManager.Data;
using WorkshoManager.Models;

namespace WorkshoManager.Controllers
{
    [Authorize(Roles = "Recepcjonista,Admin")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? customerId, int? month, int? vehicleId)
        {
            // Pobierz klientów i pojazdy do filtrowania
            ViewBag.Customers = await _context.Customers.ToListAsync();
            ViewBag.Vehicles = await _context.Vehicles.ToListAsync();

            // Pobierz wszystkie zlecenia
            var ordersQuery = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.Tasks)
                    .ThenInclude(t => t.UsedParts)
                        .ThenInclude(up => up.Part)
                .AsQueryable();

            if (customerId.HasValue)
                ordersQuery = ordersQuery.Where(o => o.CustomerId == customerId.Value);

            if (vehicleId.HasValue)
                ordersQuery = ordersQuery.Where(o => o.VehicleId == vehicleId.Value);

            if (month.HasValue)
                ordersQuery = ordersQuery.Where(o => o.CreatedAt.Month == month.Value);

            var orders = await ordersQuery.ToListAsync();

            // Oblicz koszt
            var report = orders.Select(o => new ReportViewModel
            {
                OrderId = o.Id,
                CustomerName = o.Customer.LastName,
                Vehicle = o.Vehicle.RegistrationNumber,
                CreatedAt = o.CreatedAt,
                LaborCost = o.Tasks.Sum(t => t.LaborCost),
                PartsCost = o.Tasks
                            .SelectMany(t => t.UsedParts)
                            .Sum(up => up.Quantity * up.Part.UnitPrice)
            }).ToList();

            return View(report);
        }

        [HttpPost]
        public IActionResult DownloadPdf(List<Order> filteredOrders)
        {
            var document = new RepairReportDocument(filteredOrders);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", "RaportNapraw.pdf");
        }
    }
}