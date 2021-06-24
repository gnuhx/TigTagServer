using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class PrinterResponseModel
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ConnectionEnum Status { get; set; }
        public ComputerModel Computer { get; set; }
    }

    public class ComputerModel
    {
        public ConnectionEnum Status { get; set; }
        public string Name { get; set; }
    }
}
