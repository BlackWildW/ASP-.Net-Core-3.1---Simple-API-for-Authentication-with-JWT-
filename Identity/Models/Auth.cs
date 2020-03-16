using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class Auth
    {
        [Required(ErrorMessage = "The field Email is empty")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "The field Password is empty")]
        public string Password { get; set; }

    }  
}
