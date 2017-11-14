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
    public class AlbumController : ApiController
    {
        #region Constructor

        public AlbumController()
        {
            db = new DataManager(new AppDbContext());
        }

        #endregion Constructor

        #region Properties

        private DataManager db { get; }

        #endregion Properties

        #region Verbs

        public IHttpActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var toDelete = db.Albums.Read(s => s.ID.ToString() == id);
                if (toDelete != null)
                {
                    var status = db.Albums.Delete(toDelete);
                    if (status)
                    {
                        return Ok();
                    }
                }
            }
            return NotFound();
        }

        [ResponseType(typeof(AlbumListModel))]
        public IHttpActionResult Get(string albumId)
        {
            if (albumId != null)
            {
                var item = db.Albums.Read(w => w.ID.ToString() == albumId);
                if (item != null)
                {
                    return Ok(new AlbumListModel
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Description = item.Description,
                        CreationDate = item.CreationDate,
                        ModifiedOn = item.ModifiedOn,
                        UserId = item.UserId
                    });
                }
            }
            return BadRequest();
        }

        [Route("api/My/Albums")]
        [ResponseType(typeof(List<AlbumListModel>))]
        public IHttpActionResult GetUserAlbums(string userId)
        {
            if (userId != null)
            {
                var list = db.Albums.Find(w => w.UserId == userId);
                if (list != null)
                {
                    return Ok(list.Select(item => new AlbumListModel
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Description = item.Description,
                        CreationDate = item.CreationDate,
                        ModifiedOn = item.ModifiedOn,
                        UserId = item.UserId
                    }));
                }
            }
            return BadRequest();
        }

        [ResponseType(typeof(AlbumCreateModel))]
        public IHttpActionResult Post([FromBody] AlbumCreateModel model)
        {
            IHttpActionResult response = null;

            var initialCount = db.Albums.Count();

            db.Albums.Create(new Album
            {
                Title = model.Title,
                Description = model.Description,
                CreationDate = model.CreationDate,
                ModifiedOn = model.CreationDate,
                UserId = model.UserId
            });

            var endCount = db.Albums.Count();

            var strMsg = string.Format("Message: {0}, initial count: {1}, final count: {2}", "album record was created",
                initialCount, endCount);
            var msg = Request.CreateResponse(HttpStatusCode.Created, strMsg);
            msg.Headers.Location = new Uri(Request.RequestUri.ToString());

            response = ResponseMessage(msg);
            return response;
        }

        [ResponseType(typeof(AlbumEditModel))]
        public IHttpActionResult Put(string id, [FromBody] AlbumEditModel model)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (model != null)
                {
                    var status = db.Albums.Update(new Album
                    {
                        ID = model.ID,
                        Title = model.Title,
                        Description = model.Description,
                        ModifiedOn = model.ModifiedOn
                    });
                    if (status)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        #endregion Verbs
    }
}