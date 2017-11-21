using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL;
using JQGrid4U.Models;
using System.Text;


namespace JQGrid4U.Controllers
{
    [SessionExpire]
    public class UserPreferencesController : Controller
    {
        UserBusinessLogic UserBL = new UserBusinessLogic();
        // GET: UserPreferences
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult UpdateUserPreferences(User User)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if password is updated
                    if (UserBL.UpdateUserPreferences(User) > 0)
                    {
                        return Json(new { isError = "F", message = "User details has been updated!" });
                    }
                    else
                    {
                        return Json(new { isError = "T", message = "Could not update data" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { isError = "T", message = "Could not update data." });
                }
            }
            else
            {
                //msg = "Sorry! Validation Error";
                return Json(new { isError = "T", message = "Validation Error." });
            }
        }
    }
}