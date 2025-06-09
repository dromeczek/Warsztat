using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkshoManager.Data;
using WorkshoManager.Models;

[Authorize(Roles = "Recepcjonista,Mechanik,Admin")]
public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (User.IsInRole("Mechanik"))
        {
            var myOrders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.Mechanic)   //.Where(o => o.MechanicId == user.Id)
                .ToList();
            return View(myOrders);
        }

        var allOrders = _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Vehicle)
            .Include(o => o.Mechanic)
            .ToList();

        return View(allOrders);
    }

    [Authorize(Roles = "Recepcjonista")]
    public IActionResult Create()
    {
        ViewData["Customers"] = new SelectList(_context.Customers, "Id", "LastName");
        ViewData["Vehicles"] = new SelectList(_context.Vehicles, "Id", "RegistrationNumber");
        ViewData["Mechanics"] = new SelectList(_userManager.GetUsersInRoleAsync("Mechanik").Result, "Id", "UserName");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Recepcjonista")]
    public IActionResult Create(Order order)
    {
        if (ModelState.IsValid)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewData["Customers"] = new SelectList(_context.Customers, "Id", "LastName", order.CustomerId);
        ViewData["Vehicles"] = new SelectList(_context.Vehicles, "Id", "RegistrationNumber", order.VehicleId);
        ViewData["Mechanics"] = new SelectList(_userManager.GetUsersInRoleAsync("Mechanik").Result, "Id", "UserName", order.MechanicId);

        return View(order);
    }

    [HttpPost]
    [Authorize(Roles = "Mechanik")]
    public IActionResult ChangeStatus(int id, string status)
    {
        var order = _context.Orders.FirstOrDefault(o => o.Id == id);
        var userId = _userManager.GetUserId(User);

        if (order == null || order.MechanicId != userId)
        {
            return Unauthorized();
        }

        order.Status = status;
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
    public IActionResult Details(int id)
    {
        var order = _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Vehicle)
            .Include(o => o.Mechanic)
            .Include(o => o.Tasks)
                .ThenInclude(t => t.UsedParts)
                    .ThenInclude(up => up.Part)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }


    [Authorize(Roles = "Admin,Mechanik")]
    public async Task<IActionResult> EditStatus(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);

        // Tylko przypisany mechanik lub admin
        if (User.IsInRole("Admin") || order.MechanicId == userId)
        {
            return View(order);
        }

        return Forbid(); // brak dostępu
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Mechanik")]
    public async Task<IActionResult> EditStatus(Order updatedOrder)
    {
        var order = await _context.Orders.FindAsync(updatedOrder.Id);
        if (order == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);

        // Sprawdzenie uprawnień
        if (!User.IsInRole("Admin") && order.MechanicId != userId)
        {
            return Forbid();
        }

        // Zmieniamy status
        order.Status = updatedOrder.Status;

        // Jeśli "Zakończone" → ustaw datę zakończenia
        if (order.Status == "Zakończone" && order.CompletionDate == null)
        {
            order.CompletionDate = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

}
