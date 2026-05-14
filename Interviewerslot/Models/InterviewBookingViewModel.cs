using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interviewerslot.Models
{
    public class InterviewBookingViewModel
    {
        public int BookingId { get; set; }
        public string StudentName { get; set; }
        public string SirName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}