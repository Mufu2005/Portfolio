using Microsoft.AspNetCore.Mvc;
using SubscriptionService.Data;
using SubscriptionService.Models;

namespace SubscriptionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SubscriptionController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("list")]
        public IActionResult ListAllSubscribers()
        {
            var Subscribers = _context.SubscriptionTable.ToList();
            return Ok(Subscribers);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] SubscriptionModel model)
        {
            _context.SubscriptionTable.Add(model);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("edit/{id:int}")]
        public IActionResult Edit(int id, [FromBody] SubscriptionModel model)
        {
            var Subscribers = _context.SubscriptionTable.Find(id);
            if (Subscribers == null)
            {
                return NotFound();
            }

            if (model.name != null)
            {
                Subscribers.name = model.name;
            }

            if (model.Email != null)
            {
                Subscribers.Email = model.Email;
            }

            _context.SubscriptionTable.Update(Subscribers);
            _context.SaveChanges();
            return Ok(Subscribers);
        }

        [HttpPost("delete/{id:int}")]
        public IActionResult Delete(int id, SubscriptionModel model)
        {
            var subscribers = _context.SubscriptionTable.Find(id);
            if (subscribers == null)
            {
                return NotFound();
            }

            if (model.Id == subscribers.Id && model.Email == subscribers.Email)
            {
                _context.SubscriptionTable.Remove(subscribers);
            }
            else if(model.Id == subscribers.Id && model.Email == null)
            {
                _context.SubscriptionTable.Remove(subscribers);
            }



            
            _context.SaveChanges();
            return Ok();
        }
    }
}
