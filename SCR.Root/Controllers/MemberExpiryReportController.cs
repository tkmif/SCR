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
    public class MemberExpiryReportController : Controller
    {
        //
        // GET: /MemberExpiryReport/


        public ActionResult MemberExpiryReport(int start = 0, int pagesize = 10, string order = " [MemberId] ")
        {

            MemberExpiryReportModel memberExpiryReportModel = new MemberExpiryReportModel();
            MemberExpiryReportDAL memberExpiryReportDAL = new MemberExpiryReportDAL();
            UserSession userSession = new UserSession();
            string constrian = string.Empty;
            string filter1 = string.Empty;
            string filter2 = string.Empty;
            if (userSession.Exists)
            {

                try
                {
                    if (Session["filter1"] != null)
                    {

                        filter1 = Session["filter1"].ToString();
                        //Session["filter1"] = null;
                    }
                    memberExpiryReportModel.hdnStatus = filter1;
                    constrian = getCondition(filter1, order);

                    memberExpiryReportModel.MemberExpiryReportModelList = memberExpiryReportDAL.getMemberExpiryReportList(constrian, 0);
                   
                    TempData["MemberExpiryReportModelList"] = memberExpiryReportModel.MemberExpiryReportModelList;
                    if (memberExpiryReportModel.MemberExpiryReportModelList.Count == 0)
                    {
                        TempData["error"] = "No items match your search.";
                        TempData["errtitle"] = "Member Expiry Report";
                        TempData["errType"] = "warning";
                    }
                    TempData["count1"] = Convert.ToString(memberExpiryReportModel.MemberExpiryReportModelList.Count);
                    TempData["start"] = start;
                    TempData["pagesize"] = pagesize;
                    TempData["order"] = order;
                    return View(memberExpiryReportModel);

                }
                catch (Exception ex)
                {
                    memberExpiryReportDAL = null;
                    GC.Collect();
                }

                return View(memberExpiryReportModel);
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
        public ActionResult Export()
        {
             UserSession userSession = new UserSession();
             if (userSession.Exists)
             {

                 GridView gv = new GridView();

                 if (TempData["MemberExpiryReportModelList"].ToString() != null)
                 {
                     gv.DataSource = TempData["MemberExpiryReportModelList"];

                     gv.DataBind();

                     Response.ClearContent();
                     Response.Buffer = true;
                     Response.AddHeader("content-disposition", "attachment; filename=MemberExpiryReport.xls");
                     Response.ContentType = "application/ms-excel";
                     Response.Charset = "";
                     StringWriter sw = new StringWriter();
                     HtmlTextWriter htw = new HtmlTextWriter(sw);
                     gv.RenderControl(htw);
                     Response.Output.Write(sw.ToString());
                     Response.Flush();
                     Response.End();
                     return RedirectToAction("MemberExpiryReport");
                 }
                 else
                 {
                     TempData["error"] = "No records found.";
                     TempData["errtitle"] = "Member Expiry Report";
                     TempData["errType"] = "warning";
                     return RedirectToAction("MemberExpiryReport");
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

                Data = Url.Action("MemberExpiryReport", "MemberExpiryReport"),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter1"></param>
        /// <returns></returns>
        private string getCondition(string filter1, string order = "  [MemberId] ")
        {
            UserSession userSession = new UserSession();


            string _condition = "";

            if (userSession.Exists)
            {

                var uSession = userSession.GetUser;

                if (filter1 != null)
                {
                    if (filter1 == "Expired")
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] IN ('I', 'T','S') AND [PrimaryAssociationID] = " + uSession.AssocID + " Order by " + order + "";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status] IN ('I', 'T','S') AND [PrimaryAssociationID] = " + uSession.AssocID + " Order by " + order + "";
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status] IN ('I', 'T','S')  Order by " + order + "";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status] IN ('I', 'T','S') Order by " + order + "";
                            }
                        }
                    }
                    else if (filter1 == "Active")
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status]  IN ('A','X','P') AND [PrimaryAssociationID] = " + uSession.AssocID + " Order by " + order + "";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('A','X','P') AND [PrimaryAssociationID] = " + uSession.AssocID + " Order by " + order + "";
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status]  IN ('A','X','P') Order by " + order + "";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('A','X','P') Order by " + order + "";
                            }
                        }
                    }
                    else
                    {
                        if (uSession.AssocID != 0)
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status]  IN ('I', 'T','S') AND [PrimaryAssociationID] = " + uSession.AssocID + " Order by " + order + "";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('I', 'T','S') AND [PrimaryAssociationID] = " + uSession.AssocID + " Order by " + order + "";
                            }
                        }
                        else
                        {
                            if (_condition == "")
                            {
                                _condition += " [Status]  IN ('I', 'T','S')  Order by " + order + "";
                            }
                            else
                            {
                                _condition = _condition + " AND " + "  [Status]  IN ('I', 'T','S') Order by " + order + "";
                            }
                        }
                    }

                }
            }
           

            return _condition;
        }

    }
}
