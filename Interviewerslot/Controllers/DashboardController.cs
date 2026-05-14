using Interviewerslot.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Interviewerslot.Controllers
{
    public class DashboardController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var today = DateTime.Today;
             // Counts
            ViewBag.TotalSirs = db.Sirs.Count();
            ViewBag.TotalStudents = db.Students.Count();
            ViewBag.TotalInterviews = db.InterviewBookings.Count();

            //  Today Interviews
            ViewBag.TodayInterviews = db.InterviewBookings
                                        .Count(x => x.Date == today);

            //  Slots
            ViewBag.BookedSlots = db.SirAvailabilities.Count(x => x.IsBooked);
            ViewBag.AvailableSlots = db.SirAvailabilities.Count(x => !x.IsBooked);

            return View();
        }
    }
}
