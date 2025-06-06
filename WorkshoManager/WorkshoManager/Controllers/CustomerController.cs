using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}
