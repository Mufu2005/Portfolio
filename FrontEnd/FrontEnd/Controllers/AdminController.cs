using System.Net.Http.Json;
using System.Threading.Tasks;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class AdminController : Controller
    {
        private string authBaseUrl = "http://localhost:5123/";
        private HttpClient _authClient;
        private String projectBaseUrl = "http://localhost:5053/";
        private HttpClient _projectClient;
        private string photoBaseUrl = "http://localhost:5102/";
        private HttpClient _photoClient;

        public AdminController(IHttpClientFactory factory)
        {
            _authClient = factory.CreateClient();
            _authClient.BaseAddress = new Uri(authBaseUrl);
            _projectClient = factory.CreateClient(); 
            _projectClient.BaseAddress = new Uri(projectBaseUrl);
            _photoClient = factory.CreateClient();
            _photoClient.BaseAddress = new Uri(photoBaseUrl);
        }

        public IActionResult Login()
        {
            string userAgent = Request.Headers["User-Agent"];
            if (userAgent.Contains("Mobile") || userAgent.Contains("Android") || userAgent.Contains("iPhone"))
            {
                return RedirectToAction("Index", "Home"); // or show 403 page
            }
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Login(LoginModel model)
        {
            var response = await _authClient.PostAsJsonAsync("api/AuthContoller/login", model);
            if (!response.IsSuccessStatusCode)
            {
                TempData["LoginError"] = "Invalid username or password.";
                return View(model);
            }

            HttpContext.Session.SetString("IsLoggedIn", "true");
            return RedirectToAction("Index", "Admin");

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
            var response = await _authClient.PostAsJsonAsync("api/AuthContoller/update", dto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["LoginError"] = "Invalid username or password";
                return View();
            }

            return RedirectToAction("Login", "Admin");
        }

        public IActionResult Update()
        {
            return View();
        }

        public IActionResult UnAuthorized()
        {
            return View();
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

        [HttpGet]
        public async Task<IActionResult> Projects()
        {
            var IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (IsLoggedIn == "true")
            {
                var projects = await _projectClient.GetFromJsonAsync<List<ProjectModel>>("api/Project/list");
                var view = new ProjectAdminViewModel
                {
                    AllProjects = projects,
                    Project = new ProjectModel()
                };
                return View(view);
            }

            else
            {
                return RedirectToAction("Unauthorized", "Admin");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectAdminViewModel model)
        {
            ProjectModel dto = model.Project;
            var data = await _projectClient.PostAsJsonAsync("api/Project/create", dto);
            return RedirectToAction("Projects", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var response = await _projectClient.PostAsync($"api/Project/delete/{id}",null);
            if(!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not delete project.";
            }
            return RedirectToAction("Projects", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> EditProject(int id, ProjectAdminViewModel model)
        {
            ProjectModel dto = model.Project;
            var response = await _projectClient.PostAsJsonAsync($"api/Project/edit/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "error editing the Project";
            }

            return RedirectToAction("Projects", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Photography()
        {
            var IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (IsLoggedIn == "true")
            {
                var photo = await _photoClient.GetFromJsonAsync<List<PhotoModel>>("/api/PhotoContoller/list");
                var view = new PhotographyAdminViewModel
                {
                    AllPhotos = photo,
                    Photo = new PhotoModel()
                };
                return View(view);
            }

            else
            {
                return RedirectToAction("Unauthorized", "Admin");
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var response = await _photoClient.PostAsync($"/api/PhotoContoller/delete/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not delete project.";
            }
            return RedirectToAction("Projects", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> EditPhoto(int id, PhotographyAdminViewModel model)
        {
            PhotoModel dto = model.Photo;
            var response = await _photoClient.PostAsJsonAsync($"/api/PhotoContoller/edit/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "error editing the Project";
            }

            return RedirectToAction("Projects", "Admin");
        }

    }
}
