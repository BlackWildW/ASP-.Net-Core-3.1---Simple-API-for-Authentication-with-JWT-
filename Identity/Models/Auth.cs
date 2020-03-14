using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class Auth
    {
        [Required(ErrorMessage = "Enter Login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }

    }  
}
