using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JQGrid4U
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


			

			routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dboard", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default1",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Supplier", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                      "DefaultApi3",
                      "{controller}/{action}/{id}",
                      new { controller = "SubSupplier", action = "SelectSupplierSub", id = UrlParameter.Optional }
                  );

			routes.MapRoute(
				name: "Default2",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "GraphDate", action = "SelectDeviceBParam", id = UrlParameter.Optional, fromDate = UrlParameter.Optional, toDate = UrlParameter.Optional }
				);


		}
    }
}
