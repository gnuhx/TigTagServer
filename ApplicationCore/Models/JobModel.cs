using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.Models
{
    public class JobModel
    {
        [Required]
        public int PrinterId { get; set; }

        [Required]
        public string FileUrl { get; set; }
        public string Title { get; set; }


    }
}
