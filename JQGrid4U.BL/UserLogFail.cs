using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;

namespace JQGrid4U.BL
{
	public class UserLogFail
	{
		public int ID { get; set; }
		public string EmailAdd { get; set; }		
		public string ErrorMsg { get; set; }
		public DateTime LogDate { get; set; }
		public int Typ { get; set; }
		public string SessionId { get; set; }
		public string IPadd { get; set; }
	}

	public class UserLogFailBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;


		public IEnumerable<UserLogFail> UserLogFails
		{
			get
			{
				List<UserLogFail> UserLogFails = new List<UserLogFail>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					SqlCommand cmdObj = new SqlCommand("select * from tblUserLogFail", conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						UserLogFail UserLogFail = new UserLogFail();
						UserLogFail.ID = Convert.ToInt32(readerObj["ID"]);
						UserLogFail.EmailAdd = readerObj["emailAdd"].ToString();
						UserLogFail.ErrorMsg = readerObj["errorMsg"].ToString();					
						UserLogFail.LogDate = Convert.ToDateTime(readerObj["logdate"].ToString());
						UserLogFail.Typ = Convert.ToInt32(readerObj["typ"].ToString());
						UserLogFail.SessionId = readerObj["SessionId"].ToString();
						UserLogFails.Add(UserLogFail);
					}
				}
				return UserLogFails;
			}
		}


		public int FailedCounter(string pUserName, string pSession)
		{
			int counter = 0;

			using (SqlConnection conObj = new SqlConnection(conStr))
			{


                //SqlCommand cmdObj = new SqlCommand("select count(ID) as tot from tblUserLogFail where emailadd='" + pUserName + "' and sessionId='" + pSession + "'", conObj);
                SqlCommand cmdObj = new SqlCommand("spfailedcountera", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@username", pUserName));
                cmdObj.Parameters.Add(new SqlParameter("@sessionid", pSession));
                conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();
				readerObj.Read();
				if (readerObj["tot"].ToString() != null)
				{
					counter = Convert.ToInt32(readerObj["tot"].ToString());
				}

			}
			
			return counter;

		}

		public int FailedCounterIP(string IPadd)
		{
			int counter = 0;
			DateTime vFrom = DateTime.Today;
			DateTime vTo = vFrom.AddHours(23);
			vTo = vTo.AddMinutes(59);
			vTo = vTo.AddSeconds(59);			
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				//str1 = "select count(ID) as tot from tblUserLogFail where IPadd = '" + IPadd + "' and logdate>='" + vFrom + "' and logDate<='" + vTo + "'";
				SqlCommand cmdObj = new SqlCommand("spfailedcounterB", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@ipadd", IPadd));
                cmdObj.Parameters.Add(new SqlParameter("@fromdate", vFrom));
                cmdObj.Parameters.Add(new SqlParameter("@todate", vTo));
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();
				readerObj.Read();
				if (readerObj["tot"].ToString() != null)
				{
					counter = Convert.ToInt32(readerObj["tot"].ToString());
				}

			}

			return counter;

		}



		public int ClearedLogFail(string IPadd)
		{

			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspDeleteUserLogFail", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@ipadd", IPadd));
				//cmdObj.CommandText = "delete from tblUserLogFail where IPadd='" + IPadd + "'";
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}

		public int InsertUserLogFail(UserLogFail UserLogFail)
		{

			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspInsertUserLogFail", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;
				cmdObj.Parameters.Add(new SqlParameter("@EmailAdd", UserLogFail.EmailAdd));				
				cmdObj.Parameters.Add(new SqlParameter("@ErrorMsg", UserLogFail.ErrorMsg));
				cmdObj.Parameters.Add(new SqlParameter("@LogDate", UserLogFail.LogDate));
				cmdObj.Parameters.Add(new SqlParameter("@Typ", UserLogFail.Typ));
				cmdObj.Parameters.Add(new SqlParameter("@SessionId", UserLogFail.SessionId));
				cmdObj.Parameters.Add(new SqlParameter("@IPadd", UserLogFail.IPadd));
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}

		public IEnumerable<UserLogFail> UserLogFailx(string IpAdd, string UserName)
		{

			List<UserLogFail> UserLogFailx = new List<UserLogFail>();
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				int netCount = 1;
				int grossCount = 1;
                //SqlCommand cmdObj = new SqlCommand("select count(id) as tot from tblUserLogFail where ipAdd='" + IpAdd + "' and emailadd='" + UserName + "'", conObj);
                SqlCommand cmdObj = new SqlCommand("spfailedcounterC", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@username", UserName));
                cmdObj.Parameters.Add(new SqlParameter("@ipadd", IpAdd));
                conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();

				while (readerObj.Read())
				{
					grossCount = Convert.ToInt32(readerObj["tot"].ToString());
					if (grossCount < 10)
					{
						netCount = 0;
					}


					if (grossCount >= 10 & grossCount < 50)
					{
						netCount = 5;
					}
					if (grossCount >= 50 & grossCount < 100)
					{
						netCount = 30;
					}

					if (grossCount >= 100 & grossCount < 200)
					{
						netCount = 60;
					}

					if (grossCount >= 200 & grossCount < 400)
					{
						netCount = 120;
					}

					if (grossCount >= 400 & grossCount < 800)
					{
						netCount = 240;
					}

					if (grossCount >= 800 & grossCount < 1600)
					{
						netCount = 480;
					}

					if (grossCount >= 1600 & grossCount < 32000)
					{
						netCount = 960;
					}



					UserLogFail UserLogFail = new UserLogFail();
					UserLogFail.ID = netCount;
					UserLogFail.EmailAdd = "test@gamil.com";
					UserLogFail.ErrorMsg = "testmsg";					
					UserLogFail.LogDate = DateTime.Today;
					UserLogFail.Typ = 0;
					UserLogFail.SessionId = "SessionTest";
					UserLogFail.IPadd = "1111";
					UserLogFailx.Add(UserLogFail);
				}
			}
			return UserLogFailx;

		}



	}


}
