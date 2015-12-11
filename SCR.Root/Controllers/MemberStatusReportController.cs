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
    /// <summary>
    /// Author: Geethanjali P Nair
    /// </summary>
    public class MemberStatusReportController : Controller
    {
        //
        // GET: /MemberStatusReport/

        public ActionResult MemberStatusReport(bool filterOption=false)
        {
            MemberStatusReportModel memberStatusReportModel = new MemberStatusReportModel();
            MemberStatusReportDAL memberStatusReportDAL = new MemberStatusReportDAL();
            UserSession userSession = new UserSession();
            string constrian = string.Empty;
            string filter1 = string.Empty;
            try
            {
                if (userSession.Exists)
                {
                    if (Session["filter1"] != null)
                    {

                        filter1 = Session["filter1"].ToString();
                        Session["filter1"] = null;
                    }
                    constrian = getCondition(filter1);
                    if (filterOption == true)
                    { 
                  
                        string LLR = "";
                        if (Session["LLR"] != null) { LLR = Session["LLR"].ToString(); }
                        string NRDS = "";
                        if (Session["NRDS"] != null) { NRDS = Session["NRDS"].ToString(); }

                        if (LLR != "" && LLR != "Select") { constrian = " LLR.[Status]='" + LLR + "'"; }
                        if (NRDS != "" && NRDS != "Select") { constrian += " and NRDS.[MemberStatusVal]='" + NRDS + "'"; }
                        ViewBag.LLR = LLR;
                        ViewBag.NRDS = NRDS;
                       
                    }
                    memberStatusReportModel.MemberStatusReportModelList = memberStatusReportDAL.getMemberStatusReportList(constrian);
                    List<MemberStatusReportModel> MemberExpiryR = memberStatusReportModel.MemberStatusReportModelList.Where(c => c.FirstName == "Steven" && c.LastName == "Crossland").ToList<MemberStatusReportModel>();
                    TempData["MemberStatusReportModelList"] = memberStatusReportModel.MemberStatusReportModelList;
                    if (memberStatusReportModel.MemberStatusReportModelList.Count == 0)
                    {
                        TempData["error"] = "No items match your search.";
                        TempData["errtitle"] = "Member Staus Report";
                        TempData["errType"] = "warning";
                    }
                    TempData["count"] = Convert.ToString(memberStatusReportModel.MemberStatusReportModelList.Count);
                    return View(memberStatusReportModel);
                }
                else
                {
                    TempData["expires"] = "true";
                    return RedirectToAction("Users", "Login");
                }
            }
            catch (Exception ex)
            {
                memberStatusReportDAL = null;
                GC.Collect();
            }

            return View(memberStatusReportModel);
        }

        /// <summary>
        /// To export report as Excel file
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            UserSession userSession = new UserSession();
            if (userSession.Exists)
            {
                GridView gv = new GridView();
                if (TempData["count"].ToString() != "0")
                {
                    gv.DataSource = TempData["MemberStatusReportModelList"];

                    gv.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=MemberStatusReport.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    return RedirectToAction("MemberStatusReport");
                }
                else
                {
                    TempData["error"] = "No records found.";
                    TempData["errtitle"] = "Member Status Report";
                    TempData["errType"] = "warning";
                    return RedirectToAction("MemberStatusReport");
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

                Data = Url.Action("MemberStatusReport", "MemberStatusReport"),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        public ActionResult DataFilter(int BrokerID)
        {
            bool filter = true;
            string LLR = "";
            string NRDS = "";
            // LLRStatusFilter, NRDSStatusFilter,Inactive, Active
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["LLRStatusFilter"])))
            {
                LLR = Convert.ToString(Request.QueryString["LLRStatusFilter"]);
            }
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["NRDSStatusFilter"])))
            {
                NRDS = Convert.ToString(Request.QueryString["NRDSStatusFilter"]);
            }

            Session["LLR"] = LLR;
            Session["NRDS"] = NRDS;
            return new JsonResult
            {
                Data = Url.Action("MemberStatusReport", "MemberStatusReport", new { MemberId = BrokerID, filterOption = filter }),
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
            if (userSession.Exists)
            {
                var uSession = userSession.GetUser;

                //if (filter1 != null)
                //{
                //    if (filter1 == "Active")
                //    {
                if (uSession.AssocID != 0)
                {
                    if (_condition == "")
                    {
                        _condition += " LLR.[Status] <> NRDS.[MemberStatusVal] AND LLR.[PrimaryAssociationID] = " + uSession.AssocID;
                    }
                    else
                    {
                        _condition = _condition + " AND " + "  LLR.[Status] <> NRDS.[MemberStatusVal] AND LLR.[PrimaryAssociationID]= " + uSession.AssocID;
                    }
                }
                else
                {
                    if (_condition == "")
                    {
                        _condition += " LLR.[Status] <> NRDS.[MemberStatusVal] ";
                    }
                    else
                    {
                        _condition = _condition + " AND " + "  LLR.[Status] <> NRDS.[MemberStatusVal] ";
                    }
                }
                //    }

                //}
            }

            return _condition;
        }


    }
}
