using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Globalization;


namespace JQGrid4U.BL
{
	public class DeviceA
	{

		public string x { get; set; }
		public int y { get; set; }
		public string stat { get; set; }
		public int latestvalue { get; set; }
		public string daterange { get; set; }

	}


	public class DeviceABusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		int counter = 1;
		public IEnumerable<DeviceA> DeviceAs
		{
			get
			{
				List<DeviceA> DeviceAs = new List<DeviceA>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					DateTime vDate;
					string qry;
					qry = "select a.deviceStamp, b.deviceType as Value, a.deviceInternalTemp as Item from tblDeviceData a left join tblDeviceType b on a.deviceID=b.deviceID order by devicestamp";
					SqlCommand cmdObj = new SqlCommand(qry, conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
						DeviceA DeviceA = new DeviceA();
						DeviceA.x = string.Format("{0:MM/dd/yy hh}", vDate);
						DeviceA.y = Convert.ToInt32(readerObj["item"]);
						//DeviceA.Item = Convert.ToInt32(readerObj["item"].ToString());						
						//DeviceA.Value = string.Format("{0:MM/dd/yy hh:mm}",vDate) ;
						DeviceAs.Add(DeviceA);
						counter = counter + 1;
					}
				}
				return DeviceAs;
			}
		}


        public int DeviceValuel(string deviceID)
        {

            int vRetX = 1;
            using (SqlConnection conObj = new SqlConnection(conStr))
            {

                //string qry = "select top 1 deviceInternalTemp from tblDeviceData where deviceID='" + deviceID + "' order by autoID desc";

                SqlCommand cmdObj = new SqlCommand("spdevicevalue1", conObj);
                cmdObj.CommandType = System.Data.CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceid", deviceID));
                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();
                while (readerObj.Read())
                {
                    vRetX = Convert.ToInt32(readerObj["deviceInternalTemp"].ToString());
                }
                cmdObj.Dispose();
                readerObj.Close();

            }
            return vRetX;

        }


        public IEnumerable<DeviceA> LatestValue(string pDeviceID)
        {
            int myValue = DeviceValuel(pDeviceID);
            List<DeviceA> LatestValues = new List<DeviceA>();
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
               // DateTime vDate;
                string qry;
                //qry = "select top 1 deviceInternalTemp from tbldevicedata where deviceid='" + pDeviceID + "' order by autoID desc";
                SqlCommand cmdObj = new SqlCommand("spdevicelatestvalue", conObj);
                cmdObj.CommandType = System.Data.CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceid", pDeviceID));

                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();

                while (readerObj.Read())
                {

                    DeviceA DeviceA = new DeviceA();
                    DeviceA.x = DateTime.Today.ToString();
                    DeviceA.y = Convert.ToInt32(readerObj["deviceInternalTemp"]);
                    DeviceA.latestvalue = myValue;
                    LatestValues.Add(DeviceA);
                    counter = counter + 1;
                }

            }
            return LatestValues;

        }



        public IEnumerable<DeviceA> DeviceAllDate(string deviceID, string fromDate = null, string toDate = null)
		{
			List<DeviceA> DeviceAs = new List<DeviceA>();
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				string xfrdate = "";
				string xtodate = "";
				DateTime vfrDate; //= Convert.ToDateTime(fromDate.ToString());
				DateTime vtoDate; // = Convert.ToDateTime(toDate.ToString());	
				string vRange;				
				DateTime vDate;
				if (fromDate != null)
				{
					//string msg = fromDate.ToString() + "XXX" + toDate.ToString();
					//((System.Web.UI.Page)System.Web.HttpContext.Current.Handler).ClientScript.RegisterStartupScript(this.GetType(), "yourMessage", "alert('" + msg + "');", true);

					vfrDate = Convert.ToDateTime(fromDate.ToString(), CultureInfo.InvariantCulture);
					vtoDate = Convert.ToDateTime(toDate.ToString(), CultureInfo.InvariantCulture);

					vfrDate = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy HH:mm tt}", vfrDate), CultureInfo.InvariantCulture);
					vtoDate = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy HH:mm tt}", vtoDate), CultureInfo.InvariantCulture);

					xfrdate = String.Format("{0:MM/dd/yyyy HH:mm tt}", vfrDate);
					xtodate = String.Format("{0:MM/dd/yyyy HH:mm tt}", vtoDate);

				}
				else
				{
					vfrDate = Convert.ToDateTime("01/01/1999");
					vtoDate = Convert.ToDateTime("01/01/1999");
				}

                SqlCommand cmdObj; 
                int myValue = 0;// DeviceValuel(deviceID);
				string qry;
				if (vfrDate.Year == 1999)
				{

					vRange = "From: Null To : Null ";
					//qry = "select a.deviceStamp,c.status, a.deviceInternalTemp as Item from tblDeviceData a "
					// + "left join tblMonitoringSub C on a.deviceId=c.ID where a.deviceid='" + deviceID + "'";
                    cmdObj = new SqlCommand("spDeviceAllDateB", conObj);
                    cmdObj.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdObj.Parameters.Add(new SqlParameter("@deviceid", deviceID));

                }
				else
				{

                    //vfrDate = Convert.ToDateTime(fromDate.ToString(), CultureInfo.InvariantCulture);
                    //vtoDate = Convert.ToDateTime(toDate.ToString(), CultureInfo.InvariantCulture);		
                    //qry = "select a.deviceStamp,c.status, a.deviceInternalTemp as Item from tblDeviceData a "
                    // + "left join tblMonitoringSub C on a.deviceId=c.ID where a.deviceid='" + deviceID + "' and "
                    // + "deviceStamp>='" + xfrdate + "' and deviceStamp<='" + xtodate + "' order by devicestamp";

                    cmdObj = new SqlCommand("spDeviceAllDateA", conObj);
                    cmdObj.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdObj.Parameters.Add(new SqlParameter("@deviceid", deviceID));
                    cmdObj.Parameters.Add(new SqlParameter("@fromDate", fromDate));
                    cmdObj.Parameters.Add(new SqlParameter("@toDate", toDate));

                    vRange = "From  " + String.Format("{0:dd/MM/yyyy}", vfrDate, CultureInfo.InvariantCulture) + " To  " + String.Format("{0:dd/MM/yyyy}", vtoDate, CultureInfo.InvariantCulture);
				}
				//SqlCommand cmdObj = new SqlCommand(qry, conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();

				while (readerObj.Read())
				{
					vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
					DeviceA DeviceA = new DeviceA();
					DeviceA.y = Convert.ToInt32(readerObj["item"]);
					DeviceA.stat = readerObj["status"].ToString();
					DeviceA.x = string.Format("{0:dd/MM/yy hh:mm}", vDate);
					DeviceA.latestvalue = myValue;
					DeviceA.daterange = vRange;
					DeviceAs.Add(DeviceA);
					counter = counter + 1;
				}
			}
			return DeviceAs;
		}





		public IEnumerable<DeviceA> DeviceAll(string deviceID)
		{
			List<DeviceA> DeviceAs = new List<DeviceA>();
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				DateTime vDate;

				string qry;
				//qry = "select a.deviceStamp,c.status, a.deviceInternalTemp as Item from tblDeviceData a "
				// + "left join tblMonitoringSub C on a.deviceId=c.ID where a.deviceid='" + deviceID + "' order by devicestamp";

				SqlCommand cmdObj = new SqlCommand("spDeviceAll", conObj);
                cmdObj.CommandType = System.Data.CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceid", deviceID));
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();

				while (readerObj.Read())
				{
					vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
					DeviceA DeviceA = new DeviceA();
					DeviceA.y = Convert.ToInt32(readerObj["item"]);
					DeviceA.stat = readerObj["status"].ToString();
					DeviceA.x = string.Format("{0:dd/MM/yy hh:mm}", vDate);			
					DeviceAs.Add(DeviceA);
					counter = counter + 1;
				}
			}
			return DeviceAs;
		}

		public IEnumerable<DeviceA> DeviceAfilter(int deviceID)
		{
			List<DeviceA> DeviceAs = new List<DeviceA>();
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				
				//qry = "select a.deviceStamp, b.deviceType as Value, a.deviceInternalTemp as Item from tblDeviceData a left join tblDeviceType b "
				//	+ "on a.deviceID=b.deviceID where deviceid=" + deviceID + " order by deviceStamp";
				SqlCommand cmdObj = new SqlCommand("spDeviceAfilter", conObj);            
                cmdObj.CommandType = System.Data.CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceid", deviceID));
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();

				while (readerObj.Read())
				{
					DeviceA DeviceA = new DeviceA();
					DeviceA.x = readerObj["deviceStamp"].ToString();
					DeviceA.y = Convert.ToInt32(readerObj["item"]);
					//DeviceA.Item = Convert.ToInt32(readerObj["item"].ToString());
					//DeviceA.Value = readerObj["value"].ToString();
					DeviceAs.Add(DeviceA);
					counter = counter + 1;
				}
			}
			return DeviceAs;
		}





	}

}
