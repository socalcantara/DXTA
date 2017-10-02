
using System;
using System.Linq;
using System.Web.Mvc;
using JQGrid4U.BL;

namespace JQGrid4U.Controllers
{


	public class UserController : Controller
	{
		// GET: CutOff

		UserBusinessLogic UserBL = new UserBusinessLogic();
		//
		// GET: /Supplier/
		public ActionResult Index()
		{


			if (Session["user"] == null)
			{
				return Redirect("~/Account/Login");
			}
			else
			{
				int xlevel = 1;
				xlevel = UserBL.UserLevel(Session["user"].ToString());

				if (xlevel < 5)
				{
					return Redirect("~/Account/Login");
				}

				return View();
			}


		}

		public JsonResult SelectUser()
		{




			return Json(UserBL.Users.ToList(), JsonRequestBehavior.AllowGet);
		}





		public string InsertUser([Bind(Exclude = "ID")]User User)
		{
			string msg;
			if (ModelState.IsValid)
			{
				if (UserBL.InsertUser(User) > 0)
				{
					msg = "Data Inserted Successfully";
				}
				else
				{
					msg = "Error. Could Not Insert Data";
				}
			}
			else
			{
				msg = "Sorry! Validation Error";
			}

			return msg;
		}

		public string UpdateUser(User User)
		{
			string msg;
			if (ModelState.IsValid)
			{
				if (UserBL.UpdateUser(User) > 0)
				{
					msg = "Data Updated Successfully";
				}
				else
				{
					msg = "Error. Could Not Update Data";
				}
			}
			else
			{
				msg = "Sorry! Validation Error";
			}

			return msg;
		}


		public string DeleteUser(int Id)
		{
			string msg;

			if (UserBL.DeleteUser(Id) > 0)
			{
				msg = "Data Deleted Successfully";
			}
			else
			{
				msg = "Error. Could Not Delete Data";
			}

			return msg;
		}



	}


}