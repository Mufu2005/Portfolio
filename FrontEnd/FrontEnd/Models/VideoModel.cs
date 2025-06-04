namespace FrontEnd.Models
{
    public class VideoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
