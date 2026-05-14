using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interviewerslot.Models
{
    public class InterviewBooking
    {
        [Key]
        public int BookingId { get; set; }

        public int StudentId { get; set; }
        public int SirId { get; set; }
        public int AvailabilityId { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }

        // ✅ Navigation properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("SirId")]
        public virtual Sir Sir { get; set; }

        [ForeignKey("AvailabilityId")]
        public virtual SirAvailability Availability { get; set; }
    }
}
