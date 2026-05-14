using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Interviewerslot.ViewModels
{
    public class StudentRegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

