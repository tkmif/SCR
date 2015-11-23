using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCR.Root.Models;
using SCR.Root.App_Code;

namespace SCR.Root.Controllers
{
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ChangePassword(SettingsModel settingsModel)
        {
            UserSession userSession = new UserSession();
            if (ModelState.IsValid)
            {
                if (userSession.Exists)
                {
                    SettingsDAL settingsDAL = new SettingsDAL();
                    try
                    {
                        bool correctPassword = false;
                        bool correctOldPassword = false;
                        int LoginUser = userSession.LoginId;

                        if (settingsModel.OldPassword != "" || settingsModel.OldPassword != null)
                        {

                            string boolCheckPwd = settingsDAL.checkPasswordForlogin(LoginUser);
                            if (boolCheckPwd != settingsModel.OldPassword)
                            {
                                correctOldPassword = false;
                                TempData["error"] = "Old Password is not Correct";
                                TempData["errtitle"] = "Login";
                                return RedirectToAction("ChangePassword", "Settings");
                            }
                            else
                            {
                                correctOldPassword = true;
                            }
                        }
                        if (settingsModel.NewPassword != settingsModel.ConfirmPassword)
                        {
                            correctPassword = false;
                            TempData["error"] = "Confirm Password Not Matching with New Password";
                            TempData["errtitle"] = "Login";
                            return RedirectToAction("ChangePassword", "Settings");
                        }
                        else
                        {
                            correctPassword = true;
                        }
                        if (correctOldPassword == true && correctPassword == true)
                        {
                            bool boolUpdate = settingsDAL.upadatePassword(settingsModel, LoginUser);
                            if (boolUpdate == true)
                            {
                                settingsDAL = null;
                                GC.Collect();
                                TempData["error"] = "Password Changed Succesfully";
                                TempData["errtitle"] = "Login";
                                return RedirectToAction("List", "LocalBoard");
                            }
                            else
                            {
                                TempData["error"] = "Process Failed";
                                TempData["errtitle"] = "Login";
                                return RedirectToAction("Users", "Login");
                            }
                        }

                        return View();
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
                else
                {

                    return View();
                }
            }
            else
            {
                TempData["expires"] = "true";
                return RedirectToAction("Users", "Login");
            }
        }

        public ActionResult UserChangePassword()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UserChangePassword(SettingsModel settingsModel)
        {
            UserSession userSession = new UserSession();
            if (ModelState.IsValid)
            {
                if (userSession.Exists)
                {
                    SettingsDAL settingsDAL = new SettingsDAL();
                    try
                    {
                        bool correctPassword = false;
                        bool correctOldPassword = false;
                        int LoginUser = userSession.LoginId;

                        if (settingsModel.OldPassword != "" || settingsModel.OldPassword != null)
                        {

                            string boolCheckPwd = settingsDAL.checkPasswordForlogin(LoginUser);
                            if (boolCheckPwd != settingsModel.OldPassword)
                            {
                                correctOldPassword = false;
                                TempData["error"] = "Old Password is not Correct";
                                TempData["errtitle"] = "Login";
                                return RedirectToAction("ChangePassword", "Settings");
                            }
                            else
                            {
                                correctOldPassword = true;
                            }
                        }
                        if (settingsModel.NewPassword != settingsModel.ConfirmPassword)
                        {
                            correctPassword = false;
                            TempData["error"] = "Confirm Password Not Matching with New Password";
                            TempData["errtitle"] = "Login";
                            return RedirectToAction("ChangePassword", "Settings");
                        }
                        else
                        {
                            correctPassword = true;
                        }
                        if (correctOldPassword == true && correctPassword == true)
                        {
                            bool boolUpdate = settingsDAL.upadatePassword(settingsModel, LoginUser);
                            if (boolUpdate == true)
                            {
                                settingsDAL = null;
                                GC.Collect();
                                TempData["error"] = "Password Changed Succesfully";
                                TempData["errtitle"] = "Login";
                                return RedirectToAction("List", "LocalBoard");
                            }
                            else
                            {
                                TempData["error"] = "Process Failed";
                                TempData["errtitle"] = "Login";
                                return RedirectToAction("Users", "Login");
                            }
                        }

                        return View();
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
                else
                {
                    TempData["expires"] = "true";
                    return RedirectToAction("Users", "Login");
                }
            }
            else
            {

                return View();
            }
        }
    }
}
