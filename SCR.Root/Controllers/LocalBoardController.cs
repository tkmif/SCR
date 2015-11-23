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

namespace SCR.Root.Controllers
{
    public class LocalBoardController : Controller
    {
        //
        // GET: /LocalBoard/

        public ActionResult Add(int id = 0)
        {
            LocalBoardModel localBoardModel = new LocalBoardModel();
            LocalBoardDAL localBoardDAL = new LocalBoardDAL();
            UserSession userSession = new UserSession();       
            if (userSession.Exists)
            {
                try
                {
                    if (id > 0)
                    {
                        ViewData["types"] = "edit";
                        var objLocalOfficeModel = localBoardDAL.getLocalBoardForEdit(id);
                        return View(objLocalOfficeModel);
                    }
                }
                catch (Exception ex)
                {
                    localBoardDAL = null;
                    GC.Collect();
                }
                return View(localBoardModel);
            }
            else
            {
                TempData["expires"] = "true";
                return RedirectToAction("Users", "Login");
            }
          
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Add(LocalBoardModel localBoardModel)
        {
            LocalBoardDAL localBoardDAL = new LocalBoardDAL();
            UserSession userSession = new UserSession();          
            int returVal = 0;
            int returVal1 = 0;    
            if (ModelState.IsValid)
            {
                if (userSession.Exists)
                {
                    try
                    {
                        if (ViewData["types"] == "edit" || localBoardModel.StatusEdit == "edit")
                        {
                            returVal = localBoardDAL.updateLocalBoard(localBoardModel);
                            TempData["error"] = "Local Board details updated successfully.";
                            TempData["errtitle"] = "Local Board";
                            TempData["errType"] = "success";
                            return RedirectToAction("List", "LocalBoard");
                        }
                        returVal = localBoardDAL.addLocalBoard(localBoardModel);

                        if (returVal != 0)
                        {                           
                                TempData["error"] = "Local Board details added successfully.";
                                TempData["errtitle"] = "Local Board";
                                TempData["errType"] = "success";
                                return RedirectToAction("List", "LocalBoard");
                           
                        }
                        //}

                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    TempData["expires"] = "true";
                    return RedirectToAction("Users", "Login");
                }

            }
            localBoardModel.StatusEdit = null;
            return View();
        }
        public ActionResult List(int id = 0)
        {
            LocalBoardModel localBoardModel = new LocalBoardModel();
            LocalBoardDAL localBoardDAL = new LocalBoardDAL();
            UserSession userSession = new UserSession();
            if (userSession.Exists)
            {
                try
                {

                    if (id != 0)
                    {
                        localBoardDAL.deleteLocalBoard(id);
                        TempData["error"] = "Local Board details deleted successfully.";
                        TempData["errtitle"] = "Local Board";
                        TempData["errType"] = "success";
                        // return RedirectToAction("List", "LocalOffice");
                    }

                    localBoardModel.LocalBoardModelList = localBoardDAL.getAllLocalBoardList();
                    return View(localBoardModel);

                }
                catch (Exception ex)
                {
                    localBoardDAL = null;
                    GC.Collect();
                }

                return View(localBoardModel);

            }
            else
            {
                TempData["expires"] = "true";
                return RedirectToAction("Users", "Login");
            }
        }

    }
}
