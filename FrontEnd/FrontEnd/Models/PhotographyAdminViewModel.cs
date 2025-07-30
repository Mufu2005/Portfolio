namespace FrontEnd.Models
{
    public class PhotographyAdminViewModel
    {
        public PhotoModel Photo{ get; set; }
        public List<PhotoModel> AllPhotos{ get; set; }
        public List<MediaDbModel> MediaList { get; set; }
    }
}
