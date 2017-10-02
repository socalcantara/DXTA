using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JQGrid4U.Controllers
{
	public class SessionExpireAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			HttpContext ctx = HttpContext.Current;
			// check  sessions here
			if (HttpContext.Current.Session["user"] == null)
			{
				filterContext.Result = new RedirectResult("~/Account/Login");
				return;
			}
			base.OnActionExecuting(filterContext);
		}
	}
}