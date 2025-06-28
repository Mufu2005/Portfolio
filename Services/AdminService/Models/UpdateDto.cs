using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class UpdateDto
    {
        public String Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public String newUsername { get; set; }

        [Required]
        [DataType (DataType.Password)]
        public String newPassword { get; set; }
    }
}
