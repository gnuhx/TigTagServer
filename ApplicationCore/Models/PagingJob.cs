using System;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class JobPageModel
    {
        public int JobId { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public string ComputerName { get; set; }
        public int ComputerId { get; set; }
        public string PrinterName { get; set; }
        public int PrinterId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public long? RowCounts { get; set; }

    }
}
