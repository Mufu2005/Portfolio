namespace FrontEnd.Models
{
    public class UploadMediaModel
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }
        public bool IsVideo { get; set; }
    }
}
