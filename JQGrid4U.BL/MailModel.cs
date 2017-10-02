
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace JQGrid4U.BL
{
	public class MailModel
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
	}

	public class MailBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		int counter = 1;
		ParameterBusinessLogic ParamBL = new ParameterBusinessLogic();
		int vLevel = 0;
		
		public IEnumerable<MailModel> MailModels
		{
			get
			{
				List<MailModel> MailModels = new List<MailModel>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					DateTime vDate;
					vLevel = ParamBL.CriticalLevelParameter();
					string qry;
					qry = "select a.deviceStamp, b.deviceType as Value, a.deviceInternalTemp as Item from tblDeviceData a "
						+ "left join tblDeviceType b on a.deviceID=b.deviceID where a.deviceInternalTemp>=" + vLevel + "order by devicestamp";
					SqlCommand cmdObj = new SqlCommand(qry, conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
						MailModel MailModel = new MailModel();
						MailModel.From = "alan.sepe@delonix.com.au";
						MailModel.To = "haroldtoralba@gmail.com";
						MailModel.Subject = "Critical Device";
						MailModel.Body = "Device ID  " + readerObj["deviceInterlalTemp"].ToString() + " Temperature : " +  readerObj["deviceInterlalTemp"].ToString();
						MailModels.Add(MailModel);
						counter = counter + 1;
					}
				}
				return MailModels;
			}
		}

		public string SmsMsg()
		{

			int vLevel = 0;
			string qrybody = "";
			using (SqlConnection conObj = new SqlConnection(conStr))
			{

				string qry;
				vLevel = ParamBL.CriticalLevelParameter();
				qry = "select a.deviceID, a.deviceStamp, a.deviceInternalTemp as Item from tblDeviceData a "
					+ "where a.deviceInternalTemp>=" + vLevel + " order by devicestamp";
				SqlCommand cmdObj = new SqlCommand(qry, conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();


				while (readerObj.Read())
				{
					qrybody = qrybody + "Device Type  " + readerObj["deviceid"].ToString() + " Temperature : " + readerObj["item"].ToString() + ";  ";
					qrybody = qrybody + "DateTime : " + string.Format("{0:MM dd yyyy hh:mm}", Convert.ToDateTime(readerObj["deviceStamp"].ToString()));
				}
			}

			


			return qrybody;


		}

		public string mailBody ()
		{

			int vLevel = 0;
			string qrybody = "<table>";
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				
				string qry;
				vLevel = ParamBL.CriticalLevelParameter();
				qry = "select a.deviceID, a.deviceStamp, a.deviceInternalTemp as Item from tblDeviceData a "
					+ "where a.deviceInternalTemp>=" + vLevel + " order by devicestamp";
				SqlCommand cmdObj = new SqlCommand(qry, conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();
				

			while (readerObj.Read())
				{					
		qrybody =  qrybody + "<tr><td>" + " Device ID  " + readerObj["deviceid"].ToString() + " Temperature : " + readerObj["item"].ToString() + "</td></tr>";
				}
			}

			qrybody = qrybody + "</table>";


			return qrybody;
		}


	}

}
