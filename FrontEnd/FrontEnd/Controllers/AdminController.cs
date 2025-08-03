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
        private string videoBaseUrl = "http://localhost:5180/";
        private HttpClient _videoClient;
        private string SubscribeBaseUrl = "http://localhost:5089/";
        private HttpClient _subscribeClient;
        private string baseUrl = "http://localhost:5201/";
        private HttpClient _firebaseClient;

        public AdminController(IHttpClientFactory factory)
        {
            _authClient = factory.CreateClient();
            _authClient.BaseAddress = new Uri(authBaseUrl);
            _projectClient = factory.CreateClient(); 
            _projectClient.BaseAddress = new Uri(projectBaseUrl);
            _photoClient = factory.CreateClient();
            _photoClient.BaseAddress = new Uri(photoBaseUrl);
            _videoClient = factory.CreateClient();
            _videoClient.BaseAddress = new Uri(videoBaseUrl);
            _subscribeClient = factory.CreateClient();
            _subscribeClient.BaseAddress = new Uri(SubscribeBaseUrl);
            _firebaseClient = factory.CreateClient();
            _firebaseClient.BaseAddress = new Uri(baseUrl);
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
                var Media = await RetrieveMediaDb();
                var view = new ProjectAdminViewModel
                {
                    MediaList = Media,
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
            var mailModel = new EmailContentModel
            {
                Title = dto.Title,
                Description = dto.Description,
                IsProject = true,
                IsPhoto = false,
                IsVideo = false
            };
            await _subscribeClient.PostAsJsonAsync("api/Subscription/MailContent", mailModel);
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
                var Media = await RetrieveMediaDb();
                var view = new PhotographyAdminViewModel
                {
                    MediaList = Media,
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

        public async Task<IActionResult> CreatePhoto(PhotographyAdminViewModel model)
        {
            PhotoModel dto = model.Photo;
            var data = await _photoClient.PostAsJsonAsync("api/PhotoContoller/create", dto);
            var mailModel = new EmailContentModel
            {
                Title = dto.Title,
                IsProject = true,
                IsPhoto = false,
                IsVideo = false
            };
            await _subscribeClient.PostAsJsonAsync("api/Subscription/MailContent", mailModel);
            return RedirectToAction("Photography", "Admin");
        }


        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var response = await _photoClient.PostAsync($"/api/PhotoContoller/delete/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not delete project.";
            }
            return RedirectToAction("Photography", "Admin");
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

            return RedirectToAction("Photography", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Videography()
        {
            var IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (IsLoggedIn == "true")
            {
                var video = await _videoClient.GetFromJsonAsync<List<VideoModel>>("/api/Video/list");
                var Media = await RetrieveMediaDb();
                var view = new VideographyAdminViewModel
                {
                    MediaList = Media,
                    AllVideo = video,
                    Video = new VideoModel()
                };
                return View(view);
            }

            else
            {
                return RedirectToAction("Unauthorized", "Admin");
            }

        }

        public async Task<IActionResult> CreateVideo(VideographyAdminViewModel model)
        {
            VideoModel dto = model.Video;
            var data = await _videoClient.PostAsJsonAsync("api/Video/create", dto);
            var mailModel = new EmailContentModel
            {
                Title = dto.Title,
                IsProject = true,
                IsPhoto = false,
                IsVideo = false
            };
            await _subscribeClient.PostAsJsonAsync("api/Subscription/MailContent", mailModel);
            return RedirectToAction("Videography", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var response = await _videoClient.PostAsync($"/api/Video/delete/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not delete project.";
            }
            return RedirectToAction("Videography", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> EditVideo(int id, VideographyAdminViewModel model)
        {
            VideoModel dto = model.Video;
            var response = await _videoClient.PostAsJsonAsync($"/api/Video/edit/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "error editing the Project";
            }

            return RedirectToAction("Videography", "Admin");
        }

        //Subscription

        [HttpGet]
        public async Task<IActionResult> Subscription()
        {
            var IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (IsLoggedIn == "true")
            {
                var subscribers = await _subscribeClient.GetFromJsonAsync<List<SubscriptionModel>>("/api/Subscription/list");
                var view = new SubscriptionAdminViewModel
                {
                    AllSubscribers = subscribers,
                    Subscribers = new SubscriptionModel()
                };
                return View(view);
            }

            else
            {
                return RedirectToAction("Unauthorized", "Admin");
            }

        }

        public async Task<IActionResult> CreateSubscriber(SubscriptionAdminViewModel model)
        {
            SubscriptionModel dto = model.Subscribers;
            var data = await _subscribeClient.PostAsJsonAsync("api/Subscription/add", dto);
            return RedirectToAction("Subscription", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubscriber(int id)
        {
            var response = await _subscribeClient.PostAsync($"/api/Subscription/delete/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not remove Subscriber.";
            }
            return RedirectToAction("Subscription", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> EditSubscriber(int id, SubscriptionAdminViewModel model)
        {
            SubscriptionModel dto = model.Subscribers;
            var response = await _subscribeClient.PostAsJsonAsync($"/api/Subscription/edit/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "error editing the Subscriber";
            }

            return RedirectToAction("Subscription", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Media()
        {
            var IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (IsLoggedIn == "true")
            {
                var mediaList = await _firebaseClient.GetFromJsonAsync<List<MediaDbModel>>("api/Media/list");
                var view = new UploadMediaAdminViewModel
                {
                    AllMedia = mediaList,
                    Upload = new UploadMediaModel()
                };
                return View(view);
            }

            else
            {
                return RedirectToAction("Unauthorized", "Admin");
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadMedia(UploadMediaAdminViewModel model)
        {
            var form = new MultipartFormDataContent();

            if (model.Upload.File != null)
            {
                var fileContent = new StreamContent(model.Upload.File.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Upload.File.ContentType);
                form.Add(fileContent, "File", model.Upload.File.FileName);
            }

            if (!string.IsNullOrEmpty(model.Upload.Title))
                form.Add(new StringContent(model.Upload.Title), "Title");

            if (!string.IsNullOrEmpty(model.Upload.Folder))
                form.Add(new StringContent(model.Upload.Folder), "Folder");

            var response = await _firebaseClient.PostAsync("api/Media/upload", form);

            return RedirectToAction("Media", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            var response = await _firebaseClient.PostAsync($"api/Media/delete/{id}",null);
            return RedirectToAction("Media", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> EditMedia(UploadMediaAdminViewModel model, int id)
        {
            var form = new MultipartFormDataContent();

            if (model.Upload.File != null)
            {
                var fileContent = new StreamContent(model.Upload.File.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Upload.File.ContentType);
                form.Add(fileContent, "File", model.Upload.File.FileName);
            }

            if (!string.IsNullOrEmpty(model.Upload.Title))
                form.Add(new StringContent(model.Upload.Title), "Title");

            if (!string.IsNullOrEmpty(model.Upload.Folder))
                form.Add(new StringContent(model.Upload.Folder), "Folder");

            var response = await _firebaseClient.PostAsync($"api/Media/edit/{id}", form);

            return RedirectToAction("Media", "Admin");
        }

        [HttpGet]
        public async Task<List<MediaDbModel>> RetrieveMediaDb()
        {
            var list = await _firebaseClient.GetFromJsonAsync<List<MediaDbModel>>("api/Media/list");
            return list;
        }
    }
}
