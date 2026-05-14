using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



    namespace Interviewerslot.ViewModels
{
    public class SirLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}