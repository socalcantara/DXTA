using System;
using System.Linq;
using System.Web.Mvc;
using JQGrid4U.BL;


namespace JQGrid4U.Controllers
{
	public class ParameterController : Controller
	{

		ParameterBusinessLogic ParameterBL = new ParameterBusinessLogic();
        UserBusinessLogic UserBL = new UserBusinessLogic();

        // GET: Parameter
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
                    return Redirect("~/Account/Unauthor");
                }

                return View();
            }
        }


		public JsonResult SelectParameter()
		{
			return Json(ParameterBL.Parameters.ToList(), JsonRequestBehavior.AllowGet);
		}

		public string UpdateParameter(Parameter Parameter)
		{


			string msg;
			int vlevel;
			if (ModelState.IsValid)
			{

				vlevel = ParameterBL.CriticalLevelParameter();
				Session["criticallevel"] = vlevel;


				if (ParameterBL.UpdateParameter(Parameter) > 0)
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


	}
}