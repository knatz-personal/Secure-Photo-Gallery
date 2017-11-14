using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SharedResources.Views;

namespace WebPortal.Models
{
    public class AlbumCreateModel
    {
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        public string UserId { get; set; }
    }

    public class AlbumModel
    {
        [Required]
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ID { get; set; }

        [Required]
        [DisplayName("Modified On")]
        public DateTime ModifiedOn { get; set; }

        [Required]
        public string Title { get; set; }
    }

    public class GalleryModel
    {
        public VwAlbum Album { get; set; }
        public VwImage Image { get; set; }
        public string UserId { get; set; }
    }

    public class ImageModel
    {
        [Required]
        [Display(Name = "Album")]
        public int AlbumId { get; set; }

        public SelectList Albums { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Image")]
        public HttpPostedFileBase File { get; set; }

        [Required]
        public int Id { get; set; }

        public string Path { get; set; }

        [Required]
        public string ThumbNail { get; set; }

        [Required]
        public string Title { get; set; }
    }
}