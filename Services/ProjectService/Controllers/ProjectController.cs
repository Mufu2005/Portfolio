using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectService.Data;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListAllProject()
        {
            var projects = _context.ProjectModels.ToListAsync();
            return Ok(projects);
        }

        [HttpPost]
        public IActionResult Create(ProjectModel model)
        {
            _context.ProjectModels.Add(model);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("{id:int}")]
        public IActionResult Edit(int id, [FromBody] ProjectModel model)
        {
            var project = _context.ProjectModels.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            
            project.Id = id;
            project.Title = model.Title;
            project.Description = model.Description;
            project.ImageUrl = model.ImageUrl;
            project.ProjectUrl = model.ProjectUrl;
            project.CreateAt = model.CreateAt;

            _context.ProjectModels.Add(project);
            _context.SaveChanges();
            return Ok(project);
        }

        [HttpDelete ("{id:int}")]
        public IActionResult Delete(int id)
        {
            var project = _context.ProjectModels.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.ProjectModels.Remove(project);
            _context.SaveChanges();
            return Ok();
        }
    }

    
        

        
}
