using System;
using System.Linq;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using JQGrid4U.BL;
using System.Data.SqlClient;


namespace JQGrid4U.Controllers
{
	public class SendMailerController : Controller
	{



		MailBusinessLogic MailBL = new MailBusinessLogic();

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Thankyou()
		{

			TempData["msg"] = "<script>alert('Send Email Alert Succesfully');</script>";
			return View();

		}

		//Send Email to Users
		public JsonResult SendEmail(JQGrid4U.BL.MailModel _objModelMail)
		{
			string responseText = "";
			bool success = false;
			if (ModelState.IsValid)
			{
				string vbody = MailBL.mailBody();
				string vMailTo = "";
				MailMessage mail = new MailMessage();
				mail.From = new MailAddress("haroldtoralba@gmail.com");
				mail.IsBodyHtml = true;
				mail.Subject = "Critical Device/s";
				mail.Body = vbody;
				SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = new System.Net.NetworkCredential("haroldtoralba@gmail.com", "sepealan123");
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.EnableSsl = true;
				smtp.Timeout = 30000;
				if (vbody.Trim().Length > 1)
				{
					try
					{

						string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
						using (SqlConnection conObj = new SqlConnection(conStr))
						{

							string qry1;
							qry1 = "select distinct emailadd from tblUser where autoemailalert='true'";
							SqlCommand cmdObj = new SqlCommand(qry1, conObj);
							conObj.Open();
							SqlDataReader readerObj = cmdObj.ExecuteReader();

							while (readerObj.Read())
							{
								vMailTo = readerObj["emailadd"].ToString();
								mail.To.Add(vMailTo);
								//smtp.Send(mail);
							}
							conObj.Close();
						}
						responseText = "Email has been sent successfully.";
						success = true;
					}

					catch (SmtpException e)
					{
						responseText = "A problem has been occured while sending email.";
						success = false;
					}
					finally
					{
						smtp.Send(mail);
					}
				}
			}
			return Json(new { success = success, responseText = responseText });
		}


	}
}