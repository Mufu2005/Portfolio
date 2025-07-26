using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using FirebaseMediaService.Services;
using FirebaseMediaService.Models;

namespace FirebaseMediaService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : Controller
    {
        private readonly FirebaseStorageService _firebaseStorageService;

        public MediaController()
        {
            _firebaseStorageService = new FirebaseStorageService();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] MediaModel model)
        {
            if (model.File == null)
            {
                return BadRequest("No file uploaded.");
            }

            var url = await _firebaseStorageService.UploadFileAsync(model.File, model.Folder);
            return Ok(new { Url = url });
        }

        [HttpGet("view")]
        public async Task<IActionResult> View([FromQuery] string fileName, [FromQuery] string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
            {
                return BadRequest("File name and folder name are required.");
            }
            try
            {
                await _firebaseStorageService.ViewFileAsync(fileName, folderName);
                return Ok($"File {fileName} found in folder {folderName}.");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
