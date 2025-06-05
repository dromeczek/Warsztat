using Microsoft.AspNetCore.Mvc;

namespace WorkshoManager.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
