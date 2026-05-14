using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Interviewerslot.Models
{
    public class Sir
    {
        [Key]
        public int SirId { get; set; }
        public string SirName { get; set; }

        public  string Email { get; set; }
        public string password { get; set; }

        public string Phone { get; set; }
    }
}