using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



    namespace Interviewerslot.ViewModels
{
    public class StudentLoginViewModel
    {
        [Required]
        [System.ComponentModel.DataAnnotations.EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}