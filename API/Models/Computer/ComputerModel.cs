using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Computer
{
    public class ComputerModel
    {
        public int Id { get; set; }
        [Required]
        public string MacAddress { get; set; }
        public ConnectionEnum Status { get; set; }
        public string Name { get; set; }
    }
}
