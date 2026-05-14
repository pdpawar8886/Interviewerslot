using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Interviewerslot.Models
{
    
    public class SirAvailability
    {
        [Key]
        public int AvailabilityId { get; set; }
        public int SirId { get; set; }

        public DateTime AvailableDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }

        public bool IsBooked { get; set; }

        public virtual Sir Sir { get; set; }
    }
}