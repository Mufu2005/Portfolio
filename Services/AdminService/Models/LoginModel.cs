using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
