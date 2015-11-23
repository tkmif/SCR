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
    public class MemberExpiryReportModel
    {
        public int MemberId { get; set; }      
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MemberType { get; set; }
        public string Status { get; set; }
        public string hdnStatus { get; set; }

        public List<MemberExpiryReportModel> MemberExpiryReportModelList { get; set; }
    }



    public class MemberExpiryReportDAL : TkmDataAccess.DataProviderBase
    {

        /// <summary>
        /// To generate Member Expiry Report
        /// </summary>
        /// <param name="constrain"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<MemberExpiryReportModel> getMemberExpiryReportList(string constrain,int memberId)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<MemberExpiryReportModel> lstMemberExpiryReportModel = new List<MemberExpiryReportModel>();
            try
            {

                DataSet dsMemberExpiryRpt = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (constrain != "")
                {
                 constrain =   " WHERE " + constrain;
                }                
                dbManager.AddParameters(0, "@Constrain", constrain);
                dbManager.AddParameters(1, "@MemberId", memberId);
                dsMemberExpiryRpt = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "member_expiry_report_Select_Proc");
                if (dsMemberExpiryRpt.Tables.Count > 0)
                {
                    if (dsMemberExpiryRpt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drloff in dsMemberExpiryRpt.Tables[0].Rows)
                        {
                            MemberExpiryReportModel memberExpiryReportModel = new MemberExpiryReportModel();
                            memberExpiryReportModel.MemberId = Convert.ToInt32(drloff["MemberId"]);
                            memberExpiryReportModel.MemberType = Convert.ToString(drloff["MemberType"]);
                            memberExpiryReportModel.LastName = Convert.ToString(drloff["LastName"]);
                            memberExpiryReportModel.FirstName = Convert.ToString(drloff["FirstName"]);
                            /*'A', 'I', 'T', 'P', 'X', S' is "- Active, Inactive, Terminated, Provisional, and S is suspended*/
                            string strStatus=Convert.ToString(drloff["Status"]);
                            if (strStatus == "A")
                            {
                                memberExpiryReportModel.Status = "Active";
                            }
                            else if (strStatus == "I")
                            {
                                memberExpiryReportModel.Status = "Inactive";
                            }
                            else if (strStatus == "T")
                            {
                                memberExpiryReportModel.Status = "Terminated";
                            }
                            else if (strStatus == "P")
                            {
                                memberExpiryReportModel.Status = "Provisional";
                            }
                            else if (strStatus == "X")
                            {
                                memberExpiryReportModel.Status = "Lifetime Member";
                            }
                            else if (strStatus == "S")
                            {
                                memberExpiryReportModel.Status = "Suspended";
                            }
                            
                            lstMemberExpiryReportModel.Add(memberExpiryReportModel);
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
            return lstMemberExpiryReportModel;
        }
    }
}