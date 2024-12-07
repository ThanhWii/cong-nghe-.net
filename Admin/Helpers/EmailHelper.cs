namespace Admin.Helpers
{
    using Microsoft.Extensions.Configuration;
    using System.Net;
    using System.Net.Mail;

    public class EmailHelper
    {
        private string smtpServer = "smtp.gmail.com"; 
        private int port = 587; 
        private string senderEmail = "tranthanhnhatthien@gmail.com"; 
        private string senderPassword = "uyms fkqv cbid lzzv"; 

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
