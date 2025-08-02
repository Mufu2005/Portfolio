using ProjectService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectService.Data;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("list")]
        public IActionResult ListAllProject()
        {
            var projects = _context.ProjectTable.ToList();
            return Ok(projects);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ProjectModel model)
        {
            _context.ProjectTable.Add(model);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("edit/{id:int}")]
        public IActionResult Edit(int id, [FromBody] ProjectModel model)
        {
            var project = _context.ProjectTable.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            if(model.Title != null)
            {
                project.Title = model.Title;
            }

            if (model.Description != null)
            {
                project.Description = model.Description;
            }

            if (model.ImageUrl != null)
            {
                project.ImageUrl = model.ImageUrl;
            }

            if (model.ProjectUrl != null)
            {
                project.ProjectUrl = model.ProjectUrl;
            }

            if (model.CreateAt.HasValue)
            {
                project.CreateAt = model.CreateAt;
            }

            _context.ProjectTable.Update(project);
            _context.SaveChanges();
            return Ok(project);
        }

        [HttpPost("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var project = _context.ProjectTable.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.ProjectTable.Remove(project);
            _context.SaveChanges();
            return Ok();
        }
    }

    
        

        
}
