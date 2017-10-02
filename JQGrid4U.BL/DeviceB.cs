using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;


namespace JQGrid4U.BL
{
    public class DeviceB
    {
        public string label { get; set; }
        public int figure { get; set; }
        public int latestvalue { get; set; }
        public int freqvalue { get; set; }
        public string daterange { get; set; }
    }


  
    public class DeviceBBusinessLogic
    {
        string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        

        int counter = 1;
        public IEnumerable<DeviceB> DeviceAs
        {
            get
            {
                List<DeviceB> DeviceAs = new List<DeviceB>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    DateTime vDate;
                    string qry;
                    qry = "select a.deviceStamp, b.deviceType as Value, a.deviceInternalTemp as Item, "
                        + "a.deviceFrequency from tblDeviceData a left join tblDeviceType b on a.deviceID=b.deviceID order by devicestamp";
                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
                        DeviceB DeviceA = new DeviceB();
                        DeviceA.label = string.Format("{0:MM/dd/yy hh}", vDate);                        
                        DeviceAs.Add(DeviceA);
                        counter = counter + 1;
                    }
                }
                return DeviceAs;
            }
        }




        //public IEnumerable<DeviceB> DeviceLabel(string deviceID, string fromDate, string toDate)
        //{

        //    List<DeviceB> DeviceAs = new List<DeviceB>();
        //    using (SqlConnection conObj = new SqlConnection(conStr))
        //    {
        //        DateTime vfrDate = Convert.ToDateTime(fromDate, CultureInfo.InvariantCulture);
        //        DateTime vtoDate = Convert.ToDateTime(toDate, CultureInfo.InvariantCulture);
        //        string vRange = "From  " + String.Format("{0:dd/MM/yyyy}", vfrDate) + " To  " + String.Format("{0:dd/MM/yyyy}", vtoDate);
        //        DateTime vDate;
        //        int vfigure = 0;
        //        int vfreqvalue = 0;
        //        string qry;
        //        string xfrdate;
        //        string xtodate;
        //        int myValue = DeviceValuel(deviceID);

        //        vfrDate = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy HH:mm tt}", vfrDate), CultureInfo.InvariantCulture);
        //        vtoDate = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy HH:mm tt}", vtoDate), CultureInfo.InvariantCulture);

        //        xfrdate = String.Format("{0:MM/dd/yyyy HH:mm tt}", vfrDate);
        //        xtodate = String.Format("{0:MM/dd/yyyy HH:mm tt}", vtoDate);

        //        qry = "select a.deviceStamp,c.status, a.deviceInternalTemp,a.devicefreq from tblDeviceData a "
        //         + "left join tblMonitoringSub C on a.deviceId=c.ID where a.deviceid='" + deviceID + "' and "
        //         + "deviceStamp>='" + xfrdate + "' and deviceStamp<='" + xtodate + "' order by devicestamp";
        //        SqlCommand cmdObj = new SqlCommand(qry, conObj);
        //        conObj.Open();
        //        SqlDataReader readerObj = cmdObj.ExecuteReader();

        //        while (readerObj.Read())
        //        {
        //            vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
        //            vfigure = Convert.ToInt32(readerObj["deviceinternaltemp"].ToString());
        //            vfreqvalue = Convert.ToInt32(readerObj["devicefreq"].ToString());
        //            DeviceB DeviceA = new DeviceB();
        //            DeviceA.label = string.Format("{0:dd/MM/yy hh}", vDate);
        //            DeviceA.figure = vfigure;
        //            DeviceA.freqvalue = vfreqvalue;
        //            DeviceA.daterange = vRange;
        //            DeviceA.latestvalue = myValue;
        //            DeviceAs.Add(DeviceA);
        //            counter = counter + 1;
        //        }
        //    }
        //    return DeviceAs;

        //}



        public IEnumerable<DeviceB> DeviceLabelNew(string deviceID, string fromDate, string toDate)
        {

            List<DeviceB> DeviceAs = new List<DeviceB>();
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                DateTime vfrDate = Convert.ToDateTime(fromDate, CultureInfo.InvariantCulture);
                DateTime vtoDate = Convert.ToDateTime(toDate, CultureInfo.InvariantCulture);
                string vRange = "From  " + String.Format("{0:dd/MM/yyyy}", vfrDate) + " To  " + String.Format("{0:dd/MM/yyyy}", vtoDate);
                DateTime vDate;
                int vfigure = 0;
                int vfreqvalue = 0;
                string qry;
                string xfrdate;
                string xtodate;
                int myValue = DeviceValuel(deviceID);

                vfrDate = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy HH:mm tt}", vfrDate), CultureInfo.InvariantCulture);
                vtoDate = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy HH:mm tt}", vtoDate), CultureInfo.InvariantCulture);

                xfrdate = String.Format("{0:MM/dd/yyyy HH:mm tt}", vfrDate);
                xtodate = String.Format("{0:MM/dd/yyyy HH:mm tt}", vtoDate);

                qry = "select a.deviceStamp,c.status, a.deviceInternalTemp,a.devicefreq from tblDeviceData a "
                 + "left join tblMonitoringSub C on a.deviceId=c.ID order by devicestamp";
                
                SqlCommand cmdObj = new SqlCommand("spDevicelabel", conObj);
               cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceId", deviceID));
                cmdObj.Parameters.Add(new SqlParameter("@fromDate",fromDate));
                cmdObj.Parameters.Add(new SqlParameter("@toDate", toDate));
                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();

                while (readerObj.Read())
                {
                    vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
                    vfigure = Convert.ToInt32(readerObj["deviceinternaltemp"].ToString());
                    vfreqvalue = Convert.ToInt32(readerObj["devicefreq"].ToString());
                    DeviceB DeviceA = new DeviceB();
                    DeviceA.label = string.Format("{0:dd/MM/yy hh}", vDate);
                    DeviceA.figure = vfigure;
                    DeviceA.freqvalue = vfreqvalue;
                    DeviceA.daterange = vRange;
                    DeviceA.latestvalue = myValue;
                    DeviceAs.Add(DeviceA);
                    counter = counter + 1;
                }
            }
            return DeviceAs;

        }





        public IEnumerable<DeviceB> DeviceFigureWoDate(string deviceID)
        {

            List<DeviceB> DeviceAs = new List<DeviceB>();
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                int vfig = 0;

                string qry;
                qry = "select distinct a.deviceStamp, a.deviceInternalTemp from tblDeviceData a "
                 + "left join tblMonitoringSub C on a.deviceId=c.ID";

                SqlCommand cmdObj = new SqlCommand("spdevicelabelc", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceId", deviceID));
                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();

                while (readerObj.Read())
                {
                    vfig = Convert.ToInt32(readerObj["deviceinternaltemp"].ToString());
                    DeviceB DeviceA = new DeviceB();
                    DeviceA.figure = vfig;
                    DeviceAs.Add(DeviceA);
                    counter = counter + 1;
                }
            }
            return DeviceAs;

        }



        public IEnumerable<DeviceB> DeviceLabelWoDate(string deviceID)
        {

            List<DeviceB> DeviceAs = new List<DeviceB>();
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                DateTime vDate;
                int vFigure = 0;

                string qry;
                qry = "select distinct a.deviceStamp, a.deviceInternalTemp  from tblDeviceData a "
                 + "left join tblMonitoringSub C on a.deviceId=c.ID";

                SqlCommand cmdObj = new SqlCommand("spdevicelabelb", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceId", deviceID));
                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();

                while (readerObj.Read())
                {
                    vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
                    vFigure = Convert.ToInt32(readerObj["deviceinternaltemp"].ToString());
                    DeviceB DeviceA = new DeviceB();
                    DeviceA.label = string.Format("{0:dd/MM/yy hh}", vDate);
                    DeviceA.figure = vFigure;
                    DeviceAs.Add(DeviceA);
                    counter = counter + 1;
                }
            }
            return DeviceAs;

        }

        public int DeviceValuel(string deviceID)
        {

            int vRetX = 1;
            using (SqlConnection conObj = new SqlConnection(conStr))
            {

                //string qry = "select top 1 deviceInternalTemp from tblDeviceData order by autoID desc";
                SqlCommand cmdObj = new SqlCommand("spdevicelabelA", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceId", deviceID));
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


        public IEnumerable<DeviceB> DeviceLabelWDate(string deviceID, string fromDate = null, string toDate = null)
        {

            List<DeviceB> DeviceAs = new List<DeviceB>();
            using (SqlConnection conObj = new SqlConnection(conStr))
            {

                DateTime vfrDate;
                DateTime vtoDate;
                string xfrdate = "";
                string xtodate = "";

                if (fromDate != null)
                {

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



                DateTime vDate;
                int vFigure = 0;

                string qry;
                qry = "select distinct a.deviceStamp, a.deviceInternalTemp  from tblDeviceData a "
                 + "left join tblMonitoringSub C on a.deviceId=c.ID";
                 //+ "a.deviceid='" + deviceID + "' and deviceStamp>='" + xfrdate + "' and devicestamp<='" + xtodate + "'";

                SqlCommand cmdObj = new SqlCommand(qry, conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@deviceId", deviceID))  ;
                cmdObj.Parameters.Add(new SqlParameter("@fromDate", fromDate));
                cmdObj.Parameters.Add(new SqlParameter("@toDate", toDate));
                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();

                while (readerObj.Read())
                {
                    vDate = Convert.ToDateTime(readerObj["deviceStamp"].ToString());
                    vFigure = Convert.ToInt32(readerObj["deviceinternaltemp"].ToString());
                    DeviceB DeviceA = new DeviceB();
                    DeviceA.label = string.Format("{0:dd/MM/yy hh}", vDate);
                    DeviceA.figure = vFigure;
                    DeviceAs.Add(DeviceA);
                    counter = counter + 1;
                }
            }
            return DeviceAs;

        }




    }




}
