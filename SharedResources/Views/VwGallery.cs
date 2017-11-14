using System.Collections.Generic;

namespace SharedResources.Views
{
    public class VwGallery
    {
        public VwAlbum Album { get; set; }
        public int AlbumId { get; set; }
        public VwImage Image { get; set; }
        public string UserId { get; set; }
    }
}