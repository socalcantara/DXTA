using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Text;

namespace JQGrid4U.BL
{
    public class Roles
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
    public class RoleMenuAccess
    {
        //public int ID { get; set; }
        public int RoleID { get; set; }
        public int MenuHdrID { get; set; }
    }
    public class UserRoleAccess
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
    public class MenuHdr
    {
        public int menuID { get; set; }
        public string MenuText { get; set; }
        public int? ParentID { get; set; }
        public bool isActive { get; set; }
        public string Link { get; set; }
        public bool @checked { get; set; }
        public virtual List<MenuHdr> children { get; set; }
    }
    public class RoleAccess
    {
        public int roleID { get; set; }
        public List<AccessType> dataaccess { get; set; }
    }
    public class AccessType
    {
        public int moduleID { get; set; }
        public string moduleType { get; set; }
    }

    public class RoleManagement
    {
        string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        public IEnumerable<Roles> RolesList
        {
            get
            {
                List<Roles> RoleLists = new List<Roles>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "select ID,RoleName,RoleDescription from tblUserRole";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        Roles RolesList = new Roles();
                        RolesList.ID = Convert.ToInt32(readerObj["ID"].ToString());
                        RolesList.RoleName = readerObj["RoleName"].ToString();
                        RolesList.RoleDescription = readerObj["RoleDescription"].ToString();
                        RoleLists.Add(RolesList);
                    }
                    conObj.Close();
                }
                return RoleLists;
            }
        }

        public IEnumerable<MenuHdr> MenuList
        {
            get
            {
                List<MenuHdr> MenuLists = new List<MenuHdr>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "SELECT Id, MenuText, ParentId, Active, Link FROM tblMenu";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        MenuHdr MenuList = new MenuHdr();
                        MenuList.menuID = Convert.ToInt32(readerObj["ID"].ToString());
                        MenuList.MenuText = readerObj["MenuText"].ToString();
                        MenuList.ParentID = readerObj["ParentId"] != DBNull.Value ? Convert.ToInt32(readerObj["ParentId"]) : (int?)null;
                        MenuList.isActive = Convert.ToBoolean(readerObj["Active"].ToString());
                        MenuList.Link = readerObj["Link"].ToString();
                        MenuLists.Add(MenuList);
                    }
                    conObj.Close();
                }
                return MenuLists;
            }
        }

        public IEnumerable<RoleMenuAccess> RoleMenuAcessList
        {
            get
            {
                List<RoleMenuAccess> RoleMenuAccess = new List<RoleMenuAccess>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "select RoleID,MenuHdrID from tblMenuSub";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        RoleMenuAccess MenuAccess = new RoleMenuAccess();
                        //MenuAccess.ID = Convert.ToInt32(readerObj["ID"].ToString());
                        MenuAccess.RoleID = Convert.ToInt32(readerObj["RoleID"].ToString());
                        MenuAccess.MenuHdrID = Convert.ToInt32(readerObj["MenuHdrID"].ToString());
                        RoleMenuAccess.Add(MenuAccess);
                    }
                    conObj.Close();
                }
                return RoleMenuAccess;
            }
        }

        public IEnumerable<UserRoleAccess> UserRoleAccess
        {
            get
            {
                List<UserRoleAccess> UserRoleMenuAccess = new List<UserRoleAccess>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "select UserID,RoleID from tblUserAccess";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        UserRoleAccess MenuAccess = new UserRoleAccess();
                        MenuAccess.RoleID = Convert.ToInt32(readerObj["RoleID"].ToString());
                        MenuAccess.UserID = Convert.ToInt32(readerObj["UserID"].ToString());
                        UserRoleMenuAccess.Add(MenuAccess);
                    }
                    conObj.Close();
                }
                return UserRoleMenuAccess;
            }
        }

        public IEnumerable<MenuHdr> GetUserModule(int userid)
        {
            List<MenuHdr> MenuLists = new List<MenuHdr>();
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("spGetMenuData", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@userID", userid));
                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();

                while (readerObj.Read())
                {
                    MenuHdr MenuList = new MenuHdr();
                    MenuList.menuID = Convert.ToInt32(readerObj["ID"].ToString());
                    MenuList.MenuText = readerObj["MenuText"].ToString();
                    MenuList.ParentID = readerObj["ParentId"] != DBNull.Value ? Convert.ToInt32(readerObj["ParentId"]) : (int?)null;
                    MenuList.isActive = Convert.ToBoolean(readerObj["Active"].ToString());
                    MenuList.Link = readerObj["Link"].ToString();
                    MenuLists.Add(MenuList);
                }
                conObj.Close();
            }
            return MenuLists;
        }


        public int UpdateMenu(MenuHdr menu)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                conObj.Open();
                SqlCommand cmdObj = new SqlCommand("uspUpdateMenu", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@menuID", menu.menuID));
                cmdObj.Parameters.Add(new SqlParameter("@menuText", menu.MenuText));
                //cmdObj.Parameters.Add(new SqlParameter("@parentID", menu.ParentID));
                cmdObj.Parameters.Add(new SqlParameter("@link", menu.Link));

                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        public int InsertNewMenu(MenuHdr menu)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                conObj.Open();
                SqlCommand cmdObj = new SqlCommand("uspInsertMenu", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@menuText", menu.MenuText));
                cmdObj.Parameters.Add(new SqlParameter("@parentID", menu.ParentID));
                cmdObj.Parameters.Add(new SqlParameter("@link", menu.Link));

                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        public int DeleteMenu(int menuID)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspDeleteMenu", conObj);
                conObj.Open();
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@menuID", menuID));

                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }

        public int InsertNewRole(Roles role)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                conObj.Open();
                SqlCommand cmdObj = new SqlCommand("uspInsertNewRole", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@roleName", role.RoleName));
                cmdObj.Parameters.Add(new SqlParameter("@roleDesc", role.RoleDescription));

                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        public int DeleteRolesMenu(int roleID)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspDeleteRolesMenu", conObj);
                conObj.Open();
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@roleID", roleID));

                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        public int InsertNewRoleAccess(RoleMenuAccess role)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                conObj.Open();
                SqlCommand cmdObj = new SqlCommand("uspInsertRoleAccess", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@RoleID", role.RoleID));
                cmdObj.Parameters.Add(new SqlParameter("@MenuHdrID", role.MenuHdrID));

                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
    }
}
