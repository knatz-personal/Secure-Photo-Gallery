using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataExchange.EntityModel;
using DataExchange.Repositories;
using SharedResources.Views;
using SSD.Web.Api.Models;

namespace SSD.Web.Api.Models
{
    public class GalleryListModel
    {
        public VwAlbum Album { get; set; }
        public int AlbumId { get; set; }
        public VwImage Image { get; set; }
        public string UserId { get; set; }
    }

    #region Image

    public class ImageCreateModel
    {
        [Required]
        public string EncryptedIV;

        [Required]
        public int AlbumId { get; set; }

        [Required]
        public System.DateTime CreationDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ESecretKey { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Signature { get; set; }

        [Required]
        public string ThumbNail { get; set; }

        [Required]
        public string Title { get; set; }
    }

    public class ImageEditModel
    {
        [Required]
        public string EncryptedIV;

        [Required]
        public string Description { get; set; }

        [Required]
        public string ESecretKey { get; set; }

        public int ID { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Signature { get; set; }

        [Required]
        public string ThumbNail { get; set; }

        [Required]
        public string Title { get; set; }
    }

    public class ImageListModel
    {
        [Required]
        public System.DateTime CreationDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string EncryptedIV { get; set; }

        [Required]
        public string ESecretKey { get; set; }

        public int ID { get; set; }

        [Required]
        public System.DateTime ModifiedOn { get; set; }

        [Required]
        public string Path { get; set; }

        public string Signature { get; set; }

        [Required]
        public string ThumbNail { get; set; }

        [Required]
        public string Title { get; set; }
    }

    #endregion Image

    #region Album

    public class AlbumCreateModel
    {
        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string UserId { get; set; }
    }

    public class AlbumEditModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public int ID { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }

        [Required]
        public string Title { get; set; }
    }

    public class AlbumListModel
    {
        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ID { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }

        [Required]
        public string Title { get; set; }

        public string UserId { get; set; }
    }

    #endregion Album
}

public class ImageViewModel
{
    public string AlbumName { get; set; }
    public IEnumerable<ImageListModel> Images { get; set; }
}