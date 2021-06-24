using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(List<string> to, string subject, string body, string bcc = null, string cc = null);

        Task SendEmailWithAttachmentsAsync(List<string> to, string subject,
            string body, List<EmailAttachment> attachments, string bcc = null, string cc = null);
    }
}
