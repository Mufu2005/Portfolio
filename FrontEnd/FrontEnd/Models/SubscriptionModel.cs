using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Models
{
    public class SubscriptionModel
    {
        public int Id { set; get; }
        public string? name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime SubDateTime { set; get; } = DateTime.UtcNow;
    }
}
