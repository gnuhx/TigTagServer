using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class RabbitMqMessageModel
    {
        public int PrinterId { get; set; }

        public string FileUrl { get; set; }

        public int ComputerId { get; set; }

        public int JobId { get; set; }

        public string Title { get; set; }
    }
}
