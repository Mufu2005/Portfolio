using Microsoft.AspNetCore.Mvc;
using VideographyService.Data;
using VideographyService.Models;
using Microsoft.EntityFrameworkCore;

namespace VideographyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public VideoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("list")]
        public IActionResult ListAllVideos()
        {
            var video = _context.videoTable.ToList();
            return Ok(video);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] VideoModel model)
        {
            _context.videoTable.Add(model);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("edit/{id:int}")]
        public IActionResult Edit(int id, [FromBody] VideoModel model)
        {
            var video = _context.videoTable.Find(id);
            if (video == null)
            {
                return NotFound();
            }

            if (model.Title != null)
            {
                video.Title = model.Title;
            }

            if (model.ThumbnailUrl != null)
            {
                video.ThumbnailUrl = model.VideoUrl;
            }

            if (model.VideoUrl!= null)
            {
                video.VideoUrl= model.VideoUrl;
            }

            if (model.CreatedDate.HasValue)
            {
                video.CreatedDate = model.CreatedDate;
            }

            _context.videoTable.Update(video);
            _context.SaveChanges();
            return Ok(video);
        }

        [HttpPost("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var video = _context.videoTable.Find(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.videoTable.Remove(video);
            _context.SaveChanges();
            return Ok();
        }
    }
}
