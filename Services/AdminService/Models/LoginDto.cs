using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class LoginDto
    {
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
