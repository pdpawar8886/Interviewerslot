using Interviewerslot.Models;
using Interviewerslot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interviewerslot.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
     
            ApplicationDbContext db = new ApplicationDbContext();

            // GET: Admin Login
            public ActionResult Login()
            {
                return View();
            }

            // POST: Admin Login
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Login(AdminLoginViewModel model)
            {
                if (!ModelState.IsValid)
                    return View(model);

                // Check admin credentials
                var admin = db.Admins.FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password);
                if (admin != null)
                {
                    Session["UserRole"] = "Admin";
                    Session["AdminId"] = admin.AdminId;
                    Session["AdminName"] = admin.Username;

                    return RedirectToAction("Index");
                }

                ViewBag.Error = "Invalid username or password";
                return View(model);
            }


        // GET: Admin Dashboard
        public ActionResult Index()
        {
            if (Session["UserRole"]?.ToString() != "Admin")
                return RedirectToAction("Login");

            var students = db.Students.ToList();
            var sirs = db.Sirs.ToList();

            var model = new AdminLoginViewModel
            {
                Students = students,
                Sirs = sirs
            };

            return View(model);
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
