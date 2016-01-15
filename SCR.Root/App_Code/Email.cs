using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Net.Mime;

namespace SCR.Root.App_Code
{
    public class Email
    {
        public void SendMail(string tomailaddress, string body)
        {
            try
            {
                string fromAddress = ConfigurationManager.AppSettings["FromAddress"].ToString();
                string fromPassord = ConfigurationManager.AppSettings["FromPassword"].ToString();
                string mailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
                string SMTPUser = ConfigurationManager.AppSettings["SMTPUser"].ToString();
                string SMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();
                string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
                string FromDisplay = ConfigurationManager.AppSettings["DisplayFrom"].ToString();
                string subject = ConfigurationManager.AppSettings["FromSubject"];

                MailMessage mailObj = new MailMessage();
                MailAddress fromMail = new MailAddress(fromAddress, FromDisplay);
                MailAddress ToMail = new MailAddress(tomailaddress);

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential(SMTPUser, SMTPPassword);
                smtp.Host = mailServer;
                smtp.Port = int.Parse(SMTPPort);
                // smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Timeout = 20000;
                using (var message = new MailMessage(fromMail, ToMail)
                {
                    Subject = subject,
                    Body = body,
                })
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}