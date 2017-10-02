using System;
using System.Linq;
using System.Web.Mvc;
using JQGrid4U.BL;



namespace JQGrid4U.Controllers
{
    public class DeviceDataController : Controller
    {

		DeviceDataBusinessLogic DeviceDataBL = new DeviceDataBusinessLogic();

		// GET: DeviceData
		public ActionResult Index()
		{
			return View();
		}


		public JsonResult SelectDeviceData()
		{
			return Json(DeviceDataBL.DeviceDatas .ToList(), JsonRequestBehavior.AllowGet);
		}


	}
}