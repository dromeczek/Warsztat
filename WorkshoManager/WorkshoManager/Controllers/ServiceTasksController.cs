using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshoManager.Data;
using WorkshoManager.Models;

[Authorize(Roles = "Mechanik")]
public class ServiceTasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public ServiceTasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int orderId)
    {
        ViewBag.OrderId = orderId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ServiceTask task)
    {
        if (ModelState.IsValid)
        {
            _context.ServiceTasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Details", "Orders", new { id = task.OrderId });
        }

        ViewBag.OrderId = task.OrderId;
        return View(task);
    }
}
