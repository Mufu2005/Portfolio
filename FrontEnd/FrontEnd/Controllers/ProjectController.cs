using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class ProjectController : Controller
    {
        private String projectBaseUrl = "http://localhost:5053/";
        private HttpClient _projectClient;

        public ProjectController(IHttpClientFactory factory)
        {
            _projectClient = factory.CreateClient();
            _projectClient.BaseAddress = new Uri(projectBaseUrl);
        }
        public async Task<IActionResult> Index()
        {
            var projects = await _projectClient.GetFromJsonAsync<List<ProjectModel>>("api/Project/list");
            return View(projects);
        }
    }
}
