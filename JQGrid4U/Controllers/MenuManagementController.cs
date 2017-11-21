using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JQGrid4U.BL;
using System.Data;
using System.Text;

namespace JQGrid4U.Controllers
{
    [SessionExpire]
    public class MenuManagementController : Controller
    {
        // GET: MenuManagement
        RoleManagement RoleManagementTables = new RoleManagement();

        public ActionResult Index()
        {
            return View();
        }


        public string UserMenus()
        {
            StringBuilder sb = new StringBuilder();
            List<MenuHdr> menuOne = RoleManagementTables.MenuList.Where(p => p.ParentID == null).ToList();//db.SubMenus.Where(p => p.parentMenuId == parentid).ToList();

            //sb.Append("<ul>");
            foreach (MenuHdr list in menuOne)
            {
                sb.Append("<li value=" + list.menuID + "><a id=" + list.menuID + " parentvalue=" + list.ParentID + " moduleType=\"MainMenu\" class='mainMenu'>" + list.MenuText + "</a> <i class='glyphicon glyphicon-remove Deletemenu' data-toggle='tooltip' data-placement='top' title='Delete' menuID=" + list.menuID + "></i> <i editmenuID=" + list.menuID + " class='glyphicon glyphicon-pencil editmodal' data-toggle='tooltip' data-placement='right' title='Edit'></i>");
                sb.Append(SubMenu(list.menuID, list.menuID));
                sb.Append("</li>");

            }
            sb.Append("<li class='form-inline'><input style='width:90%; margin-bottom:5px;' type='text' id='parentMenu' class='input-sm form-control'/> <i value='1' parentID='null' class='glyphicon glyphicon-plus one'></i></li>");

            //sb.Append("</ul>");
            return sb.ToString();
        }
        public String SubMenu(int? id, int? parentID)
        {
            StringBuilder sb = new StringBuilder();
            List<MenuHdr> submenu = RoleManagementTables.MenuList.Where(p => p.ParentID == id).ToList();
            sb.Append("<ul>");
            foreach (var list in submenu)
            {
                sb.Append("<li value=" + list.menuID + "><a id=" + list.menuID + " parentvalue=" + list.ParentID + " moduleType=\"2ndMenu\">" + list.MenuText + "</a> <i class='glyphicon glyphicon-remove Deletemenu' data-toggle='tooltip' data-placement='top' title='Delete' menuID=" + list.menuID + "></i> <i editmenuID=" + list.menuID + " class='glyphicon glyphicon-pencil editmodal' data-toggle='tooltip' data-placement='right' title='Edit'></i>");
                sb.Append(SubMenu(list.menuID, list.menuID));
                sb.Append("</li>");
            }
            sb.Append("<li class='form-inline'><input style='width:90%; margin-bottom:5px;' type='text' class='input-sm form-control'/> <i parentID='" + parentID + "' class='glyphicon glyphicon-plus two'></i></li>");
            sb.Append("</ul>");
            return sb.ToString();
        }
        public JsonResult InsertNewMenu(MenuHdr menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (RoleManagementTables.InsertNewMenu(new MenuHdr { MenuText = menu.MenuText, ParentID = (menu.ParentID == null ? 0 : menu.ParentID), Link = menu.Link == null ? "" : menu.Link }) > 0)
                        return Json(new { isError = "F", message = "New menu has been added!" });
                    else
                        return Json(new { isError = "T", message = "Menu already exist." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Json(new { isError = "T", message = "Could not insert data." });
                }
            }
            else
            {
                return Json(new { isError = "T", message = "Could not insert data." });
            }
        }
        public JsonResult GetMenuDetails(int menuid)
        {
            MenuHdr data = new MenuHdr();
            MenuHdr container = RoleManagementTables.MenuList.Single(p => p.menuID == menuid);
            data.menuID = container.menuID;
            data.MenuText = container.MenuText;
            data.ParentID = container.ParentID;
            data.Link = container.Link;

            return Json(data);
        }
        public JsonResult DeleteMenu(int menuID)
        {
            int Id = Convert.ToInt32(menuID);

            if (RoleManagementTables.DeleteMenu(menuID) > 0)
            {
                return Json(new { isError = "F", message = "Menu Deleted Successfully." });
            }
            else
            {
                return Json(new { isError = "T", message = "Error. Could Not Delete Data" });
            }
        }
        public JsonResult UpdateMenu(MenuHdr menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (RoleManagementTables.UpdateMenu(new MenuHdr { menuID = menu.menuID, Link = menu.Link, MenuText = menu.MenuText }) > 0)
                        return Json(new { isError = "F", message = "Menu has been Updated!" });
                    else
                        return Json(new { isError = "T", message = "Menu already exist." });
                }
                catch (Exception ex)
                {
                    return Json(new { isError = "T", message = "Could not insert data." });
                }
            }
            else
            {
                return Json(new { isError = "T", message = "Could not insert data." });
            }
        }
    }
}