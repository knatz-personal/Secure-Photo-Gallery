using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace WebPortal.Models
{
    public class ImageViewModel
    {
        public string AlbumName { get; set; }
        public IPagedList<ImageModel> ImagePagedList { get; set; }
        public IEnumerable<ImageModel> Images { get; set; }

        public ImageViewModel()
        {
        }
    }
}