using First_Sample.Domain.InterFaces;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Persistence.Repositories
{
    public class MessageSender : IMessageSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    UserName = "pooriabizhani@gmail.com", // without @gmail.com
                    Password = "lrmo ukhc hcny oobn"
                };

                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toEmail) },
                    From = new MailAddress("pooriabizhani@gmail.com"), // with @gmail.com
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                };

                    await client.SendMailAsync(emailMessage); // اصلاح شده
            }
        }
    }
}
