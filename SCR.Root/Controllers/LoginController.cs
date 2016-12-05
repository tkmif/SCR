using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCR.Root.Models;
using SCR.Root.App_Code;

namespace SCR.Root.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Users()
        {
              UserSession userSession = new UserSession();
              if (!userSession.Exists)
              {
                  if(Convert.ToString(TempData["expires"]) == "true")
                  {

                  TempData["error"] = "Your session expired";
                  TempData["errtitle"] = "Login";
                  TempData["expires"] = "false";
                  }
              }
            return View();
        }
        [HttpPost]
        public ActionResult Users(LoginModel loginModel, string command)
        {
            LoginDAL loginDAL = new LoginDAL();
            UserModel userModel = new UserModel();
            UserDAL userDAL = new UserDAL();
            UserSession userSession = new UserSession();          
            if (ModelState.IsValid)
            {
               
                    try
                    {
                        var boolres = loginDAL.IsValidUser(loginModel.EmailId.Trim(), loginModel.Password);
                        loginDAL.getUserRolePagePrivilege(loginModel.EmailId.Trim());
                        if (boolres.Count > 0)
                        {
                            if (boolres[0].Status == -1)
                            {
                                return RedirectToAction("UserChangePassword", "Settings");
                            }
                            if (boolres[0].Status == 1)
                            {

                                return RedirectToAction("MemberExpiryReport", "MemberExpiryReport");
                            }
                        }
                        else
                        {
                            TempData["error"] = "Not Registered User";
                            TempData["errtitle"] = "Login";
                            return RedirectToAction("Users", "Login");
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
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(LoginModel loginModel)
        {
            LoginDAL loginDAL = new LoginDAL();

            if (ModelState.IsValid)
            {
                bool returnValue = false;
                bool lstUserModel = loginDAL.UserEmailCheck(loginModel.EmailIdForgot);
                if (lstUserModel == true)
                {
                    Guid id = Guid.NewGuid();
                    loginModel.Key = Convert.ToString(id);
                    DateTime date = System.DateTime.Now;
                    loginModel.ExpiredOn = date.AddMinutes(30);
                    returnValue = loginDAL.ForgotPwd(loginModel);
                    if (returnValue == true)
                    {
                        TempData["error"] = "We have emailed you a link to change your password.";
                        TempData["errtitle"] = "Login";
                        return RedirectToAction("Users", "Login");
                    }
                    else
                    {
                        TempData["error"] = "Process Failed";
                        TempData["errtitle"] = "Login";
                        return RedirectToAction("Users", "Login");
                    }
                }
                else
                {
                    TempData["error"] = "This Email is not registered";
                    TempData["errtitle"] = "Login";
                    return RedirectToAction("Users", "Login");
                  //  return View();
                }
            }
            else
            {
                return View();
            }
            return View();
        }

        /// <summary>
        /// For log out the current login User 
        /// </summary>
        /// <returns></returns>
        #region UserLogout
        public ActionResult Logout()
        {
            UserSession userSession = new UserSession();
            userSession.EmptyUserSession();
            return RedirectToAction("Users", "Login");
        }
        #endregion UserLogout

        public ActionResult ChangePassword()
        {
            LoginDAL loginDAL = new LoginDAL();
            LoginModel loginModel = new LoginModel();

            if (ModelState.IsValid)
            {
                try
                {
                    var id = ControllerContext.RouteData.Values["id"];
                    string key = Convert.ToString(id);
                    var boolres = loginDAL.IsValidKey(key);
                    DateTime nowTime = System.DateTime.Now;
                    if (boolres.Count > 0)
                    {
                        if (boolres[0].ExpiredOn >= nowTime && boolres[0].ForgotStatus == 1)
                        {
                            loginModel.EmailId = boolres[0].EmailId;
                            return View(loginModel);
                        }
                        else
                        {
                            TempData["error"] = "Your Credential details is not Matching";
                            TempData["errtitle"] = "Login";
                            return RedirectToAction("Users", "Login");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Your Credential details is not Matching";
                        TempData["errtitle"] = "Login";
                        return RedirectToAction("Users", "Login");
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    return View();
                }

            }
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(LoginModel loginModel)
        {
            LoginDAL loginDAL = new LoginDAL();

            if (ModelState.IsValid)
            {
                try
                {
                    bool returnValue = false;
                    bool correctPassword = false;
                    if (loginModel.NewPassword != loginModel.ConfirmPassword)
                    {
                        correctPassword = false;
                        TempData["error"] = "Confirm Password Not Matching with New Password";
                        TempData["errtitle"] = "Login";
                        return View();
                    }
                    else
                    {
                        correctPassword = true;
                    }
                    if (correctPassword == true)
                    {
                        returnValue = loginDAL.UpdatePasswordDetails(loginModel);
                        if (returnValue == true)
                        {
                            TempData["error"] = "Password Updated Succesfully";
                            TempData["errtitle"] = "Login";
                            return RedirectToAction("Users", "Login");
                        }
                        else
                        {
                            TempData["error"] = "Process can't Complete Successfully";
                            TempData["errtitle"] = "User";
                            return RedirectToAction("List", "User");
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View();
        }
    }
}
