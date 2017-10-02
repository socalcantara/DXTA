using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JQGrid4U.BL;

namespace JQGrid4U
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
		protected void Application_BeginRequest()
		{
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
			Response.Cache.SetNoStore();
		}
		//void Session_End(object sender, EventArgs e)
		//{
		//	// Code that runs when a session ends.   
		//	// Note: The Session_End event is raised only when the sessionstate mode  
		//	// is set to InProc in the Web.config file. If session mode is set to StateServer   
		//	// or SQLServer, the event is not raised.  
		//	//Response.Redirect("Login.aspx");

		//}
	}
}
