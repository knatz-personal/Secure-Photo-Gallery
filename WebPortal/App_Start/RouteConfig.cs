using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //enabling attribute routing
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "NoAction",
                  "{controller}/{q}/{pg}/{ps}",
                 new { controller = "Home", action = "Index", q = UrlParameter.Optional, pg = UrlParameter.Optional, ps = UrlParameter.Optional },
                new { pg = @"\d+", ps = @"\d+" }
                );
        }
    }
}