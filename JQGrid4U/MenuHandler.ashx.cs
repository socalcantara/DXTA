using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using JQGrid4U.Models;
using System.Web.Script.Serialization;
using System.Web.SessionState;



namespace JQGrid4U.Views
{
    /// <summary>
    /// Summary description for MenuHandler
    /// </summary>
    public class MenuHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            List<Menu> listMenu = new List<Menu>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string UserID = context.Request["userid"].ToString();

                SqlCommand cmd = new SqlCommand("spGetMenuData",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userID", UserID));
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    Menu menu = new Menu();
                    menu.Id = Convert.ToInt32(rdr["Id"]);
                    menu.MenuText = rdr["MenuText"].ToString();
                    menu.ParentId = rdr["ParentId"] != DBNull.Value ? Convert.ToInt32(rdr["ParentId"]) : (int?)null;
                    menu.Link = rdr["Link"].ToString();
                    menu.Active = Convert.ToBoolean(rdr["Active"]);
                    listMenu.Add(menu);
                }
            }
            List<Menu> menuTree = GetMenuTree(listMenu, null);

            JavaScriptSerializer js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(menuTree));
        }

        private List<Menu> GetMenuTree(List<Menu> list, int? parentId)
        {
            return list.Where(x => x.ParentId == parentId).Select(x => new Menu()
            {
                Id = x.Id,
                MenuText = x.MenuText,
                ParentId = x.ParentId,
                Active = x.Active,
                Link = x.Link,
                List = GetMenuTree(list, x.Id)

            }).ToList();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}