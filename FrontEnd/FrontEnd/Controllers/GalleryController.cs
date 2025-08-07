using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace FrontEnd.Controllers
{
    public class GalleryController : Controller
    {
        private string photoBaseUrl = "http://photographyservice:8080/";
        private readonly HttpClient _photoClient;
        private string videoBaseUrl = "http://videographyservice:8080/";
        private readonly HttpClient _videoClient;
       
        public GalleryController(IHttpClientFactory factory)
        {
            _photoClient = factory.CreateClient();
            _photoClient.BaseAddress = new Uri(photoBaseUrl);
            _videoClient = factory.CreateClient();
            _videoClient.BaseAddress = new Uri(videoBaseUrl);
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Photo()
        {
            var photos = await _photoClient.GetFromJsonAsync<List<PhotoModel>>("api/PhotoContoller/list");
            return View(photos);
        }

        public async Task<IActionResult> Video()
        {
            var videos = await _videoClient.GetFromJsonAsync<List<VideoModel>>("api/Video/list");
            return View(videos);
        }
    }
}
