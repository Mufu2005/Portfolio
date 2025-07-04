using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotographyService.Data;
using PhotographyService.Models;

namespace PhotographyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoContoller : Controller
    {
        private readonly ApplicationDbContext _context;
        public PhotoContoller(ApplicationDbContext context)
        {
            this._context = context;
            
        }

        [HttpGet("list")]
        public IActionResult ListAllPhotos()
        {
            var photo = _context.photoTable.ToList();
            return Ok(photo);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] PhotoModel model)
        {
            _context.photoTable.Add(model);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("edit/{id:int}")]
        public IActionResult Edit(int id, [FromBody] PhotoModel model)
        {
            var project = _context.photoTable.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Id = id;
            project.Title = model.Title;
            project.ImageUrl = model.ImageUrl;
            project.CreatedAt = model.CreatedAt;

            _context.photoTable.Update(project);
            _context.SaveChanges();
            return Ok(project);
        }

        [HttpPost("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var project = _context.photoTable.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.photoTable.Remove(project);
            _context.SaveChanges();
            return Ok();
        }
    }
}
