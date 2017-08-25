using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TkmDataAccess;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;
using SCR.Root.App_Code;

namespace SCR.Root.Models
{
    [MetadataType(typeof(LoginModel))]
    public class LoginModel
    {
        [DataObjectField(true)]
        public int LoginId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

        public string Key { get; set; }

        public DateTime ExpiredOn { get; set; }

        public int ForgotStatus { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailIdForgot { get; set; }

        public string RoleName { get; set; }

        public int AssocID { get; set; }
        public string AssociationName { get; set; }

        public List<UserRolePagePrivilegeModel> UserRolePagePrivilege { get; set; }
    }
    public class LoginDAL : TkmDataAccess.DataProviderBase
    {
        /// It is to Check User trying to log in is a valid user        
        public List<LoginModel> IsValidUser(string Email, string Password)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<LoginModel> lstLoginModel = new List<LoginModel>();
            try
            {

                DataSet dsUserInfo = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@EmailIds", Email);
                dbManager.AddParameters(1, "@UserPassword", Password);
                dsUserInfo = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[db_owner].login_Select_Proc");
                if (dsUserInfo.Tables.Count > 0)
                {
                    if (dsUserInfo.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drUser in dsUserInfo.Tables[0].Rows)
                        {
                            LoginModel LoginModel = new LoginModel();
                            LoginModel.LoginId = Convert.ToInt32(drUser["LoginId"]);
                            LoginModel.EmailId = Convert.ToString(drUser["Email"]);
                            LoginModel.Password = Convert.ToString(drUser["Password"]);
                            LoginModel.Name = Convert.ToString(drUser["Name"]);
                            LoginModel.Status = Convert.ToInt32(drUser["IsActive"]);
                            LoginModel.RoleName = Convert.ToString(drUser["RoleName"]);
                            LoginModel.AssocID = Convert.ToInt32(drUser["AssocID"] != DBNull.Value ? Convert.ToInt32(drUser["AssocID"]) : 0);
                            LoginModel.AssociationName = Convert.ToString(drUser["AssociationName"] != DBNull.Value ? Convert.ToString(drUser["AssociationName"]) : null);                          
                            UserSession user = new UserSession();
                            user.setUser(LoginModel);
                            GC.Collect();
                            lstLoginModel.Add(LoginModel);

                        }
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
            return lstLoginModel;
        }

        public bool UserEmailCheck(string email)
        {
            bool boolresult = true;

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                DataSet dsUserInfo = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@EmailIds", email);
                dbManager.AddParameters(1, "@UserPassword", "");
                dsUserInfo = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[db_owner].login_Select_Proc");

                if (dsUserInfo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drUserInfo in dsUserInfo.Tables[0].Rows)
                    {
                        LoginModel LoginModel = new LoginModel();
                        LoginModel.LoginId = Convert.ToInt32(Convert.ToString(drUserInfo["LoginId"]));
                        LoginModel.EmailIdForgot = Convert.ToString(drUserInfo["Email"]);
                        LoginModel.Password = Convert.ToString(drUserInfo["Password"]);
                        GC.Collect();
                    }
                }
                else
                {
                    boolresult = false;
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
            return boolresult;
        }

        public bool ForgotPwd(LoginModel loginModel)
        {

            bool returnValue = false;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@Email", loginModel.EmailIdForgot);
                //  dbManager.AddParameters(1, "@Password", loginModel.Password);
                dbManager.AddParameters(1, "@Key", loginModel.Key);
                dbManager.AddParameters(2, "@ExpiredOn", loginModel.ExpiredOn);
                dbManager.AddParameters(3, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "[dbo].login_UpdatePassword_Proc");

                returnVal = (int)dbManager.Parameters[3].Value;

                if (returnVal > 0)
                {
                    returnValue = true;
                    /*// Address from where you send the mail
                    // var fromAddress = "geethanjali@tkminfotech.com";
                    string fromAddress = System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString();

                    // any address where the email will be sending
                    var toAddress = loginModel.EmailIdForgot;
                    //Password of your from email address
                    //  const string fromPassword = "Gee@123!@#*";
                    string fromPassword = System.Configuration.ConfigurationManager.AppSettings["FromPassword"].ToString();
                    // Passing the values and make a email formate to display
                    string subject = "User Credentials";
                    string body = "Hi ,\n\n";

                    body += "Welcome to SCR Realtors! \n";

                    if (!string.IsNullOrEmpty(loginModel.Key))
                    {
                        string mail_url = System.Configuration.ConfigurationManager.AppSettings["MailUrl"].ToString();
                        body += "Please click the link given below ";
                        body += "http://http://localhost:62262/Login/ChangePassword/" + loginModel.Key + "\n";  
                        // body += ".\n";
                        //body += "Your  Username is: " + loginModel.EmailIdForgot + "\n";
                        //body += "Your temporary Password is: " + loginModel.Password + "\n";
                    }

                    body += "\n";
                    body += "\nSCR Realtors\n";
                    // smtp settings
                    var smtp = new System.Net.Mail.SmtpClient();
                    {
                        // smtp.Host = "mail.tkminfotech.com";
                        smtp.Host = System.Configuration.ConfigurationManager.AppSettings["MailServer"].ToString();
                        smtp.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                        smtp.EnableSsl = false;
                        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                        smtp.Timeout = 20000;
                    }
                    // Passing values to smtp object
                    smtp.Send(fromAddress, toAddress, subject, body);*/
                    NetworkCredential cred = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString(), System.Configuration.ConfigurationManager.AppSettings["FromPassword"].ToString());                    
                    MailMessage msg = new MailMessage();
                    msg.To.Add(loginModel.EmailIdForgot);
                    msg.Subject = "User Credentials";
                    string body = "Hi ,\n\n";

                    body += "Welcome to SCR! \n";

                    if (!string.IsNullOrEmpty(loginModel.Key))
                    {
                        string mail_url = System.Configuration.ConfigurationManager.AppSettings["MailUrl"].ToString();
                        body += "Please click the link given below ";
                        //body += "http://http://localhost:62262/Login/ChangePassword/" + loginModel.Key + "\n";



                        body += mail_url + "Login/ChangePassword/" + loginModel.Key + " . \n";
                        body += "\n";
                        // body += ".\n";
                        //body += "Your  Username is: " + loginModel.EmailIdForgot + "\n";
                        //body += "Your temporary Password is: " + loginModel.Password + "\n";
                    }

                    body += "\n";
                    body += "\nSCR Realtors\n";
                    //msg.Body = body;
                    //msg.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString());
                    //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    //client.Credentials = cred;
                    //client.EnableSsl = true;
                    //client.Send(msg);

                 UserDAL obj= new UserDAL();
                    obj.SendMail(loginModel.EmailIdForgot,body);
                }
                else
                {
                    returnValue = false;
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
            return returnValue;

        }

        public List<LoginModel> IsValidKey(string key)
        {
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<LoginModel> lstLoginModel = new List<LoginModel>();
            try
            {

                DataSet dsUserInfo = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@key", key);
                dsUserInfo = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[db_owner].temp_forgot_password_SelectForKeyCheck_Proc");
                if (dsUserInfo.Tables.Count > 0)
                {
                    if (dsUserInfo.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drUser in dsUserInfo.Tables[0].Rows)
                        {
                            LoginModel LoginModel = new LoginModel();
                            LoginModel.LoginId = Convert.ToInt32(Convert.ToString(drUser["LoginId"]));
                            LoginModel.Key = Convert.ToString(drUser["KeyId"]);
                            LoginModel.ExpiredOn = Convert.ToDateTime(drUser["ExpiredOn"]);
                            LoginModel.ForgotStatus = Convert.ToInt32(drUser["Status"]);
                            LoginModel.EmailId = Convert.ToString(drUser["Email"]);
                            lstLoginModel.Add(LoginModel);
                        }
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
            return lstLoginModel;
        }

        public bool UpdatePasswordDetails(LoginModel loginModel)
        {

            bool returnValue = false;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, "@Email", loginModel.EmailId);
                dbManager.AddParameters(1, "@Password", loginModel.NewPassword);
                dbManager.AddParameters(2, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "[dbo].temp_forgot_password_Update_Proc");

                returnVal = (int)dbManager.Parameters[2].Value;

                if (returnVal > 0)
                {
                    returnValue = true;                  
                }
                else
                {
                    returnValue = false;
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
            return returnValue;

        }

        /// <summary>
        /// Get User role page privilege
        /// </summary>
        /// <returns></returns>
        public List<UserRolePagePrivilegeModel> getUserRolePagePrivilege(string Email)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<UserRolePagePrivilegeModel> lstUserRolePagePrivilegeModel = new List<UserRolePagePrivilegeModel>();
            try
            {

                DataSet dsUser = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@EmailIds", Email);
                //dbManager.AddParameters(1, "@Constrain", null);
                dsUser = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[db_owner].menu_list_Select_Proc");
                if (dsUser.Tables.Count > 0)
                {
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drUser in dsUser.Tables[0].Rows)
                        {
                            UserRolePagePrivilegeModel userRolePagePrivilegeModel = new UserRolePagePrivilegeModel();
                            userRolePagePrivilegeModel.IntPageId = Convert.ToInt32(drUser["PageId"]);
                            userRolePagePrivilegeModel.StrUIName = Convert.ToString(drUser["UIName"]);
                            userRolePagePrivilegeModel.StrStyleClass = Convert.ToString(drUser["StyleClass"]);
                            userRolePagePrivilegeModel.BoolContainsChild = Convert.ToBoolean(drUser["ContainChild"]);
                            userRolePagePrivilegeModel.IntParentPageId = (drUser["ParentPageId"]) != DBNull.Value ? Convert.ToInt32(drUser["ParentPageId"]) : 0;
                            userRolePagePrivilegeModel.StrAction = Convert.ToString(drUser["Action"]);
                            userRolePagePrivilegeModel.StrController = Convert.ToString(drUser["Controller"]);
                            userRolePagePrivilegeModel.IntOrder = Convert.ToInt32(drUser["Order"]);
                            UserSession user = new UserSession();
                            //user.setUserRolesPagePrivilege(userRolePagePrivilegeModel);
                            GC.Collect();
                            //System.Web.HttpContext.Current.Session["PageId1"] = userRolePagePrivilegeModel.IntPageId;
                            lstUserRolePagePrivilegeModel.Add(userRolePagePrivilegeModel);
                            user.setUserRolesPagePrivilege(lstUserRolePagePrivilegeModel);
                        }
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
            return lstUserRolePagePrivilegeModel;
        }
    }

    

    public class validate
    {
        public int check()
        {
            DateTime trailstart = Convert.ToDateTime("05-24-2017");

            DateTime currentdate =  DateTime.Now;
            System.TimeSpan diff = currentdate.Subtract(trailstart);
            if (diff.Days > 120)
                return 1;

            return 0;
        }

    }
}