using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using TkmDataAccess;

namespace SCR.Root.Models
{
    /// <summary>
    /// Author : Geethanjali P Nair , Created on : 26 Feb 2015
    /// </summary>
    public class LocalOfficeModel
    {
        [Key]

        public int LocalOfficeId { get; set; }
        public string LocalOfficeName { get; set; }
        public string RegisteredNumber { get; set; }
        public string LocalOfficeAddress { get; set; }
        public int? MailingZipCode { get; set; }
        public string OfficePhoneNo { get; set; }
        public string Email { get; set; }
        public DateTime LastUpdated { get; set; }

        public int LoginId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? RoleId { get; set; }


        public List<LocalOfficeModel> LocalOfficeModelList { get; set; }
    }

    public class LocalOfficeDAL : TkmDataAccess.DataProviderBase
    {

        /// <summary>
        /// To Add Local Office
        /// </summary>
        /// <param name="localOfficeModel"></param>
        /// <returns></returns>
        public int addLocalOffice(LocalOfficeModel localOfficeModel)
        {
            int returnVal = 0;
          /*  int ret_Val = 0;
            int retVal = 0;*/
            LocalOfficeDAL objLocalOfficeDAL=new LocalOfficeDAL();
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, "@LocalOfficeId", localOfficeModel.LocalOfficeId);
                dbManager.AddParameters(1, "@LocalOfficeName", localOfficeModel.LocalOfficeName);
                dbManager.AddParameters(2, "@RegisteredNumber", localOfficeModel.RegisteredNumber);
                dbManager.AddParameters(3, "@LocalOfficeAddress", localOfficeModel.LocalOfficeAddress);
                dbManager.AddParameters(4, "@MailingZipCode", localOfficeModel.MailingZipCode);
                dbManager.AddParameters(5, "@OfficePhoneNo", localOfficeModel.OfficePhoneNo);
                dbManager.AddParameters(6, "@LastUpdated", DateTime.Now);
                dbManager.AddParameters(7, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "local_office_InsertUpdate_Proc");
                returnVal = (int)dbManager.Parameters[7].Value;

                /*Geethanjali on MAR 03 2015*/
                //if (returnVal != 0)
                //{
                //    ret_Val = objLocalOfficeDAL.addLocalOfficeUser(localOfficeModel);
                //    if (ret_Val != 0 && type!=1)
                //    {
                //        retVal = objLocalOfficeDAL.addLocalOfficeAdmin(ret_Val, returnVal);

                //    }
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
            return returnVal;
        }


