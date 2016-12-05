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
    /// <summary>
    /// Author: Geethanjali P Nair
    /// </summary>
    /// 

    public class MemberStatusReportModel
    {

        public int MemberId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string LLRStatus { get; set; }
        public string NRDSStatus { get; set; }

        public List<MemberStatusReportModel> MemberStatusReportModelList { get; set; }
    }


    public class MemberStatusReportDAL : TkmDataAccess.DataProviderBase
    {

        public List<MemberStatusReportModel> getMemberStatusReportList(string constrain)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<MemberStatusReportModel> lstMemberStatusReportModel = new List<MemberStatusReportModel>();
            try
            {

                DataSet dsMemberStatusRpt = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (constrain != "")
                {
                    constrain = " WHERE " + constrain;
                }
                else
                {
                    constrain = null;
                }
                dbManager.AddParameters(0, "@Constrain", constrain); /*WHERE*/
                dsMemberStatusRpt = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[db_owner].member_status_mismmatch_report_Select_Proc");
                if (dsMemberStatusRpt.Tables.Count > 0)
                {
                    if (dsMemberStatusRpt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drloff in dsMemberStatusRpt.Tables[0].Rows)
                        {
                            MemberStatusReportModel memberStatusReportModel = new MemberStatusReportModel();
                            memberStatusReportModel.MemberId = Convert.ToInt32(drloff["MemberId"]);
                            memberStatusReportModel.LastName = Convert.ToString(drloff["LastName"]);
                            memberStatusReportModel.FirstName = Convert.ToString(drloff["FirstName"]);
                            /*'A', 'I', 'T', 'P', 'X', S' is "- Active, Inactive, Terminated, Provisional, and S is suspended*/
                            string strLLRStatus = Convert.ToString(drloff["LLRStatus"]);
                            if (strLLRStatus == "A")
                            {
                                memberStatusReportModel.LLRStatus = "Active";
                            }
                            else if (strLLRStatus == "I")
                            {
                                memberStatusReportModel.LLRStatus = "Inactive";
                            }
                            else if (strLLRStatus == "T")
                            {
                                memberStatusReportModel.LLRStatus = "Terminated";
                            }
                            else if (strLLRStatus == "P")
                            {
                                memberStatusReportModel.LLRStatus = "Provisional";
                            }
                            else if (strLLRStatus == "X")
                            {
                                memberStatusReportModel.LLRStatus = "Lifetime Member";
                            }
                            else if (strLLRStatus == "S")
                            {
                                memberStatusReportModel.LLRStatus = "Suspended";
                            }
                            string strNRDSStatus = Convert.ToString(drloff["NRDSStatus"]);
                            memberStatusReportModel.NRDSStatus = Convert.ToString(drloff["NRDSStatus"]);
                            //if (strNRDSStatus == "ACTIVE")
                            //{
                            //    memberStatusReportModel.NRDSStatus = "Active";
                            //}
                            //else if (strNRDSStatus == "I")
                            //{
                            //    memberStatusReportModel.NRDSStatus = "Inactive";
                            //}
                            //else if (strNRDSStatus == "T")
                            //{
                            //    memberStatusReportModel.NRDSStatus = "Terminated";
                            //}
                            //else if (strNRDSStatus == "P")
                            //{
                            //    memberStatusReportModel.NRDSStatus = "Provisional";
                            //}
                            //else if (strNRDSStatus == "X")
                            //{
                            //    memberStatusReportModel.NRDSStatus = "Lifetime Member";
                            //}
                            //else if (strNRDSStatus == "S")
                            //{
                            //    memberStatusReportModel.NRDSStatus = "Suspended";
                            //}

                            lstMemberStatusReportModel.Add(memberStatusReportModel);
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
            return lstMemberStatusReportModel;
        }
    }
}