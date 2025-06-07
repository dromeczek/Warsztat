using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkshoManager.Data;
using WorkshoManager.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkshoManager.Controllers
{
    public class UsedPartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsedPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create(int serviceTaskId)
        {
            ViewBag.ServiceTaskId = serviceTaskId;
            ViewBag.Parts = new SelectList(_context.Parts.ToList(), "Id", "Name");
            return View(new UsedPart { ServiceTaskId = serviceTaskId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UsedPart usedPart)
        {
            if (ModelState.IsValid)
            {
                _context.UsedParts.Add(usedPart);
                _context.SaveChanges();
                return RedirectToAction("Details", "Orders", new { id = GetOrderIdByTask(usedPart.ServiceTaskId) });
            }

            ViewBag.Parts = new SelectList(_context.Parts.ToList(), "Id", "Name");
            return View(usedPart);
        }

        private int GetOrderIdByTask(int serviceTaskId)
        {
            var task = _context.ServiceTasks.Include(t => t.Order).FirstOrDefault(t => t.Id == serviceTaskId);
            return task?.OrderId ?? 0;
        }
    }
}
