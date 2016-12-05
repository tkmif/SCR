using SCR.Root.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using TkmDataAccess;

namespace SCR.Root.Models
{
    public class MasterModel
    {
        public int PageId { get; set; }
        public string UIName { get; set; }
        public string StyleClass { get; set; }
        public bool ContainChild { get; set; }
        public int ParentPageId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int Order { get; set; }
    }

    public class MasterDAL : TkmDataAccess.DataProviderBase
    {
        public List<MasterModel> GetPrivilegedPages()
        {
            List<MasterModel> lstMasterModel = new List<MasterModel>();

            UserSession userSession = new UserSession();
            if (userSession.Exists)
            {
                if (HttpContext.Current.Session["UserPrivilegedPages"] == null)
                {
                    lstMasterModel = GetRolePageAccessList(userSession.LoginId);
                    HttpContext.Current.Session["UserPagePrivilege"] = lstMasterModel;
                    return lstMasterModel;
                    
                }
                else
                {
                    //LstOBJPPM = (List<UserGroupPagePrivilegesModel>)HttpContext.Current.Session["UserPagePrivilege"];
                    //if (string.IsNullOrEmpty(StrPageURL.Trim()))
                    //{
                    //    return LstOBJPPM;
                    //}
                    //else
                    //{
                    //    List<UserGroupPagePrivilegesModel> LstSinglePagePrivilege = new List<UserGroupPagePrivilegesModel>();
                    //    LstSinglePagePrivilege = LstOBJPPM.Where(priv => priv.StrPageURL.Contains(StrPageURL.Trim())).ToList();
                    //    return LstSinglePagePrivilege;
                    //}
                }
                return lstMasterModel;
            }
            else
            {
                return lstMasterModel;
            }
        }

        public List<MasterModel> GetRolePageAccessList(int LoginId)
        {
            List<MasterModel> lstMasterModel = new List<MasterModel>();
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                DataSet dsPrivilegedPages = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@LoginId", LoginId);
                dsPrivilegedPages = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[db_owner].role_page_access_Select_Proc");
                if (dsPrivilegedPages.Tables.Count > 0)
                {
                    foreach (DataRow drPrivilegedPages in dsPrivilegedPages.Tables[0].Rows)
                    {
                        MasterModel masterModel = new MasterModel();
                        masterModel.PageId = Convert.ToInt32(Convert.ToString(drPrivilegedPages["PageId"]));
                        masterModel.UIName = Convert.ToString(Convert.ToString(drPrivilegedPages["UIName"]));
                        masterModel.StyleClass = Convert.ToString(drPrivilegedPages["StyleClass"]);
                        if (Convert.ToString(drPrivilegedPages["ContainChild"]) != "")
                        masterModel.ContainChild = Convert.ToBoolean(Convert.ToString(drPrivilegedPages["ContainChild"]));
                        if (Convert.ToString(drPrivilegedPages["ParentPageId"]) != "")
                        masterModel.ParentPageId = Convert.ToInt32(Convert.ToString(drPrivilegedPages["ParentPageId"]));
                        masterModel.Action = Convert.ToString(drPrivilegedPages["Action"]);
                        masterModel.Controller = Convert.ToString(drPrivilegedPages["Controller"]);
                        masterModel.Order = Convert.ToInt32(Convert.ToString(drPrivilegedPages["Order"]));
                        lstMasterModel.Add(masterModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
            return lstMasterModel;
        }
    }
}