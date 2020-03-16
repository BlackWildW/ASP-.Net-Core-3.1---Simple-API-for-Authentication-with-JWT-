using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class User
    {
        public  int Id { get; set; }
        [Required(ErrorMessage ="The field UserName is empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field Email is empty")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "The field Password is empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "The field Confirm Password is empty")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public string token { get; set; }
    }
}
