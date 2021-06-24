using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class EmailService: IEmailService
    {
        private readonly SendGridConfigure _sendGridConfigure;

        public EmailService(SendGridConfigure sendGridConfigure)
        {
            _sendGridConfigure = sendGridConfigure;
        }

        public async Task SendEmailAsync(List<string> to, string subject, string body,string bcc = null, string cc = null)
        {
            SendGridMessage myMessage = new SendGridMessage();
            foreach (var toMail in to)
            {
                myMessage.AddTo(new EmailAddress(toMail));
            }
            if (!string.IsNullOrEmpty(bcc))
                myMessage.AddCc(new EmailAddress(bcc));
            if (!string.IsNullOrEmpty(cc))
                myMessage.AddCc(new EmailAddress(cc));

            myMessage.From = new EmailAddress(_sendGridConfigure.FromEmail, _sendGridConfigure.FromName);
            myMessage.Subject = subject;
            myMessage.HtmlContent = body;
            var client = new SendGridClient(_sendGridConfigure.ApiKey);
            // Create a Web transport, using API Key
            await client.SendEmailAsync(myMessage);
        }


        public async Task SendEmailWithAttachmentsAsync(List<string> to, string subject,
            string body, List<EmailAttachment> attachments, string bcc = null, string cc = null )
        {
            
            SendGridMessage myMessage = new SendGridMessage();
            foreach (var toMail in to)
            {
                myMessage.AddTo(new EmailAddress(toMail));
            }
            if (!string.IsNullOrEmpty(bcc))
                myMessage.AddCc(new EmailAddress(bcc));
            if (!string.IsNullOrEmpty(cc))
                myMessage.AddCc(new EmailAddress(cc));

            myMessage.From = new EmailAddress(_sendGridConfigure.FromEmail, _sendGridConfigure.FromName);
            myMessage.Subject = subject;
            myMessage.HtmlContent = body;
            foreach (var attachment in attachments)
            {
                var bytes = await ReadFully(attachment.Attachment);
                myMessage.AddAttachment(attachment.FileName, Convert.ToBase64String(bytes));
            }
            var client = new SendGridClient(_sendGridConfigure.ApiKey);
            // Create a Web transport, using API Key
            await client.SendEmailAsync(myMessage);
        }

        private async Task<byte[]> ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                   await ms.WriteAsync(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
