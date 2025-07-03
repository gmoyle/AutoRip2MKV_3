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
        private static readonly ILogger _logger = ServiceContainer.Instance.Resolve<ILogger>();
        private static readonly ICredentialManager _credentialManager = ServiceContainer.Instance.Resolve<ICredentialManager>();
        
        public static string userfromemail = Properties.Settings.Default.FromEmail;
        public static string usertxtto = Properties.Settings.Default.PhoneNumber + Properties.Settings.Default.CurrentProvider;
        public static int userport = Properties.Settings.Default.SMTPPort;

        public static void Main(bool results)
        {
            try
            {
                _logger.Info("Starting SMTP email send, success: {0}", results);
                
                // Get credentials from secure storage
                var smtpCredential = _credentialManager.GetCredential("SMTP");
                if (smtpCredential == null)
                {
                    _logger.Error("SMTP credentials not found in secure storage");
                    Ripping.UpdateStatusText("SMTP credentials not configured");
                    return;
                }
                
                SmtpClient smtpClient = new SmtpClient();
                NetworkCredential basicCredential = new NetworkCredential(smtpCredential.Username, smtpCredential.PasswordPlainText);
                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(userfromemail, "AutoRip2MKV");

                smtpClient.Host = Properties.Settings.Default.SMTPAddress;
                smtpClient.Port = Properties.Settings.Default.SMTPPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = Properties.Settings.Default.EnableTTL;
                smtpClient.Credentials = basicCredential;

                message.From = fromAddress;
                message.Subject = "AutoRip2MKV Status";
                
                if (results)
                {
                    message.Body = "Rip of " + Properties.Settings.Default.CurrentTitle + " was successful";
                }
                else
                {
                    message.Body = "Rip of " + Properties.Settings.Default.CurrentTitle + 
                        " failed. \r\n  Verify you have the latest version of MakeMKV installed.";
                }

                message.To.Add(usertxtto);

                try
                {
                    smtpClient.Send(message);
                    _logger.Info("Email sent successfully");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to send email");
                    Ripping.UpdateStatusText(ex.Message);
                }
                finally
                {
                    smtpCredential?.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to send SMTP email");
                Ripping.UpdateStatusText($"Email failed: {ex.Message}");
            }
        }
    }
}

