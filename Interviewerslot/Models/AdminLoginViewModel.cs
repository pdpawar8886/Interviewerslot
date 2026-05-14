using Interviewerslot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Interviewerslot.ViewModels

{
    public class AdminLoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public List<Student> Students { get; set; }
        public List<Sir> Sirs { get; set; }
    }
}
