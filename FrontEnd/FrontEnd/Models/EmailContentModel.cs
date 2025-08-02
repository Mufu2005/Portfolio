namespace FrontEnd.Models
{
    public class EmailContentModel
    {
        public string Title{ get; set; }
        public string? Description { get; set; }
        public bool IsProject { get; set; }
        public bool IsPhoto { get; set; }
        public bool IsVideo { get; set; }
    }
}
