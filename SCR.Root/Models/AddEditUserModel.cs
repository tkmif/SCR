using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TkmDataAccess;
using System.Configuration;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SCR.Root.Models
{
    public class AddEditUserModel
    {
        [DataObjectField(true)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int AssetId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string OfficePhoneNo { get; set; }
        public string MobileNo { get; set; }
        public int Status { get; set; }
        public int RoleId { get; set; }
        public string ImageType { get; set; }
        public string ImagePath { get; set; }
        public string ImageLength { get; set; }
        public string CurrentPassword { get; set; }
        public List<UserRolesModel> UserRole { get; set; }
    }
    public class AddEditUserDAL : TkmDataAccess.DataProviderBase
    {

        public List<UserRolesModel> FillUserRoles()
        {
            List<UserRolesModel> lstUserRolesModel = new List<UserRolesModel>();
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                DataSet dsUserRoles = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(0);
                dsUserRoles = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "login_roles_Select_Proc");
                if (dsUserRoles.Tables.Count > 0)
                {
                    UserRolesModel userRolesModel = new UserRolesModel();
                    userRolesModel.RoleId = 0;
                    userRolesModel.RoleName = "----Select----";
                    lstUserRolesModel.Add(userRolesModel);
                    foreach (DataRow drCategory in dsUserRoles.Tables[0].Rows)
                    {
                        UserRolesModel userRolesModel1 = new UserRolesModel();
                        userRolesModel1.RoleId = Convert.ToInt32(drCategory["RoleId"]);
                        userRolesModel1.RoleName = Convert.ToString(drCategory["RoleName"]);
                        lstUserRolesModel.Add(userRolesModel1);
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

            return lstUserRolesModel;
        }
    }
}