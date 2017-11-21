using JQGrid4U.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using JQGrid4U.BL;
using System.Data;

namespace JQGrid4U.Controllers
{

    public class MonitController : Controller
    {
		MonitBusinessLogic MonitBL = new MonitBusinessLogic();

        [HttpGet]
        [SessionExpire]
        public ActionResult Index()
		{
			return View();
		}

		public JsonResult SelectMonitParam(int siteId = 0, string type = null, string status = null, string Acknow = "0")
		{
			string[] stat = status.ToLower().Split(',');
			//if status equals to all and type equals to all
			if (stat.Contains("all") && type.ToLower() == "all")
			{
                //return Json(MonitBL.Monits.Where(p => p.siteId == siteId && (p.isAck == Acknow)).OrderBy(x => x.Status).ThenBy(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
                //return Json(MonitBL.Monits.Where(p => (p.isAck == Acknow)).OrderBy(x => x.Status).ThenBy(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
                return Json(MonitBL.Monits.Where(p => p.siteId == siteId && (p.isAck == Acknow || Acknow == "0")).OrderBy(x => x.Status).ThenBy(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
            }
			//if status not equal all
			else
			{
				//if status not equal to all
				if (stat.Contains("all"))
					if (type.ToLower() != null && stat.Contains("all"))
						return Json(MonitBL.Monits.Where(p => p.siteId == siteId && stat.Contains(p.Status.ToLower())).OrderByDescending(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
					else
						return Json(MonitBL.Monits.Where(p => p.siteId == siteId && p.Typ.ToLower() == type.ToLower() && (p.isAck == Acknow || Acknow == "0")).OrderByDescending(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
				else
					if (type.ToLower() == "all")
					return Json(MonitBL.Monits.Where(p => p.siteId == siteId && stat.Contains(p.Status.ToLower()) && (p.isAck == Acknow || Acknow == "0")).OrderByDescending(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
				else
					return Json(MonitBL.Monits.Where(p => p.siteId == siteId && stat.Contains(p.Status.ToLower()) && (p.isAck == Acknow || Acknow == "0") && p.Typ.ToLower() == type.ToLower()).OrderByDescending(p => p.LastUpdate).ToList(), JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult SelectMonit()
		{
			return Json(MonitBL.Monits.ToList(), JsonRequestBehavior.AllowGet);
		}

        public JsonResult LogResolve(LogResolve Log)
        {
            var cur_date = MonitBL.tblDeviceHistory.FirstOrDefault(o => o.deviceSerial == Log.Serial)?.deviceInstallDate ?? DateTime.Now;
            //var cur_date = MonitBL.tblDeviceHistory.Where(x => x.deviceID == "R120").Select(o => o.deviceInstallDate);
            DateTime cur_date2 = DateTime.Now;
            string NewSerial = GenerateNewSerial();
            string final_serial = (Log.Serial == "System Generated" ? NewSerial : Log.Serial);
            //bool isExist = MonitBL.Monits.Any(x => x.serialNo == final_serial) ? true : false;
            try
            {
                //if (Log.Action == "Replaced" && Log.Typ == "Pulley" && isExist)
                //{
                //    return Json(new { isError = "T", message = "Serial No already exist!" });
                //}
                //else
                //{

                if (Log.NoteTypeID == 3)
                {
                    MonitBL.InsertResolveLogReplace(new LogResolve { LogDate = cur_date, DeviceID = Log.DeviceID, Serial = Log.org_serial, NoteTypeID = Log.NoteTypeID });
                }
                MonitBL.InsertResolveLog(new LogResolve { LogDate = cur_date, DeviceID = Log.DeviceID, Serial = final_serial });
                MonitBL.UpdateResolveDevice(new LogResolve { DeviceID = Log.DeviceID, Serial = final_serial, NoteTypeID = Log.NoteTypeID });
                MonitBL.LogNoteAck(new LogResolve { Serial = final_serial, LogDate = cur_date, Note = Log.Note, userID = Log.userID });
                return Json(new { isError = "F", message = "Device successfully fixed!" });
            }
            catch (Exception ex)
            {
                return Json(new { isError = "T", message = "Failed fixing device!" });
            }
        }
        public JsonResult LogNoteResolve(LogResolve Log)
        {
            DateTime cur_date = DateTime.Now;

            try
            {
                MonitBL.LogNoteAck(new LogResolve { Serial = Log.Serial, LogDate = cur_date, Note = Log.Note, userID = Log.userID });
                return Json(new { isError = "F", message = "Note successfully recorded!" });
            }
            catch (Exception e)
            {
                return Json(new { isError = "T", message = e.Message });
            }
        }
        private string GenerateNewSerial()
        {
            List<int> list_of_serial = new List<int>();
            foreach (var itm in MonitBL.Monits.Select(o => o.serialNo))
                list_of_serial.Add(Convert.ToInt32(itm));

            return list_of_serial.Count > 0 ? string.Format("{0:00000000}", list_of_serial.Max() + 1) : "00000001";
        }
        public JsonResult SelectNoteList(string Serialno)
        {
            return Json(MonitBL.NoteList.Where(x => x.Serial == Serialno).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}