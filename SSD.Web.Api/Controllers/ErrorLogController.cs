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
    public class ErrorLogController : ApiController
    {
        #region Properties

        private DataManager db { get; }

        #endregion Properties

        public string Username { get; set; }

        public ErrorLogController()
        {
            db = new DataManager(new AppDbContext());
        }

        // POST: api/ErrorLog
        [ResponseType(typeof(LogBindingModel))]
        public IHttpActionResult Post([FromBody]LogBindingModel log)
        {
            IHttpActionResult response = null;

            int initialCount = db.ErrorLogs.Count();

            db.ErrorLogs.Create(new ErrorLog()
            {
                DateTriggered = log.DateAndTime,
                InnerException = log.ExceptionAndTrace,
                Message = log.Message,
                Username = log.Username
            });

            int endCount = db.ErrorLogs.Count();

            var strMsg = $"Message: error log value was created, initial count: {initialCount}, final count: {endCount}";
            var msg = Request.CreateResponse(HttpStatusCode.Created, strMsg);
            msg.Headers.Location = new Uri(Request.RequestUri.ToString());

            response = ResponseMessage(msg);
            return response;
        }
    }
}