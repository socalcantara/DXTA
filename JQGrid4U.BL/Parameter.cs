using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace JQGrid4U.BL
{
	public class Parameter
	{

		public int ID { get; set; }
		public int TempCriticalLevel { get; set; }
		public int EmailAlertInterval { get; set; } 
		public int RollerDeviceExpiry { get; set; }
		public int PulleyDeviceExpiry { get; set; }
		public string ComPort { get; set; } 


	}

	public class ParameterBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		
		public IEnumerable<Parameter> Parameters
		{
			get
			{
				List<Parameter> Parameters = new List<Parameter>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					SqlCommand cmdObj = new SqlCommand("select * from tblParameter", conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						Parameter Parameter = new Parameter();
						Parameter.ID = Convert.ToInt32(readerObj["ID"].ToString());
						Parameter.TempCriticalLevel = Convert.ToInt32(readerObj["Tempcriticallevel"].ToString());
						Parameter.EmailAlertInterval = Convert.ToInt32(readerObj["emailAlertInterval"].ToString());
						Parameter.RollerDeviceExpiry = Convert.ToInt32(readerObj["RollerDeviceExpiry"].ToString());
						Parameter.PulleyDeviceExpiry = Convert.ToInt32(readerObj["PulleyDeviceExpiry"].ToString());
						Parameter.ComPort = readerObj["ComPort"].ToString();
						Parameters.Add(Parameter);
					}
				}
				return Parameters;
			}
		}


		public string ComPort()
		{

			string vComPort = "";
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("select comPort from tblParameter", conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();
				readerObj.Read();
				//vComPort = readerObj["comPort"].ToString();
				if (!String.IsNullOrEmpty ( readerObj["comPort"].ToString() ) )
				{
					vComPort = readerObj["comPort"].ToString();
				}


			}
			return vComPort; 


		}


		public int CriticalLevelParameter()
		{

			int vLevel = 0; 
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("select * from tblParameter", conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();
				while (readerObj.Read())
				{
					vLevel = Convert.ToInt32(readerObj["Tempcriticallevel"].ToString());
				}

			}
			return vLevel;


		}

		public int PulleyExpiry()
		{

			int vExpiry = 0;
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("select * from tblParameter", conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();
				while (readerObj.Read())
				{
					vExpiry = Convert.ToInt32(readerObj["PulleyDeviceExpiry"].ToString());
				}

			}
			return vExpiry;


		}



		public int RollerExpiry()
		{

			int vExpiry = 0;
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("select * from tblParameter", conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();
				while (readerObj.Read())
				{
					vExpiry = Convert.ToInt32(readerObj["RollerDeviceExpiry"].ToString());
				}

			}
			return vExpiry;


		}




		public int InsertParameter(Parameter Parameter)
		{
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspInsertParameter", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;				
				cmdObj.Parameters.Add(new SqlParameter("@TempCriticalLevel", Parameter.TempCriticalLevel));
				cmdObj.Parameters.Add(new SqlParameter("@EmailAlertInterval", Parameter.EmailAlertInterval));
				cmdObj.Parameters.Add(new SqlParameter("@RollerDeviceExpiry", Parameter.RollerDeviceExpiry));
				cmdObj.Parameters.Add(new SqlParameter("@PulleyDeviceExpiry", Parameter.PulleyDeviceExpiry));
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}

		public int UpdateParameter(Parameter Parameter)
		{
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspUpdateParameter", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;
				cmdObj.Parameters.Add(new SqlParameter("@ID", Parameter.ID));
				cmdObj.Parameters.Add(new SqlParameter("@TempCriticalLevel", Parameter.TempCriticalLevel));
				cmdObj.Parameters.Add(new SqlParameter("@EmailAlertInterval", Parameter.EmailAlertInterval));
				cmdObj.Parameters.Add(new SqlParameter("@RollerDeviceExpiry", Parameter.RollerDeviceExpiry));
				cmdObj.Parameters.Add(new SqlParameter("@PulleyDeviceExpiry", Parameter.PulleyDeviceExpiry));
				cmdObj.Parameters.Add(new SqlParameter("@Comport", Parameter.ComPort));
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}


	}

	}
