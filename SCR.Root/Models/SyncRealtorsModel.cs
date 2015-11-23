using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TkmDataAccess;
using System.Configuration;
using System.Data;
using SCR.Root.Controllers;

namespace SCR.Root.Models
{
    [MetadataType(typeof(SyncRealtorsModel))]
    public class SyncRealtorsModel
    {
        public int MemberId { get; set; } 
        public int RecordType { get; set; }
        public int TransmittalBatchNumber { get; set; }
        public int TransactionNumber { get; set; }
        public int TransactionDate { get; set; }
        public int TransactionTime { get; set; }
        public string RecordChangeType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public string Salutation { get; set; }
        public string Gender { get; set; }
        public string MailAddress { get; set; }
        public string MailAttnCareOf { get; set; }
        public string MailCity { get; set; }
        public string MailState { get; set; }
        public string MailZIPCode { get; set; }
        public string MailZIPCode6 { get; set; }
        public int OfficeId { get; set; }
        public string MemberType { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string RELicenseNumber { get; set; }
        public DateTime LocalJoinDate { get; set; }
        public string OnlineStatus { get; set; }
        public DateTime OnlineStatusDate { get; set; }
        public int PrimaryAssociationID { get; set; }
        public string PrimaryStateAssociationID { get; set; }
        public string PrimaryFieldofBusiness { get; set; }
        public string EMailAddress { get; set; }
        public DateTime NARDuesPaid { get; set; }
        public DateTime StateDuesPaid { get; set; }
        public string MemberSubclass { get; set; }
        public int CellPhoneAreaCode { get; set; }
        public int CellPhoneNumber { get; set; }
        public string DesignatedRealtor { get; set; }
        public int NRDSInsertDate { get; set; }
        public int NRDSStatus { get; set; }
        public DateTime InsertedOn { get; set; }
     
        

        public Int32 OfficeTransactionDate { get; set; }
        public int SenderId { get; set; }       
        public int TransactionTotal { get; set; }
        public int AssociationId { get; set; }
        public string OfficeBusinessName { get; set; }
        public string SortSequence { get; set; }
        public string StreetAddress { get; set; }
        public string StreetCity { get; set; }
        public string StreetState { get; set; }
        public string StreetZIP { get; set; }
        public int OfficeAreaCode { get; set; }
        public int OfficePhoneNumber { get; set; }
        public int OfficeContactDR { get; set; }
        public int NMSalespersonCount { get; set; }
        public int PointOfEntry { get; set; }
        public int PrimaryStateCode { get; set; }
        public int OfficeContactManager { get; set; }

        public int HddnTransmittalBatchNumber { get; set; }
        public int HddnTransactionTotal { get; set; }
        public int HddnBoolIsMemberFile { get; set; }

        public string Hddnfilepath { get; set; }
        public string Filename { get; set; }
       // public HttpPostedFileBase MyFile { get; set; }
    }
    public class Member
    {
        public List<string> Certifications { get; set; }
        public string DesignatedRealtor { get; set; }
        public List<string> Designations { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public int MemberId { get; set; }
        public string MemberStatusVal { get; set; }
        public string MemberTypeVal { get; set; }
        public int MemberOffficeAreaCode { get; set; }
        public string MemberOfficeBusinessName { get; set; }
        public int MemberOfficeId { get; set; }
        public int MemberOfficePhoneNumber { get; set; }
        public int MemberPrimaryAssociationId { get; set; }
        public int MemberPrimaryStateAssociationId { get; set; }
        public string MemberOfficeStreetAddress { get; set; }
        public object MemberOfficeStreetAttnCareOf { get; set; }
        public string MemberOfficeStreetCity { get; set; }
        public string MemberOfficeStreetState { get; set; }
        public string MemberOfficeStreetZip { get; set; }
        public object MemberOfficeStreetZip6 { get; set; }
        public int TransmittalBatchNumber { get; set; }
        public DateTime InsertedOn { get; set; }
        public List<int> MemberSecondaryAssociationIds { get; set; }
    }

