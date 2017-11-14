using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DataExchange;
using DataExchange.EntityModel;

namespace SSD.Web.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            using (var db = new DataManager(new AppDbContext()))
            {
                db.ErrorLogs.Create(new ErrorLog()
                {
                    DateTriggered = DateTime.Now,
                    InnerException = exception.InnerException?.Message + " " + exception.StackTrace,
                    Message = exception.Message,
                    Username = User.Identity.Name == string.Empty ? "Guest" : User.Identity.Name
                });
            }

            Server.ClearError();
        }

        protected void Application_Start()
        {
#if DEBUG
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
#endif
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}