using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JQGrid4U.BL;

namespace JQGrid4U.Controllers
{
	public class SessionExpireAttribute : ActionFilterAttribute
	{
        UserBusinessLogic UserBL = new UserBusinessLogic();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			HttpContext ctx = HttpContext.Current;
			// check  sessions here
			if (HttpContext.Current.Session["user"] == null)
			{
				filterContext.Result = new RedirectResult("~/Account/Login");
				return;
			}
            else
            {
                HttpContext.Current.Session["rowfilter"] = UserBL.Users.SingleOrDefault(x => x.EmailAdd == HttpContext.Current.Session["user"].ToString()).iNumRows.ToString();
            }
            base.OnActionExecuting(filterContext);
		}
	}
}