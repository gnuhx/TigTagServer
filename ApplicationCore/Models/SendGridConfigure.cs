using System.IO;

namespace ApplicationCore.Models
{
    public class EmailAttachment
    {
        public string FileName { get; set; }

        public Stream Attachment { get; set; }
    }

    public class SendGridConfigure
    {
        public  string FromEmail { get; set; }
        public  string FromName { get; set; }
        public  string ApiKey { get; set; }
    }
}
