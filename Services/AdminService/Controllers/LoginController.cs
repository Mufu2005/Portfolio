using System.Threading.Tasks;
using AdminService.Data;
using AdminService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context)
        {
            this._context = context;

        }
        [HttpGet]
        public ActionResult GetCredentials()
        {
            var users = _context.LoginModels.ToList();
            return Ok(users);
        }

        [HttpPost]
        public ActionResult PostCredentials(LoginDto dto)
        {
            var conversion = new LoginModel()
            {
                Password = dto.Password,
                Username = dto.Username
            };

            _context.LoginModels.Add(conversion);
            _context.SaveChanges();

            return Ok(dto);

        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.LoginModels.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            else return Ok(user);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateUser(int id, LoginDto dto)
        {
            var conversion = new LoginModel()
            {
                Password = dto.Password,
                Username = dto.Username
            };

            var user = _context.LoginModels.Find(id);
            if(user == null)
            {
                return BadRequest();
            }
            user.Username = dto.Username;
            user.Password = dto.Password;
            _context.SaveChanges();

            return Ok(dto);
            
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.LoginModels.Find(id);
            if(user == null)
            {
                return NotFound();
            }

            _context.LoginModels.Remove(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.LoginModels.SingleOrDefaultAsync(x => x.Username == dto.Username);
            if(user == null || user.Password != dto.Password)
            {
                return Unauthorized("Invalid Credentials");
            }

            return Ok();
        }
    }
}
