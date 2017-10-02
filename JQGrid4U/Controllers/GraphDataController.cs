using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JQGrid4U.BL.Models;
using JQGrid4U.BL;

namespace JQGrid4U.Controllers
{
	public class GraphDataController : Controller
	{
		// GET: GraphData

		DeviceABusinessLogic DeviceABL = new DeviceABusinessLogic();

		JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
		private static List<DataPoint> _dataPoints = new List<DataPoint>();




		public ActionResult Index()
		{


			if (Session["user"] == null)
			{
				return Redirect("~/Account/Login");
			}
			else
			{



				try
				{
					ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAs.ToList(), _jsonSetting);
					//ViewBag.DataPoints = JsonConvert.SerializeObject(_db.Points.ToList(), _jsonSetting);

					return View();
				}
				catch (System.Data.Entity.Core.EntityException)
				{
					return View("Error");
				}
				catch (System.Data.SqlClient.SqlException)
				{
					return View("Error");
				}



			}



		}


		public JsonResult SelectDeviceA()
		{
			return Json(DeviceABL.DeviceAs.ToList(), JsonRequestBehavior.AllowGet);
		}


		public JsonResult SelectDeviceB(string deviceId = "R1")
		{
			string country = "US";
			if (Session["country"] != null)
			{
				return Json(DeviceABL.DeviceAll(deviceId).ToList(), JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(DeviceABL.DeviceAll(deviceId).ToList(), JsonRequestBehavior.AllowGet);
			}

			//return Json(DeviceABL.DeviceAll(deviceId, Session["country"].ToString()).ToList(), JsonRequestBehavior.AllowGet);
		}


		public JsonResult SelectDeviceBParamJson(string deviceID, string fromdate, string todate)
		{

			ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAll(deviceID).ToList(), _jsonSetting);
			List<DeviceA> objList = DeviceABL.DeviceAllDate(deviceID, fromdate, todate).ToList();


			return Json(objList, JsonRequestBehavior.AllowGet);


		}





		public ActionResult DataFromDataBase()
		{
			try
			{
				ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAs.ToList(), _jsonSetting);
				//ViewBag.DataPoints = JsonConvert.SerializeObject(_db.Points.ToList(), _jsonSetting);

				return View();
			}
			catch (System.Data.Entity.Core.EntityException)
			{
				return View("Error");
			}
			catch (System.Data.SqlClient.SqlException)
			{
				return View("Error");
			}
		}


	}
}