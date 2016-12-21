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
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

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

            string LLR="";
            string NRDS="";
            string deq="";
            string filter1 = "InActive+Active";
            try
            {
                if (userSession.Exists)
                {
                    var uSession = userSession.GetUser;
                    if (Session["LLR"] != null)
                    {
                         LLR = Session["LLR"].ToString();
                         ViewBag.LLR = LLR;
                    }
                    if (Session["NRDS"] != null)
                    {
                         NRDS = Session["NRDS"].ToString();
                         ViewBag.NRDS = NRDS;
                    }
                    if (Session["deq"] != null)
                    {
                         deq = Session["deq"].ToString();
                         ViewBag.deq = deq;
                    }



                    constrian = getCondition(LLR, NRDS, deq, uSession.AssocID);
                   string type;

                   if (deq == "Delinquent")
                   {
                       type = "deq";
                       
                   }
                   else { type = ""; }

                   BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerListConstrain(type, constrian);
                   var reducedBrokerList = BrokerDetailModel.BrokerModelList.Select(e => new { e.MemberId, e.OfficeId, e.MemberType, e.LastName, e.FirstName, e.Status, e.LLRStatus, e.NRDSStatus }).ToList();

                   TempData["BrokerList"] = reducedBrokerList;
                   TempData["ReportCount"] = reducedBrokerList.Count.ToString();
                   Session["LLR"] = null; Session["NRDS"] = null; Session["deq"] = null;
                    //if (Session["filter1"] != null)
                    //{
                    //    filter1 = Session["filter1"].ToString();
                    //}
                    //if (filter1 != null && filter1 != "")
                    //{
                    //    ViewData["ChkStatus"] = filter1;
                    //    BrokerDetailModel.hdnStatus = filter1;
                    //    if (filter1 == "Delinquent")
                    //    {
                    //        //constrian = getCondition(filter1);
                    //        var uSession = userSession.GetUser;
                    //        BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerWithDelinquentAgents(constrian, uSession.AssocID);
                    //        var reducedBrokerList = BrokerDetailModel.BrokerModelList.Select(e => new { e.MemberId, e.OfficeId, e.MemberType, e.LastName, e.FirstName, e.Status, e.LLRStatus, e.NRDSStatus }).ToList();

                    //        TempData["BrokerList"] = reducedBrokerList;
                    //        ViewData["ChkStatus"] = filter1;
                    //        TempData["ReportCount"] = reducedBrokerList.Count.ToString();
                    //    }
                    //    else
                    //    {
                    //        constrian = getCondition(filter1);
                    //        BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerListConstrain(constrian);
                    //        var reducedBrokerList = BrokerDetailModel.BrokerModelList.Select(e => new { e.MemberId, e.OfficeId, e.MemberType, e.LastName, e.FirstName, e.Status, e.LLRStatus, e.NRDSStatus }).ToList();

                    //        TempData["BrokerList"] = reducedBrokerList;
                    //        TempData["ReportCount"] = reducedBrokerList.Count.ToString();
                    //    }
                    //    return View(BrokerDetailModel);
                    //}
                    ////else if (filter1 == "")
                    ////{
                    ////    ViewData["ChkStatus"] = filter1;
                    ////    BrokerDetailModel.hdnStatus = filter1;
                    ////    constrian = getCondition(filter1);
                    ////    BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerListConstrain(constrian);
                    ////    TempData["BrokerListConstrain"] = BrokerDetailModel.BrokerModelListConstain;
                    ////    return View(BrokerDetailModel);
                    ////}
                    //else
                    //{
                    //    var uSession = userSession.GetUser;

                    //    ViewData["ChkStatus"] = filter1;
                    //    BrokerDetailModel.hdnStatus = filter1;
                    //    BrokerDetailModel.BrokerModelList = BrokerDAL.getBrokerList(uSession.AssocID);
                    //    var reducedBrokerList = BrokerDetailModel.BrokerModelList.Select(e => new { e.MemberId, e.OfficeId, e.MemberType,e.LastName,e.FirstName,e.Status,e.LLRStatus,e.NRDSStatus }).ToList();
                    //    TempData["BrokerList"] = reducedBrokerList;
                    //    TempData["ReportCount"] = reducedBrokerList.Count.ToString();
                    //    return View(BrokerDetailModel);

                    //}

                    //Session["filter1"] = null;
                    //Session.Remove("filter1");
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


        public ActionResult DataFilter()
        {
            bool filter = true;
            string LLR = "";
            string NRDS = "";
            string deq = "";
            // LLRStatusFilter, NRDSStatusFilter,Inactive, Active
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["LLRStatusFilter"])))
            {
                LLR = Convert.ToString(Request.QueryString["LLRStatusFilter"]);
            }
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["NRDSStatusFilter"])))
            {
                NRDS = Convert.ToString(Request.QueryString["NRDSStatusFilter"]);
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["deq"])))
            {
                deq = Convert.ToString(Request.QueryString["deq"]);
            }
            
            Session["LLR"] = LLR;
            Session["NRDS"] = NRDS;
            Session["deq"] = deq;
            return new JsonResult
            {
                Data = Url.Action("BrokerList", "Broker", new { }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public string getCondition(string LLR, string NRD, string deq,int uid)
        {

            string Condition="";
            LLR= (LLR == "Select") ? "" : LLR;
            NRD = (NRD == "Select") ? "" : NRD;


            if (deq == "Delinquent")
            {

                Condition = "";
            
            }
            else if (LLR != "" && NRD == "")
            {
                Condition = " AND LLR.MemberStatusVal='" + LLR + "'";

            }
            else if (NRD != "" && LLR=="")
            {

                Condition = " AND NRDS.Status='"+NRD+"'";
            }
            else if (NRD != "" && LLR != "")
            {


                Condition = " AND NRDS.Status='" + NRD + "'  AND LLR.MemberStatusVal='" + LLR + "' ";

            }

            else { Condition = " AND 1=1"; }

            if (uid > 0)
            {

                Condition = Condition + " AND NRDS.PrimaryAssociationID="+uid+"";
            }


            return Condition;
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
                        Session["brokerid"] = broker_Id;
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/HTMLTemplates/_PrintBrokerDetails.html")))
                        {
                            brokerDetailModel = BrokerDAL.getBrokerDetails(broker_Id, reader.ReadToEnd());
                          //  TempData["BrokerList"] = brokerDetailModel.AgentsModelList;

                            //list_broker_detaisl = brokerDetailModel.AgentsModelList;

                        }
                        string LLR="";
                        if (Session["LLR"] != null ) { LLR = Session["LLR"].ToString(); }
                        string NRDS="";
                        if (Session["NRDS"] != null ) { NRDS = Session["NRDS"].ToString(); }
                        string condition = "";
                        if (filterOption == true)
                        {
                            if (LLR != "" && LLR != "Select") { condition += " and NRDS.[Status]='" + LLR + "'"; }
                            if (NRDS != "" && NRDS != "Select") { condition += " and LLR.[MemberStatusVal]='" + NRDS + "'"; }
                            ViewBag.LLR = LLR;
                            ViewBag.NRDS = NRDS;
                        }
                        brokerDetailModel.AgentsModelList = agentsDAL.getAgentsList(broker_Id, 0, "and [OfficeContactDR]= " + broker_Id + condition);
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
                        Session["querystring"] = Request.QueryString["Status"];
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
                                    agentsModel.NRDSStatus = "ACTIVE";
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
                        Session["detailexport"] = brokerDetailModel;
                        //ExportDetails();
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
        [HttpGet]
        public FileStreamResult ExportDetails()
        {
            string url;
            BrokerDetailsModel brokerDetailModel = new BrokerDetailsModel();
             UserSession userSession = new UserSession();
            GridView gv = new GridView();
            url = "~/Broker/BrokerDetails?MemberId=" + Session["brokerid"].ToString() + "&Status=" + Session["querystring"].ToString();
            //TempData["brokerid"] = TempData["brokerid"].ToString();
            //Session["querystring"] = TempData["querystring"].ToString();
            
            if (userSession.Exists)
            {

                try
                {
                   
                    brokerDetailModel = (BrokerDetailsModel)Session["detailexport"];

                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                    if (xlApp == null)
                    {
                        // MessageBox.Show("Excel is not properly installed!!");
                        // return;
                    }


                    Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;

                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    //xlWorkSheet.Cells[100, 1] = "ID";
                    //xlWorkSheet.Cells[100, 2] = "Name";
                    //xlWorkSheet.Cells[101, 1] = "1";
                    //xlWorkSheet.Cells[101, 2] = "One";
                    //xlWorkSheet.Cells[102, 1] = "2";
                    //xlWorkSheet.Cells[102, 2] = "Two";

                    xlWorkSheet.Columns[1].ColumnWidth = 17.57;
                    xlWorkSheet.Columns[2].ColumnWidth = 17.57;
                    xlWorkSheet.Columns[3].ColumnWidth = 17.57;
                    xlWorkSheet.Columns[4].ColumnWidth = 17.57;
                    xlWorkSheet.Columns[5].ColumnWidth = 17.57;
                    xlWorkSheet.Columns[6].ColumnWidth = 17.57;
                    xlWorkSheet.Columns[7].ColumnWidth = 17.57;


                    Microsoft.Office.Interop.Excel.Range formatRange;
                    formatRange = xlWorkSheet.get_Range("a1");
                    formatRange.EntireRow.Font.Bold = true;
                    formatRange.EntireRow.Font.Size = 13;
                    xlWorkSheet.get_Range("a1", "h1").Merge(false);
                    formatRange = xlWorkSheet.get_Range("a2");
                    formatRange.EntireRow.Font.Bold = true;
                    xlWorkSheet.Cells[1, 1] = "Agent Details";
                    xlWorkSheet.Cells[2, 1] = "Member Id";
                    xlWorkSheet.Cells[2, 2] = "Agent's Name";
                    xlWorkSheet.Cells[2, 3] = "Agent Type";
                    xlWorkSheet.Cells[2, 4] = "R/E License No";
                    xlWorkSheet.Cells[2, 5] = "NRDS Status";
                    xlWorkSheet.Cells[2, 6] = "LLR Status";
                    xlWorkSheet.Cells[2, 7] = "Office Id";
                    
                    //xlWorkSheet.Cells[2, 1] = "Agent Details";
                    xlWorkSheet.Cells[3, 1] = brokerDetailModel.MemberId;
                    xlWorkSheet.Cells[3, 2] = brokerDetailModel.LastName;
                    xlWorkSheet.Cells[3, 3] = brokerDetailModel.AgentType;
                    xlWorkSheet.Cells[3, 4] = brokerDetailModel.RELicenseNumber;
                    xlWorkSheet.Cells[3, 5] = brokerDetailModel.NRDSStatus;
                    xlWorkSheet.Cells[3, 6] = brokerDetailModel.LLRStatus;
                    xlWorkSheet.Cells[3, 7] = brokerDetailModel.OfficeId;



                    formatRange = xlWorkSheet.get_Range("a4");
                    formatRange.EntireRow.Font.Bold = true;
                    formatRange.EntireRow.Font.Size = 13;
                    xlWorkSheet.get_Range("a4", "h4").Merge(false);

                    formatRange = xlWorkSheet.get_Range("a5");
                    formatRange.EntireRow.Font.Bold = true;
                    xlWorkSheet.Cells[4, 1] = "OFFICE DETAILS";
                    xlWorkSheet.Cells[5, 1] = "Bussiness Name";
                    xlWorkSheet.Cells[5, 2] = "Office Address";
                    xlWorkSheet.Cells[5, 3] = "Email";
                    xlWorkSheet.Cells[5, 4] = "City";
                    xlWorkSheet.Cells[5, 5] = "State";
                    xlWorkSheet.Cells[5, 6] = "Zipcode";
                    xlWorkSheet.Cells[6, 1] = brokerDetailModel.OfficeName;
                    xlWorkSheet.Cells[6, 2] = brokerDetailModel.OfficeAddress;
                    xlWorkSheet.Cells[6, 3] = brokerDetailModel.Email;
                    xlWorkSheet.Cells[6, 4] = brokerDetailModel.City;
                    xlWorkSheet.Cells[6, 5] = brokerDetailModel.State;
                    xlWorkSheet.Cells[6, 6] = brokerDetailModel.Zipcode;

                    formatRange = xlWorkSheet.get_Range("a7");
                    formatRange.EntireRow.Font.Bold = true;
                    formatRange.EntireRow.Font.Size = 13;
                    xlWorkSheet.get_Range("a7", "h7").Merge(false);
                    formatRange = xlWorkSheet.get_Range("a8");
                    formatRange.EntireRow.Font.Bold = true;
                    xlWorkSheet.Cells[7, 1] = "Agents under Broker " + brokerDetailModel.LastName; 
                    xlWorkSheet.Cells[8, 1] = "Member Id";
                    xlWorkSheet.Cells[8, 2] = "Last Name";
                    xlWorkSheet.Cells[8, 3] = "First Name";
                    xlWorkSheet.Cells[8, 4] = "Office Id";
                    xlWorkSheet.Cells[8, 5] = "Member Type";
                    xlWorkSheet.Cells[8, 6] = "LLR Status";
                    xlWorkSheet.Cells[8, 7] = "NRDS Status";
                    int i = 9;
                    foreach (AgentsModel agentsModel in brokerDetailModel.AgentsModelList)
                    {
                       
                      

                        xlWorkSheet.Cells[i, 1] = agentsModel.MemberId;
                        xlWorkSheet.Cells[i, 2] = agentsModel.LastName;
                        xlWorkSheet.Cells[i, 3] = agentsModel.FirstName;
                        xlWorkSheet.Cells[i, 4] = agentsModel.OfficeId;
                        xlWorkSheet.Cells[i, 5] = agentsModel.MemberType;
                        xlWorkSheet.Cells[i, 6] = agentsModel.LLRStatus;
                        xlWorkSheet.Cells[i, 7] = agentsModel.NRDSStatus;
                        i++;
                    }


                    string path = Server.MapPath("~/Files/exceltemp/");
                    string file = path + "Brokerdetails_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls";
                    xlApp.DisplayAlerts = false;
                    xlWorkBook.SaveAs(file, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                  //  xlWorkBook.Save();
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);



                    var cd = new System.Net.Mime.ContentDisposition { FileName = "Brokerdetails_" + brokerDetailModel.LastName + ".xls", Inline = false };
                    byte[] arr = System.IO.File.ReadAllBytes(file);

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=Brokerdetails_" + brokerDetailModel.LastName + ".xls");
                    Response.ContentType = "application/ms-excel";
                 
                   // Response.AddHeader("content-disposition", cd.ToString());
                    Response.Buffer = true;
                    Response.Clear();
                    Response.BinaryWrite(arr);
                    Response.End();
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                   // return new FileStreamResult(Response.OutputStream, Response.ContentType);

                  
                }
                catch (Exception ex)
                {

                    TempData["error"] = "Error";
                    TempData["errtitle"] = "Broker List";
                    TempData["errType"] = "warning";
                 
                   
                }

            }
            else
            {
                TempData["expires"] = "true";
              
            }
      
          
            return new FileStreamResult(Response.OutputStream, Response.ContentType);
          
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
                    Response.AddHeader("content-disposition", "attachment; filename=BrokerList.xls");
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
                    TempData["errtitle"] = "Broker List";
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
                                _condition += " [Status] IN ('I','T')  AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status] IN ('I','T') AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
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
                                //_condition += " [Status] IN ('A') AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
                                _condition += "  LLR.[MemberStatusVal]='ACTIVE' AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
                                
                            }
                            else
                            {
                               // _condition = _condition + " AND " + "  [Status]  IN ('A') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                                _condition = _condition + " AND " + "  LLR.[MemberStatusVal]='ACTIVE' AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                              //  _condition += " [Status] IN ('A') ";
                                _condition += "LLR.[MemberStatusVal]='ACTIVE' ";
                            }
                            else
                            {
                                //_condition = _condition + " AND " + "  [Status]  IN ('A') ";
                                _condition = _condition + " AND " + " LLR.[MemberStatusVal]='ACTIVE' ";
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
                        //if (uSession.AssocID != 0)
                        //{
                        //    if (_condition == "")
                        //    {
                        //        _condition += " [Status]  IN ('A', 'I') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                        //    }
                        //    else
                        //    {
                        //        _condition = _condition + " AND " + "  [Status]  IN ('A', 'I') AND L.[PrimaryAssociationID] = " + uSession.AssocID;
                        //    }
                        //}
                        //else
                        //{
                        //    if (_condition == "")
                        //    {
                        //        _condition += " [Status]  IN ('A', 'I') ";
                        //    }
                        //    else
                        //    {
                        //        _condition = _condition + " AND " + "  [Status]  IN ('A', 'I') ";
                        //    }
                        //}
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += "  LLR.[MemberStatusVal]='ACTIVE' AND [Status]  = 'I' AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                            else
                            {
                                _condition = _condition + " AND " + " LLR.[MemberStatusVal]='ACTIVE' AND [Status]  = 'I'  AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " LLR.[MemberStatusVal]='ACTIVE' AND [Status]  = 'I'  ";
                            }
                            else
                            {
                                _condition = _condition + " AND " + " LLR.[MemberStatusVal]='ACTIVE' AND [Status]  = 'I'  ";
                            }
                        }
                    }
                    else if (filter1 == "NOInActive+NOActive")
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] NOT IN ('A', 'I') AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status] NOT IN ('A', 'I') AND NRDS.[PrimaryAssociationID] = " + uSession.AssocID;
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
