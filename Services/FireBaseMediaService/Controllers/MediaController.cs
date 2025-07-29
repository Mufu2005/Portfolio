using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using FirebaseMediaService.Services;
using FirebaseMediaService.Models;
using FirebaseMediaService.Data;
using System.Threading.Tasks;

namespace FirebaseMediaService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : Controller
    {
        private readonly FirebaseStorageService _firebaseStorageService;
        private readonly ApplicationDbContext _context;

        public MediaController(ApplicationDbContext context)
        {
            _context = context;
            _firebaseStorageService = new FirebaseStorageService();
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            var mediaList = _context.MediaTable.ToList();
            return Ok(mediaList);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] MediaModel model)
        {
            if (model.File == null)
            {
                return BadRequest("No file uploaded.");
            }

            var parameters = await _firebaseStorageService.UploadFileAsync(model.File, model.Folder);

            var dto = new MediaDbModel
            {
                Title = parameters.Title,
                Url = parameters.Url,
                Folder = parameters.Folder
            };

            _context.MediaTable.Add(dto);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            MediaDbModel model = _context.MediaTable.Find(id);
            if (model == null)
            {
                return NotFound("Media not found.");
            }

            try
            {
                await _firebaseStorageService.DeleteFileAsync(model.Title, model.Folder);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, $"Error deleting file: {ex.Message}");
            }

            _context.MediaTable.Remove(model);
            _context.SaveChanges();
            return Ok("Media deleted successfully.");
        }

        [HttpPost("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromForm] MediaModel model)
        {
            var existingModel = _context.MediaTable.Find(id);
            if (existingModel == null)
            {
                return NotFound("Media not found.");
            }
            if (!string.IsNullOrEmpty(model.Title))
            {
                existingModel.Title = model.Title;
            }
            if (!(model.File == null || model.File.Length == 0))
            {
                await _firebaseStorageService.DeleteFileAsync(existingModel.Title, existingModel.Folder);
                var parameters = await _firebaseStorageService.UploadFileAsync(model.File, model.Folder);
                existingModel.Url = parameters.Url;
                existingModel.Title = parameters.Title;
                existingModel.Folder = parameters.Folder;
            }


            _context.MediaTable.Update(existingModel);
            _context.SaveChanges();
            return Ok(existingModel);
        }



    }
}
