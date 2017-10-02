using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JQGrid4U.BL.Models;
using JQGrid4U.BL;
using System.Data.SqlClient;
using System.Configuration;

namespace JQGrid4U.Controllers
{
	public class GraphDataParamController : Controller
	{

		DeviceABusinessLogic DeviceABL = new DeviceABusinessLogic();
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

		JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
		private static List<DataPoint> _dataPoints = new List<DataPoint>();


		// GET: GraphDataParam

		public ActionResult Index()
		{
			return View();
		}


		public ActionResult SelectDeviceAParam(string deviceID)
		{

			ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAll(deviceID).ToList(), _jsonSetting);
			List<DeviceA> objList = DeviceABL.DeviceAll(deviceID).ToList();
			//return Json(objList, JsonRequestBehavior.AllowGet);
			//ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAs.ToList(), _jsonSetting);
			//ViewBag.DataPoints = JsonConvert.SerializeObject(_db.Points.ToList(), _jsonSetting);
			return View(Json(objList, JsonRequestBehavior.AllowGet));
		}

		public ActionResult SelectDeviceBParam(string deviceID, string fromdate, string todate)
		{

			ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAllDate(deviceID, fromdate, todate).ToList(), _jsonSetting);
			IList<DeviceA> objList = DeviceABL.DeviceAllDate(deviceID, fromdate, todate).ToList();
			//return Json(objList, JsonRequestBehavior.AllowGet);
			//ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAs.ToList(), _jsonSetting);
			//ViewBag.DataPoints = JsonConvert.SerializeObject(_db.Points.ToList(), _jsonSetting);
			return View(Json(objList, JsonRequestBehavior.AllowGet));


		}

		public JsonResult SelectDeviceBParamJson(string deviceID, string fromdate, string todate)
		{

			ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAll(deviceID).ToList(), _jsonSetting);
			List<DeviceA> objList = DeviceABL.DeviceAllDate(deviceID, fromdate, todate).ToList();

			//return Json(objList, JsonRequestBehavior.AllowGet);
			//ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAs.ToList(), _jsonSetting);
			//ViewBag.DataPoints = JsonConvert.SerializeObject(_db.Points.ToList(), _jsonSetting);

			return Json(objList, JsonRequestBehavior.AllowGet);


		}



		public JsonResult SelectDeviceAParamJson(string deviceID)
		{
			ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAll(deviceID).ToList(), _jsonSetting);
			List<DeviceA> objList = DeviceABL.DeviceAll(deviceID).ToList();
			return Json(objList, JsonRequestBehavior.AllowGet);


		}



		public JsonResult SelectLatestDeviceValue(string deviceID)
		{
			//ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAll(deviceID, Session["country"].ToString()).ToList(), _jsonSetting);
			List<DeviceA> objList = DeviceABL.LatestValue(deviceID).ToList();
			return Json(objList, JsonRequestBehavior.AllowGet);


		}




	}
}