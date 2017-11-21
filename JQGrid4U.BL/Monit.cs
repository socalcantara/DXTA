using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace JQGrid4U.BL
{
	public class Monit
	{

		public int autoID { get; set; }
		public string ID { get; set; }
		public string Status { get; set; }
		public string Typ { get; set; }
		public DateTime LastUpdate { get; set; }
		public string Laps { get; set; }
		public int siteId { get; set; }
		public string siteName { get; set; }
        public string serialNo { get; set; }
        public string isAck { get; set; }
        public string Note { get; set; }
	}

    public class LogResolve
    {
        public int LogID { get; set; }
        public DateTime? LogDate { get; set; }
        public int NoteTypeID { get; set; }
        public string Serial { get; set; }
        public string DeviceID { get; set; }
        public string Typ { get; set; }
        public string Note { get; set; }
        public string Action { get; set; }
        public string userID { get; set; }
        public string org_serial { get; set; }
    }
    public class NoteList
    {
        public string Serial { get; set; }
        public DateTime noteDate { get; set; }
        public int NoteTypeID { get; set; }
        public string NoteTypeIdDesc { get; set; }
        public string Note { get; set; }
        public string userID { get; set; }
        public string fullname { get; set; }
    }
    public class tblDeviceHistory
    {
        public string deviceID { get; set; }
        public string deviceSerial { get; set; }
        public DateTime deviceInstallDate { get; set; }
        public DateTime? deviceRetireDate { get; set; }
    }
    
	public class MonitBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		public IEnumerable<Monit> Monits
		{
			get
			{
				List<Monit> Monits = new List<Monit>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					string qry;
					qry = "select aa.autoID, aa.id,aa.status,aa.typ ,aa.lastupdate, Days = datediff(dd, 0, DateDif) ," +
                        "aa.siteId,aa.siteName, aa.serialNo, (select top 1 noteTypeID from tblDeviceNotes where deviceSerial = aa.serialNo and noteTypeID = 1) isAck,(select top 1 note from tblDeviceNotes where deviceSerial = aa.serialNo and noteTypeID = 1 order by noteDate desc) Note," +
						"Hours = datepart(hour, DateDif)," +
						"Minutes = datepart(minute, DateDif)," +
						"Seconds = datepart(second, DateDif)," +
						"MS = datepart(ms, DateDif) ," +
						"convert(nvarchar(6), datediff(dd, 0, DateDif)) + 'd' + space(1) + " +
						"convert(nvarchar(6), datepart(hour, DateDif)) + 'h' + space(1) + " +
						"convert(nvarchar(6), datepart(minute, DateDif)) + 'm' + space(1) + " +
						"convert(nvarchar(6), datepart(second, DateDif)) + 's' as Laps " +
                        "from (select autoID,a.siteid, b.sitename, a.id, status, typ, lastupdate, serialNo," +
						"DateDif = getdate() - lastupdate from tblMonitoringSub a left join tblSite b on a.siteid=b.siteid) aa";
					SqlCommand cmdObj = new SqlCommand(qry, conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						Monit Monit = new Monit();
						Monit.autoID = Convert.ToInt32(readerObj["autoID"]);
						Monit.ID = readerObj["ID"].ToString();
						Monit.Status = readerObj["status"].ToString();
						Monit.Typ = readerObj["typ"].ToString();
						Monit.LastUpdate = Convert.ToDateTime(readerObj["lastupdate"].ToString());
						Monit.Laps = readerObj["laps"].ToString();
						Monit.siteId = Convert.ToInt32(readerObj["siteid"].ToString());
						Monit.siteName = readerObj["siteName"].ToString();
                        Monit.serialNo = readerObj["serialNo"].ToString();
                        Monit.isAck = readerObj["isAck"].ToString();
                        Monit.Note = readerObj["Note"].ToString();
                        Monits.Add(Monit);
					}
					conObj.Close();
				}
				return Monits;
			}
		}

        public int InsertResolveLogReplace(LogResolve ResolveLog)
        {

            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspInsertDeviceHistory", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@DeviceID", ResolveLog.DeviceID));
                cmdObj.Parameters.Add(new SqlParameter("@Serial", ResolveLog.Serial));
                cmdObj.Parameters.Add(new SqlParameter("@LogDate", ResolveLog.LogDate));
                cmdObj.Parameters.Add(new SqlParameter("@LogRetireDate", ResolveLog.NoteTypeID == 3 ? ResolveLog.LogDate : null));
                conObj.Open();
                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        public int InsertResolveLog(LogResolve ResolveLog)
        {

            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspInsertDeviceHistory", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@DeviceID", ResolveLog.DeviceID));
                cmdObj.Parameters.Add(new SqlParameter("@Serial", ResolveLog.Serial));
                cmdObj.Parameters.Add(new SqlParameter("@LogDate", ResolveLog.LogDate));
                cmdObj.Parameters.Add(new SqlParameter("@LogRetireDate", DBNull.Value));
                conObj.Open();
                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }

        public int UpdateResolveDevice(LogResolve ResolveDevice)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspUpdateFixedDevice", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@DeviceID", ResolveDevice.DeviceID));
                cmdObj.Parameters.Add(new SqlParameter("@Serial", ResolveDevice.Serial));
                cmdObj.Parameters.Add(new SqlParameter("@NoteTypeID", ResolveDevice.NoteTypeID));
                conObj.Open();
                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        public int LogNoteAck(LogResolve NoteResolveDevice)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspInsertNoteFixedLog", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@Serial", NoteResolveDevice.Serial));
                cmdObj.Parameters.Add(new SqlParameter("@LogDate", NoteResolveDevice.LogDate));
                cmdObj.Parameters.Add(new SqlParameter("@userID", NoteResolveDevice.userID));
                cmdObj.Parameters.Add(new SqlParameter("@Note", NoteResolveDevice.Note));
                conObj.Open();
                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        public IEnumerable<NoteList> NoteList
        {
            get
            {
                List<NoteList> NoteLists = new List<NoteList>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "select deviceSerial,noteDate,noteTypeID,(select top 1 noteTypeDesc from tblDeviceNoteType where noteTypeID = b.noteTypeID) notetypedesc,note,(select FirstName + ' ' + SurName  from tblUser a where a.userId = b.userId) fullname from tblDeviceNotes	b 	order by deviceSerial,noteDate desc";
                    
                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        NoteList NoteList = new NoteList();
                        NoteList.Serial = readerObj["deviceSerial"].ToString();
                        NoteList.noteDate = Convert.ToDateTime(readerObj["noteDate"].ToString());
                        NoteList.NoteTypeID = Convert.ToInt32(readerObj["noteTypeID"]);
                        NoteList.Note = readerObj["note"].ToString();
                        NoteList.fullname = readerObj["fullname"].ToString();
                        NoteList.NoteTypeIdDesc = readerObj["notetypedesc"].ToString();

                        NoteLists.Add(NoteList);
                    }
                    conObj.Close();
                }
                return NoteLists;
            }
        }
        public IEnumerable<tblDeviceHistory> tblDeviceHistory
        {
            get
            {
                List<tblDeviceHistory> HistoryLists = new List<tblDeviceHistory>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "SELECT * FROM tblDeviceHistory";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        DateTime defaultInstalldate = DateTime.Now;
                        DateTime defaultRetireDate = DateTime.Now;
                        DateTime.TryParse(readerObj["deviceInstallDate"].ToString(), out defaultInstalldate);
                        DateTime.TryParse(readerObj["deviceRetireDate"].ToString(), out defaultRetireDate);

                        tblDeviceHistory HistoryList = new tblDeviceHistory();
                        HistoryList.deviceID = readerObj["deviceID"].ToString();
                        HistoryList.deviceSerial = readerObj["deviceSerial"].ToString();
                        HistoryList.deviceInstallDate = defaultInstalldate;
                        HistoryList.deviceRetireDate = defaultRetireDate;

                        HistoryLists.Add(HistoryList);
                    }
                    conObj.Close();
                }
                return HistoryLists;
            }
        }

    }


}
