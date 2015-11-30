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
    public class BrokerModel
    {
        public int MemberId { get; set; }
        public int OfficeId { get; set; }


        public string MemberType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Status { get; set; }

        public string hdnStatus { get; set; }



        public List<BrokerModel> BrokerModelListConstain { get; set; }

        public List<BrokerModel> BrokerModelList { get; set; }
    }

    public class BrokerDetailsModel
    {
        public int MemberId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AgentType { get; set; }
        public string AgentStatus { get; set; }
        public string BrokerId { get; set; }
        public string OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string RELicenseNumber { get; set; }

        public string PrintHtmlContent { get; set; }
        public string PrintHtmlContentAgentList { get; set; }
        public List<BrokerDetailsModel> BrokerDetailsModelList { get; set; }
        public List<AgentsModel> AgentsModelList { get; set; }
    }

    public class BrokerDAL : TkmDataAccess.DataProviderBase
    {

        public List<BrokerModel> getBrokerList(int AssociationId)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<BrokerModel> lstBrokerModel = new List<BrokerModel>();
            try
            {

                DataSet dsBrokerList = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@PrimaryAssociatioId", AssociationId);
                dsBrokerList = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Broker__Select_Proc");
                if (dsBrokerList.Tables.Count > 0)
                {
                    if (dsBrokerList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drAgents in dsBrokerList.Tables[0].Rows)
                        {
                            BrokerModel agentsModel = new BrokerModel();
                            agentsModel.MemberId = Convert.ToInt32(drAgents["MemberId"]);
                            agentsModel.LastName = Convert.ToString(drAgents["LastName"]);
                            agentsModel.FirstName = Convert.ToString(drAgents["FirstName"]);
                            agentsModel.OfficeId = Convert.ToInt32(drAgents["OfficeId"]);
                            agentsModel.MemberType = Convert.ToString(drAgents["MemberType"]);
                            string strStatus = Convert.ToString(drAgents["Status"]);
                            if (strStatus == "A")
                            {
                                agentsModel.Status = "Active";
                            }
                            lstBrokerModel.Add(agentsModel);
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
            return lstBrokerModel;
        }

        public List<BrokerModel> getBrokerListConstrain(string constrain)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<BrokerModel> lstBrokerModel = new List<BrokerModel>();
            try
            {

                DataSet dsBrokerList = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@Constrain", " AND " + constrain);

                dsBrokerList = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Active__Broker__Select_Proc");
                if (dsBrokerList.Tables.Count > 0)
                {
                    if (dsBrokerList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drAgents in dsBrokerList.Tables[0].Rows)
                        {
                            BrokerModel agentsModel = new BrokerModel();
                            agentsModel.MemberId = Convert.ToInt32(drAgents["MemberId"]);
                            agentsModel.LastName = Convert.ToString(drAgents["LastName"]);
                            agentsModel.FirstName = Convert.ToString(drAgents["FirstName"]);
                            agentsModel.OfficeId = Convert.ToInt32(drAgents["OfficeId"]);
                            agentsModel.MemberType = Convert.ToString(drAgents["MemberType"]);
                            string strStatus = Convert.ToString(drAgents["Status"]);
                            if (strStatus == "A")
                            {
                                agentsModel.Status = "Active";
                            }
                            else if (strStatus == "I")
                            {
                                agentsModel.Status = "Inactive";
                            }
                            else if (strStatus == "T")
                            {
                                agentsModel.Status = "Terminated";
                            }
                            else if (strStatus == "P")
                            {
                                agentsModel.Status = "Provisional";
                            }
                            else if (strStatus == "X")
                            {
                                agentsModel.Status = "Lifetime Member";
                            }
                            else if (strStatus == "S")
                            {
                                agentsModel.Status = "Suspended";
                            }
                            lstBrokerModel.Add(agentsModel);
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
            return lstBrokerModel;
        }
        //getBrokerWithDelinquentAgents
        public List<BrokerModel> getBrokerWithDelinquentAgents(string constrain)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<BrokerModel> lstBrokerModel = new List<BrokerModel>();
            try
            {

                DataSet dsBrokerList = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@Constrain", " AND " + constrain);

                dsBrokerList = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Active_Brokers_with_Delinquent_Agents");
                if (dsBrokerList.Tables.Count > 0)
                {
                    if (dsBrokerList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drAgents in dsBrokerList.Tables[0].Rows)
                        {
                            BrokerModel agentsModel = new BrokerModel();
                            agentsModel.MemberId = Convert.ToInt32(drAgents["MemberId"]);
                            agentsModel.LastName = Convert.ToString(drAgents["LastName"]);
                            agentsModel.FirstName = Convert.ToString(drAgents["FirstName"]);
                            agentsModel.OfficeId = Convert.ToInt32(drAgents["OfficeId"]);
                            agentsModel.MemberType = Convert.ToString(drAgents["MemberType"]);
                            string strStatus = Convert.ToString(drAgents["Status"]);
                            if (strStatus == "A")
                            {
                                agentsModel.Status = "Active";
                            }
                            else if (strStatus == "I")
                            {
                                agentsModel.Status = "Inactive";
                            }
                            else if (strStatus == "T")
                            {
                                agentsModel.Status = "Terminated";
                            }
                            else if (strStatus == "P")
                            {
                                agentsModel.Status = "Provisional";
                            }
                            else if (strStatus == "X")
                            {
                                agentsModel.Status = "Lifetime Member";
                            }
                            else if (strStatus == "S")
                            {
                                agentsModel.Status = "Suspended";
                            }
                            lstBrokerModel.Add(agentsModel);
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
            return lstBrokerModel;
        }
        public BrokerDetailsModel getBrokerDetails(int agentid, string printHtmlTemplete)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            BrokerDetailsModel brokerDetailsModel = new BrokerDetailsModel();
            try
            {

                DataSet dsBrokerdetails = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@AgentId", agentid);

                dsBrokerdetails = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "agents_details_Select_Proc");
                if (dsBrokerdetails.Tables.Count > 0)
                {
                    if (dsBrokerdetails.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drAgents in dsBrokerdetails.Tables[0].Rows)
                        {
                            AgentDetailsModel agentsModel = new AgentDetailsModel();

                            brokerDetailsModel.MemberId = Convert.ToInt32(drAgents["MemberId"]);
                            brokerDetailsModel.LastName = Convert.ToString(drAgents["Name"]);
                            brokerDetailsModel.FirstName = Convert.ToString(drAgents["Name"]);
                            brokerDetailsModel.AgentType = Convert.ToString(drAgents["MemberType"]);
                            brokerDetailsModel.AgentStatus = Convert.ToString(drAgents["Status"]);
                            brokerDetailsModel.BrokerId = Convert.ToString(drAgents["brokerId"]);
                            brokerDetailsModel.OfficeId = Convert.ToString(drAgents["OfficeId"]);
                            brokerDetailsModel.OfficeName = Convert.ToString(drAgents["OfficeBusinessName"]);
                            brokerDetailsModel.OfficeAddress = Convert.ToString(drAgents["StreetAddress"]);
                            brokerDetailsModel.Email = Convert.ToString(drAgents["Email"]);
                            brokerDetailsModel.City = Convert.ToString(drAgents["StreetCity"]);
                            brokerDetailsModel.State = Convert.ToString(drAgents["StreetState"]);
                            brokerDetailsModel.Zipcode = Convert.ToString(drAgents["StreetZIP"]);
                            brokerDetailsModel.RELicenseNumber = Convert.ToString(drAgents["RELicenseNo"]);

                            printHtmlTemplete = printHtmlTemplete.Replace("{MemberId}", Convert.ToString(brokerDetailsModel.MemberId));
                            printHtmlTemplete = printHtmlTemplete.Replace("{AgentName}", brokerDetailsModel.FirstName);
                            printHtmlTemplete = printHtmlTemplete.Replace("{AgentType}", brokerDetailsModel.AgentType);
                            printHtmlTemplete = printHtmlTemplete.Replace("{RELicenseNumber}", brokerDetailsModel.RELicenseNumber);
                            printHtmlTemplete = printHtmlTemplete.Replace("{AgentStatus}", brokerDetailsModel.AgentStatus);
                            printHtmlTemplete = printHtmlTemplete.Replace("{OfficeId}", brokerDetailsModel.OfficeId);
                            // printHtmlTemplete = printHtmlTemplete.Replace("{BrokerId}", brokerDetailsModel.BrokerId);
                            printHtmlTemplete = printHtmlTemplete.Replace("{BussinessName}", brokerDetailsModel.OfficeName);
                            printHtmlTemplete = printHtmlTemplete.Replace("{OfficeAddress}", brokerDetailsModel.OfficeAddress);
                            printHtmlTemplete = printHtmlTemplete.Replace("{Email}", Convert.ToString(brokerDetailsModel.Email));
                            printHtmlTemplete = printHtmlTemplete.Replace("{City}", Convert.ToString(brokerDetailsModel.City));
                            printHtmlTemplete = printHtmlTemplete.Replace("{State}", Convert.ToString(brokerDetailsModel.State));
                            printHtmlTemplete = printHtmlTemplete.Replace("{Zipcode}", Convert.ToString(brokerDetailsModel.Zipcode));
                            brokerDetailsModel.PrintHtmlContent = Convert.ToString(printHtmlTemplete);
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
            return brokerDetailsModel;

        }

    }
}