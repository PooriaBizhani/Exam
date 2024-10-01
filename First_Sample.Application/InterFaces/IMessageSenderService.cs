using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Application.InterFaces
{
    public interface IMessageSenderService
    {
        Task SendEmailService(string email, string subject, string message, bool isMessageHtml = false);
    }
}
