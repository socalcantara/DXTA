using System;
using System.Linq;
using System.Web.Mvc;
using JQGrid4U.BL;


namespace JQGrid4U.Controllers
{
	public class ParameterController : Controller
	{

		ParameterBusinessLogic ParameterBL = new ParameterBusinessLogic();

		// GET: Parameter
		public ActionResult Index()
		{
			return View();
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