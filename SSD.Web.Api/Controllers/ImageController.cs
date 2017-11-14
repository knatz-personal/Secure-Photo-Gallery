using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataExchange;
using DataExchange.EntityModel;
using DataExchange.Repositories;
using SSD.Web.Api.Models;

namespace SSD.Web.Api.Controllers
{
    public class ImageController : ApiController
    {
        private ImageRepository _repository;
        private DataManager db1;

        #region Constructor

        public ImageController()
        {
            db = new DataManager(new AppDbContext());
        }

        public ImageController(DataManager testDataManager)
        {
            db = testDataManager;
        }

        #endregion Constructor

        #region Verbs

        public IHttpActionResult Delete(int id)
        {
            if (id > 0)
            {
                var toDelete = db.Images.Read(s => s.ID == id);
                if (toDelete != null)
                {
                    var status = db.Images.Delete(toDelete);
                    if (status)
                    {
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }

        [ResponseType(typeof(ImageListModel))]
        public IHttpActionResult Get(int id)
        {
            if (id > 0)
            {
                var item = db.Images.Read(m => m.ID == id);
                if (item != null)
                {
                    return Ok(new ImageListModel()
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Description = item.Description,
                        Path = item.Path,
                        ThumbNail = item.ThumbNail,
                        ModifiedOn = item.ModifiedOn,
                        CreationDate = item.CreationDate,
                        ESecretKey = item.ESecretKey,
                        EncryptedIV = item.EncryptedIV,
                        Signature = item.Signature
                    });
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("api/CanAccessImage")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult GetAccessCheck(string filename, string userId)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                List<Image> images = db.Images.Find(m => m.ThumbNail == filename || m.Path == filename);
                if (images != null)
                {
                    foreach (var image in images)
                    {
                        bool isVerified = image.Album.UserId == userId;
                        if (isVerified)
                        {
                            return Ok(true);
                        }
                    }
                }
            }
            return BadRequest();
        }

        [Route("api/My/Images")]
        [ResponseType(typeof(ImageViewModel))]
        public IHttpActionResult GetUserImages(int albumId)
        {
            if (albumId > 0)
            {
                var album = db.Albums.Read(al => al.ID == albumId);

                if (album == null) return NotFound();

                var albumName = album.Title;
                var list = db.Images.Find(m => m.AlbumId == albumId);
                var images = list.Select(item => new ImageListModel()
                {
                    ID = item.ID,
                    Title = item.Title,
                    Description = item.Description,
                    Path = item.Path,
                    ThumbNail = item.ThumbNail,
                    ModifiedOn = item.ModifiedOn,
                    CreationDate = item.CreationDate,
                    ESecretKey = item.ESecretKey,
                    EncryptedIV = item.EncryptedIV,
                    Signature = item.Signature
                });
                return Ok(new ImageViewModel()
                {
                    AlbumName = albumName,
                    Images = images
                });
            }
            return BadRequest();
        }

        [ResponseType(typeof(ImageCreateModel))]
        public IHttpActionResult Post([FromBody] ImageCreateModel model)
        {
            IHttpActionResult response = null;

            int initialCount = db.Images.Count();

            db.Images.Create(new Image
            {
                Title = model.Title,
                Description = model.Description,
                Path = model.Path,
                ThumbNail = model.ThumbNail,
                CreationDate = model.CreationDate,
                ModifiedOn = model.CreationDate,
                ESecretKey = model.ESecretKey,
                EncryptedIV = model.EncryptedIV,
                Signature = model.Signature,
                AlbumId = model.AlbumId
            });

            int endCount = db.Images.Count();

            var strMsg =
                $"Image record was created, initial count: {initialCount}, final count: {endCount}";
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Created);
            return ResponseMessage(message);
        }

        #endregion Verbs

        #region Properties

        private DataManager db { get; }

        #endregion Properties
    }
}