using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace JQGrid4U.BL
{
	public class DashBoard
	{

		public int siteID { get; set; }
		public string Sitename { get; set; }
		public int RolStatusOk { get; set; }
		public int RolStatusCritical { get; set; }
		public int RolStatusWarning { get; set; }
		public int PulStatusOk { get; set; }
		public int PulStatusCritical { get; set; }
		public int PulStatusWarning { get; set; }

	}

	public class DashBoardBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		public IEnumerable<DashBoard> DashBoards
		{
			get
			{
				List<DashBoard> DashBoards = new List<DashBoard>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					string qry;
                    //qry = "select distinct a.siteid,b.sitename, rolstatusok,rolstatuscritical,rolstatuswarning,pulstatusok, pulstatuswarning,pulstatuscritical from TblMonitoring a left join tblSite b on a.siteid=b.siteid";
                    qry = "SELECT        aa.siteId, c.SiteName, SUM(aa.RolStatusOk) AS RolstatusOk, SUM(aa.RolStatusWarning) AS RolStatusWarning, "
                         + "SUM(aa.RolStatusCritical) AS RolStatusCritical, SUM(aa.PulStatusOk) AS PulStatusOk, SUM(aa.PulStatusWarning) "
                          + "AS PulstatusWarning, SUM(aa.PulStatusCritical) AS PulStatusCritical "
                          + "FROM ( SELECT        siteId, COUNT(ID) AS RolStatusOk, 0 AS RolStatusWarning, 0 AS RolStatusCritical, 0 AS PulStatusOk, "
                          + "0 AS PulStatusWarning, 0 AS PulStatusCritical "
                           + "FROM            dbo.tblMonitoringSub AS a "
                           + "WHERE(LOWER(Status) = 'ok') AND(SUBSTRING(ID, 1, 1) = 'R') "
                           + "GROUP BY siteId "
                           + "UNION ALL "
                           + "SELECT        siteId, 0 AS Expr1, COUNT(ID) AS Expr2, 0 AS Expr3, 0 AS Expr4, 0 AS Expr5, 0 AS Expr6 "
                           + "FROM            dbo.tblMonitoringSub AS a "
                           + "WHERE(LOWER(Status) = 'warning') AND(SUBSTRING(ID, 1, 1) = 'R') "
                           + "GROUP BY siteId "
                           + "UNION ALL "
                           + "SELECT        siteId, 0 AS Expr1, 0 AS Expr2, COUNT(ID) AS Expr3, 0 AS Expr4, 0 AS Expr5, 0 AS Expr6 "
                           + "FROM            dbo.tblMonitoringSub AS a "
                           + "WHERE(LOWER(Status) = 'critical') AND(SUBSTRING(ID, 1, 1) = 'R') "
                           + "GROUP BY siteId "
                           + "UNION ALL "
                           + "SELECT        siteId, 0 AS Expr1, 0 AS Expr2, 0 AS Expr3, COUNT(ID) AS Expr4, 0 AS Expr5, 0 AS Expr6 "
                           + "FROM            dbo.tblMonitoringSub AS a "
                           + "WHERE(LOWER(Status) = 'ok') AND(SUBSTRING(ID, 1, 1) = 'P') "
                           + "GROUP BY siteId "
                           + "UNION ALL "
                           + "SELECT        siteId, 0 AS Expr1, 0 AS Expr2, 0 AS Expr3, 0 AS Expr4, COUNT(ID) AS Expr5, 0 AS Expr6 "
                           + "FROM            dbo.tblMonitoringSub AS a "
                           + "WHERE(LOWER(Status) = 'warning') AND(SUBSTRING(ID, 1, 1) = 'P') "
                           + "GROUP BY siteId "
                           + "UNION ALL "
                           + "SELECT        siteId, 0 AS Expr1, 0 AS Expr2, 0 AS Expr3, 0 AS Expr4, 0 AS Expr5, COUNT(ID) AS Expr6 "
                           + "FROM            dbo.tblMonitoringSub AS a "
                           + "WHERE(LOWER(Status) = 'critical') AND(SUBSTRING(ID, 1, 1) = 'P') "
                           + "GROUP BY siteId) AS aa LEFT OUTER JOIN "
                          + "dbo.tblSite AS c ON aa.siteId = c.SiteID "
                         + "GROUP BY c.SiteName, aa.siteId";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						DashBoard DashBoard = new DashBoard();
						DashBoard.siteID = Convert.ToInt32(readerObj["siteID"]);
						DashBoard.Sitename = readerObj["Sitename"].ToString();
						DashBoard.RolStatusOk = Convert.ToInt32(readerObj["Rolstatusok"].ToString());
						DashBoard.RolStatusCritical = Convert.ToInt32(readerObj["Rolstatuscritical"].ToString());
						DashBoard.RolStatusWarning = Convert.ToInt32(readerObj["RolstatusWarning"].ToString());
						DashBoard.PulStatusOk = Convert.ToInt32(readerObj["PulStatusOk"].ToString());
						DashBoard.PulStatusCritical = Convert.ToInt32(readerObj["PulStatusCritical"].ToString());
						DashBoard.PulStatusWarning = Convert.ToInt32(readerObj["PulStatusWarning"].ToString());
						DashBoards.Add(DashBoard);
					}
					conObj.Close();
				}
				return DashBoards;
			}
		}

	}






}
