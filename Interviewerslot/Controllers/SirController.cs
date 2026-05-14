using Interviewerslot.Models;
using Interviewerslot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interviewerslot.Controllers
{
    public class SirController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult AddAvailability()
        {
            ViewBag.SirList = new SelectList(db.Sirs, "SirId", "SirName");
            return View();
        }
        public ActionResult Index()
        {
            if (Session["UserRole"]?.ToString() != "Sir")
                return RedirectToAction("Login", "Sir");

            var data = db.SirAvailabilities
                         .Include("Sir")
                         .OrderByDescending(x => x.AvailableDate)
                         .ToList();

            return View(data);
        }






        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = db.SirAvailabilities.Find(id);

            if (data == null)
                return HttpNotFound();

            
            if (data.IsBooked)
            {
                TempData["ErrorMessage"] = "This slot is already booked and cannot be edited.";
                return RedirectToAction("Index");
            }

            ViewBag.SirList = new SelectList(db.Sirs, "SirId", "SirName", data.SirId);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(SirAvailability model)
        {
            var existing = db.SirAvailabilities
                             .FirstOrDefault(x => x.AvailabilityId == model.AvailabilityId);

            if (existing.IsBooked)
            {
                TempData["ErrorMessage"] = "Booked slot cannot be updated.";
                return RedirectToAction("Index");
            }

            existing.SirId = model.SirId;
            existing.AvailableDate = model.AvailableDate;
            existing.FromTime = model.FromTime;
            existing.ToTime = model.ToTime;

            db.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult AddAvailability(SirAvailability model)
        {


            TimeSpan slotDuration = TimeSpan.FromHours(1); 

            TimeSpan start = model.FromTime;
            TimeSpan end = model.ToTime;

            while (start < end)
            {
                var slotEnd = start.Add(slotDuration);

                // extra safety
                if (slotEnd > end)
                    break;

                
                bool alreadyExists = db.SirAvailabilities.Any(x =>
                    x.SirId == model.SirId &&
                    x.AvailableDate == model.AvailableDate &&
                    x.FromTime == start &&
                    x.ToTime == slotEnd
                );

                if (!alreadyExists)
                {
                    db.SirAvailabilities.Add(new SirAvailability
                    {
                        SirId = model.SirId,
                        AvailableDate = model.AvailableDate,
                        FromTime = start,
                        ToTime = slotEnd,
                        IsBooked = false
                    });
                }

                start = slotEnd;
            }

            db.SaveChanges();

            TempData["SuccessMessage"] = "Slots generated successfully!";
            return RedirectToAction("AddAvailability");
        }







        public JsonResult GetSlots(int sirId, DateTime date)
        {
            var slots = db.SirAvailabilities
                .Where(x => x.SirId == sirId &&
                            x.AvailableDate == date.Date &&
                            !x.IsBooked)
                .AsEnumerable()
                .Select(x => new
                {
                    x.AvailabilityId,
                    SlotText =
                        DateTime.Today.Add(x.FromTime).ToString("hh:mm tt") +
                        " - " +
                        DateTime.Today.Add(x.ToTime).ToString("hh:mm tt")
                })
                .ToList();

            return Json(slots, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: Sir Register
        [HttpPost]
        public ActionResult Register(SirRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (db.Sirs.Any(x => x.Email == model.Email))
            {
                ViewBag.Error = "Email already exists";
                return View(model);
            }

            Sir sir = new Sir
            {
                SirName = model.SirName,
                Email = model.Email,
                Phone = model.Phone,
                password = model.Password
            };

            db.Sirs.Add(sir);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        // GET: Sir Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SirLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model); //  validation errors (email format, required)

            var sir = db.Sirs.FirstOrDefault(x => x.Email == model.Email);

            if (sir == null)
            {
                ModelState.AddModelError("", "Account does not exist");
                return View(model);
            }

            if (sir.password != model.Password)
            {
                ModelState.AddModelError("", "Invalid password");
                return View(model);
            }

            // ✅ LOGIN SUCCESS
            Session["UserRole"] = "Sir";
            Session["SirId"] = sir.SirId;
            Session["SirName"] = sir.SirName;

          
            return RedirectToAction("Index", "Dashboard");
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }



}