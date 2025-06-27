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
            var response = await _client.PostAsJsonAsync("api/login", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "LoginFailed");
                return View(model);
            }


            return RedirectToAction("Index", "Home");

        }


    }
}
