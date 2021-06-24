using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Student
{
    public class StudentModel
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Please enter Email.")]
        public string Email { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }
        //public List<Test> Tests { get; set; }
    }
}
