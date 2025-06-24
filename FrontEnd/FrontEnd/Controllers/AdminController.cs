using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
