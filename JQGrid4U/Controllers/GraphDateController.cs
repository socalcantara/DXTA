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
    public class GraphDateController : Controller
    {
		DeviceABusinessLogic DeviceABL = new DeviceABusinessLogic();
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
		// GET: GraphDate

		public ActionResult Index()
		{
			return View();
		}





		public ActionResult SelectDeviceBParam(string deviceID, string fromdate, string todate)
		{

			ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAllDate(deviceID, fromdate, todate).ToList(), _jsonSetting);
			List<DeviceA> objList = DeviceABL.DeviceAllDate(deviceID, fromdate, todate).ToList();
			//return Json(objList, JsonRequestBehavior.AllowGet);
			//ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceABL.DeviceAs.ToList(), _jsonSetting);
			//ViewBag.DataPoints = JsonConvert.SerializeObject(_db.Points.ToList(), _jsonSetting);
			return View(objList);


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


	}
}