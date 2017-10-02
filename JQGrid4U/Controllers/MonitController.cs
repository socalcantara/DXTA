using JQGrid4U.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using JQGrid4U.BL;
using System.Data;

namespace JQGrid4U.Controllers
{
    public class MonitController : Controller
    {
		//

		MonitBusinessLogic MonitBL = new MonitBusinessLogic();

		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}


		//public JsonResult SelectMonitParam(int siteId = 0)
		//{
		//	return Json(MonitBL.Monits.Where(p => p.siteId == siteId).ToList(), JsonRequestBehavior.AllowGet);
		//}
		public JsonResult SelectMonitParam(int siteId = 0, string type = null, string status = null)
		{
			string[] stat = status.ToLower().Split(',');
			//if status equals to all and type equals to all
			if (stat.Contains("all") && type.ToLower() == "all")
			{
				return Json(MonitBL.Monits.Where(p => p.siteId == siteId).OrderBy(x => x.Status).ThenBy(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
			}
			//if status not equal all
			else
			{
				//if status not equal to all
				if (stat.Contains("all"))
					if (type.ToLower() != null && stat.Contains("all"))
						return Json(MonitBL.Monits.Where(p => p.siteId == siteId && stat.Contains(p.Status.ToLower())).OrderByDescending(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
					else
						return Json(MonitBL.Monits.Where(p => p.siteId == siteId && p.Typ.ToLower() == type.ToLower()).OrderByDescending(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
				else
					if (type.ToLower() == "all")
					return Json(MonitBL.Monits.Where(p => p.siteId == siteId && stat.Contains(p.Status.ToLower())).OrderByDescending(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
				else
					return Json(MonitBL.Monits.Where(p => p.siteId == siteId && stat.Contains(p.Status.ToLower()) && p.Typ.ToLower() == type.ToLower()).OrderByDescending(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult SelectMonit()
		{
			return Json(MonitBL.Monits.ToList(), JsonRequestBehavior.AllowGet);
		}
		public class status
		{
			public eStatus stats { get; set; }
			public int status_int { get; set; }
		}
		public enum eStatus { Critical = 1, Warning = 2, OK = 3 }
		public class Car
		{
			public int CarId { get; set; }

			public string CarName { get; set; }

			public CarCategory CarCategory { get; set; }
		}

		public enum CarCategory
		{
			None = 0,
			kLowRange = 1,
			kMidRange = 2,
			kHighRange = 3
		}
	}
}