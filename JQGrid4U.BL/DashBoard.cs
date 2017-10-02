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
					qry = "select distinct a.siteid,b.sitename, rolstatusok,rolstatuscritical,rolstatuswarning,pulstatusok, pulstatuswarning,pulstatuscritical from TblMonitoring a left join tblSite b on a.siteid=b.siteid";
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
