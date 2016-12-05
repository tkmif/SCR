using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TkmDataAccess;
using System.Configuration;
using System.Data;
namespace SCR.Root.Models
{
    [MetadataType(typeof(SettingsModel))]
    public class SettingsModel
    {
        [DataObjectField(true)]
        public int? LoginId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public DateTime LastUpdated { get; set; }

    }
    public class SettingsDAL : TkmDataAccess.DataProviderBase
    {
        public bool upadatePassword(SettingsModel settingsModel,int LoginUser)
        {
            bool boolresult = true;

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                DataSet dsUserInfo = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, "@LoginId", LoginUser);
                dbManager.AddParameters(1, "@Password", settingsModel.NewPassword);
                dbManager.AddParameters(2, "@LastUpdated", System.DateTime.Now);
                dsUserInfo = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].login_PasswordUpdate_Proc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
            return boolresult;
        }

        public string checkPasswordForlogin(int LoginUser)
        {
            string result = null;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                DataSet dsUserInfo = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@LoginId", LoginUser);
                dsUserInfo = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].login_SelectPassword_Proc");
                if (dsUserInfo.Tables[0].Rows.Count > 0)
                {
                    result = Convert.ToString(dsUserInfo.Tables[0].Rows[0]["Password"]);
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
            return result;
        }
      
    }
}