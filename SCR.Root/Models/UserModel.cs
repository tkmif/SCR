using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using TkmDataAccess;

namespace SCR.Root.Models
{
    public class UserModel
    {
        [DataObjectField(true)]
        public int LoginId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string MobileNo { get; set; }
        public DateTime DOB { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime LastUpdated { get; set; }
        public string CurrentPassword { get; set; }
        //public int LocalOfficeId { get; set; }
        //public string LocalOfficeName { get; set; }

        public int AssocID { get; set; }
        public string AssociationName { get; set; }

        public List<UserRoleModel> UserRole { get; set; }
        public List<LocalBoardModel> LocalBoard { get; set; }

        public List<UserModel> UserModelList { get; set; }
    }
    public class UserDAL : TkmDataAccess.DataProviderBase
    {

        public List<UserRoleModel> FillUserRoles()
        {
            List<UserRoleModel> lstUserRolesModel = new List<UserRoleModel>();
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                DataSet dsUserRoles = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(0);
                dsUserRoles = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "login_roles_Select_Proc");
                if (dsUserRoles.Tables.Count > 0)
                {
                    UserRoleModel userRolesModel = new UserRoleModel();
                    userRolesModel.RoleId = 0;
                    userRolesModel.RoleName = "----Select----";
                    lstUserRolesModel.Add(userRolesModel);
                    foreach (DataRow drCategory in dsUserRoles.Tables[0].Rows)
                    {
                        UserRoleModel userRolesModel1 = new UserRoleModel();
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
        public List<LocalBoardModel> FillLocalBoard()
        {
            List<LocalBoardModel> lstLocalBoardModel = new List<LocalBoardModel>();
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                DataSet dsBoard = new DataSet();
                dbManager.Open();  
                dbManager.CreateParameters(0);
                dsBoard = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "local_board_Select_Proc");
                if (dsBoard.Tables.Count > 0)
                {
                    LocalBoardModel localBoardModel = new LocalBoardModel();
                    localBoardModel.AssocID = 0;
                    localBoardModel.AssociationName = "----Select----";
                    lstLocalBoardModel.Add(localBoardModel);
                    foreach (DataRow drCategory in dsBoard.Tables[0].Rows)
                    {
                        LocalBoardModel userRolesModel1 = new LocalBoardModel();
                        userRolesModel1.AssocID = Convert.ToInt32(drCategory["AssocID"]);
                        userRolesModel1.AssociationName = Convert.ToString(drCategory["AssociationName"]);
                        lstLocalBoardModel.Add(userRolesModel1);
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

            return lstLocalBoardModel;
        }
        public UserModel UserEdit(int LoginId)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            UserModel userModel = new UserModel();

            try
            {
                DataSet dsUser = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@LoginId", LoginId);
                dbManager.AddParameters(1, "@Constrain", null);
                dsUser = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "login_SelectForList_Proc");
                if (dsUser.Tables.Count > 0)
                {
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        userModel.LoginId = Convert.ToInt32(dsUser.Tables[0].Rows[0]["LoginId"]);
                        userModel.Email = Convert.ToString(dsUser.Tables[0].Rows[0]["Email"]);
                        userModel.FirstName = Convert.ToString(dsUser.Tables[0].Rows[0]["FirstName"]);
                        userModel.LastName = Convert.ToString(dsUser.Tables[0].Rows[0]["LastName"]);
                        userModel.RoleId = Convert.ToInt32(dsUser.Tables[0].Rows[0]["RoleId"]);
                        userModel.RoleName = Convert.ToString(dsUser.Tables[0].Rows[0]["RoleName"]);
                        userModel.AssocID = Convert.ToInt32(dsUser.Tables[0].Rows[0]["AssocID"] != DBNull.Value ? Convert.ToInt32(dsUser.Tables[0].Rows[0]["AssocID"]) : 0);
                        userModel.AssociationName = Convert.ToString(dsUser.Tables[0].Rows[0]["AssociationName"] != DBNull.Value ? Convert.ToString(dsUser.Tables[0].Rows[0]["AssociationName"]) : null);
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
            return userModel;
        }
        public bool AddUserProfileDetails(UserModel userModel)
        {

            bool returnValue = false;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, "@LoginId", userModel.LoginId);
                dbManager.AddParameters(1, "@Email", userModel.Email);
                dbManager.AddParameters(2, "@FirstName", userModel.FirstName);
                dbManager.AddParameters(3, "@LastName", userModel.LastName);
                dbManager.AddParameters(4, "@RoleId", userModel.RoleId);
                dbManager.AddParameters(5, "@LastUpdated", System.DateTime.Now);
                dbManager.AddParameters(6, "@ReturnVal", 0, ParameterDirection.Output);
                dbManager.AddParameters(7, "@AssocID", userModel.AssocID);
                dbManager.AddParameters(8, "@Password", userModel.Password);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "login_InsertUpdate_Proc");

                returnVal = (int)dbManager.Parameters[6].Value;

                if (returnVal > 0)
                {
                    returnValue = true;
                    NetworkCredential cred = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString(), System.Configuration.ConfigurationManager.AppSettings["FromPassword"].ToString());
                    MailMessage msg = new MailMessage();
                    msg.To.Add(userModel.Email);
                    msg.Subject = "User Credentials";
                    string body = "Hi " + userModel.FirstName + ",\n\n";

                    body += "Welcome to SCR! \n";

                    if (!string.IsNullOrEmpty(userModel.Password))
                    {
                        string mail_url = System.Configuration.ConfigurationManager.AppSettings["MailUrl"].ToString();
                        body += "Please log in to ";
                        body += mail_url;
                        body += ".\n";
                        body += "Your SCR Username is : " + userModel.Email + "\n";
                        body += "Your temporary SCR Password is: " + userModel.Password + "\n";
                    }
                    body += "\n";
                    body += "Thank you, \n SCR \n";
                    msg.Body = body;
                    msg.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString());
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.Credentials = cred;
                    client.EnableSsl = true;
                    client.Send(msg);
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
        /// Delete User
        /// </summary>
        /// <param name="LoginId"></param>
        /// <returns></returns>
        public int deleteUser(int LoginId)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            int retval = -1;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@LoginId", LoginId);
                dbManager.AddParameters(1, "@ReturnVal", 0, ParameterDirection.Output);
                retval = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "login_Delete_Proc");

