using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshoManager.Data;
using WorkshoManager.Models;

[Authorize(Roles = "Recepcjonista,Admin")]
public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var customers = _context.Customers.ToList();
        return View(customers);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (ModelState.IsValid)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(customer);
    }

    public IActionResult Delete(int id)
    {
        var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }
    public IActionResult Details(int id)
    {
        var customer = _context.Customers
            .Include(c => c.Vehicles)
            .FirstOrDefault(c => c.Id == id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }

}
