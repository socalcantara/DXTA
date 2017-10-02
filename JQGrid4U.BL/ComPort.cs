using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace JQGrid4U.BL
{
	public class ComPort
	{

		public int ID { get; set; }
		public string Name { get; set; }
	

	}



	public class ComPortBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		public IEnumerable<ComPort> ComPorts
		{
			get
			{
				List<ComPort> ComPorts = new List<ComPort>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					SqlCommand cmdObj = new SqlCommand("select * from tblComPort", conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{


						ComPort ComPort = new ComPort();
						ComPort.ID = Convert.ToInt32(readerObj["ID"]);
						ComPort.Name = readerObj["Name"].ToString();
						ComPorts.Add(ComPort);
					}
				}
				return ComPorts;
			}
		}




		public string ComPortNumber()
		{
			string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
			string vComPort = "";
			using (SqlConnection conObj = new SqlConnection(conStr))
			{

				string qry;

				qry = "select comPort from tblParameter";
				SqlCommand cmdObj = new SqlCommand(qry, conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();
				if (readerObj["comPort"] != null)
				{
					vComPort = readerObj["comPort"].ToString();
				}
			}

			return vComPort;


		}



	}

}