using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Models
{
    public class SubscriptionModel
    {
        public int Id { set; get; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime SubDateTime { set; get; } = DateTime.UtcNow;

    }
}
