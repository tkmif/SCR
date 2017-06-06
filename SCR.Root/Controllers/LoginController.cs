using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCR.Root.Models;
using SCR.Root.App_Code;
using wyDay.TurboActivate;



namespace SCR.Root.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
       // readonly TurboActivate ta;
        TurboActivate ta;
        bool isGenuine;
        // Set the trial flags you want to use. Here we've selected that the
        // trial data should be stored system-wide (TA_SYSTEM) and that we should
        // use un-resetable verified trials (TA_VERIFIED_TRIAL).
        readonly TA_Flags trialFlags = TA_Flags.TA_SYSTEM | TA_Flags.TA_VERIFIED_TRIAL;

        // Don't use 0 for either of these values.
        // We recommend 90, 14. But if you want to lower the values
        // we don't recommend going below 7 days for each value.
        // Anything lower and you're just punishing legit users.
        const uint DaysBetweenChecks = 90;
        const uint GracePeriodLength = 14;

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

        //public int validate()
        //{ 
        //    try
        //    {
        //        //TODO: goto the version page at LimeLM and paste this GUID here
        //        ta = new TurboActivate("66be3546589ab508282122.91204848");

        //        // Check if we're activated, and every 90 days verify it with the activation servers
        //        // In this example we won't show an error if the activation was done offline
        //        // (see the 3rd parameter of the IsGenuine() function)
        //        // https://wyday.com/limelm/help/offline-activation/
        //        IsGenuineResult gr = ta.IsGenuine(DaysBetweenChecks, GracePeriodLength, true);

        //        isGenuine = gr == IsGenuineResult.Genuine ||
        //                    gr == IsGenuineResult.GenuineFeaturesChanged ||

        //                    // an internet error means the user is activated but
        //            // TurboActivate failed to contact the LimeLM servers
        //                    gr == IsGenuineResult.InternetError;


        //        // If IsGenuineEx() is telling us we're not activated
        //        // but the IsActivated() function is telling us that the activation
        //        // data on the computer is valid (i.e. the crypto-signed-fingerprint matches the computer)
        //        // then that means that the customer has passed the grace period and they must re-verify
        //        // with the servers to continue to use your app.

        //        //Note: DO NOT allow the customer to just continue to use your app indefinitely with absolutely
        //        //      no reverification with the servers. If you want to do that then don't use IsGenuine() or
        //        //      IsGenuineEx() at all -- just use IsActivated().
        //        if (!isGenuine && ta.IsActivated())
        //        {
        //            // We're treating the customer as is if they aren't activated, so they can't use your app.

        //            // However, we show them a dialog where they can reverify with the servers immediately.

        //           // ReVerifyNow frmReverify = new ReVerifyNow(ta, DaysBetweenChecks, GracePeriodLength);

        //            //if (frmReverify.ShowDialog(this) == DialogResult.OK)
        //            //{
        //                isGenuine = true;
        //           // }
        //            //else if (!frmReverify.noLongerActivated) // the user clicked cancel and the user is still activated
        //            //{
        //                // Just bail out of your app
        //             //   Environment.Exit(1);
        //                return 0;
        //           // }
        //        }
        //    }
        //    catch (TurboActivateException ex)
        //    {
        //        // failed to check if activated, meaning the customer screwed
        //        // something up so kill the app immediately
        //      //  MessageBox.Show("Failed to check if activated: " + ex.Message);
        //      //  Environment.Exit(1);
        //        return 1;
        //    }

        //    return ShowTrial(!isGenuine);

        //    // If this app is activated then you can get custom license fields.
        //    // See: https://wyday.com/limelm/help/license-features/
        //    /*
        //    if (isGenuine)
        //    {
        //        string featureValue = ta.GetFeatureValue("your feature name");

        //        //TODO: do something with the featureValue
        //    }
        //    */
        //    //return 2;
        //}

        //int ShowTrial(bool show)
        //{
        //  //  lblTrialMessage.Visible = show;
        //  //  btnExtendTrial.Visible = show;

        ////    mnuActDeact.Text = show ? "Activate..." : "Deactivate";

        //    if (show)
        //    {
        //        uint trialDaysRemaining = 0;

        //        try
        //        {
        //            ta.UseTrial(trialFlags);

        //            // get the number of remaining trial days
        //            trialDaysRemaining = ta.TrialDaysRemaining(trialFlags);
        //        }
        //        catch (TurboActivateException ex)
        //        {
        //           // MessageBox.Show("Failed to start the trial: " + ex.Message);
        //        }

        //        // if no more trial days then disable all app features
        //        if (trialDaysRemaining == 0)
        //            // DisableAppFeatures();
        //            return 1;
        //       // else
                   
        //          //  lblTrialMessage.Text = "Your trial expires in " + trialDaysRemaining + " days.";
        //    }

        //         return 0;
        //}
        [HttpPost]
        public ActionResult Users(LoginModel loginModel, string command)
        {
            LoginDAL loginDAL = new LoginDAL();
            UserModel userModel = new UserModel();
            UserDAL userDAL = new UserDAL();
            validate obj = new validate();
            
            UserSession userSession = new UserSession();          
            if (ModelState.IsValid)
            {
               
                    try
                    {
                        int val = obj.check();// validate();
                        if (val > 0)
                        {
                            Session["error"] = "TRIAL EXPIRED PLEASE CONTACT ADMIN";
                            TempData["error"] = "TRIAL EXPIRED PLEASE CONTACT ADMIN";
                            TempData["errtitle"] = "Login";
                            TempData.Keep("error");
                            TempData.Keep("errtitle");
                            //TempData.Peek("error");
                            //TempData.Peek("errtitle");
                           // return RedirectToAction("Users", "Login");
                            return View();
                        }
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
                        TempData["error"] = ex.Message.ToString();
                        TempData["errtitle"] = "Login";
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
