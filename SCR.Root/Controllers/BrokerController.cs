using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCR.Root.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using SCR.Root.App_Code;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCR.Root.Controllers
{
    public class BrokerController : Controller
    {
        //
        // GET: /Broker/

        public ActionResult BrokerList()
        {
            UserSession userSession = new UserSession();
            BrokerModel BrokerDetailModel = new BrokerModel();
            BrokerDAL BrokerDAL = new BrokerDAL();
            string constrian = string.Empty;

            string filter1 = string.Empty;
            try
            {
                if (userSession.Exists)
                {


                    if (Session["filter1"] != null)
                    {

                        filter1 = Session["filter1"].ToString();
                    }
                    if (filter1 != null && filter1 != "")
                    {
                        ViewData["ChkStatus"] = filter1;
                        BrokerDetailModel.hdnStatus = filter1;
                        if (filter1 == "Delinquent")
                        {
                            constrian = getCondition(filter1);
                            BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerWithDelinquentAgents(constrian);
                            TempData["BrokerList"] = BrokerDetailModel.BrokerModelList;
                            ViewData["ChkStatus"] = filter1;
                        }
                        else
                        {
                            constrian = getCondition(filter1);
                            BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerListConstrain(constrian);
                            TempData["BrokerList"] = BrokerDetailModel.BrokerModelListConstain;
                        }
                        return View(BrokerDetailModel);
                    }
                    //else if (filter1 == "")
                    //{
                    //    ViewData["ChkStatus"] = filter1;
                    //    BrokerDetailModel.hdnStatus = filter1;
                    //    constrian = getCondition(filter1);
                    //    BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerListConstrain(constrian);
                    //    TempData["BrokerListConstrain"] = BrokerDetailModel.BrokerModelListConstain;
                    //    return View(BrokerDetailModel);
                    //}
                    else
                    {
                        var uSession = userSession.GetUser;

                        ViewData["ChkStatus"] = filter1;
                        BrokerDetailModel.hdnStatus = filter1;
                        BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerList(uSession.AssocID);
                        TempData["BrokerList"] = BrokerDetailModel.BrokerModelList;
                        return View(BrokerDetailModel);

                    }

                    Session["filter1"] = null;
                    Session.Remove("filter1");
                }
                else
                {
                    TempData["expires"] = "true";
                    return RedirectToAction("Users", "Login");
                }
            }
            catch (Exception ex)
            {
                BrokerDAL = null;
                GC.Collect();
            }

            return View(BrokerDetailModel);

        }

        public ActionResult BrokerDetails(bool filterOption = false)
        {
            UserSession userSession = new UserSession();
            BrokerDetailsModel brokerDetailModel = new BrokerDetailsModel();
            BrokerDAL BrokerDAL = new BrokerDAL();
            AgentsDAL agentsDAL = new AgentsDAL();
            int broker_Id = 0;
            

            string filter1 = string.Empty;
            if (userSession.Exists)
            {

                try
                {
                    if (Request.QueryString["MemberId"] != null)
                    {
                        broker_Id = Convert.ToInt32(Request.QueryString["MemberId"].ToString());
                        
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/HTMLTemplates/_PrintBrokerDetails.html")))
                        {
                            brokerDetailModel = BrokerDAL.getBrokerDetails(broker_Id, reader.ReadToEnd());
                            TempData["BrokerList"] = brokerDetailModel.AgentsModelList;
                        }
                        string LLR="";
                        if (Session["LLR"] != null ) { LLR = Session["LLR"].ToString(); }
                        string NRDS="";
                        if (Session["NRDS"] != null ) { NRDS = Session["NRDS"].ToString(); }
                        string condition = "";
                        if (filterOption == true)
                        {
                            if (LLR != "" && LLR != "Select") { condition += " and L.[Status]='" + LLR + "'"; }
                            if (NRDS != "" && NRDS != "Select") { condition += " and NRDS.[MemberStatusVal]='" + NRDS + "'"; }
                            ViewBag.LLR = LLR;
                            ViewBag.NRDS = NRDS;
                        }
                        brokerDetailModel.AgentsModelList = agentsDAL.getAgentsList(broker_Id, 0, "and [OfficeContactDR]= " + broker_Id + condition, "");
                        //List<AgentsModel> AllAgents = agentsDAL.getAgentsList(broker_Id, 0, "and [OfficeContactDR]= " + broker_Id + "", "");
                        //brokerDetailModel.AgentsModelList = AllAgents.Where(c => c.LLRStatus == "A" && c.NRDSStatus == "A").ToList<AgentsModel>();
                        //filterOption = "Active";
                        //if (Session["filter1"] != null)
                        //{
                        //    filter1 = Session["filter1"].ToString();
                            
                        //}
                        //if (filter1 != null && filter1 != "")
                        //{
                        //    if (filter1 == "Active")
                        //    {
                        //        brokerDetailModel.AgentsModelList = AllAgents.Where(c => c.LLRStatus == "A" && c.NRDSStatus == "A").ToList<AgentsModel>();
                        //        filterOption = "Active";
                        //    }
                        //    else
                        //    {
                        //        brokerDetailModel.AgentsModelList = AllAgents.Where(c => (c.LLRStatus == "I" || c.LLRStatus == "T") && (c.NRDSStatus == "I" || c.NRDSStatus == "T")).ToList<AgentsModel>();
                        //        filterOption = "InActive";
                        //    }
                        //}
                        if (Request.QueryString["Status"] == "Delinquent") { filter1 = Request.QueryString["Status"].ToString(); filterOption = false; brokerDetailModel.AgentsModelList = brokerDetailModel.AgentsModelList.Where(c => (c.LLRStatus == "A" ) && (c.NRDSStatus == "I" )).ToList<AgentsModel>(); }
                        ViewBag.Filter = filterOption;
                        ViewBag.Delinq = filter1;
                        if (brokerDetailModel.AgentsModelList.Count > 0)
                        {

                            foreach (AgentsModel agentsModel in brokerDetailModel.AgentsModelList)
                            {

                                /*'A', 'I', 'T', 'P', 'X', S' is "- Active, Inactive, Terminated, Provisional, and S is suspended*/
                                string strLLRStatus = Convert.ToString(agentsModel.LLRStatus);
                                if (strLLRStatus == "A")
                                {
                                    agentsModel.LLRStatus = "Active";
                                }
                                else if (strLLRStatus == "I")
                                {
                                    agentsModel.LLRStatus = "Inactive";
                                }
                                else if (strLLRStatus == "T")
                                {
                                    agentsModel.LLRStatus = "Terminated";
                                }
                                else if (strLLRStatus == "P")
                                {
                                    agentsModel.LLRStatus = "Provisional";
                                }
                                else if (strLLRStatus == "X")
                                {
                                    agentsModel.LLRStatus = "Lifetime Member";
                                }
                                else if (strLLRStatus == "S")
                                {
                                    agentsModel.LLRStatus = "Suspended";
                                }
                                string strNRDSStatus = Convert.ToString(agentsModel.NRDSStatus);
                                if (strNRDSStatus == "A")
                                {
                                    agentsModel.NRDSStatus = "Active";
                                }
                                else if (strNRDSStatus == "I")
                                {
                                    agentsModel.NRDSStatus = "Inactive";
                                }
                                else if (strNRDSStatus == "T")
                                {
                                    agentsModel.NRDSStatus = "Terminated";
                                }
                                else if (strNRDSStatus == "P")
                                {
                                    agentsModel.NRDSStatus = "Provisional";
                                }
                                else if (strNRDSStatus == "X")
                                {
                                    agentsModel.NRDSStatus = "Lifetime Member";
                                }
                                else if (strNRDSStatus == "S")
                                {
                                    agentsModel.NRDSStatus = "Suspended";
                                }


                            }

                        }


                        StringBuilder sbPrintHtmlContentAgentsList = new StringBuilder();
                        if (brokerDetailModel.AgentsModelList.Count > 0)
                        {
                            sbPrintHtmlContentAgentsList.Append("<tr> <td colspan='2'><h4>Agents's Details</h4></td></tr>");

                            sbPrintHtmlContentAgentsList.Append("<tr> <td colspan='2'><table width=100%><tr class='evenrow'><td><b>Member Id</b></td><td><b>First Name</b></td><td><b>Last Name</b></td><td><b>Office Id</b></td><td><b>Member Type</b></td><td><b>LLR Status</b></td><td><b>NRDS Status</b></td></tr>");

                            bool isOdd = true;
                            foreach (AgentsModel agentsModel in brokerDetailModel.AgentsModelList)
                            {
                                if (isOdd)
                                {
                                    sbPrintHtmlContentAgentsList.Append("<tr class='oddrow'>");
                                    isOdd = false;
                                }
                                else
                                {
                                    sbPrintHtmlContentAgentsList.Append("<tr class='evenrow'>");
                                    isOdd = true;
                                }
                                sbPrintHtmlContentAgentsList.Append("<td>" + agentsModel.MemberId + "</td><td>" + agentsModel.FirstName + "</td><td>" + agentsModel.LastName + "</td><td>" + agentsModel.OfficeId + "</td><td>" + agentsModel.MemberType + "</td><td>" + agentsModel.LLRStatus + "</td><td>" + agentsModel.NRDSStatus + "</td></tr>");
                            }
                            sbPrintHtmlContentAgentsList.Append("</table></td></tr>");
                        }
                        //brokerDetailModel.PrintHtmlContent = brokerDetailModel.PrintHtmlContent.Replace("{AgentDetails}", sbPrintHtmlContentAgentsList.ToString());
                        brokerDetailModel.PrintHtmlContentAgentList = sbPrintHtmlContentAgentsList.ToString();
                        return View(brokerDetailModel);
                    }

                }
                catch (Exception ex)
                {
                    BrokerDAL = null;
                    GC.Collect();
                }
            }
            else
            {
                TempData["expires"] = "true";
                return RedirectToAction("Users", "Login");
            }

            return View(brokerDetailModel);
        }
        //AgentsFilter
        //public ActionResult AgentsFilter(int BrokerID)
        //{
        //    string value1 = "";
        //    if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["value1"])))
        //    {
        //        if (Convert.ToString(Request.QueryString["value1"]) == "")
        //        {
        //            value1 = null;
        //        }
        //        else
        //        {
        //            value1 = Convert.ToString(Convert.ToString(Request.QueryString["value1"]));
        //        }
        //    }

        //    UserSession userSession = new UserSession();
        //    BrokerDetailsModel brokerDetailModel = new BrokerDetailsModel();
        //    BrokerDAL BrokerDAL = new BrokerDAL();
        //    AgentsDAL agentsDAL = new AgentsDAL();
        //    int broker_Id = 0;
        //    string filter1 = string.Empty;
        //    if (userSession.Exists)
        //    {

        //        try
        //        {
        //            if (BrokerID >0)
        //            {
        //                broker_Id = BrokerID;

        //                using (StreamReader reader = new StreamReader(Server.MapPath("~/HTMLTemplates/_PrintBrokerDetails.html")))
        //                {
        //                    brokerDetailModel = BrokerDAL.getBrokerDetails(broker_Id, reader.ReadToEnd());
        //                    TempData["BrokerList"] = brokerDetailModel.AgentsModelList;
        //                }

        //                brokerDetailModel.AgentsModelList = agentsDAL.getAgentsList(broker_Id, 0, "and [OfficeContactDR]= " + broker_Id + "", "");
        //                if (value1 =="Active")
        //                brokerDetailModel.AgentsModelList = brokerDetailModel.AgentsModelList.Where(c=>c.LLRStatus=="A"&&c.NRDSStatus=="A").ToList<AgentsModel>();
        //                else
        //                    brokerDetailModel.AgentsModelList = brokerDetailModel.AgentsModelList.Where(c => c.LLRStatus == "I" && c.NRDSStatus == "I").ToList<AgentsModel>();
        //                if (brokerDetailModel.AgentsModelList.Count > 0)
        //                {

        //                    foreach (AgentsModel agentsModel in brokerDetailModel.AgentsModelList)
        //                    {
        //                        //MemberStatusReportModel memberStatusReportModel = new MemberStatusReportModel();
        //                        //memberStatusReportModel.MemberId = Convert.ToInt32(drloff["MemberId"]);
        //                        //memberStatusReportModel.LastName = Convert.ToString(drloff["LastName"]);
        //                        //memberStatusReportModel.FirstName = Convert.ToString(drloff["FirstName"]);
        //                        /*'A', 'I', 'T', 'P', 'X', S' is "- Active, Inactive, Terminated, Provisional, and S is suspended*/
        //                        string strLLRStatus = Convert.ToString(agentsModel.LLRStatus);
        //                        if (strLLRStatus == "A")
        //                        {
        //                            agentsModel.LLRStatus = "Active";
        //                        }
        //                        else if (strLLRStatus == "I")
        //                        {
        //                            agentsModel.LLRStatus = "Inactive";
        //                        }
        //                        else if (strLLRStatus == "T")
        //                        {
        //                            agentsModel.LLRStatus = "Terminated";
        //                        }
        //                        else if (strLLRStatus == "P")
        //                        {
        //                            agentsModel.LLRStatus = "Provisional";
        //                        }
        //                        else if (strLLRStatus == "X")
        //                        {
        //                            agentsModel.LLRStatus = "Lifetime Member";
        //                        }
        //                        else if (strLLRStatus == "S")
        //                        {
        //                            agentsModel.LLRStatus = "Suspended";
        //                        }
        //                        string strNRDSStatus = Convert.ToString(agentsModel.NRDSStatus);
        //                        if (strNRDSStatus == "A")
        //                        {
        //                            agentsModel.NRDSStatus = "Active";
        //                        }
        //                        else if (strNRDSStatus == "I")
        //                        {
        //                            agentsModel.NRDSStatus = "Inactive";
        //                        }
        //                        else if (strNRDSStatus == "T")
        //                        {
        //                            agentsModel.NRDSStatus = "Terminated";
        //                        }
        //                        else if (strNRDSStatus == "P")
        //                        {
        //                            agentsModel.NRDSStatus = "Provisional";
        //                        }
        //                        else if (strNRDSStatus == "X")
        //                        {
        //                            agentsModel.NRDSStatus = "Lifetime Member";
        //                        }
        //                        else if (strNRDSStatus == "S")
        //                        {
        //                            agentsModel.NRDSStatus = "Suspended";
        //                        }

        //                        //lstMemberStatusReportModel.Add(memberStatusReportModel);
        //                    }

        //                }


        //                StringBuilder sbPrintHtmlContentAgentsList = new StringBuilder();
        //                if (brokerDetailModel.AgentsModelList.Count > 0)
        //                {
        //                    sbPrintHtmlContentAgentsList.Append("<tr> <td colspan='2'><h4>Agents's Details</h4></td></tr>");

        //                    sbPrintHtmlContentAgentsList.Append("<tr> <td colspan='2'><table width=100%><tr class='evenrow'><td><b>Member Id</b></td><td><b>First Name</b></td><td><b>Last Name</b></td><td><b>Office Id</b></td><td><b>Member Type</b></td><td><b>LLR Status</b></td><td><b>NRDS Status</b></td></tr>");

        //                    bool isOdd = true;
        //                    foreach (AgentsModel agentsModel in brokerDetailModel.AgentsModelList)
        //                    {
        //                        if (isOdd)
        //                        {
        //                            sbPrintHtmlContentAgentsList.Append("<tr class='oddrow'>");
        //                            isOdd = false;
        //                        }
        //                        else
        //                        {
        //                            sbPrintHtmlContentAgentsList.Append("<tr class='evenrow'>");
        //                            isOdd = true;
        //                        }
        //                        sbPrintHtmlContentAgentsList.Append("<td>" + agentsModel.MemberId + "</td><td>" + agentsModel.FirstName + "</td><td>" + agentsModel.LastName + "</td><td>" + agentsModel.OfficeId + "</td><td>" + agentsModel.MemberType + "</td><td>" + agentsModel.LLRStatus + "</td><td>" + agentsModel.NRDSStatus + "</td></tr>");
        //                    }
        //                    sbPrintHtmlContentAgentsList.Append("</table></td></tr>");
        //                }
        //                //brokerDetailModel.PrintHtmlContent = brokerDetailModel.PrintHtmlContent.Replace("{AgentDetails}", sbPrintHtmlContentAgentsList.ToString());
        //                brokerDetailModel.PrintHtmlContentAgentList = sbPrintHtmlContentAgentsList.ToString();
        //                return View("BrokerDetails", brokerDetailModel);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            BrokerDAL = null;
        //            GC.Collect();
        //        }
        //    }
        //    else
        //    {
        //        TempData["expires"] = "true";
        //        return RedirectToAction("Users", "Login");
        //    }

        //    return View("BrokerDetails",brokerDetailModel);
        //}

        public ActionResult AgentsFilter(int BrokerID)
        {
            bool filter = true;
            string LLR = "";
            string NRDS = "";
            // LLRStatusFilter, NRDSStatusFilter,Inactive, Active
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["LLRStatusFilter"])))
            {
                LLR=Convert.ToString(Request.QueryString["LLRStatusFilter"]);
            }
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["NRDSStatusFilter"])))
            {
                NRDS = Convert.ToString(Request.QueryString["NRDSStatusFilter"]);
            }

            Session["LLR"] = LLR;
            Session["NRDS"] = NRDS;
            return new JsonResult
            {
                Data = Url.Action("BrokerDetails", "Broker", new { MemberId = BrokerID, filterOption = filter }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult Export()
        {
            UserSession userSession = new UserSession();
            GridView gv = new GridView();
            if (userSession.Exists)
            {
                gv.DataSource = TempData["BrokerList"];

                gv.DataBind();
                if (gv.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=AgentsList.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    return RedirectToAction("Brokerlist");
                }
                else
                {
                    TempData["error"] = "No records found.";
                    TempData["errtitle"] = "Agents List";
                    TempData["errType"] = "warning";
                    return RedirectToAction("Brokerlist");
                }
            }
            else
            {
                TempData["expires"] = "true";
                return RedirectToAction("Users", "Login");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ReportFilter()
        {
            string value1 = "";
            string value2 = "";
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["value1"])))
            {
                if (Convert.ToString(Request.QueryString["value1"]) == "")
                {
                    value1 = null;
                }
                else
                {
                    value1 = Convert.ToString(Convert.ToString(Request.QueryString["value1"]));
                }
            }



            Session["filter1"] = value1;
            return new JsonResult
            {

                Data = Url.Action("BrokerList", "Broker"),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter1"></param>
        /// <returns></returns>
        private string getCondition(string filter1)
        {

            UserSession userSession = new UserSession();
            string _condition = "";
            Session["filter1"] = null;
            Session.Remove("filter1");
            if (userSession.Exists)
            {
                if (filter1 != null)
                {
                    var uSession = userSession.GetUser;

                    if (filter1 == "InActive")
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] IN ('I','T')  AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status] IN ('I','T') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] IN ('I','T')";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status] IN ('I','T') ";
                            }
                        }
                    }
                    else if (filter1 == "Active")
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] IN ('A') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('A') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] IN ('A') ";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('A') ";
                            }
                        }
                    }
                    else if (filter1 == "Delinquent")
                    {
                        //if (uSession.AssocID != 0)
                        //{
                        //    if (_condition == "")
                        //    {
                        //        _condition += " [Status] IN ('A') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                        //    }
                        //    else
                        //    {
                        //        _condition = _condition + " AND " + "  [Status]  IN ('A') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                        //    }
                        //}
                        //else
                        //{
                        //    if (_condition == "")
                        //    {
                        //        _condition += " [Status] IN ('A') ";
                        //    }

                        //    else
                        //    {
                        //        _condition = _condition + " AND " + "  [Status]  IN ('A') ";
                        //    }
                        //}
                    }

                    else if (filter1 == "InActive+Active")
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status]  IN ('A', 'I') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('A', 'I') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status]  IN ('A', 'I') ";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('A', 'I') ";
                            }
                        }
                    }
                    else if (filter1 == "NOInActive+NOActive")
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] NOT IN ('A', 'I') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status] NOT IN ('A', 'I') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] NOT IN ('A', 'I') ";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status] NOT IN ('A', 'I') ";
                            }
                        }
                    }
                    else
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status]  IN ('A') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('A')  AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status]  IN ('A') ";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('A') ";
                            }
                        }
                    }

                }
            }

            return _condition;
        }
    }
}
