using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using SCR.Root.Models;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.Net;
using System.Data;
using System.Drawing;
//using SCR.Root.RandomPassword;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Configuration;
using System.IO;

namespace SCR.Root.Controllers
{
    public class AddEditUserController : Controller
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

       // bool isValidEmail = false;
        public ActionResult Add(int id = 0)
        {
            AddEditUserModel addEditUserModel = new AddEditUserModel();
            AddEditUserDAL addEditUserDAL = new AddEditUserDAL();
            addEditUserModel.UserRole = addEditUserDAL.FillUserRoles();
            return View();
        }
        public static bool IsEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }
        private void Alert(Control control, string Message)
        {
            string jscript = "alert('" + Message + "');";
            ScriptManager.RegisterStartupScript(control, (typeof(System.Web.UI.Page)), "Startup", jscript, true);
        }
        //protected void lbtnCreateUser_Click(object sender, EventArgs e)
        //{
        //    DataSet dsmailcheck = new DataSet();
        //    bool isValid = true;
        //    string Email = txtEmailAddress.Text;
        //    dsmailcheck = UserBLL.user_mail_check(Email);
        //    string fileName1 = fleupProfilePhoto.PostedFile.FileName;
        //    var extension = Path.GetExtension(fileName1);
        //    var allowedExtensions = new[] { ".JPG", ".jpeg", ".png", ".jpg", ".JPEG" };
        //    if (txtEmailAddress.Text != "")
        //    {
        //        string email = txtEmailAddress.Text.Trim();
        //        isValidEmail = IsEmail(email);
        //        if (!isValidEmail)
        //        {
        //            Alert(this.txtEmailAddress, "Email is not correct!");
        //            isValid = false;
        //        }

        //    }
        //    if (Convert.ToInt32(Session["UserId"]) == 0 && dsmailcheck.Tables.Count > 0)
        //    {
        //        txtEmailAddress.Text = "";
        //        Session["UserId"] = null;
        //        Alert(this.txtEmailAddress, "Your Email is already registered!");
        //        isValid = false;
        //    }
        //    if (extension != "")
        //    {
        //        if (!allowedExtensions.Contains(extension))
        //        {
        //            Session["UserId"] = null;
        //            Alert(this.fleupProfilePhoto, "Attachment extensions should be pdf JPG, jpeg, png");
        //            isValid = false;
        //        }
        //    }
        //    //else
        //    //{
        //    if (isValid == true)
        //    {
        //        UserTemporaryPasswordBOL objTempPasswordBOL = new UserTemporaryPasswordBOL();
        //        UserTemporaryPasswordBLL objTempPasswordBLL = new UserTemporaryPasswordBLL();
        //        _UserSession userSession = new _UserSession();
        //        var user_Role = userSession.GetUserRoles.FirstOrDefault();
        //        if (user_Role != null)
        //        {
        //            if (user_Role.RoleName == "Owner" || user_Role.RoleName == "Administrator")
        //            {
        //                using (TransactionScope transaction_scope = new TransactionScope())
        //                {
        //                    /*if (Convert.ToInt32(Session["UserId"]) > 0)
        //                    objUserBOL.IntUserId = Convert.ToInt32(Session["UserId"]);
        //                    Session["UserId"] = null;*/
        //                    objUserBOL.StrName = txtFirstName.Text;
        //                    objUserBOL.StrLastName = txtLastName.Text;
        //                    objUserBOL.StrOfficePhoneNo = txtOfficePhoneNumber.Text;
        //                    objUserBOL.StrMobileNo = txtMobilePhoneNumber.Text;
        //                    objUserBOL.StrEmail = txtEmailAddress.Text;
        //                    objUserBOL.IntRoleId = Convert.ToInt32(ddlUserRole.SelectedValue);
        //                    objUserBOL.StrPassword = RandomPassword.Generate();

        //                    objTempPasswordBOL.StrTempPassword = objUserBOL.StrPassword;
        //                    objTempPasswordBOL.CreatedOn = DateTime.Now;
        //                    objTempPasswordBOL.Status = -1;

        //                    int retval = objUserBLL.insert_new_user(objUserBOL);


        //                    string cfPath;
        //                    string filename = Path.GetFileName(fleupProfilePhoto.PostedFile.FileName);
        //                    string profilePicName = Convert.ToString(retval) + "_" + objUserBOL.StrName + "_" + filename;
        //                    if (!Directory.Exists(Server.MapPath("../Images/ProfilePic/")))
        //                    {
        //                        Directory.CreateDirectory(Server.MapPath("../Images/ProfilePic/"));
        //                    }

        //                    fleupProfilePhoto.SaveAs(Server.MapPath("../Images/ProfilePic/" + profilePicName));
        //                    cfPath = "~/Images/ProfilePic/" + profilePicName;
        //                    if (fleupProfilePhoto.PostedFile.FileName != null && fleupProfilePhoto.PostedFile.FileName != "")
        //                    {
        //                        objUserBOL.StrImageType = fleupProfilePhoto.PostedFile.ContentType;
        //                        objUserBOL.StrImagePath = cfPath;
        //                        objUserBOL.StrImageLength = Convert.ToString(fleupProfilePhoto.PostedFile.ContentLength);
        //                    }
        //                    else
        //                    {
        //                        objUserBOL.StrImageType = "jpg";
        //                        objUserBOL.StrImagePath = "~/Images/MyPortal/userPic.jpg";
        //                        objUserBOL.StrImageLength = null;
        //                    }
        //                    if (retval > 0)
        //                    {
        //                        objTempPasswordBOL.IntUserId = retval;
        //                        objUserBOL.IntUserId = retval;
        //                        int returnval = objUserBLL.update_user_profile_details(objUserBOL);
        //                        int ret = objTempPasswordBLL.save_user_temporary_password(objTempPasswordBOL);//To save temporary password
        //                    }

        //                    // Address from where you send the mail
        //                    // var fromAddress = "geethanjali@tkminfotech.com";
        //                    string fromAddress = System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString();

        //                    // any address where the email will be sending
        //                    var toAddress = objUserBOL.StrEmail;
        //                    //Password of your from email address
        //                    //  const string fromPassword = "Gee@123!@#*";
        //                    string fromPassword = System.Configuration.ConfigurationManager.AppSettings["FromPassword"].ToString();
        //                    // Passing the values and make a email formate to display
        //                    string subject = "User Credentials";
        //                    string body = "Hi " + objUserBOL.StrName + ",\n\n";

        //                    body += "Welcome to myPortal! \n";

        //                    if (!string.IsNullOrEmpty(objUserBOL.StrPassword))
        //                    {
        //                        string mail_url = System.Configuration.ConfigurationManager.AppSettings["MailUrl"].ToString();
        //                        body += "Please log in to ";
        //                        body += mail_url;
        //                        body += ".\n";
        //                        body += "Your myPortal Username is your Portal 724 E-mail address.\n";
        //                        body += "Your temporary myPortal Password is: " + objUserBOL.StrPassword + "\n";
        //                    }



        //                    body += "\n";
        //                    body += "Thank you, \nPortal 724\n";
        //                    // smtp settings
        //                    var smtp = new System.Net.Mail.SmtpClient();
        //                    {
        //                        // smtp.Host = "mail.tkminfotech.com";
        //                        smtp.Host = System.Configuration.ConfigurationManager.AppSettings["MailServer"].ToString();
        //                        smtp.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
        //                        smtp.EnableSsl = false;
        //                        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //                        smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
        //                        smtp.Timeout = 20000;
        //                    }
        //                    // Passing values to smtp object
        //                    smtp.Send(fromAddress, toAddress, subject, body);
        //                    ClearAll();
        //                    transaction_scope.Complete();
        //                }
        //            }
        //        }
        //    }
        //}

    }
}
