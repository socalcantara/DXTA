using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;

namespace JQGrid4U.BL
{
	public class UserLog
	{
		public int ID { get; set; }
		public string EmailAdd { get; set; }
		public DateTime LogDate { get; set; }
		public int Typ { get; set; }
		public string IPadd { get; set; }
	}

	public class UserLogBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;	


		public IEnumerable<UserLog> UserLogs
		{
			get
			{
				List<UserLog> UserLogs = new List<UserLog>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					SqlCommand cmdObj = new SqlCommand("select * from tblUserLog", conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						UserLog UserLog = new UserLog();
						UserLog.ID = Convert.ToInt32(readerObj["ID"]);												
						UserLog.EmailAdd = readerObj["emailAdd"].ToString();
						UserLog.LogDate = Convert.ToDateTime(readerObj["logdate"].ToString());
						UserLog.Typ = Convert.ToInt32(readerObj["typ"].ToString());
						UserLogs.Add(UserLog);
					}
				}
				return UserLogs;
			}
		}


		public int InsertUserLog(UserLog UserLog)
		{
			
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspInsertUserLog", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;			
				cmdObj.Parameters.Add(new SqlParameter("@EmailAdd", UserLog.EmailAdd));				
				cmdObj.Parameters.Add(new SqlParameter("@LogDate", UserLog.LogDate));
				cmdObj.Parameters.Add(new SqlParameter("@Typ", UserLog.Typ));
				cmdObj.Parameters.Add(new SqlParameter("@IpAdd", UserLog.IPadd));
				conObj.Open();				
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}




	}


	}
