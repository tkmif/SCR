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
    public class LocalOfficeController : Controller
    {
        //
        // GET: /LocalOffice/

        public const string MatchEmailPattern =
         @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
          + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				        [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
          + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				        [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
         + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        public ActionResult Add(int id = 0)
        {
            LocalOfficeModel localOfficeModel = new LocalOfficeModel();
            LocalOfficeDAL localOfficeDAL = new LocalOfficeDAL();
         
             try
             {
                 if (id > 0)
                 {
                     ViewData["type1"] = "edit";
                    var objLocalOfficeModel = localOfficeDAL.getLocalOfficeForEdit(id);
                    return View(objLocalOfficeModel);
                 }
             }
             catch (Exception ex)
             {
                 localOfficeDAL = null;
                 GC.Collect();
             }
             return View(localOfficeModel);
           
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add(LocalOfficeModel localOfficeModel)
        {
            LocalOfficeDAL localOfficeDAL =new LocalOfficeDAL();
            UserSession userSession = new UserSession();

           // bool isValid = true;
            int returVal = 0;
            // int type= 0;
            if (ModelState.IsValid)
            {
                if (userSession.Exists)
                {
                    try
                    {

                        //if (localOfficeModel.Password != localOfficeModel.ConfirmPassword)
                        //{
                        //    isValid = false;
                        //    TempData["error"] = "Confirm Password is not matching with Password";
                        //    TempData["errtitle"] = "Local Office";
                        //    return RedirectToAction("Add", "LocalOffice");
                        //}
                        //if (IsEmail(localOfficeModel.Email))
                        //{
                        //    isValid = true;
                        //}
                        //else
                        //{
                        //    isValid = false;
                        //}
                        //if (isValid)
                        //{
                        //    localOfficeModel.RoleId = 2;
                        //    if (localOfficeModel.LocalOfficeId!=0)
                        //     {
                        //         type=1;
                        //     }
                        //     else
                        //     {
                        //          type=0;
                        //     }
                            returVal = localOfficeDAL.addLocalOffice(localOfficeModel);

                            if (returVal != 0)
                            {
                               //if (localOfficeModel.LocalOfficeId == 0 && localOfficeModel.LoginId == 0){
                                if (localOfficeModel.LocalOfficeId == 0 )
                                {
                                    TempData["error"] = "Local Office details added successfully.";
                                    TempData["errtitle"] = "Local Office";
                                    TempData["errType"] = "success";
                                    return RedirectToAction("List", "LocalOffice");
                                }
                                else
                                {
                                    TempData["error"] = "Local Office details updated successfully.";
                                    TempData["errtitle"] = "Local Office";
                                    TempData["errType"] = "success";
                                    return RedirectToAction("List", "LocalOffice");
                                }
                            }
                                //}

                    }
                    catch (Exception ex)
                    {
                    }
                }
            
            }
            return View();
        }

        public ActionResult List(int id = 0)
        {
            LocalOfficeModel localOfficeModel = new LocalOfficeModel();
            LocalOfficeDAL localOfficeDAL = new LocalOfficeDAL();
             UserSession userSession = new UserSession();
             try
             {
                 if (userSession.Exists)
                 {
                     if (id != 0)
                     {
                         localOfficeDAL.deleteLocalOffice(id);
                         TempData["error"] = "Local Office details deleted successfully.";
                         TempData["errtitle"] = "Local Office";
                         TempData["errType"] = "success";
                         // return RedirectToAction("List", "LocalOffice");
                     }

                     localOfficeModel.LocalOfficeModelList = localOfficeDAL.getAllLocalOfficeList();
                     return View(localOfficeModel);
                 }
             }
             catch (Exception ex)
             {
                 localOfficeDAL = null;
                 GC.Collect();
             }
            
            return View(localOfficeModel);

        }
        /// <summary>
        /// To check whether password is valid.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsValidPassword(string str)
        {
            var isvalid = false;
            int errorCounter = 0;
            int errorCounternum = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 'a' && str[i] <= 'z' || str[i] >= 'A' && str[i] <= 'Z')
                {
                    errorCounter++;
                }
                if (str[i] >= '0' && str[i] <= '9')
                {
                    errorCounternum++;
                }
            }
            if (errorCounter > 0 && errorCounternum > 0)
            {
                isvalid = true;
            }
            else
            {
                isvalid = false;
            }
            return isvalid;
        }
        public static bool IsEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }
    }
}
