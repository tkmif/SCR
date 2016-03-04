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
    public class AgentsListController : Controller
    {
        //
        // GET: /AgentsList/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AgentsList(int start = 0, int pagesize = 10,string order = "  [MemberId] ")
        {
            AgentsModel agentsModel = new AgentsModel();
            AgentsDAL agentsDAL = new AgentsDAL();
            UserSession userSession = new UserSession();
            int broker_Id = 0;
            int office_Id = 0;
            string condition = "";
            string filter1 = string.Empty;
            if (userSession.Exists)
            {
                try
                {

                    if (Session["BrokerId"] != null)
                    {

                        broker_Id = Convert.ToInt32(Session["BrokerId"]);
                    }

                    if (Session["OfficeId"] != null)
                    {

                        office_Id = Convert.ToInt32(Session["OfficeId"]);
                    }

                    if (Session["condition"] != null)
                    {

                        condition = Session["condition"].ToString().TrimStart().TrimEnd();
                    }
                    else
                    {
                        var uSession = userSession.GetUser;
                        if (uSession.AssocID != 0)
                        { 
                            condition = " AND L.[PrimaryAssociationID] = " + uSession.AssocID + " order by  " + order;
                        }
                    }

                    if (TempData["search"] == null)
                    {
                        TempData["search"] = 0;
                        Session["OfficeId"] = null;
                        Session["BrokerId"] = null;
                        Session["condition"] = null;
                    }

                    if (TempData["value1"] == null)
                    {
                        TempData["value1"] = "";
                    }

                    if (TempData["type"] == null)
                    {
                        TempData["type"] = "";
                    }
                    if (TempData["name"] == null)
                    {
                        TempData["name"] = "";
                    }
                    if (TempData["lname"] == null)
                    {
                        TempData["lname"] = "";
                    }


                    agentsModel.AgentsModelList = agentsDAL.getAgentsList(broker_Id, office_Id, condition);
                    TempData["AgentsList"] = agentsModel.AgentsModelList;

                    //  TempData["count"] = Convert.ToString(agentsModel.AgentsModelList.Count);
                    TempData["start"] = start;
                    TempData["pagesize"] = pagesize;
                    TempData["order"] = order;
                    if (agentsModel.AgentsModelList.Count == 0)
                    {
                        TempData["error"] = "No items match your search.";
                        TempData["errtitle"] = "Agents List";
                        TempData["errType"] = "warning";

                        Session["OfficeId"] = null;
                        Session["BrokerId"] = null;
                        Session["condition"] = null;
                    }

                    TempData["count"] = Convert.ToString(agentsModel.AgentsModelList.Count);
                    if (Convert.ToInt32(TempData["reset"]) == 1)
                    {
                        Session["OfficeId"] = null;
                        Session["BrokerId"] = null;
                        Session["condition"] = null;
                        TempData["reset"] = 0;
                    }
                    return View(agentsModel);

                }
                catch (Exception ex)
                {
                    Session["OfficeId"] = null;
                    Session["BrokerId"] = null;
                    Session["condition"] = null;
                    TempData["error"] = "Enter Valid Data";
                    TempData["errtitle"] = "Agents List";
                    TempData["errType"] = "warning";
                    agentsDAL = null;
                    GC.Collect();
                }

                return View(agentsModel);
            }
            else
            {
                TempData["expires"] = "true";
                return RedirectToAction("Users", "Login");
            }
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
                if (TempData["AgentsList"] != null)
                {
                    gv.DataSource = TempData["AgentsList"];
                    gv.DataBind();
                }

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
                    return RedirectToAction("AgentsList");
                }
                else
                {
                    TempData["error"] = "No records found.";
                    TempData["errtitle"] = "Agents List";
                    TempData["errType"] = "warning";
                    return RedirectToAction("AgentsList");
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
        public ActionResult SearchFilter()
        {
            UserSession userSession = new UserSession();
            string value1 = null;
            string type = null;
            string name = null;
            string lname = null;
            string order = " [MemberId] ";
            string start = "0";
            string pagesize = "10";
            string memberid = null;
            string agentfname = null;
            string agentLname = null;
            string membertype = null;

            TempData["search"] = 1;
            TempData["reset"] = 1;
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
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["type"])))
            {
                if (Convert.ToString(Request.QueryString["type"]) == "")
                {
                    type = null;
                }
                else
                {
                    type = Convert.ToString(Convert.ToString(Request.QueryString["type"]));
                }
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["name"])))
            {
                if (Convert.ToString(Request.QueryString["name"]) == "")
                {
                    name = null;
                }
                else
                {
                    name = Convert.ToString(Convert.ToString(Request.QueryString["name"]));
                }
            }
            else { name = null; }
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["lname"])))
            {
                if (Convert.ToString(Request.QueryString["lname"]) == "")
                {
                    lname = null;
                }
                else
                {
                    lname = Convert.ToString(Convert.ToString(Request.QueryString["lname"]));
                }
            }
            else { lname = null; }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["order"])))
            {
                if (Convert.ToString(Request.QueryString["order"]) == "")
                {
                    order = null;
                }
                else
                {
                    order = Convert.ToString(Convert.ToString(Request.QueryString["order"]));
                }
            }
            else { order = " [MemberId] "; }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["start"])))
            {
                if (Convert.ToString(Request.QueryString["start"]) == "")
                {
                    start = null;
                }
                else
                {
                    start = Convert.ToString(Convert.ToString(Request.QueryString["start"]));
                }
            }
            else { start = "0"; }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["pagesize"])))
            {
                if (Convert.ToString(Request.QueryString["pagesize"]) == "")
                {
                    pagesize = null;
                }
                else
                {
                    pagesize = Convert.ToString(Convert.ToString(Request.QueryString["pagesize"]));
                }
            }
            else { pagesize = "10"; }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["memberid"])))
            {
                if (Convert.ToString(Request.QueryString["memberid"]) == "")
                {
                    memberid = null;
                }
                else
                {
                    memberid = Convert.ToString(Convert.ToString(Request.QueryString["memberid"]));
                }
            }
            else { memberid = null; }
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["agentfname"])))
            {
                if (Convert.ToString(Request.QueryString["agentfname"]) == "")
                {
                    agentfname = null;
                }
                else
                {
                    agentfname = Convert.ToString(Convert.ToString(Request.QueryString["agentfname"]));
                }
            }
            else { agentfname = null; }
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["agentLname"])))
            {
                if (Convert.ToString(Request.QueryString["agentLname"]) == "")
                {
                    agentLname = null;
                }
                else
                {
                    agentLname = Convert.ToString(Convert.ToString(Request.QueryString["agentLname"]));
                }
            }
            else { agentLname = null; }
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["membertype"])))
            {
                if (Convert.ToString(Request.QueryString["membertype"]) == "")
                {
                    membertype = null;
                }
                else
                {
                    string memtype = Convert.ToString(Convert.ToString(Request.QueryString["membertype"])).ToLower();
                    switch (memtype)
                    {
                        case "realtor":
                            membertype = "R";
                            break;
                        case "affiliate":
                            membertype = "AFF";
                            break;
                        case "non-member":
                            membertype = "N";
                            break;
                        case "staff":
                            membertype = "S";
                            break;
                        case "institute affiliate member":
                            membertype = "I";
                            break;
                        case "rarealtor associate":
                            membertype = "RA";
                            break;

                    }

                }
            }
            else { membertype = null; }


            TempData["value1"] = value1;
            TempData["type"] = type;
            TempData["name"] = name;
            TempData["lname"] = lname;
            Session["OfficeId"] = 0;
            Session["BrokerId"] = 0;

            TempData["agentfname"] = agentfname;
            TempData["agentlname"] = agentLname;
            TempData["memid"] = memberid;

            switch (membertype)
            {
                case "R":
                    TempData["memtype"] = "Realtor";
                    break;
                case "AFF":
                    TempData["memtype"] = "Affiliate";
                    break;
                case "N":
                    TempData["memtype"] = "Non-member";
                    break;
                case "S":
                    TempData["memtype"] = "Staff";
                    break;
                case "I":
                    TempData["memtype"] = "Institute Affiliate Member";
                    break;
                case "RA":
                    TempData["memtype"] = "RARealtor Associate";
                    break;

            }


            if (userSession.Exists)
            {
                var uSession = userSession.GetUser;

                if (type == "1")
                {
                    Session["OfficeId"] = type;
                    if (uSession.AssocID != 0)
                    {
                        if (value1 != null && name != null)
                        {
                            Session["condition"] = " AND L.[OfficeId]=" + value1 + " and O.OfficeBusinessName like '%" + name.TrimEnd().TrimStart() + "%' AND L.[PrimaryAssociationID] = " + uSession.AssocID + " order by " + order + " ";

                        }
                        else if (name != null && value1 == null)
                        {

                            Session["condition"] = " AND O.OfficeBusinessName like '%" + name.TrimEnd().TrimStart() + "%' AND L.[PrimaryAssociationID] = " + uSession.AssocID + " order by " + order + "";
                        }
                        else
                        {

                            Session["condition"] = "AND L.[OfficeId]=" + value1 + "  AND L.[PrimaryAssociationID] = " + uSession.AssocID + " order by " + order + "";
                        }
                    }
                    else
                    {

                        if (value1 != null && name != null)
                        {
                            Session["condition"] = " AND L.[OfficeId]=" + value1 + " and O.OfficeBusinessName like '%" + name.TrimEnd().TrimStart() + "%'  order by " + order + " ";

                        }
                        else if (name != null && value1 == null)
                        {

                            Session["condition"] = " AND O.OfficeBusinessName like '%" + name.TrimEnd().TrimStart() + "%' order by " + order + "";
                        }
                        else
                        {

                            Session["condition"] = "AND L.[OfficeId]=" + value1 + " order by " + order + "";
                        }
                    }
                }
                else if (type == "2")
                {
                    Session["BrokerId"] = type;
                    if (uSession.AssocID != 0)
                    {
                        if (value1 != null && name != null && lname != null)
                        {
                            Session["condition"] = " AND L.[OfficeId] IN (SELECT DISTINCT [OfficeId] FROM [office_details] WHERE [OfficeContactDR]= " + value1 + " ) and L1.[FirstName] like '%" + name.TrimStart().TrimEnd() + "%' And L1.[LastName] like '%" + lname.TrimStart().TrimEnd() + "%' AND L.[PrimaryAssociationID] = " + uSession.AssocID + " order by " + order + "";

                        }
                        else if (name != null && lname != null && value1 == null)
                        {
                            Session["condition"] = "  and L1.[FirstName] like '%" + name.TrimStart().TrimEnd() + "%' And L1.[LastName] like '%" + lname.TrimStart().TrimEnd() + "%' AND L.[PrimaryAssociationID] = " + uSession.AssocID + " order by " + order + "  ";

                        }
                        else if (name != null && lname == null && value1 == null)
                        {
                            Session["condition"] = " AND  L1.[FirstName]   like '%" + name.TrimStart().TrimEnd() + "%' AND L.[PrimaryAssociationID] = " + uSession.AssocID + " order by " + order + "";

                        }
                        else if (name == null && lname != null && value1 == null)
                        {
                            Session["condition"] = " AND L1.[LastName] like '%" + lname.TrimStart().TrimEnd() + "%'AND L.[PrimaryAssociationID] = " + uSession.AssocID + " order by " + order + " ";

                        }
                        else
                        {

                            Session["condition"] = "AND L.[OfficeId] IN (SELECT DISTINCT [OfficeId] FROM [office_details] WHERE [OfficeContactDR]= " + value1 + " ) AND L.[PrimaryAssociationID] = " + uSession.AssocID + "  order by " + order + "";
                        }
                    }
                    else
                    {

                        if (value1 != null && name != null && lname != null)
                        {
                            Session["condition"] = " AND L.[OfficeId] IN (SELECT DISTINCT [OfficeId] FROM [office_details] WHERE [OfficeContactDR]= " + value1 + " ) and L1.[FirstName] like '%" + name.TrimStart().TrimEnd() + "%' And L1.[LastName] like '%" + lname.TrimStart().TrimEnd() + "%' order by " + order + "";

                        }
                        else if (name != null && lname != null && value1 == null)
                        {
                            Session["condition"] = "  and L1.[FirstName] like '%" + name.TrimStart().TrimEnd() + "%' And L1.[LastName] like '%" + lname.TrimStart().TrimEnd() + "%' order by " + order + "  ";

                        }
                        else if (name != null && lname == null && value1 == null)
                        {
                            Session["condition"] = " AND  L1.[FirstName]   like '%" + name.TrimStart().TrimEnd() + "%' order by " + order + "";

                        }
                        else if (name == null && lname != null && value1 == null)
                        {
                            Session["condition"] = " AND L1.[LastName] like '%" + lname.TrimStart().TrimEnd() + "%' order by " + order + " ";

                        }
                        else
                        {

                            Session["condition"] = "AND L.[OfficeId] IN (SELECT DISTINCT [OfficeId] FROM [office_details] WHERE [OfficeContactDR]= " + value1 + " )  order by " + order + "";
                        }
                    }
                }
                if (type == "3")
                {
                    //memberid, agentfname, agentLname, membertype
                    Session["OfficeId"] = type;
                    if (uSession.AssocID != 0)
                    {
                        string qry = "";
                        if (memberid != null)
                        {
                            qry += " and L.[MemberId]=" + memberid.TrimEnd().TrimStart();
                        }
                        if (agentfname != null)
                        {
                            qry += " and L.[FirstName]=" + agentfname.TrimEnd().TrimStart();
                        }
                        if (agentLname != null)
                        {
                            qry += " and L.[LastName]=" + agentLname.TrimEnd().TrimStart();
                        }
                        if (membertype != null)
                        {
                            qry += " and L.[MemberType]=" + membertype.TrimEnd().TrimStart();
                        }


                        qry += "  AND L.[PrimaryAssociationID] = " + uSession.AssocID + "  order by " + order + "";
                        Session["condition"] = qry;

                    }
                    else
                    {
                        string qry = "";
                        if (memberid != null)
                        {
                            qry += " and L.[MemberId]=" + memberid.TrimEnd().TrimStart();
                        }
                        if (agentfname != null)
                        {
                            qry += " and L.[FirstName]='" + agentfname.TrimEnd().TrimStart() + "'";
                        }
                        if (agentLname != null)
                        {
                            qry += " and L.[LastName]='" + agentLname.TrimEnd().TrimStart() + "'";
                        }
                        if (membertype != null)
                        {
                            qry += " and L.[MemberType]='" + membertype.TrimEnd().TrimStart() + "'";
                        }


                        // qry += " order by " + order + "";
                        Session["condition"] = qry;

                    }
                }
            }


            return new JsonResult
            {
                Data = Url.Action("AgentsList", "AgentsList", new { start = start, pagesize = pagesize, order = order }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetOffice(string term, int type)
        {
            AgentsModel agentsModel = new AgentsModel();
            AgentsDAL agentsDAL = new AgentsDAL();
            List<AgentsModel> lstAgentsModel = new List<AgentsModel>();
            if (term.Length > 0)
            {
                lstAgentsModel = agentsDAL.getOfficeList(term, type);
            }
            return new JsonResult
            {
                Data = lstAgentsModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetBroker(string term, int type)
        {
            AgentsModel agentsModel = new AgentsModel();
            AgentsDAL agentsDAL = new AgentsDAL();
            List<AgentsModel> lstAgentsModel = new List<AgentsModel>();
            if (term.Length > 0)
            {
                lstAgentsModel = agentsDAL.getBrokerList(term, type);
            }
            return new JsonResult
            {
                Data = lstAgentsModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public ActionResult AgentDetails()
        {
            AgentDetailsModel agentDetailModel = new AgentDetailsModel();
            AgentsDAL agentsDAL = new AgentsDAL();
            UserSession userSession = new UserSession();
            int broker_Id = 0;
            string filter1 = string.Empty;
            try
            {
                if (Request.QueryString["MemberId"] != null)
                {
                    broker_Id = Convert.ToInt32(Request.QueryString["MemberId"].ToString());

                    using (StreamReader reader = new StreamReader(Server.MapPath("~/HTMLTemplates/_PrintAgentDetails.html")))
                    {
                        agentDetailModel = agentsDAL.getAgentDetails(broker_Id, reader.ReadToEnd());
                    }

                    TempData["AgentsList"] = agentDetailModel.dt;
                    return View(agentDetailModel);
                }
            }
            catch
            {
                agentsDAL = null;
                GC.Collect();
            }

            return View(agentDetailModel);
        }

        public ActionResult AgentDetailsPopup()
        {
            AgentDetailsModel agentDetailModel = new AgentDetailsModel();
            AgentsDAL agentsDAL = new AgentsDAL();
            UserSession userSession = new UserSession();
            int broker_Id = 0;
            string filter1 = string.Empty;
            try
            {
                if (Request.QueryString["MemberId"] != null)
                {
                    broker_Id = Convert.ToInt32(Request.QueryString["MemberId"].ToString());

                    using (StreamReader reader = new StreamReader(Server.MapPath("~/HTMLTemplates/_PrintAgentDetails.html")))
                    {
                        agentDetailModel = agentsDAL.getAgentDetails(broker_Id, reader.ReadToEnd());
                    }

                    TempData["AgentsList"] = agentDetailModel.dt;
                    return View(agentDetailModel);
                }
            }
            catch
            {
                agentsDAL = null;
                GC.Collect();
            }

            return View(agentDetailModel);
        }
    }
}
