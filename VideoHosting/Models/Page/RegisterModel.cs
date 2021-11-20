using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Page
{
    public class RegisterModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Login { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Password mismatch"), DisplayName("Repeat Password")]
        public string RepeatPassword { get; set; }
    }
}