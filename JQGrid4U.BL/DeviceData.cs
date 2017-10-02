using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace JQGrid4U.BL
{
	public class DeviceData
	{

		public int autoID { get; set; }
		public string deviceId { get; set; }
		public int deviceFreq { get; set; }
		public int deviceAmbientTemp { get; set; }
		public int deviceInternalTemp { get; set; }
		public DateTime deviceStamp { get; set; }
	}



	public class DeviceDataBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		public IEnumerable<DeviceData> DeviceDatas
		{
			get
			{
				List<DeviceData> DeviceDatas = new List<DeviceData>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					SqlCommand cmdObj = new SqlCommand("select * from tblDeviceData", conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						DeviceData DeviceData = new DeviceData();
						DeviceData.autoID = Convert.ToInt32(readerObj["autoID"]);
						DeviceData.deviceId = readerObj["deviceId"].ToString();
						DeviceData.deviceFreq = Convert.ToInt32(readerObj["devicefreq"].ToString()) ;
						DeviceData.deviceAmbientTemp = Convert.ToInt32(readerObj["deviceAmbientTemp"].ToString());
						DeviceData.deviceInternalTemp = Convert.ToInt32(readerObj["deviceInternalTemp"].ToString());
						DeviceData.deviceStamp = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
						DeviceDatas.Add(DeviceData);
					}
				}
				return DeviceDatas;
			}
		}

		public int InsertDeviceData(DeviceData DeviceData)
		{
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspInsertDeviceData", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;
				cmdObj.Parameters.Add(new SqlParameter("@AuitoId", DeviceData.autoID));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceId", DeviceData.deviceId));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceFreq", DeviceData.deviceFreq));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceAmbientTemp", DeviceData.deviceAmbientTemp));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceInternalTemp", DeviceData.deviceInternalTemp));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceStamp", DeviceData.deviceStamp));
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}

		public int UpdateDeviceData(DeviceData DeviceData)
		{
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspUpdateDeviceData", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;
				cmdObj.Parameters.Add(new SqlParameter("@autoID", DeviceData.autoID));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceId", DeviceData.deviceId));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceFreq", DeviceData.deviceFreq));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceAmbientTemp", DeviceData.deviceAmbientTemp));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceInternalTemp", DeviceData.deviceInternalTemp));
				cmdObj.Parameters.Add(new SqlParameter("@DeviceStamp", DeviceData.deviceStamp));				
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}






		public int DeleteDeviceData(int ID)
		{
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspDeleteDeviceData", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;
				cmdObj.Parameters.Add(new SqlParameter("@ID", ID));
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}
	}





}
