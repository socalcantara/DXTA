using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JQGrid4U.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return RedirectPermanent(Url.Content("~/api/Product"));
				return RedirectPermanent(Url.Content("~/Dboard"));

		}
    }

        
    
}