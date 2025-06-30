using FrontEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class AdminController : Controller
    {
        private string baseUrl = "http://localhost:5123/";
        private HttpClient _client;

        public AdminController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(baseUrl);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Login(LoginModel model)
        {
            var response = await _client.PostAsJsonAsync("api/AuthContoller/login", model);
            if (!response.IsSuccessStatusCode)
            {
                TempData["LoginError"] = "Invalid username or password.";
                return View(model);
            }

            HttpContext.Session.SetString("IsLoggedIn", "true");
            return RedirectToAction("Index", "Admin");

        }

        public IActionResult Logout()
        {

            HttpContext.Session.SetString("IsLoggedIn", "flase");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            var IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (IsLoggedIn == "true")
            {
                return View();
            }

            else
            {
                return RedirectToAction("Unauthorized", "Admin");
            }

        }

        public IActionResult Projects()
        {
            var IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (IsLoggedIn == "true")
            {
                return View();
            }

            else
            {
                return RedirectToAction("Unauthorized", "Admin");
            }

        }

        public IActionResult UnAuthorized()
        {
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(LoginModel model, String NewUsername, String NewPass)
        {
            var dto = new AdminService.Models.UpdateDto
            {
                Username = model.Username,
                Password = model.Password,
                newPassword = NewPass,
                newUsername = NewUsername
            };
            var response = await _client.PostAsJsonAsync("api/AuthContoller/update", dto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["LoginError"] = "Invalid username or password";
                return View();
            }

            return RedirectToAction("Login", "Admin");
        }

    }
}
