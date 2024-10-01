using First_Sample.Application.InterFaces;
using First_Sample.Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Application.Services
{
    public class MessageSenderService : IMessageSenderService
    {
        private readonly IMessageSender _messageSender;
        public MessageSenderService(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }
        public async Task SendEmailService(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            await _messageSender.SendEmailAsync(toEmail,subject,message,isMessageHtml);        }
    }
}
