namespace Admin.Helpers
{
    //using Microsoft.Extensions.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Configuration;
    using System;

    public class EmailHelper
    {
        private string smtpServer = ConfigurationManager.AppSettings["SmtpServer"]; 
        private int port = Int16.Parse(ConfigurationManager.AppSettings["Port"]); 
        private string senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
        private string senderPassword = ConfigurationManager.AppSettings["SenderPass"]; 

        // Empty constructor for default settings
        public EmailHelper()
        {
        }

        // Optional constructor for manual configuration
        public EmailHelper(string smtpServer, int port, string senderEmail, string senderPassword)
        {
            this.smtpServer = smtpServer;
            this.port = port;
            this.senderEmail = senderEmail;
            this.senderPassword = senderPassword;
        }

        public void SendEmail(string recipientEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(recipientEmail);

            smtpClient.Send(mailMessage);
        }
    }
}
