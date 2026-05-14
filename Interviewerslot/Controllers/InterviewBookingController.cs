using Interviewerslot.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace Interviewerslot.Controllers
{
    public class InterviewBookingController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        
        [HttpGet]
        public ActionResult Index()
        {
            var data = db.InterviewBookings
                         .Select(b => new InterviewBookingViewModel
                         {
                             BookingId = b.BookingId,
                             StudentName = b.Student.Name,
                             SirName = b.Sir.SirName,
                             Date = b.Date,
                             FromTime = b.FromTime,
                             ToTime = b.ToTime
                         })
                         .ToList();  

            return View(data);
        }
       
        [HttpGet]
        public JsonResult GetSlotsBySir(int sirId)
        {
            var slots = db.SirAvailabilities
                          .Where(x => x.SirId == sirId && !x.IsBooked)
                          .ToList()
                          .Select(x => new
                          {
                              x.AvailabilityId,
                              SlotText =
                                  x.AvailableDate.ToString("dd-MM-yyyy") + " | " +
                                  DateTime.Today.Add(x.FromTime).ToString("hh:mm tt") +
                                  " - " +
                                  DateTime.Today.Add(x.ToTime).ToString("hh:mm tt")
                          });

            return Json(slots, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.StudentList = new SelectList(db.Students, "StudentId", "Name");
            ViewBag.SirList = new SelectList(db.Sirs, "SirId", "SirName");

            ViewBag.ErrorMsg = null;
            ViewBag.SuccessMsg = TempData["SuccessMsg"];

            return View();
        }



        [HttpPost]
        
        public ActionResult Create(InterviewBooking model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StudentList = new SelectList(db.Students, "StudentId", "Name");
                ViewBag.SirList = new SelectList(db.Sirs, "SirId", "SirName");
                return View(model);
            }

            //  Get selected slot
            var availability = db.SirAvailabilities
                                 .FirstOrDefault(x => x.AvailabilityId == model.AvailabilityId);

            if (availability == null || availability.IsBooked)
            {
                ViewBag.ErrorMsg = " Selected slot already booked.";

                ViewBag.StudentList = new SelectList(db.Students, "StudentId", "Name");
                ViewBag.SirList = new SelectList(db.Sirs, "SirId", "SirName");

                return View(model);
            }

            //  Book slot
            availability.IsBooked = true;

            model.Date = availability.AvailableDate;
            model.FromTime = availability.FromTime;
            model.ToTime = availability.ToTime;
            model.SirId = availability.SirId;

            db.InterviewBookings.Add(model);
            db.SaveChanges();

            //  SEND EMAIL TO STUDENT
            try
            {
                var student = db.Students
                                .FirstOrDefault(s => s.StudentId == model.StudentId);

                var sir = db.Sirs
                            .FirstOrDefault(s => s.SirId == model.SirId);

                if (student != null)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(student.Email);
                    mail.Subject = "Interview Slot Booked Successfully";
                    mail.IsBodyHtml = true;

                    mail.Body = $@"
<h3>Hello {student.Name},</h3>
<p>Your interview slot has been <b>booked successfully</b>.</p>
<p><b>Sir:</b> {sir?.SirName}</p>
<p><b>Date:</b> {model.Date:dd-MM-yyyy}</p>
<p><b>Time:</b> {DateTime.Today.Add(model.FromTime):hh:mm tt}
 - {DateTime.Today.Add(model.ToTime):hh:mm tt}</p>
<br/>
<p>All the best 👍</p>";

                    new SmtpClient().Send(mail);
                }
            }
            catch
            {
            }

            // Redirect with success message
            TempData["SuccessMsg"] = " Interview slot booked successfully! Email sent to student.";
            return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var booking = db.InterviewBookings.FirstOrDefault(x => x.BookingId == id);

            if (booking == null)
                return HttpNotFound();

            ViewBag.StudentList = new SelectList(
                db.Students, "StudentId", "Name", booking.StudentId);

            ViewBag.SirList = new SelectList(
                db.Sirs, "SirId", "SirName", booking.SirId);

            return View(booking);
        }
        [HttpGet]
        public JsonResult GetSlotsBySirDate(int sirId, DateTime date)
        {
            var slots = db.SirAvailabilities
                          .Where(x => x.SirId == sirId &&
                                      x.AvailableDate == date.Date &&
                                      !x.IsBooked)
                          .ToList()
                          .Select(x => new
                          {
                              x.AvailabilityId,
                              SlotText =
                                  DateTime.Today.Add(x.FromTime).ToString("hh:mm tt") +
                                  " - " +
                                  DateTime.Today.Add(x.ToTime).ToString("hh:mm tt")
                          });

            return Json(slots, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InterviewBooking model)
        {
            var booking = db.InterviewBookings
                            .FirstOrDefault(x => x.BookingId == model.BookingId);

            if (booking == null)
                return HttpNotFound();

            // Reload dropdowns
            ViewBag.StudentList = new SelectList(db.Students, "StudentId", "Name", model.StudentId);
            ViewBag.SirList = new SelectList(db.Sirs, "SirId", "SirName", model.SirId);

            // Slot change zala ka?
            if (booking.AvailabilityId != model.AvailabilityId)
            {
                var newSlot = db.SirAvailabilities
                                .FirstOrDefault(x => x.AvailabilityId == model.AvailabilityId);

                if (newSlot == null || newSlot.IsBooked)


                {
                    ViewBag.ErrorMsg = " Selected slot already booked.";
                    return View(model);
                }


                // Old slot free
                var oldSlot = db.SirAvailabilities
                                .FirstOrDefault(x => x.AvailabilityId == booking.AvailabilityId);

                if (oldSlot != null)
                    oldSlot.IsBooked = false;

                // New slot book
                newSlot.IsBooked = true;

                booking.SirId = newSlot.SirId;
                booking.AvailabilityId = newSlot.AvailabilityId;
                booking.Date = newSlot.AvailableDate;
                booking.FromTime = newSlot.FromTime;
                booking.ToTime = newSlot.ToTime;
            }

            booking.StudentId = model.StudentId;

            db.SaveChanges();

            //  MAIL SEND AFTER EDIT
            try
            {
                var student = db.Students.FirstOrDefault(s => s.StudentId == booking.StudentId);
                var sir = db.Sirs.FirstOrDefault(s => s.SirId == booking.SirId);

                if (student != null)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(student.Email);
                    mail.Subject = "Interview Slot Updated";
                    mail.IsBodyHtml = true;
                    mail.Body = $@"
Hello {student.Name},<br/><br/>
Your interview slot has been <b>updated</b>.<br/><br/>
<b>Sir:</b> {sir?.SirName}<br/>
<b>Date:</b> {booking.Date:dd-MM-yyyy}<br/>
<b>Time:</b> {DateTime.Today.Add(booking.FromTime):hh:mm tt}
 - {DateTime.Today.Add(booking.ToTime):hh:mm tt}<br/><br/>
Thank you.";

                    new SmtpClient().Send(mail);
                }
            }
            catch { }

            ViewBag.SuccessMsg = "Interview slot updated & mail sent!";
            return View(model);
        }




    }
}
