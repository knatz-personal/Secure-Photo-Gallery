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
    public class TownController : ApiController
    {
        public TownController(DataManager db)
        {
            this.db = db;
        }

        public TownController()
        {
            db = new DataManager(new AppDbContext());
        }

        private DataManager db { get; }

        #region Verbs

        public IHttpActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var toDelete = db.Towns.Read(s => s.ID.ToString() == id);
                if (toDelete != null)
                {
                    var status = db.Towns.Delete(toDelete);
                    if (status)
                    {
                        return Ok();
                    }
                }
            }
            return NotFound();
        }

        [ResponseType(typeof(List<TownListBindingModel>))]
        public IHttpActionResult Get()
        {
            var list = db.Towns.ListAll();
            if (list.Any())
            {
                return Ok(list.Select(item => new TownListBindingModel()
                {
                    Name = item.Name,
                    Id = item.ID
                }));
            }
            return NotFound();
        }

        [ResponseType(typeof(TownListBindingModel))]
        public IHttpActionResult Get(string id)
        {
            if (id != null)
            {
                var item = db.Towns.Read(w => w.ID.ToString() == id);
                if (item != null)
                {
                    return Ok(new TownListBindingModel()
                    {
                        Name = item.Name,
                        Id = item.ID
                    });
                }
            }
            return NotFound();
        }

        [ResponseType(typeof(TownCreateBindingModel))]
        public IHttpActionResult Post([FromBody] TownCreateBindingModel model)
        {
            IHttpActionResult response = null;

            int initialCount = db.Towns.Count();

            db.Towns.Create(new Town
            {
                Name = model.Name
            });

            int endCount = db.Towns.Count();

            var strMsg = string.Format("Message: {0}, initial count: {1}, final count: {2}", "Town value was created", initialCount,
                endCount);
            var msg = Request.CreateResponse(HttpStatusCode.Created, strMsg);
            msg.Headers.Location = new Uri(Request.RequestUri.ToString());

            response = ResponseMessage(msg);
            return response;
        }

        [ResponseType(typeof(TownEditBindingModel))]
        public IHttpActionResult Put(string id, [FromBody] TownEditBindingModel model)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (model != null)
                {
                    var status = db.Towns.Update(new Town()
                    {
                        ID = model.Id,
                        Name = model.Name
                    });
                    if (status)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            return NotFound();
        }

        #endregion Verbs
    }
}