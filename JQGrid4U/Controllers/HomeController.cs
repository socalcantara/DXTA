using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using JQGrid4U.Models;
using JQGrid4U.BL;
using System.Web.Script.Serialization;

namespace JQGrid4U.Controllers
{
    public class HomeController : Controller
    {
        RoleManagement RoleManagementTables = new RoleManagement();

        public ActionResult Index()
        {
            //return RedirectPermanent(Url.Content("~/api/Product"));
				return RedirectPermanent(Url.Content("~/Dboard"));

		}
        public bool hasChild(int? parentID, int roleid)
        {
            return RoleManagementTables.GetUserModule(roleid).Any(p => p.ParentID == parentID && p.isActive == true);
        }
        public string buildMenu(int UserID)
        {
            StringBuilder sb = new StringBuilder();
            //List<UserRoleAccess> userAccess = RoleManagementTables.UserRoleAccess.Where(x)
            List<MenuHdr> menuOne = RoleManagementTables.GetUserModule(UserID).Where(p => p.ParentID == null).ToList();//db.SubMenus.Where(p => p.parentMenuId == parentid).ToList();

            //sb.Append("<ul>");
            foreach (MenuHdr list in menuOne)
            {
                sb.Append("<li" + (hasChild(list.menuID, UserID) ? " class='dropdown'" : " " ) + "><a href=\"" + (list.Link != null ? (list.Link.Trim() != "") ? Url.Content("~/" + list.Link) : "#" : "#" ) + "\"" + (hasChild(list.menuID, UserID) ? "class='dropdown-toggle' role='button' data-toggle='dropdown'" : "") + ">" + list.MenuText +  (hasChild(list.menuID, UserID) ? " <span class='caret'></span>" : "") + "</a>");
                    if (hasChild(list.menuID, UserID))
                    {
                        sb.Append(SubMenu(list.menuID, UserID));
                    }
                sb.Append("</li>");

            }
            //sb.Append("</ul>");
            return sb.ToString();
        }

        public string SubMenu(int? SecondMenu, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            List<MenuHdr> submenu = RoleManagementTables.GetUserModule(UserID).Where(p => p.ParentID == SecondMenu).ToList();

            sb.Append("<ul class='dropdown-menu'>");
            foreach (var list in submenu)
            {
                sb.Append("<li" + (hasChild(list.menuID, UserID) ? " class='dropdown-submenu'" : " ") + "><a href=\"" + (list.Link != null ? (list.Link.Trim() != "") ? Url.Content("~/" + list.Link) : "#" : "#") + "\""+  (hasChild(list.menuID, UserID) ? " class='dropdown-toggle' data-toggle='dropdown' " : " ") +  ">" + list.MenuText + "</a>");
                    if (hasChild(list.menuID, UserID))
                    {
                        sb.Append(SubMenu(list.menuID, UserID));
                    }
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}