                retval = (int)dbManager.Parameters[1].Value;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
            return retval;
        }

        /// <summary>
        /// Get all User details
        /// </summary>
        /// <returns></returns>
        public List<UserModel> getAllUsersList()
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<UserModel> lstUserModel = new List<UserModel>();
            try
            {

                DataSet dsUser = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@LoginId", 0);
                dbManager.AddParameters(1, "@Constrain", null);
                dsUser = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "login_SelectForList_Proc");
                if (dsUser.Tables.Count > 0)
                {
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drUser in dsUser.Tables[0].Rows)
                        {
                            UserModel userModel = new UserModel();
                            userModel.LoginId = Convert.ToInt32(drUser["LoginId"]);
                            userModel.Email = Convert.ToString(drUser["Email"]);
                            userModel.FirstName = Convert.ToString(drUser["FirstName"]);
                            userModel.LastName = Convert.ToString(drUser["LastName"]);
                            userModel.RoleId = Convert.ToInt32(drUser["RoleId"]);
                            userModel.RoleName = Convert.ToString(drUser["RoleName"]);
                            userModel.AssocID = Convert.ToInt32(drUser["AssocID"] != DBNull.Value ? Convert.ToInt32(drUser["AssocID"]) : 0);
                            userModel.AssociationName = Convert.ToString(drUser["AssociationName"] != DBNull.Value ? Convert.ToString(drUser["AssociationName"]) : null);
                            lstUserModel.Add(userModel);
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
            return lstUserModel;
        }

        public bool UserEmailCheck(string Email)
        {
            bool boolresult = true;

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                DataSet dsUserInfo = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@EmailIds", Email);
                dbManager.AddParameters(1, "@UserPassword", "");
                dsUserInfo = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "login_Select_Proc");

                if (dsUserInfo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drUserInfo in dsUserInfo.Tables[0].Rows)
                    {
                        LoginModel LoginModel = new LoginModel();
                        LoginModel.LoginId = Convert.ToInt32(Convert.ToString(drUserInfo["LoginId"]));
                        LoginModel.EmailId = Convert.ToString(drUserInfo["Email"]);
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
    }
}