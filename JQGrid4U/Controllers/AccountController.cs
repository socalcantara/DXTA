using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using JQGrid4U.Models;
using JQGrid4U.BL;
using System.Data;
using System.Data.SqlClient;
using System.Web.Helpers;
using System.Text;

namespace JQGrid4U.Controllers
{
	[Authorize]
    public class AccountController : Controller
	{

		UserBusinessLogic UserBL = new UserBusinessLogic();
		ParameterBusinessLogic ParamBL = new ParameterBusinessLogic();
		UserLogBusinessLogic UserLogBL = new UserLogBusinessLogic();
		UserLogFailBusinessLogic UserLogFailBL = new UserLogFailBusinessLogic();

		[AllowAnonymous]
		public ActionResult UnAuthor()
		{
			return View();
		}

		public AccountController()
			: this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
		{
		}

		public AccountController(UserManager<ApplicationUser> userManager)
		{
			UserManager = userManager;
		}

		[AllowAnonymous]
		public ActionResult InvalidEmailAdd()
		{

			TempData["msg"] = "<script>alert('Invalid Email Adress');</script>";
			return View();

		}

		public UserManager<ApplicationUser> UserManager { get; private set; }



		[AllowAnonymous]
		public ActionResult UnAuthorized(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}


		[HttpGet]
		[AllowAnonymous]
		public JsonResult IsUserNameExist(string userName)
		{

			bool isExist = UserBL.Users.Where(m => m.EmailAdd.ToLowerInvariant().Equals(userName.ToLower())).FirstOrDefault() != null;
			return Json(isExist, JsonRequestBehavior.AllowGet);
		}

        [HttpGet]
		[AllowAnonymous]
		public bool IsValidEmailAdd(string email)
		{
			try
			{
				var emailChecked = new System.Net.Mail.MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}

		}



