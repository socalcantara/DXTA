using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL;
using System.Text;
using System.Net.Mail;
using JQGrid4U.Models;
using System.Web.Helpers;


namespace JQGrid4U.Controllers
{

    public class UserManagementController : Controller
	{
		UserBusinessLogic UserBL = new UserBusinessLogic();		
		AccountController account = new AccountController();
        RoleManagement RoleManagementTables = new RoleManagement();
        // GET: UserManagement
        [SessionExpire]
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
		//Get Users List to Display to Table
		public JsonResult GetUsersList()
		{
			return Json(UserBL.Users.ToList(), JsonRequestBehavior.AllowGet);
		}
		//Add Users to Table
		public JsonResult InsertUser([Bind(Exclude = "ID")]User User)
		{
			if (ModelState.IsValid)
			{
				try
				{
                    if (UserBL.InsertUser(User) > 0)
                    {
                        return Json(new { isError = "F", message = "New user has been added!" });
                    }
                    else
                        return Json(new { isError = "T", message = "Email address already exist." });
				}
				catch (Exception ex)
				{
					return Json(new { isError = "T", message = "Could not insert data." });
				}
			}
			else
			{
				return Json(new { isError = "T", message = "Could not insert data." });
			}
		}
		//Update User Details
		public JsonResult UpdateUser(User User)
		{
			string msg;
			if (ModelState.IsValid)
			{
				try
				{
					String vNewSha1 = "";
					if (User.Pwd != null)
					{
						String vNew = User.Pwd;
						vNewSha1 = Crypto.SHA1(vNew);
						string str = vNewSha1;
						vNewSha1 = UserBL.Puz1(vNewSha1, User.EmailAdd);
						byte[] bytes = UTF8Encoding.ASCII.GetBytes(str);
						string str2 = UTF8Encoding.ASCII.GetString(bytes);
					}


                    //if password is updated
                    if (UserBL.UpdateUser(new User { ID = User.ID, FirstName = User.FirstName, SurName = User.SurName, EmailAdd = User.EmailAdd, UserLevel = User.UserLevel, AutoEmailAlert = User.AutoEmailAlert, Mobileno = User.Mobileno, Disabled = User.Disabled, Pwd = (User.Pwd == "" || User.Pwd == null) ? "" : vNewSha1, site = User.site }) > 0)
                    {
                        UserBL.InsertRole(new User { ID = User.ID, UserRole = User.UserRole });
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
		//Delete User But not used.
		public JsonResult DeleteUser(String empid)
		{
			int Id = Convert.ToInt32(empid);

			if (UserBL.DeleteUser(Id) > 0)
			{
				return Json(new { isError = "F", message = "Data Deleted Successfully." });
			}
			else
			{
				return Json(new { isError = "T", message = "Error. Could Not Delete Data" });
			}
		}
		//Get Details per User
		public JsonResult GetUserDetails(int empid)
		{
			return Json(UserBL.Users.SingleOrDefault(x => x.ID == empid), JsonRequestBehavior.AllowGet);
		}
		//Send Test Emails
		public JsonResult SendTestMail(string MailTo)
		{
			try
			{
				MailMessage mail = new MailMessage();
				mail.From = new MailAddress("haroldtoralba@gmail.com");
				//mail.Sender = new MailAddress("haroldtoralba@gmail.com");
				//mail.To.Add("alan.sepe@delonix.com.au");
				mail.IsBodyHtml = true;
				mail.Subject = "Test Email";
				mail.Body = "This is a test to check if your email address is valid. Thanks.";
                //temporary send email to soc
                //mail.To.Add(MailTo);
                mail.To.Add("soc.alcantara@delonix.com.au");

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
				smtp.UseDefaultCredentials = false;

				smtp.Credentials = new System.Net.NetworkCredential("haroldtoralba@gmail.com", "sepealan123");
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.EnableSsl = true;
				smtp.Timeout = 30000;
				smtp.Send(mail);

				return Json(new { isError = "F", message = "Email sent successfully." });
			}
			catch (Exception ex)
			{
				return Json(new { isError = "T", message = "Invalid Email address." });
			}

		}
		//Change Password
		public String ChangePwd(HashViewModel model)
		{
			string vNew = "";
			string vNewSha1 = "";
			string vEmailAdd = "";
			string result = "";
			string vOld = model.OldPwd.ToString();

			if (ModelState.IsValid)
			{

				vNew = model.NewPwd;
				vNewSha1 = Crypto.SHA1(vNew);
				string pwdX = Crypto.SHA1(model.OldPwd);
				Boolean IsValid = UserBL.isValidUser(vEmailAdd, pwdX);
				if (IsValid == true)
				{

					string str = vNewSha1;
					vNewSha1 = UserBL.Puz1(vNewSha1, vEmailAdd);
					byte[] bytes = UTF8Encoding.ASCII.GetBytes(str);
					string str2 = UTF8Encoding.ASCII.GetString(bytes);

					UserBL.UpdatePwd(vEmailAdd, vNewSha1);
					//UserBL.UserSendMail(vEmailAdd, vNew);
					UserBL.UserSendMail("alan.sepe@delonix.com.au", vNew);
					result = "success";
				}
				else
				{
					ViewBag.EmailAdd = vEmailAdd;
					ModelState.AddModelError("", "Invalid Old Password");
				}

			}
			else
			{
				ViewBag.Emailadd = model.EmailAdd.ToString();
				ModelState.AddModelError("", "Invalid Email Address");
			}
			return result;
		}
        //GetSiteList
        public JsonResult GetSiteList(int empid = 0)
        {
            StringBuilder sb = new StringBuilder();
            List<UserSub> Sitelist = UserBL.SiteList.ToList();
            List<UserSub> HasSite = UserBL.UserSub.Where(x => x.userID == empid).ToList();
          
            foreach (var container in Sitelist)
            {
                if (HasSite.Any(x => container.siteID == x.siteID) == true)
                    sb.Append("<option value='" + container.siteID + "'selected>" + container.siteName + "</option>");
                else
                    sb.Append("<option value='" + container.siteID + "'>" + container.siteName + "</option>");
            }
            return Json(sb.ToString());

        }
        //GET All Role List
        public JsonResult GetUserRoleList(int empid = 0)
        {
            StringBuilder sb = new StringBuilder();
            List<Roles> Roles =  RoleManagementTables.RolesList.ToList();
            List<UserRoleAccess> HasRole = RoleManagementTables.UserRoleAccess.Where(x => x.UserID == empid).ToList();

            foreach (var container in Roles)
            {
                if (HasRole.Any(x => container.ID == x.RoleID) == true)
                    sb.Append("<option value='" + container.ID + "'selected>" + container.RoleName + "</option>");
                else
                    sb.Append("<option value='" + container.ID + "'>" + container.RoleName + "</option>");
            }
            return Json(sb.ToString());

        }

    }
	
}