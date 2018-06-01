using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

namespace AutoRip2MKV
{


    class SMTPSender
    {
        public static string smtpuser = Properties.Settings.Default.SMTPUser;
        public static string smtppass = Properties.Settings.Default.SMTPPass;
        public static string userfromemail = Properties.Settings.Default.FromEmail;
        public static string usertxtto = Properties.Settings.Default.PhoneNumber + Properties.Settings.Default.CurrentProvider;


        public static void Main(bool results)
        {

            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(smtpuser, smtppass);
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(userfromemail, "AutoRip2MKV");

            smtpClient.Host = Properties.Settings.Default.SMTPAddress;
                //"email-smtp.us-east-1.amazonaws.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = Properties.Settings.Default.EnableTTL;
            smtpClient.Credentials = basicCredential;

            message.From = fromAddress;
            message.Subject = "AutoRip2MKV Status";
            //Set IsBodyHtml to true means you can send HTML email.
            //message.IsBodyHtml = true;
            if  (results)
            {
                message.Body = "Rip of " + Properties.Settings.Default.CurrentTitle + " was successful";
            }
            else
            {
                message.Body = "Rip of " + Properties.Settings.Default.CurrentTitle + 
                    " failed. /r/n  Verify you have the latest version of MakeMKV installed."; 
            }

            message.To.Add(usertxtto);

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                //Error, could not send the message
                Ripping.UpdateStatusText(ex.Message);
            }
            
        }
    }
}

