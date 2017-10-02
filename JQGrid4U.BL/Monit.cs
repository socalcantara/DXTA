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
						"aa.siteId,aa.siteName, " +
						"Hours = datepart(hour, DateDif)," +
						"Minutes = datepart(minute, DateDif)," +
						"Seconds = datepart(second, DateDif)," +
						"MS = datepart(ms, DateDif) ," +
						"convert(nvarchar(6), datediff(dd, 0, DateDif)) + 'd' + space(1) + " +
						"convert(nvarchar(6), datepart(hour, DateDif)) + 'h' + space(1) + " +
						"convert(nvarchar(6), datepart(minute, DateDif)) + 'm' + space(1) + " +
						"convert(nvarchar(6), datepart(second, DateDif)) + 's' as Laps " +
						"from (select autoID,a.siteid, b.sitename, a.id, status, typ, lastupdate, " +
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
						Monits.Add(Monit);
					}
					conObj.Close();
				}
				return Monits;
			}
		}

	}


}
