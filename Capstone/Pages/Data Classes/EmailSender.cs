
using System;
using System.Net;
using System.Net.Mail;

namespace Capstone.Pages.Data_Classes
{
    public static class EmailSender
    {
        public static void SendEmail(EmailModel emailModel)
        {
            string smtpServer = "smtp.office365.com"; // Outlook SMTP server
            int smtpPort = 587; // Outlook uses port 587
            string smtpUsername = "host.management.system@outlook.com"; // Your Outlook email address
            string smtpPassword = "12Hands3!"; // Your Outlook password or App Password

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = emailModel.Subject,
                    Body = emailModel.Body,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(emailModel.ToEmail);

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email. Error: {ex.Message}");
                }
            }
        }
    }
}
