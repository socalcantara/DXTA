using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL;
using System.Text;

namespace JQGrid4U.Controllers
{
    [SessionExpire]
    public class DboardController : Controller
	{
		DashBoardBusinessLogic DashBoardBL = new DashBoardBusinessLogic();
        UserBusinessLogic Users = new UserBusinessLogic();

        
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
		public JsonResult SelectDashBoard(int UserID = 0)
		{
            List<int> listofSites = new List<int>();
            if ((Session["isAdmin"].ToString()) == "True")
            {
                listofSites.AddRange(Users.UserSub.Select(x => x.siteID).ToList());
            }
            else
            {
                listofSites.AddRange(Users.UserSub.Where(x => x.userID == UserID).Select(x => x.siteID).ToList());
            }
			return Json(DashBoardBL.DashBoards.Where(x => listofSites.Contains(x.siteID)).ToList(), JsonRequestBehavior.AllowGet);
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