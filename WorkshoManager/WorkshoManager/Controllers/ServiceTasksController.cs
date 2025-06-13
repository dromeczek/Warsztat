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
        var task = new ServiceTask
        {
            OrderId = orderId
        };

        return View(task);
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

        
        return View(task);

    }
}
