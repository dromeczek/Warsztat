using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkshoManager.Controllers
{
    public class VehicleController : Controller
    {
        public IActionResult Index() 
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) {
            return View();
        }
    }
}
