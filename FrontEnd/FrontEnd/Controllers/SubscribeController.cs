using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class SubscribeController : Controller
    {
        private readonly HttpClient _subClient;
        private readonly string baseUrl = "http://subscriptionservice:8080/";

        public SubscribeController(IHttpClientFactory factory)
        {
            _subClient = factory.CreateClient();
            _subClient.BaseAddress = new Uri(baseUrl);
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SubscriptionModel model)
        {
            var response = await _subClient.PostAsJsonAsync("api/Subscription/add", model);
            return RedirectToAction("Thankyou", "Subscribe");
        }
        public IActionResult Thankyou()
        {
            return View();
        }
        public IActionResult Unsubscribe()
        {
            return View();
        }

        public IActionResult Goodbye()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Unsubscribe(SubscriptionModel model, int id)
        {
            id = model.Id;
            var response = await _subClient.PostAsJsonAsync($"/api/Subscription/delete/{id}", model);
            return RedirectToAction("Goodbye", "Subscribe");

        }
    }
}
