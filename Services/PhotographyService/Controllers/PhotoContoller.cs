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
            var photo = _context.photoDb.ToList();
            return Ok(photo);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] PhotoModel model)
        {
            _context.photoDb.Add(model);
            _context.SaveChanges();
            return Ok();
        }
    }
}