        /// <summary>
        /// To Add Local Office User details
        /// </summary>
        /// <param name="localOfficeModel"></param>
        /// <returns></returns>
        public int addLocalOfficeUser(LocalOfficeModel localOfficeModel)
        {
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, "@LoginId", localOfficeModel.LoginId);
                dbManager.AddParameters(1, "@Password", localOfficeModel.Password);
                dbManager.AddParameters(2, "@FirstName", localOfficeModel.FirstName);
                dbManager.AddParameters(3, "@LastName", localOfficeModel.LastName);
                dbManager.AddParameters(4, "@RoleId", localOfficeModel.RoleId);
                dbManager.AddParameters(5, "@Email", localOfficeModel.Email);
                dbManager.AddParameters(6, "@LastUpdated", DateTime.Now);
                dbManager.AddParameters(7, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "login_InsertUpdate_Proc");
                returnVal = (int)dbManager.Parameters[7].Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
            return returnVal;
        }

        /// <summary>
        /// To Add Local office Admin
        /// </summary>
        /// <param name="localOfficeModel"></param>
        /// <returns></returns>
        public int addLocalOfficeAdmin(int loginId, int localOfficeId)
        {
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, "@LoginId", loginId);
                dbManager.AddParameters(1, "@LocalOfficeId", localOfficeId);               
                dbManager.AddParameters(2, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "local_office_admin_InsertUpdate_Proc");
                returnVal = (int)dbManager.Parameters[2].Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
            return returnVal;
        }


        /// <summary>
        /// Get Local Office For Edit
        /// </summary>
        /// <param name="localOfficeId"></param>
        /// <returns></returns>
        public LocalOfficeModel getLocalOfficeForEdit(int localOfficeId)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            LocalOfficeModel localOfficeModel = new LocalOfficeModel();

            try
            {
                DataSet dsLocalOffice = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@LocalOfficeId", localOfficeId);
                dbManager.AddParameters(1, "@Constrain", null);
                dsLocalOffice = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "local_office_Select_Proc");
                if (dsLocalOffice.Tables.Count > 0)
                {
                    if (dsLocalOffice.Tables[0].Rows.Count > 0)
                    {
                        localOfficeModel.LocalOfficeId = Convert.ToInt32(dsLocalOffice.Tables[0].Rows[0]["LocalOfficeId"]);
                        localOfficeModel.LocalOfficeName = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["LocalOfficeName"]);
                        localOfficeModel.RegisteredNumber = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["RegisteredNumber"]);
                        localOfficeModel.LocalOfficeAddress = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["LocalOfficeAddress"]);
                        localOfficeModel.MailingZipCode = Convert.ToInt32(dsLocalOffice.Tables[0].Rows[0]["MailingZipCode"]);
                        localOfficeModel.OfficePhoneNo = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["OfficePhoneNo"]);
                        /* localOfficeModel.LoginId = Convert.ToInt32(dsLocalOffice.Tables[0].Rows[0]["LoginId"]);
                         localOfficeModel.Email = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["Email"]);
                        localOfficeModel.FirstName = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["FirstName"]);
                        localOfficeModel.LastName = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["LastName"]);
                        localOfficeModel.Password = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["Password"]);
                        localOfficeModel.ConfirmPassword = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["Password"]);*/
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
            return localOfficeModel;
        }

        /// <summary>
        /// Get all Local Office details
        /// </summary>
        /// <returns></returns>
        public List<LocalOfficeModel> getAllLocalOfficeList()
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
          
            List<LocalOfficeModel> lstLocalOfficeModel = new List<LocalOfficeModel>();
            try
            {

                DataSet dsLocalOffice = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@LocalOfficeId", 0);
                dbManager.AddParameters(1, "@Constrain", null);
                dsLocalOffice = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "local_office_Select_Proc");
                if (dsLocalOffice.Tables.Count > 0)
                {
                    if (dsLocalOffice.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drloff in dsLocalOffice.Tables[0].Rows)
                        {
                            LocalOfficeModel localOfficeModel = new LocalOfficeModel();
                            localOfficeModel.LocalOfficeId =  Convert.ToInt32(drloff["LocalOfficeId"]);
                            localOfficeModel.LocalOfficeName = Convert.ToString(drloff["LocalOfficeName"]);
                            localOfficeModel.RegisteredNumber = Convert.ToString(drloff["RegisteredNumber"]);
                            localOfficeModel.LocalOfficeAddress = Convert.ToString(drloff["LocalOfficeAddress"]);
                            localOfficeModel.MailingZipCode = Convert.ToInt32(drloff["MailingZipCode"]);
                            localOfficeModel.OfficePhoneNo = Convert.ToString(drloff["OfficePhoneNo"]);
                            /* localOfficeModel.Email = Convert.ToString(dsLocalOffice.Tables[0].Rows[0]["Email"]);
                           localOfficeModel.FirstName = Convert.ToString(drloff["FirstName"]);
                            localOfficeModel.LastName = Convert.ToString(drloff["LastName"]);
                            localOfficeModel.Password = Convert.ToString(drloff["Password"]);
                            localOfficeModel.ConfirmPassword = Convert.ToString(drloff["Password"]);*/
                            lstLocalOfficeModel.Add(localOfficeModel);
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
            return lstLocalOfficeModel;
        }

        /// <summary>
        /// Delete Local Office
        /// </summary>
        /// <param name="localOfficeid"></param>
        /// <returns></returns>
        public int deleteLocalOffice(int localOfficeid)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            int retval = -1;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@LocalOfficeId", localOfficeid);
                dbManager.AddParameters(1, "@ReturnVal", 0, ParameterDirection.Output);
                retval = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "local_office_Delete_Proc");

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
    }
}