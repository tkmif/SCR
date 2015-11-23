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


    public class AgentsModel
    {

        public int MemberId { get; set; }
        public int OfficeId { get; set; }
        public int TransmittalBatchNumber { get; set; }
        public int OfficeContactDR { get; set; }
        public int Filter { get; set; }
        public int BrokerId { get; set; }

        public string MemberType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DesignatedRealtor { get; set; }
        public string OfficeBusinessName { get; set; }
        public string BrokersFirstName { get; set; }
        public string BrokersLastName { get; set; }
        public string strBrokerId { get; set; }
        public string strOfficeId { get; set; }

        public string strRELicenseNumber { get; set; }


        public List<AgentsModel> AgentsModelList { get; set; }
    }
    public class AgentDetailsModel
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
        public string strRELicenseNumber { get; set; }
        public string PrintHtmlContent { get; set; }
        public List<AgentDetailsModel> AgentsDetailsModelList { get; set; }
        public DataTable dt = new DataTable();
    }
    public class AgentsDAL : TkmDataAccess.DataProviderBase
    {

        public AgentDetailsModel getAgentDetails(int agentid, string printHtmlTemplete)
        {
            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            AgentDetailsModel agentsDtailsModel = new AgentDetailsModel();
            try
            {
                DataSet dsAgentsList = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@AgentId", agentid);

                dsAgentsList = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "agents_details_Select_Proc");
                agentsDtailsModel.dt = dsAgentsList.Tables[0];
                if (dsAgentsList.Tables.Count > 0)
                {
                    if (dsAgentsList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drAgents in dsAgentsList.Tables[0].Rows)
                        {
                            AgentDetailsModel agentsModel = new AgentDetailsModel();

                            agentsDtailsModel.MemberId = Convert.ToInt32(drAgents["MemberId"]);
                            agentsDtailsModel.LastName = Convert.ToString(drAgents["Name"]);
                            agentsDtailsModel.FirstName = Convert.ToString(drAgents["Name"]);
                            agentsDtailsModel.AgentType = Convert.ToString(drAgents["MemberType"]);
                            agentsDtailsModel.AgentStatus = Convert.ToString(drAgents["Status"]);
                            agentsDtailsModel.BrokerId = Convert.ToString(drAgents["brokerId"]);
                            agentsDtailsModel.OfficeId = Convert.ToString(drAgents["OfficeId"]);
                            agentsDtailsModel.OfficeName = Convert.ToString(drAgents["OfficeBusinessName"]);
                            agentsDtailsModel.OfficeAddress = Convert.ToString(drAgents["StreetAddress"]);
                            agentsDtailsModel.Email = Convert.ToString(drAgents["Email"]);
                            agentsDtailsModel.City = Convert.ToString(drAgents["StreetCity"]);
                            agentsDtailsModel.State = Convert.ToString(drAgents["StreetState"]);
                            agentsDtailsModel.Zipcode = Convert.ToString(drAgents["StreetZIP"]);
                            agentsDtailsModel.strRELicenseNumber = Convert.ToString(drAgents["RELicenseNo"]);
                            

                            printHtmlTemplete = printHtmlTemplete.Replace("{MemberId}", Convert.ToString(agentsDtailsModel.MemberId));
                            printHtmlTemplete = printHtmlTemplete.Replace("{AgentName}", agentsDtailsModel.FirstName );
                            printHtmlTemplete = printHtmlTemplete.Replace("{AgentType}", agentsDtailsModel.AgentType);
                            printHtmlTemplete = printHtmlTemplete.Replace("{RELicenseNo}", agentsDtailsModel.strRELicenseNumber);
                            printHtmlTemplete = printHtmlTemplete.Replace("{AgentStatus}", agentsDtailsModel.AgentStatus);
                            printHtmlTemplete = printHtmlTemplete.Replace("{OfficeId}", agentsDtailsModel.OfficeId);
                            printHtmlTemplete = printHtmlTemplete.Replace("{BrokerId}", agentsDtailsModel.BrokerId);
                            printHtmlTemplete = printHtmlTemplete.Replace("{BussinessName}", agentsDtailsModel.OfficeName);
                            printHtmlTemplete = printHtmlTemplete.Replace("{OfficeAddress}", agentsDtailsModel.OfficeAddress);
                            printHtmlTemplete = printHtmlTemplete.Replace("{Email}", Convert.ToString(agentsDtailsModel.Email));
                            printHtmlTemplete = printHtmlTemplete.Replace("{City}", Convert.ToString(agentsDtailsModel.City));
                            printHtmlTemplete = printHtmlTemplete.Replace("{State}", Convert.ToString(agentsDtailsModel.State));
                            printHtmlTemplete = printHtmlTemplete.Replace("{Zipcode}", Convert.ToString(agentsDtailsModel.Zipcode));
                            agentsDtailsModel.PrintHtmlContent = Convert.ToString(printHtmlTemplete);
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
            return agentsDtailsModel;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constrain"></param>
        /// <returns></returns>
        public List<AgentsModel> getAgentsList(int brokerId, int officeId, string condition,string order)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());

            List<AgentsModel> lstAgentsModel = new List<AgentsModel>();
            try
            {

                DataSet dsAgentsList = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@BrokerId", brokerId);
                dbManager.AddParameters(1, "@OfficeId", officeId);
                dbManager.AddParameters(2, "@condition", condition);
                dbManager.AddParameters(3, "@order", order);
                
                dsAgentsList = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "agents_lists_Select_Proc");
                if (dsAgentsList.Tables.Count > 0)
                {
                    if (dsAgentsList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drAgents in dsAgentsList.Tables[0].Rows)
                        {
                            AgentsModel agentsModel = new AgentsModel();
                            agentsModel.MemberId = Convert.ToInt32(drAgents["MemberId"]);
                            agentsModel.LastName = Convert.ToString(drAgents["LastName"]);
                            agentsModel.FirstName = Convert.ToString(drAgents["FirstName"]);
                            agentsModel.OfficeId = Convert.ToInt32(drAgents["OfficeId"]);
                            agentsModel.MemberType = Convert.ToString(drAgents["MemberType"]);
                            agentsModel.OfficeContactDR = Convert.ToInt32(drAgents["OfficeContactDR"]);
                            agentsModel.OfficeBusinessName = Convert.ToString(drAgents["OfficeBusinessName"]);
                            if (drAgents["BrokerId"] != null)
                            {
                                agentsModel.BrokerId = Convert.ToInt32(drAgents["BrokerId"]);
                                agentsModel.BrokersFirstName = Convert.ToString(drAgents["BrokerFirstName"]);
                                agentsModel.BrokersLastName = Convert.ToString(drAgents["BrokerLastName"]);
                            }
                            lstAgentsModel.Add(agentsModel);
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
            return lstAgentsModel;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public List<AgentsModel> getOfficeList(string term,int type)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            List<AgentsModel> lstAgentsModel = new List<AgentsModel>();
            try
            {

                DataSet dsOffice = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@SearchTerm", term);
                dbManager.AddParameters(1, "@type", type);
                dsOffice = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "office_details_Select_Proc");
                if (dsOffice.Tables.Count > 0)
                {
                    if (dsOffice.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drOfiice in dsOffice.Tables[0].Rows)
                        {
                            AgentsModel agentsModel = new AgentsModel();
                            agentsModel.OfficeId = Convert.ToInt32(drOfiice["OfficeId"]);
                            agentsModel.OfficeBusinessName = Convert.ToString(drOfiice["OfficeBusinessName"]);
                            lstAgentsModel.Add(agentsModel);
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
            return lstAgentsModel;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public List<AgentsModel> getBrokerList(string term,int type)
        {

            IDBManager dbManager = new DBManager(base.GetProvider(), ConfigurationManager.AppSettings["DbConnection"].ToString());
            List<AgentsModel> lstAgentsModel = new List<AgentsModel>();
            try
            {

                DataSet dsOffice = new DataSet();
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@SearchTerm", term);
                dbManager.AddParameters(1, "@type", type);
                dsOffice = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "broker_Select_Proc");
                if (dsOffice.Tables.Count > 0)
                {
                    if (dsOffice.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drOfiice in dsOffice.Tables[0].Rows)
                        {
                            AgentsModel agentsModel = new AgentsModel();
                            agentsModel.BrokerId = Convert.ToInt32(drOfiice["MemberId"]);
                            agentsModel.BrokersFirstName = Convert.ToString(drOfiice["FirstName"]);
                            agentsModel.BrokersLastName = Convert.ToString(drOfiice["LastName"]);
                            lstAgentsModel.Add(agentsModel);
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
            return lstAgentsModel;
        }
    }
}