using SCR.Root.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SCR.Root.App_Code
{
   public class UserSession
    {
        static UserModel user = new UserModel();        
        public  int LoginId
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    return ((LoginModel)HttpContext.Current.Session["User"]).LoginId;
                }
                return 0;
            }

        }
        public string Password
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    return ((LoginModel)HttpContext.Current.Session["User"]).Password;
                }
                return null;
            }
        }
        public bool Exists
        {
            get
            {
                return HttpContext.Current.Session["User"] != null ? true : false;
            }
        }
        public LoginModel GetUser
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                {
                    return ((LoginModel)HttpContext.Current.Session["User"]);
                }
                return null;
            }
        }
        public void setUser(LoginModel objuser)
        {
            HttpContext.Current.Session["User"] = objuser;
        }


        public List<UserRoleModel> GetUserRoles
        {
            get
            {
                if (HttpContext.Current.Session["UserRoles"] != null)
                {
                    return ((List<UserRoleModel>)HttpContext.Current.Session["UserRoles"]);
                }
                return null;
            }
        }

        public void setUserRoles(List<UserRoleModel> objLstUserRoles)
        {
            HttpContext.Current.Session["UserRoles"] = objLstUserRoles;
        }
        

        public List<UserRolePagePrivilegeModel> GetUserRolesPagePrivilege
        {
            get
            {
                if (HttpContext.Current.Session["UserRolesPagePrivilege"] != null)
                {
                    return ((List<UserRolePagePrivilegeModel>)HttpContext.Current.Session["UserRolesPagePrivilege"]);
                }
                return null;
            }
        }
        public void setUserRolesPagePrivilege(List<UserRolePagePrivilegeModel> objuserpageprivilege)
        {
            HttpContext.Current.Session["UserRolesPagePrivilege"] = objuserpageprivilege;
        }

        public int PageNo1
        {
            get
            {
                if (HttpContext.Current.Session["PageId1"] != null)
                {
                    return ((UserRolePagePrivilegeModel)HttpContext.Current.Session["PageId1"]).IntPageId;
                }
                return 0;
            }

        }
        
        internal void EmptyUserSession()
        {
            HttpContext.Current.Session.Abandon();
        }
    }
}