		//
		// GET: /Account/Parameter
		[AllowAnonymous]
		public ActionResult ChangePwd(string returnUrl)
		{
			if (Session["user"] == null)
			{
				return Redirect("~/Account/login");
			}
			else
			{
				string vUser = "";
				vUser = Session["user"].ToString();
				ViewBag.EmailAdd = vUser;

				return View();
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> ChangePwd(HashViewModel model, string returnUrl)
		{

			string vNewX = "";
			int vLen = 0;
			string vEmailAdd = "";
			string vOld = "";
			string vNew = "";
			string vNewH = "";
			string vNewSha256 = "";
			string vNewSha1 = "";
			string vSalt = "";
			string vConfirm = "";
			//vNewH = Crypto.Hash(vNew, "MD5");
			//vNewSha256 = Crypto.SHA256(vNew);
			//vNewSha1  = Crypto.SHA1(vNew);
			//vSalt = Crypto.GenerateSalt();


			vNewX = Crypto.HashPassword(vNewSha1);
			vLen = vNewX.Length;
			if (model.EmailAdd != null)
			{
				vEmailAdd = model.EmailAdd;
			}

			if (model.OldPwd == null)
			{
				ViewBag.EmailAdd = model.EmailAdd.ToString();
				//ModelState.AddModelError("", "Old Password Is Required");
			}
			else
			{
				vOld = model.OldPwd.ToString();
			}

			if (model.NewPwd == null)
			{
				ViewBag.EmailAdd = model.EmailAdd.ToString();
				//ModelState.AddModelError("", "New Password Is Required");
			}
			else
			{
				vNew = model.NewPwd.ToString();
			}

			if (model.ConfirmPwd == null)
			{
				ViewBag.EmailAdd = model.EmailAdd.ToString();
				//ModelState.AddModelError("", "Confirmed Password Is Required");
			}
			else
			{
				vConfirm = model.ConfirmPwd.ToString();
			}


			if (ModelState.IsValid)
			{

				if (IsValidEmailAdd(vEmailAdd) == true)
				{
					if (vNew != vConfirm)
					{
						ViewBag.EmailAdd = vEmailAdd;
						ModelState.AddModelError("", "New Password does not match with Confirmed Password");
					}
					else
					{
						if (model.OldPwd == model.NewPwd)
						{
							ViewBag.EmailAdd = vEmailAdd;
							ModelState.AddModelError("", "Old Password is the same with New Password");
						}
						else
						{
							vNew = model.NewPwd;
							vNewSha1 = Crypto.SHA1(vNew);
							string pwdX = Crypto.SHA1(model.OldPwd);
							Boolean IsValid = UserBL.isValidUser(vEmailAdd, pwdX);
							if (IsValid == true)
							{
								if (vNew.Length >= 8)
								{
									string str = vNewSha1;
									vNewSha1 = UserBL.Puz1(vNewSha1, vEmailAdd);
									byte[] bytes = UTF8Encoding.ASCII.GetBytes(str);
									string str2 = UTF8Encoding.ASCII.GetString(bytes);

									UserBL.UpdatePwd(vEmailAdd, vNewSha1);
									//UserBL.UserSendMail(vEmailAdd, vNew);
									UserBL.UserSendMail("alan.sepe@delonix.com.au", vNew);

									//return Redirect("~/dboard");
									ViewBag.EmailAdd = vEmailAdd;
									TempData["success"] = "Your password has been successfully changed.";
								}
								else
								{
									ViewBag.EmailAdd = vEmailAdd;
									ModelState.AddModelError("", "Password must contain at least 8 characters");
								}
							}
							else
							{
								ViewBag.EmailAdd = vEmailAdd;
								ModelState.AddModelError("", "Invalid Old Password");
							}
						}
					}

				}
				else
				{
					ViewBag.Emailadd = model.EmailAdd.ToString();
					ModelState.AddModelError("", "Invalid Email Address");
				}
			}
			return View(model);
		}






		//
		// GET: /Account/Parameter
		[AllowAnonymous]
        
        public ActionResult Parameter(string returnUrl)
		{
			//ViewBag.ReturnUrl = returnUrl;
			//ViewBag.TempCriticalLevel = ParamBL.CriticalLevelParameter();
			//ViewBag.PulleyDeviceExpiry = ParamBL.PulleyExpiry();
			//ViewBag.RollerDeviceExpiry = ParamBL.RollerExpiry();
			//ViewBag.EmailAlertInterval = 3;
			//ViewBag.ComPort = ParamBL.ComPort();
			List<Parameter> objList = ParamBL.Parameters.ToList();

			return View(objList);


		}




        //
        // GET: /Account/Parameters
        //[HttpPost]
        [AllowAnonymous]
        public ActionResult Parameters(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			ViewBag.TempCriticalLevel = ParamBL.CriticalLevelParameter();
			ViewBag.PulleyDeviceExpiry = ParamBL.PulleyExpiry();
			ViewBag.RollerDeviceExpiry = ParamBL.RollerExpiry();
			ViewBag.EmailAlertInterval = 3;
			ViewBag.ComPort = ParamBL.ComPort();
			return View();
		}

		//
		// POST: /Account/Parameters
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> Parameters(ParameterViewModel model, string returnUrl)
		{

			string xurl;
			ParameterBusinessLogic ParamBL = new ParameterBusinessLogic();
			if (ModelState.IsValid)
			{
				int vRet = 0;
				int vLevel = Convert.ToInt32(model.TempCriticalLevel.ToString());
				int vPulley = Convert.ToInt32(model.PulleyDeviceExpiry.ToString());
				int vRoller = Convert.ToInt32(model.RollerDeviceExpiry.ToString());
				int vInterval = Convert.ToInt32(model.EmailAlertInterval.ToString());
				string vCom = model.ComPort.ToString();

				string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					SqlCommand cmdObj = new SqlCommand("uspUpdateParameterNoParam", conObj);
					cmdObj.CommandType = CommandType.StoredProcedure;
					cmdObj.Parameters.Add(new SqlParameter("@TempCriticalLevel", vLevel));
					cmdObj.Parameters.Add(new SqlParameter("@EmailAlertInterval", vInterval));
					cmdObj.Parameters.Add(new SqlParameter("@RollerDeviceExpiry", vRoller));
					cmdObj.Parameters.Add(new SqlParameter("@PulleyDeviceExpiry", vPulley));
					cmdObj.Parameters.Add(new SqlParameter("@Comport", vCom));
					conObj.Open();
					vRet = Convert.ToInt32(cmdObj.ExecuteScalar());
				}


			}
			// If we got this far, something failed, redisplay form
			//return View(model);
			return Redirect("~/account/Parameter");

		}




		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;


			string vIPadd = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
			int xcounter = 0;
			int xdelay = 0;
			xcounter = 0;//  UserLogFailBL.FailedCounterIP(vIPadd);
			DateTime DueDate = DateTime.Now;
			ViewBag.duetime = DueDate;
			if (Session["user"] != null)
			{
				return Redirect("~/Dboard");
			}
			else
			{
				return View();
			}
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{

			string vIPadd = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
			string xurl;
			UserBusinessLogic UserBL = new UserBusinessLogic();
			if (ModelState.IsValid)
			{
				string pwdX = Crypto.SHA1(model.Password);
				Boolean IsValid = UserBL.isValidUser(model.UserName, pwdX);
				Boolean isDisabled = UserBL.isAcctDisabled(model.UserName, model.Password);
				if (isDisabled == true)
				{
					ModelState.AddModelError("", "Your account is currently disabled");

					IList<UserLogFail> UserLogFails = new List<UserLogFail>();
					UserLogFail Ulog = new UserLogFail();
					Ulog.EmailAdd = model.UserName;				
					Ulog.ErrorMsg = "Your account is currently disabled";
					Ulog.LogDate = DateTime.Now;
					Ulog.SessionId = Session.SessionID.ToString();
					Ulog.Typ = 0;
					Ulog.IPadd = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
					UserLogFails.Add(Ulog);
					UserLogFailBL.InsertUserLogFail(Ulog);

				}
				else
				{

					//int xcounter = 0;
					//xcounter = UserLogFailBL.FailedCounter(model.UserName, Session.SessionID.ToString());
					//if (xcounter >= 10 || xcounter <50 )
					//{						
					//System.Threading.Thread.Sleep(5000);
					//}


					if (IsValid == true)
					{


						string username = NewMethod(model);
						Session["user"] = model.UserName;
                        Session["userID"] = UserBL.Users.SingleOrDefault(x => x.EmailAdd == username).ID.ToString();
                        Session["rowfilter"] = UserBL.Users.SingleOrDefault(x => x.EmailAdd == username).iNumRows.ToString();
                        Session["fullname"] = UserBL.Users.SingleOrDefault(x => x.EmailAdd == username).FirstName.ToString() + " " + UserBL.Users.SingleOrDefault(x => x.EmailAdd == username).SurName.ToString();

						IList<UserLog> UserLogs = new List<UserLog>();
						UserLog Ulog = new UserLog();
						Ulog.EmailAdd = model.UserName;
						Ulog.LogDate = DateTime.Now;
						Ulog.Typ = 0;
						Ulog.IPadd = vIPadd;
						UserLogs.Add(Ulog);
						UserLogBL.InsertUserLog(Ulog);
						UserLogFailBL.ClearedLogFail(System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString());


						//  Session["cutoff"] = UserBL.ActiveCutOff(model.UserName);					
						xurl = "dboard";
						return RedirectToLocal(xurl);
					}
					else
					{

						IList<UserLogFail> UserLogFails = new List<UserLogFail>();
						UserLogFail Ulog = new UserLogFail();
						Ulog.EmailAdd = model.UserName;						
						Ulog.ErrorMsg = "Invalid username or password.";
						Ulog.LogDate = DateTime.Now;
						Ulog.Typ = 0;
						Ulog.SessionId = Session.SessionID.ToString();
						Ulog.IPadd = vIPadd;
						UserLogFails.Add(Ulog);
						UserLogFailBL.InsertUserLogFail(Ulog);
						Session["user"] = model.UserName;

						vIPadd = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();

						DateTime DueDate = DateTime.Now;

						ViewBag.duetime = DateTime.Now.AddSeconds(1);

						ModelState.AddModelError("", "Invalid username or password.");


					}
				}
			}
			// If we got this far, something failed, redisplay form
			return View(model);
		}


		[AllowAnonymous]
		public JsonResult SelectFailedCounter()
		{
			string vIp = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
			string UserNameX = "";
			if (Session["user"] != null)
			{
				UserNameX = Session["user"].ToString();
			}
			return Json(UserLogFailBL.UserLogFailx(vIp, UserNameX).ToList(), JsonRequestBehavior.AllowGet);
		}



		private static string NewMethod(LoginViewModel model)
		{
			return model.UserName;
		}

		//
		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser() { UserName = model.UserName };
				var result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await SignInAsync(user, isPersistent: false);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					AddErrors(result);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// POST: /Account/Disassociate
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
		{
			ManageMessageId? message = null;
			IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
			if (result.Succeeded)
			{
				message = ManageMessageId.RemoveLoginSuccess;
			}
			else
			{
				message = ManageMessageId.Error;
			}
			return RedirectToAction("Manage", new { Message = message });
		}

		//
		// GET: /Account/Manage
		public ActionResult Manage(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
				: message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
				: message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
				: message == ManageMessageId.Error ? "An error has occurred."
				: "";
			ViewBag.HasLocalPassword = HasPassword();
			ViewBag.ReturnUrl = Url.Action("Manage");
			return View();
		}

		//
		// POST: /Account/Manage
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Manage(ManageUserViewModel model)
		{
			bool hasPassword = HasPassword();
			ViewBag.HasLocalPassword = hasPassword;
			ViewBag.ReturnUrl = Url.Action("Manage");
			if (hasPassword)
			{
				if (ModelState.IsValid)
				{
					IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
					if (result.Succeeded)
					{
						return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
					}
					else
					{
						AddErrors(result);
					}
				}
			}
			else
			{
				// User does not have a password so remove any validation errors caused by a missing OldPassword field
				ModelState state = ModelState["OldPassword"];
				if (state != null)
				{
					state.Errors.Clear();
				}

				if (ModelState.IsValid)
				{
					IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
					if (result.Succeeded)
					{
						return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
					}
					else
					{
						AddErrors(result);
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// POST: /Account/ExternalLogin
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		//
		// GET: /Account/ExternalLoginCallback
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var user = await UserManager.FindAsync(loginInfo.Login);
			if (user != null)
			{
				await SignInAsync(user, isPersistent: false);
				return RedirectToLocal(returnUrl);
			}
			else
			{
				// If the user does not have an account, then prompt the user to create an account
				ViewBag.ReturnUrl = returnUrl;
				ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
				return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
			}
		}

		//
		// POST: /Account/LinkLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkLogin(string provider)
		{
			// Request a redirect to the external login provider to link a login for the current user
			return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
		}

		//
		// GET: /Account/LinkLoginCallback
		public async Task<ActionResult> LinkLoginCallback()
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
			if (loginInfo == null)
			{
				return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
			}
			var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
			if (result.Succeeded)
			{
				return RedirectToAction("Manage");
			}
			return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
		}

		//
		// POST: /Account/ExternalLoginConfirmation
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Manage");
			}

			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return View("ExternalLoginFailure");
				}
				var user = new ApplicationUser() { UserName = model.UserName };
				var result = await UserManager.CreateAsync(user);
				if (result.Succeeded)
				{
					result = await UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded)
					{
						await SignInAsync(user, isPersistent: false);
						return RedirectToLocal(returnUrl);
					}
				}
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		//
		// POST: /Account/LogOff
		[AllowAnonymous]
		public ActionResult LogOff(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}
		// POST: /Account/LogOff
		[AllowAnonymous]
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			Session["user"] = null;
			HttpContext.Session.Clear();
			HttpContext.Session.Abandon();
			HttpContext.Session.RemoveAll();
			Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
			//HttpContext.User = null;
			//System.Web.Security.FormsAuthentication.SignOut();
			return RedirectToAction("Login", "Account");
			//AuthenticationManager.SignOut();
		}

		//
		// GET: /Account/ExternalLoginFailure
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}

		[ChildActionOnly]
		public ActionResult RemoveAccountList()
		{
			var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
			ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
			return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && UserManager != null)
			{
				UserManager.Dispose();
				UserManager = null;
			}
			base.Dispose(disposing);
		}

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private async Task SignInAsync(ApplicationUser user, bool isPersistent)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private bool HasPassword()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null)
			{
				return user.PasswordHash != null;
			}
			return false;
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			Error
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		private class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
        #endregion
    }
}