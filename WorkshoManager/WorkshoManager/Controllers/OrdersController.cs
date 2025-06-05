using Microsoft.AspNetCore.Mvc;

namespace WorkshoManager.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult MyOrders()
        {
            return View();
        }
    }
}
