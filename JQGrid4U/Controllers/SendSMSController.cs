using System;
using System.Linq;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using JQGrid4U.BL;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;



namespace JQGrid4U.Controllers
{
	public class SendSMSController : Controller
	{


		//SMSBusinessLogic SmsBL = new SMSBusinessLogic();
		MailBusinessLogic MailBL = new MailBusinessLogic();
		ParameterBusinessLogic ParamBL = new ParameterBusinessLogic();





		public ActionResult Index()
		{



			if (Session["user"] == null)
			{
				return Redirect("~/Account/Login");
			}
			else
			{
				return View();
			}
		}

		public ActionResult Thankyou()
		{

			TempData["msg"] = "<script>alert('Send SMS Alert Succesfully');</script>";
			return View();

		}


		public ActionResult DoesNotExist()
		{

			TempData["msg"] = "<script>alert('Send SMS Alert Failed, Communication Port Does Not Exist');</script>";
			return View();

		}

		//[HttpPost]
		//public RedirectToRouteResult Index(JQGrid4U.BL.MailModel _objModelMail)
		//{
		//	if (ModelState.IsValid)
		//	{

		//		string vbody = MailBL.SmsMsg();
		//		string vComPort = ParamBL.ComPort();

		//		SerialPort port = new SerialPort();

		//		if (vbody.Trim().Length > 1)
		//		{
		//			try
		//			{

		//				string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		//				using (SqlConnection conObj = new SqlConnection(conStr))
		//				{



		//					string qry1;
		//					string msg = "";
		//					string vMobileno = "";
		//					qry1 = "select distinct mobileno from tblUser where autoemailalert='true'";
		//					SqlCommand cmdObj = new SqlCommand(qry1, conObj);
		//					conObj.Open();
		//					SqlDataReader readerObj = cmdObj.ExecuteReader();

		//					while (readerObj.Read())
		//					{

		//						bool isSend = false;

		//						try
		//						{

		//							//	SMSa objclas = new SMSa();
		//							//	objclas.sendMsg(port, "09324016842", "test msg");
		//							vMobileno = readerObj["mobileno"].ToString();
		//							if (port.IsOpen == true)
		//							{
		//								port.Close();
		//							}
		//							port.PortName = vComPort;
		//							port.BaudRate = 9600;
		//							port.Parity = Parity.None;
		//							port.StopBits = StopBits.One;
		//							port.DataBits = 8;
		//							port.Handshake = Handshake.RequestToSend;
		//							port.DtrEnable = true;
		//							port.RtsEnable = true;
		//							port.NewLine = System.Environment.NewLine;
		//							msg = vbody;
		//							port.Open();
		//							//SMSa objclas = new SMSa();
		//							//objclas.OpenPort(port.ToString(), 9600, 8, 300, 300);
		//							if (port.IsOpen == true)
		//							{
		//								port.Write("AT" + System.Environment.NewLine);
		//								port.Write("AT+CMGF=1" + System.Environment.NewLine);
		//								port.Write("AT+CMGS=" + "\u0022" + vMobileno + "\u0022" + System.Environment.NewLine);
		//								//port.Write(msg + Convert.ToChar(26));
		//								port.Write(msg + char.ConvertFromUtf32(26) + "\r");
		//							}
		//							//SMSa objclas = new SMSa();
		//							//objclas.OpenPort(port.ToString(), 9600, 8, 300, 300);
		//							//objclas.sendMsg(port, vMobileno, msg);





		//						}
		//						catch (Exception ex)
		//						{
		//							string msg1 = ex.Message.ToString().Substring(16, 14).ToLower();

		//							if (msg1 != null)
		//							{
		//								return RedirectToAction("doesnotexist");
		//							}
		//							//throw ex;
		//						}

		//					}
		//					conObj.Close();
		//				}


		//				return RedirectToAction("thankyou");

		//			}
		//			catch (SmtpException e)
		//			{

		//				return RedirectToAction("thankyou");
		//			}

		//		}
		//		else
		//		{
		//			return RedirectToAction("thankyou");
		//		}



		//	}
		//	else
		//	{
		//		return RedirectToAction("thankyou");
		//	}
		//}

		public JsonResult SendSMS(JQGrid4U.BL.MailModel _objModelMail)
		{
			string responseText = "";
			bool success = false;
			if (ModelState.IsValid)
			{

				string vbody = MailBL.SmsMsg();
				string vComPort = ParamBL.ComPort();

				SerialPort port = new SerialPort();

				if (vbody.Trim().Length > 1)
				{
					try
					{

						string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
						using (SqlConnection conObj = new SqlConnection(conStr))
						{
							string qry1;
							string msg = "";
							string vMobileno = "";
							qry1 = "select distinct mobileno from tblUser where autoemailalert='true'";
							SqlCommand cmdObj = new SqlCommand(qry1, conObj);
							conObj.Open();
							SqlDataReader readerObj = cmdObj.ExecuteReader();

							while (readerObj.Read())
							{

								bool isSend = false;

								try
								{

									//	SMSa objclas = new SMSa();
									//	objclas.sendMsg(port, "09324016842", "test msg");
									vMobileno = readerObj["mobileno"].ToString();
									if (port.IsOpen == true)
									{
										port.Close();
									}
									port.PortName = vComPort;
									port.BaudRate = 9600;
									port.Parity = Parity.None;
									port.StopBits = StopBits.One;
									port.DataBits = 8;
									port.Handshake = Handshake.RequestToSend;
									port.DtrEnable = true;
									port.RtsEnable = true;
									port.NewLine = System.Environment.NewLine;
									msg = vbody;
									port.Open();
									//SMSa objclas = new SMSa();
									//objclas.OpenPort(port.ToString(), 9600, 8, 300, 300);
									if (port.IsOpen == true)
									{
										port.Write("AT" + System.Environment.NewLine);
										port.Write("AT+CMGF=1" + System.Environment.NewLine);
										port.Write("AT+CMGS=" + "\u0022" + vMobileno + "\u0022" + System.Environment.NewLine);
										//port.Write(msg + Convert.ToChar(26));
										port.Write(msg + char.ConvertFromUtf32(26) + "\r");
									}
									//SMSa objclas = new SMSa();
									//objclas.OpenPort(port.ToString(), 9600, 8, 300, 300);
									//objclas.sendMsg(port, vMobileno, msg);





								}
								catch (Exception ex)
								{
									string msg1 = ex.Message.ToString().Substring(16, 14).ToLower();

									if (msg1 != null)
									{
										success = false; responseText = "Send SMS alert failed, Communication port does not exist.";
									}
								}

							}
							conObj.Close();
						}
						

					}
					catch (SmtpException e)
					{
						success = false;
						responseText = "Send SMS alert failed, Communication port does not exist.";
					}

				}
				else
				{
					success = false;
					responseText = "Send SMS alert failed, Communication port does not exist.";
				}
			}
			return Json(new { success = success, responseText = responseText });
		}






	}
}