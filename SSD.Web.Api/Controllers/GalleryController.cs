using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataExchange;
using DataExchange.EntityModel;
using SSD.Web.Api.Models;

namespace SSD.Web.Api.Controllers
{
    public class GalleryController : ApiController
    {
        #region Constructor

        public GalleryController()
        {
            db = new DataManager(new AppDbContext());
        }

        #endregion Constructor

        #region Verbs

        [ResponseType(typeof(List<GalleryListModel>))]
        public IHttpActionResult Get()
        {
            var list = db.Gallery.GetGallerys();

            if (list != null)
            {
                return Ok(list.Select(item => new GalleryListModel()
                {
                    AlbumId = item.AlbumId,
                    UserId = item.UserId,
                    Image = item.Image
                }));
            }
            return NotFound();
        }

        [ResponseType(typeof(GalleryListModel))]
        public IHttpActionResult Get(string id)
        {
            if (id != null)
            {
                var items = db.Gallery.GetUserAlbums(id);
                if (items != null)
                {
                    return Ok(items.Select(i => new GalleryListModel()
                    {
                        Album = i.Album,
                        UserId = i.UserId,
                        Image = i.Image
                    }));
                }
            }
            return NotFound();
        }

        #endregion Verbs

        #region Properties

        private DataManager db { get; }

        #endregion Properties
    }
}