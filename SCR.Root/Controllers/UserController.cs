using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using SCR.Root.Models;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using SCR.Root.App_Code;

namespace SCR.Root.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /AddEditUser/

        public const string MatchEmailPattern =
    @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        bool isValidEmail = false;
        public ActionResult Add(int id = 0)
        {
            UserSession userSession = new UserSession();
            if (userSession.Exists)
            {
                UserModel userModel = new UserModel();
                UserDAL userDAL = new UserDAL();
                userModel.UserRole = userDAL.FillUserRoles();
                userModel.LocalBoard = userDAL.FillLocalBoard();
                if (id > 0)
                {
                    ViewData["type1"] = "edit";
                    var lstUserModel = userDAL.UserEdit(id);
                    lstUserModel.LocalBoard = userModel.LocalBoard;
                    lstUserModel.UserRole = userModel.UserRole;
                    return View(lstUserModel);
                }
                else
                {

                    return View(userModel);
                }
               
            }
            else
            {
                TempData["expires"] = "true";
                return RedirectToAction("Users", "Login");
            }
        }
        public static bool IsEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add(UserModel userModel)
        {
            UserDAL userDAL = new UserDAL();
            if (ModelState.IsValid)
            {
                 UserSession userSession = new UserSession();
                 if (userSession.Exists)
                 {
                     int retnVal = 0;
                     bool isValid = true;
                     bool returnValue = false;
                     bool lstUserModel = false;
                     try
                     {
                         if (userModel.Email != "")
                         {
                             string email = userModel.Email.Trim();
                             isValidEmail = IsEmail(email);
                             if (!isValidEmail)
                             {
                                 TempData["error"] = "Email is not correct!";
                                 TempData["errtitle"] = "User";
                                 isValid = false;
                                 return RedirectToAction("Add", "User");
                             }
                         }
                         if (userModel.LoginId == 0)
                             lstUserModel = userDAL.UserEmailCheck(userModel.Email);
                         if (userModel.RoleId == 1)
                         {
                             userModel.AssocID = 0;
                         }
                         userModel.Password = RandomPassword.Generate();
                         if (lstUserModel == false)
                         {
                             returnValue = userDAL.AddUserProfileDetails(userModel);
                             if (returnValue == true)
                             {
                                 if (userModel.LoginId == 0)
                                 {
                                     TempData["error"] = "User Created Succesfully";
                                     TempData["errtitle"] = "User";
                                     return RedirectToAction("List", "User");
                                 }
                                 else
                                 {
                                     TempData["error"] = "User Updated Succesfully";
                                     TempData["errtitle"] = "User";
                                     return RedirectToAction("List", "User");
                                 }
                             }
                         }
                         else
                         {
                             TempData["error"] = "Email Already Exist";
                             TempData["errtitle"] = "User";
                             return RedirectToAction("Add", "User");
                         }
                     }
                     catch (Exception ex)
                     {
                         throw ex;
                     }
                     userDAL = null;
                     userModel = null;
                     GC.Collect();
                 }
                 else
                 {
                     TempData["expires"] = "true";
                     return RedirectToAction("Users", "Login");
                 }
            }
            else
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                return View(userModel);
            }
            return View(userModel);
        }
        //public static bool IsEmail(string email)
        //{
        //    if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
        //    else return false;
        //}
        public ActionResult List(int id = 0)
        {
            UserModel userModel = new UserModel();
            UserDAL userDAL = new UserDAL();
          
            UserSession userSession = new UserSession();
            try
            {
                if (userSession.Exists)
                {
                    if (id != 0)
                    {
                        userDAL.deleteUser(id);
                        TempData["error"] = "User details deleted successfully.";
                        TempData["errtitle"] = "Local Board";
                        TempData["errType"] = "success";
                         
                    }

                    var UserModelList1 = userDAL.getAllUsersList();
                    return View(UserModelList1);
                   // return RedirectToAction("List", "User");
                }
            }
            catch (Exception ex)
            {
                userDAL = null;
                GC.Collect();
            }

            return View(userModel);
        }

    }
}
