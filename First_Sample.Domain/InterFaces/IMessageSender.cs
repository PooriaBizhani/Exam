using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Domain.InterFaces
{
    public interface IMessageSender
    {
        Task SendEmailAsync(string toEmail, string subject,string message,bool isMessageHtml=false);
    }
}
