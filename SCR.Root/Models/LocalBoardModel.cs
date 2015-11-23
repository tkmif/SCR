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
    public class LocalBoardModel
    {
        public int? AssocID { get; set; }
        public string AssociationName { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }

        public bool Active { get; set; }
        public bool InActive { get; set; }

        public string StatusEdit { get; set; }

        public List<LocalBoardModel> LocalBoardModelList { get; set; }
    }
    public class LocalBoardDAL : TkmDataAccess.DataProviderBase
    {
        /// <summary>
        /// To Add Local Board
        /// </summary>
        /// <param name="localBoardModel"></param>
        /// <returns></returns>
        public int addLocalBoard(LocalBoardModel localBoardModel)
        {
            int returnVal = 0;
            LocalBoardDAL objLocalBoardDAL = new LocalBoardDAL();
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                if (Convert.ToString(localBoardModel.Active) == "True")
                {
                    localBoardModel.Status = "1";
                }
                else 
                {
                    localBoardModel.Status = "0";
                }
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, "@AssocID", localBoardModel.AssocID);
                dbManager.AddParameters(1, "@AssociationName", localBoardModel.AssociationName);
                dbManager.AddParameters(2, "@Status",localBoardModel.Status);
                dbManager.AddParameters(3, "@City", localBoardModel.City);
                dbManager.AddParameters(4, "@State", localBoardModel.State);
                dbManager.AddParameters(5, "@Phone", localBoardModel.Phone);
                dbManager.AddParameters(6, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "local_board_InsertUpdate_Proc");
                returnVal = (int)dbManager.Parameters[6].Value;
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
        /// Delete Local Board
        /// </summary>
        /// <param name="localBoardid"></param>
        /// <returns></returns>
        public int deleteLocalBoard(int localBoardid)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            int retval = -1;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@AssecId", localBoardid);
                dbManager.AddParameters(1, "@ReturnVal", 0, ParameterDirection.Output);
                retval = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "local_boards_Delete_Proc");

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
        /// Get all Local Board details
        /// </summary>
        /// <returns></returns>
        public List<LocalBoardModel> getAllLocalBoardList()
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<LocalBoardModel> lstLocalBoardModel = new List<LocalBoardModel>();
            try
            {

                DataSet dsLocalBoard = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@AssocID", 0);
                dbManager.AddParameters(1, "@Constrain", null);
                dsLocalBoard = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "local_Board_Select_Proc");
                if (dsLocalBoard.Tables.Count > 0)
                {
                    if (dsLocalBoard.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drloff in dsLocalBoard.Tables[0].Rows)
                        {
                            LocalBoardModel localBoardModel = new LocalBoardModel();
                            localBoardModel.AssocID = Convert.ToInt32(drloff["AssocID"]);
                            localBoardModel.AssociationName = Convert.ToString(drloff["AssociationName"]);
                            localBoardModel.Status = Convert.ToString(drloff["Status"]);
                            if (Convert.ToString(drloff["Status"]) == "True")
                            {
                                localBoardModel.Status = "Active";
                            }
                            if (Convert.ToString(drloff["Status"]) == "False")
                            {
                                localBoardModel.Status = "InActive";
                            }
                            localBoardModel.City = Convert.ToString(drloff["City"]);
                            localBoardModel.State = Convert.ToString(drloff["State"]);
                            localBoardModel.Phone = Convert.ToString(drloff["Phone"]);
                            lstLocalBoardModel.Add(localBoardModel);
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
            return lstLocalBoardModel;
        }
        /// <summary>
        /// Get Local Board For Edit
        /// </summary>
        /// <param name="localBoardId"></param>
        /// <returns></returns>
        public LocalBoardModel getLocalBoardForEdit(int localBoardId)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            LocalBoardModel localBoardModel = new LocalBoardModel();

            try
            {
                DataSet dsLocalBoard = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@AssocId", localBoardId);
                dbManager.AddParameters(1, "@Constrain", null);
                dsLocalBoard = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "local_board_Select_Proc");
                if (dsLocalBoard.Tables.Count > 0)
                {
                    if (dsLocalBoard.Tables[0].Rows.Count > 0)
                    {
                        localBoardModel.AssocID = Convert.ToInt32(dsLocalBoard.Tables[0].Rows[0]["AssocID"]);
                        localBoardModel.AssociationName = Convert.ToString(dsLocalBoard.Tables[0].Rows[0]["AssociationName"]);
                        //localBoardModel.Status = Convert.ToString(dsLocalBoard.Tables[0].Rows[0]["Status"]);

                        if (Convert.ToString(dsLocalBoard.Tables[0].Rows[0]["Status"]) == "True")
                        {
                            localBoardModel.Active = true;
                            localBoardModel.InActive = false;
                        }
                        if (Convert.ToString(dsLocalBoard.Tables[0].Rows[0]["Status"]) == "False")
                        {
                            localBoardModel.InActive = true;
                            localBoardModel.Active = false;
                        }
                        localBoardModel.City = Convert.ToString(dsLocalBoard.Tables[0].Rows[0]["City"]);
                        localBoardModel.State = Convert.ToString(dsLocalBoard.Tables[0].Rows[0]["State"]);
                        localBoardModel.Phone = Convert.ToString(dsLocalBoard.Tables[0].Rows[0]["Phone"]);
                        localBoardModel.StatusEdit = "edit";
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
            return localBoardModel;
        }
        /// <summary>
        /// To Add Local Board
        /// </summary>
        /// <param name="localBoardModel"></param>
        /// <returns></returns>
        public int updateLocalBoard(LocalBoardModel localBoardModel)
        {
            int returnVal = 0;
            LocalBoardDAL objLocalBoardDAL = new LocalBoardDAL();
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            try
            {
                if (Convert.ToString(localBoardModel.Active) == "True")
                {
                    localBoardModel.Status = "1";
                }
                else
                {
                    localBoardModel.Status = "0";
                }
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, "@AssocID", localBoardModel.AssocID);
                dbManager.AddParameters(1, "@AssociationName", localBoardModel.AssociationName);
                dbManager.AddParameters(2, "@Status", localBoardModel.Status);
                dbManager.AddParameters(3, "@City", localBoardModel.City);
                dbManager.AddParameters(4, "@State", localBoardModel.State);
                dbManager.AddParameters(5, "@Phone", localBoardModel.Phone);
                dbManager.AddParameters(6, "@ReturnVal", 0, ParameterDirection.Output);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "local_board_Update_Proc");
                returnVal = (int)dbManager.Parameters[6].Value;
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
    }
}