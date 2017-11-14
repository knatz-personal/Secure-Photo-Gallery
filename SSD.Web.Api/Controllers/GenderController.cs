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
    public class GenderController : ApiController
    {
        #region Constructor

        public GenderController(DataManager db)
        {
            this.db = db;
        }

        public GenderController()
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
                var toDelete = db.Genders.Read(s => s.ID.ToString() == id);
                if (toDelete != null)
                {
                    var status = db.Genders.Delete(toDelete);
                    if (status)
                    {
                        return Ok();
                    }
                }
            }
            return NotFound();
        }

        [ResponseType(typeof(List<GenderListBindingModel>))]
        public IHttpActionResult Get()
        {
            var list = db.Genders.ListAll();
            if (list.Any())
            {
                return Ok(list.Select(item => new GenderListBindingModel()
                {
                    Name = item.Name,
                    Id = item.ID
                }));
            }
            return NotFound();
        }

        [ResponseType(typeof(GenderListBindingModel))]
        public IHttpActionResult Get(string id)
        {
            if (id != null)
            {
                var item = db.Genders.Read(w => w.ID.ToString() == id);
                if (item != null)
                {
                    return Ok(new GenderListBindingModel()
                    {
                        Name = item.Name,
                        Id = item.ID
                    });
                }
            }
            return NotFound();
        }

        [ResponseType(typeof(GenderCreateBindingModel))]
        public IHttpActionResult Post([FromBody] GenderCreateBindingModel model)
        {
            IHttpActionResult response = null;

            int initialCount = db.Genders.Count();

            db.Genders.Create(new Gender
            {
                Name = model.Name
            });

            int endCount = db.Genders.Count();

            var strMsg = string.Format("Message: {0}, initial count: {1}, final count: {2}", "gender value was created", initialCount,
                endCount);
            var msg = Request.CreateResponse(HttpStatusCode.Created, strMsg);
            msg.Headers.Location = new Uri(Request.RequestUri.ToString());

            response = ResponseMessage(msg);
            return response;
        }

        [ResponseType(typeof(GenderEditBindingModel))]
        public IHttpActionResult Put(string id, [FromBody] GenderEditBindingModel model)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (model != null)
                {
                    var status = db.Genders.Update(new Gender()
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