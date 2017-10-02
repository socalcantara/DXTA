using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL.Models;
using JQGrid4U.BL;
using System.Configuration;

namespace JQGrid4U.Controllers
{
    public class ChartJSController : Controller
    {
        // GET: ChartJS

        DeviceBBusinessLogic DeviceBBL = new DeviceBBusinessLogic();
        string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;


        // GET: ChartJS
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SelectDeviceLabel(string deviceID, string fromdate, string todate)
        {

            //ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceBBL.Device(deviceID).ToList(), _jsonSetting);
            List<DeviceB> objList = DeviceBBL.DeviceLabelNew(deviceID, fromdate, todate).ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);


        }


        public JsonResult SelectDeviceFigureWoDate(string deviceID)
        {

            //ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceBBL.Device(deviceID).ToList(), _jsonSetting);
            List<DeviceB> objList = DeviceBBL.DeviceFigureWoDate(deviceID).ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);


        }

        public JsonResult SelectDeviceLabelWoDate(string deviceID)
        {

            //ViewBag.DataPoints = JsonConvert.SerializeObject(DeviceBBL.Device(deviceID).ToList(), _jsonSetting);
            List<DeviceB> objList = DeviceBBL.DeviceLabelWoDate(deviceID).ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);


        }



    }
}