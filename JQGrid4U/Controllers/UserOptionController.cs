using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL.Models;
using JQGrid4U.BL;

namespace JQGrid4U.Controllers
{
    public class UserOptionController : Controller
    {

    UserBusinessLogic UserBL = new UserBusinessLogic();

    public ActionResult Index()
    {

      return Json(UserBL.Users.ToList(), JsonRequestBehavior.AllowGet);

    }



  }
}