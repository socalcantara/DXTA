using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL;

namespace JQGrid4U.Controllers
{
	public class DboardController : Controller
	{
		DashBoardBusinessLogic DashBoardBL = new DashBoardBusinessLogic();
		[SessionExpire]
		public ActionResult Index()
		{
			if (Session["user"] == null)
			{
				return Redirect("~/Account/login");
			}
			else
			{

				List<DashBoard> objList = DashBoardBL.DashBoards.ToList();

				return View(objList);
				//return View();
			}
		}
		//Get Dashboard List
		public JsonResult SelectDashBoard()
		{
			return Json(DashBoardBL.DashBoards.ToList(), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult IsSessionAlive()
		{
			if (Session.Contents.Count == 0)
			{
				return this.Json(new { IsAlive = false }, JsonRequestBehavior.AllowGet);
			}
			return this.Json(new { IsAlive = true }, JsonRequestBehavior.AllowGet);
		}
	}
}