    public class OfficeDeatils
    {
        public int OfficeId { get; set; }
        public DateTime InsertedOn { get; set; }
        public int RecordType { get; set; }
        public int SenderId { get; set; }
        public int TransmittalBatchNumber { get; set; }
        public int TransactionNumber { get; set; }
        public int TransactionTotal { get; set; }
        public int TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string RecordChangeType { get; set; }
        public int AssociationId { get; set; }
        public string OfficeBusinessName { get; set; }
        public string SortSequence { get; set; }
        public string StreetAddress { get; set; }
        public string StreetCity { get; set; }
        public string StreetState { get; set; }
        public string StreetZIP { get; set; }
        public int OfficeAreaCode { get; set; }
        public int OfficePhoneNumber { get; set; }
        public int OfficeContactDR { get; set; }
        public string Status { get; set; }
        public int NMSalespersonCount { get; set; }
        public int PointOfEntry { get; set; }
        public int PrimaryStateCode { get; set; }
        public int OfficeContactManager { get; set; }

    }

    //public class UploadedFile
    //{
    //    public int FileSize { get; set; }
    //    public string Filename { get; set; }
    //    public string ContentType { get; set; }
    //    public byte[] Contents { get; set; }
    //}





    public class RootObject
    {
        public Member member { get; set; }
        public OfficeDeatils officeDeatils { get; set; }
    }
    public class SyncRealtorsDAL : TkmDataAccess.DataProviderBase
    {
        /// <summary>
        /// Detete existing data from DB using TransmittalBatchNumber
        /// </summary>
        ///
        /// <returns></returns>
        public void delete_data(string strfilePath, Boolean boolIsMemberFile)
        {
            int TransmittalBatchNumber;
            using (CsvFileReader reader = new CsvFileReader(strfilePath))
            { 
             CsvRow row = new CsvRow();
                    int i = 0;
                 

                    while (reader.ReadRow(row))
                    { 
                     i++;

                     if (i == 7)
                     {
                         TransmittalBatchNumber = Convert.ToInt32(row[2]);
                          IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
                          try
                          {
                              dbManager.Open();
                              if (boolIsMemberFile)
                              {
                                  dbManager.ExecuteNonQuery(CommandType.Text, " delete llr_csv_file_details where TransmittalBatchNumber=" + TransmittalBatchNumber + "");
                              }
                              else
                              {
                                  dbManager.ExecuteNonQuery(CommandType.Text, " delete office_details where TransmittalBatchNumber=" + TransmittalBatchNumber + "");
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
                          break;
                     }
                    }
            
            
            }
        
        
        }

        public void delete_temp_data(string strfilePath, Boolean boolIsMemberFile)
        {
            int TransmittalBatchNumber;
            using (CsvFileReader reader = new CsvFileReader(strfilePath))
            {
                CsvRow row = new CsvRow();
                int i = 0;


                while (reader.ReadRow(row))
                {
                    i++;

                    if (i == 7)
                    {
                        TransmittalBatchNumber = Convert.ToInt32(row[2]);
                        IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
                        try
                        {
                            dbManager.Open();
                            if (boolIsMemberFile)
                            {
                                dbManager.ExecuteNonQuery(CommandType.Text, " delete temp_llr_csv_file_details where TransmittalBatchNumber=" + TransmittalBatchNumber + "");
                            }
                            //else
                            //{
                            //    dbManager.ExecuteNonQuery(CommandType.Text, " delete office_details where TransmittalBatchNumber=" + TransmittalBatchNumber + "");
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
                        break;
                    }
                }


            }


        }



        /// <summary>
        /// To Import Member Details CSV to Table
        /// </summary>
        /// <param name="objSyncRealtorsModel"></param>
        /// <returns></returns>
        public bool ImportMemberCSVFiles(SyncRealtorsModel objSyncRealtorsModel)
        {
            bool boolresult = true;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(22);
               
                dbManager.AddParameters(0, "@MemberId", objSyncRealtorsModel.MemberId);
                dbManager.AddParameters(1, "@TransmittalBatchNumber", objSyncRealtorsModel.TransmittalBatchNumber);
                dbManager.AddParameters(2, "@TransactionNumber", objSyncRealtorsModel.TransactionNumber);
                //if (objSyncRealtorsModel.TransactionDate != DateTime.MinValue)
                dbManager.AddParameters(3, "@TransactionDate", objSyncRealtorsModel.TransactionDate);
                dbManager.AddParameters(4, "@TransactionTime", objSyncRealtorsModel.TransactionTime);
                dbManager.AddParameters(5, "@FirstName", objSyncRealtorsModel.FirstName);
                dbManager.AddParameters(6, "@LastName", objSyncRealtorsModel.LastName);
                dbManager.AddParameters(7, "@Status", objSyncRealtorsModel.Status);
                dbManager.AddParameters(8, "@MemberType", objSyncRealtorsModel.MemberType);
                dbManager.AddParameters(9, "@PrimaryFieldofBusiness", objSyncRealtorsModel.PrimaryFieldofBusiness);
                dbManager.AddParameters(10, "@OfficeId", objSyncRealtorsModel.OfficeId);
                dbManager.AddParameters(11, "@PrimaryAssociationID", objSyncRealtorsModel.PrimaryAssociationID);
                dbManager.AddParameters(12, "@PrimaryStateAssociationID", objSyncRealtorsModel.PrimaryStateAssociationID);
                dbManager.AddParameters(13, "@MailAddress", objSyncRealtorsModel.MailAddress);
                dbManager.AddParameters(14, "@MailCity", objSyncRealtorsModel.MailCity);
                dbManager.AddParameters(15, "@MailState", objSyncRealtorsModel.MailState);
                dbManager.AddParameters(16, "@MailZIPCode", objSyncRealtorsModel.MailZIPCode);
                dbManager.AddParameters(17, "@MailZIPCode6", objSyncRealtorsModel.MailZIPCode6);
                dbManager.AddParameters(18, "@NRDSStatus", objSyncRealtorsModel.NRDSStatus);
                dbManager.AddParameters(19, "@InsertedOn", objSyncRealtorsModel.InsertedOn);
                dbManager.AddParameters(20, "@RELicenseNo", objSyncRealtorsModel.RELicenseNumber);
                dbManager.AddParameters(21, "@ReturnVal", 0, ParameterDirection.Output);               
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "llr_csv_file_details_Insert_Proc");
                returnVal = (int)dbManager.Parameters[21].Value;
                if (returnVal != 0)
                {
                    boolresult = true;
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


        /// <summary>
        /// To Import Member Details CSV to Temp Table
        /// </summary>
        /// <param name="objSyncRealtorsModel"></param>
        /// <returns></returns>
        public bool ImportMemberCSVFilestoTemp(SyncRealtorsModel objSyncRealtorsModel)
        {
            bool boolresult = true;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(22);

                dbManager.AddParameters(0, "@MemberId", objSyncRealtorsModel.MemberId);
                dbManager.AddParameters(1, "@TransmittalBatchNumber", objSyncRealtorsModel.TransmittalBatchNumber);
                dbManager.AddParameters(2, "@TransactionNumber", objSyncRealtorsModel.TransactionNumber);
                //if (objSyncRealtorsModel.TransactionDate != DateTime.MinValue)
                dbManager.AddParameters(3, "@TransactionDate", objSyncRealtorsModel.TransactionDate);
                dbManager.AddParameters(4, "@TransactionTime", objSyncRealtorsModel.TransactionTime);
                dbManager.AddParameters(5, "@FirstName", objSyncRealtorsModel.FirstName);
                dbManager.AddParameters(6, "@LastName", objSyncRealtorsModel.LastName);
                dbManager.AddParameters(7, "@Status", objSyncRealtorsModel.Status);
                dbManager.AddParameters(8, "@MemberType", objSyncRealtorsModel.MemberType);
                dbManager.AddParameters(9, "@PrimaryFieldofBusiness", objSyncRealtorsModel.PrimaryFieldofBusiness);
                dbManager.AddParameters(10, "@OfficeId", objSyncRealtorsModel.OfficeId);
                dbManager.AddParameters(11, "@PrimaryAssociationID", objSyncRealtorsModel.PrimaryAssociationID);
                dbManager.AddParameters(12, "@PrimaryStateAssociationID", objSyncRealtorsModel.PrimaryStateAssociationID);
                dbManager.AddParameters(13, "@MailAddress", objSyncRealtorsModel.MailAddress);
                dbManager.AddParameters(14, "@MailCity", objSyncRealtorsModel.MailCity);
                dbManager.AddParameters(15, "@MailState", objSyncRealtorsModel.MailState);
                dbManager.AddParameters(16, "@MailZIPCode", objSyncRealtorsModel.MailZIPCode);
                dbManager.AddParameters(17, "@MailZIPCode6", objSyncRealtorsModel.MailZIPCode6);
                dbManager.AddParameters(18, "@NRDSStatus", objSyncRealtorsModel.NRDSStatus);
                dbManager.AddParameters(19, "@InsertedOn", objSyncRealtorsModel.InsertedOn);
                dbManager.AddParameters(20, "@RELicenseNo", objSyncRealtorsModel.RELicenseNumber);
                dbManager.AddParameters(21, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "temp_llr_csv_file_details_Insert_Proc");
                returnVal = (int)dbManager.Parameters[21].Value;
                if (returnVal != 0)
                {
                    boolresult = true;
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




        /// <summary>
        /// to update the NRDSStatus
        /// </summary>
        /// <param name="objSyncRealtorsModel"></param>
        /// <returns></returns>
        public bool updateNRDSStatus(SyncRealtorsModel objSyncRealtorsModel)
        {
            bool boolresult = true;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, "@MemberId", objSyncRealtorsModel.MemberId);
                dbManager.AddParameters(1, "@LastName", objSyncRealtorsModel.LastName);
                dbManager.AddParameters(2, "@NRDSStatus", objSyncRealtorsModel.NRDSStatus);
                dbManager.AddParameters(3, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "llr_csv_file_details_StatusUpdate_Proc");
                returnVal = (int)dbManager.Parameters[3].Value;
                if (returnVal == -1)
                {
                    boolresult = true;
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



        /// <summary>
        /// to update the NRDSStatus in temp table
        /// </summary>
        /// <param name="objSyncRealtorsModel"></param>
        /// <returns></returns>
        public bool updateNRDSStatusTemp(SyncRealtorsModel objSyncRealtorsModel)
        {
            bool boolresult = true;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, "@MemberId", objSyncRealtorsModel.MemberId);
                dbManager.AddParameters(1, "@LastName", objSyncRealtorsModel.LastName);
                dbManager.AddParameters(2, "@NRDSStatus", objSyncRealtorsModel.NRDSStatus);
                dbManager.AddParameters(3, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "temp_llr_csv_file_details_StatusUpdate_Proc");
                returnVal = (int)dbManager.Parameters[3].Value;
                if (returnVal == -1)
                {
                    boolresult = true;
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

        /// <summary>
        /// Add NRDSJson Data 
        /// </summary>
        /// <param name="objMemberModel"></param>
        /// <returns></returns>
        public bool addNRDSJsonData(RootObject objMemberModel)
        {
            bool boolresult = true;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
               
                dbManager.Open();
                dbManager.CreateParameters(21);
                dbManager.AddParameters(0, "@MemberId", objMemberModel.member.MemberId);
                dbManager.AddParameters(1, "@TransmittalBatchNumber", objMemberModel.member.TransmittalBatchNumber);
                dbManager.AddParameters(2, "@InsertedOn", objMemberModel.member.InsertedOn);
                dbManager.AddParameters(3, "@MemberFirstName", objMemberModel.member.MemberFirstName);
                dbManager.AddParameters(4, "@MemberLastName", objMemberModel.member.MemberLastName);
                dbManager.AddParameters(5, "@DesignatedRealtor", objMemberModel.member.DesignatedRealtor);
                dbManager.AddParameters(6, "@MemberStatusVal", objMemberModel.member.MemberStatusVal);
                dbManager.AddParameters(7, "@MemberTypeVal", objMemberModel.member.MemberTypeVal);
                dbManager.AddParameters(8, "@MemberOffficeAreaCode", objMemberModel.member.MemberOffficeAreaCode);
                dbManager.AddParameters(9, "@MemberOfficeBusinessName", objMemberModel.member.MemberOfficeBusinessName);
                dbManager.AddParameters(10, "@MemberOfficeId", objMemberModel.member.MemberOfficeId);
                dbManager.AddParameters(11, "@MemberOfficePhoneNumber", objMemberModel.member.MemberOfficePhoneNumber);
                dbManager.AddParameters(12, "@MemberPrimaryAssociationId", objMemberModel.member.MemberPrimaryAssociationId);
                dbManager.AddParameters(13, "@MemberPrimaryStateAssociationId", objMemberModel.member.MemberPrimaryStateAssociationId);
                dbManager.AddParameters(14, "@MemberOfficeStreetAddress", objMemberModel.member.MemberOfficeStreetAddress);
                dbManager.AddParameters(15, "@MemberOfficeStreetCity", objMemberModel.member.MemberOfficeStreetCity);
                dbManager.AddParameters(16, "@MemberOfficeStreetState", objMemberModel.member.MemberOfficeStreetState);
                dbManager.AddParameters(17, "@MemberOfficeStreetZip", objMemberModel.member.MemberOfficeStreetZip);
                dbManager.AddParameters(18, "@MemberOfficeStreetAttnCareOf", objMemberModel.member.MemberOfficeStreetAttnCareOf);
                dbManager.AddParameters(19, "@MemberOfficeStreetZip6", objMemberModel.member.MemberOfficeStreetZip6);
                dbManager.AddParameters(20, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "nrds_json_details_Insert_Proc");
                returnVal = (int)dbManager.Parameters[20].Value;
                if (returnVal != 0)
                {
                    boolresult = true;
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


        /// <summary>
        /// Add temp NRDSJson Data 
        /// </summary>
        /// <param name="objMemberModel"></param>
        /// <returns></returns>
        public bool addTempNRDSJsonData(RootObject objMemberModel)
        {
            bool boolresult = true;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(21);
                dbManager.AddParameters(0, "@MemberId", objMemberModel.member.MemberId);
                dbManager.AddParameters(1, "@TransmittalBatchNumber", objMemberModel.member.TransmittalBatchNumber);
                dbManager.AddParameters(2, "@InsertedOn", objMemberModel.member.InsertedOn);
                dbManager.AddParameters(3, "@MemberFirstName", objMemberModel.member.MemberFirstName);
                dbManager.AddParameters(4, "@MemberLastName", objMemberModel.member.MemberLastName);
                dbManager.AddParameters(5, "@DesignatedRealtor", objMemberModel.member.DesignatedRealtor);
                dbManager.AddParameters(6, "@MemberStatusVal", objMemberModel.member.MemberStatusVal);
                dbManager.AddParameters(7, "@MemberTypeVal", objMemberModel.member.MemberTypeVal);
                dbManager.AddParameters(8, "@MemberOffficeAreaCode", objMemberModel.member.MemberOffficeAreaCode);
                dbManager.AddParameters(9, "@MemberOfficeBusinessName", objMemberModel.member.MemberOfficeBusinessName);
                dbManager.AddParameters(10, "@MemberOfficeId", objMemberModel.member.MemberOfficeId);
                dbManager.AddParameters(11, "@MemberOfficePhoneNumber", objMemberModel.member.MemberOfficePhoneNumber);
                dbManager.AddParameters(12, "@MemberPrimaryAssociationId", objMemberModel.member.MemberPrimaryAssociationId);
                dbManager.AddParameters(13, "@MemberPrimaryStateAssociationId", objMemberModel.member.MemberPrimaryStateAssociationId);
                dbManager.AddParameters(14, "@MemberOfficeStreetAddress", objMemberModel.member.MemberOfficeStreetAddress);
                dbManager.AddParameters(15, "@MemberOfficeStreetCity", objMemberModel.member.MemberOfficeStreetCity);
                dbManager.AddParameters(16, "@MemberOfficeStreetState", objMemberModel.member.MemberOfficeStreetState);
                dbManager.AddParameters(17, "@MemberOfficeStreetZip", objMemberModel.member.MemberOfficeStreetZip);
                dbManager.AddParameters(18, "@MemberOfficeStreetAttnCareOf", objMemberModel.member.MemberOfficeStreetAttnCareOf);
                dbManager.AddParameters(19, "@MemberOfficeStreetZip6", objMemberModel.member.MemberOfficeStreetZip6);
                dbManager.AddParameters(20, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "temp_nrds_json_details_Insert_Proc");
                returnVal = (int)dbManager.Parameters[20].Value;
                if (returnVal != 0)
                {
                    boolresult = true;
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


        /// <summary>
        /// To Import Office Details CSV to Table
        /// </summary>
        /// <param name="objSyncRealtorsModel"></param>
        /// <returns></returns>
        public bool ImportOfficeCSVFiles(SyncRealtorsModel objSyncRealtorsModel)
        {
            bool boolresult = true;
            int returnVal = 0;
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(26);
                dbManager.AddParameters(0, "@OfficeId", objSyncRealtorsModel.OfficeId);
                dbManager.AddParameters(1, "@InsertedOn", objSyncRealtorsModel.InsertedOn);
                dbManager.AddParameters(2, "@RecordType", objSyncRealtorsModel.RecordType);
                dbManager.AddParameters(3, "@SenderId", objSyncRealtorsModel.SenderId);
                dbManager.AddParameters(4, "@TransmittalBatchNumber", objSyncRealtorsModel.TransmittalBatchNumber);
                dbManager.AddParameters(5, "@TransactionNumber", objSyncRealtorsModel.TransactionNumber);
                dbManager.AddParameters(6, "@TransactionTotal", objSyncRealtorsModel.TransactionTotal);
                dbManager.AddParameters(7, "@TransactionDate", objSyncRealtorsModel.OfficeTransactionDate);
                dbManager.AddParameters(8, "@TransactionTime", objSyncRealtorsModel.TransactionTime);
                dbManager.AddParameters(9, "@RecordChangeType", objSyncRealtorsModel.RecordChangeType);
                dbManager.AddParameters(10, "@AssociationId", objSyncRealtorsModel.AssociationId);
                dbManager.AddParameters(11, "@OfficeBusinessName", objSyncRealtorsModel.OfficeBusinessName);
                dbManager.AddParameters(12, "@SortSequence", objSyncRealtorsModel.SortSequence);
                dbManager.AddParameters(13, "@StreetAddress", objSyncRealtorsModel.StreetAddress);
                dbManager.AddParameters(14, "@StreetCity", objSyncRealtorsModel.StreetCity);
                dbManager.AddParameters(15, "@StreetState", objSyncRealtorsModel.StreetState);
                dbManager.AddParameters(16, "@StreetZIP", objSyncRealtorsModel.StreetZIP);
                dbManager.AddParameters(17, "@OfficeAreaCode", objSyncRealtorsModel.OfficeAreaCode);
                dbManager.AddParameters(18, "@OfficePhoneNumber", objSyncRealtorsModel.OfficePhoneNumber);
                dbManager.AddParameters(19, "@OfficeContactDR", objSyncRealtorsModel.OfficeContactDR);
                dbManager.AddParameters(20, "@Status", objSyncRealtorsModel.Status);
                dbManager.AddParameters(21, "@NMSalespersonCount", objSyncRealtorsModel.NMSalespersonCount);
                dbManager.AddParameters(22, "@PointOfEntry", objSyncRealtorsModel.PointOfEntry);
                dbManager.AddParameters(23, "@PrimaryStateCode", objSyncRealtorsModel.PrimaryStateCode);
                dbManager.AddParameters(24, "@OfficeContactManager", objSyncRealtorsModel.OfficeContactManager);
                dbManager.AddParameters(25, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "office_details_Insert_Proc");
                returnVal = (int)dbManager.Parameters[25].Value;
                if (returnVal == -1)
                {
                    boolresult = true;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transBatchNo"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        public int getDataImportProgress(int transBatchNo, int type)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
           int resultcount=-1;
            try
            {
              
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, "@TransBatchNo", transBatchNo);
                dbManager.AddParameters(1, "@Type", type);
                dbManager.AddParameters(2, "@ReturnVal", 0, ParameterDirection.Output);
                resultcount = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "data_import_progress_Select_Proc");
                resultcount = (int)dbManager.Parameters[2].Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
            return resultcount;
        }
    }
}