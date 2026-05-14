using Interviewerslot.Models;
using Interviewerslot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interviewerslot.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Index()
        {
            if (Session["UserRole"]?.ToString() != "Student")
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
       
        public ActionResult Login(StudentLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var student = db.Students
                .FirstOrDefault(x => x.Email == model.Email && x.password == model.Password);

            if (student != null) // check if student exists
            {
                Session["UserRole"] = "Student";
                Session["StudentId"] = student.StudentId;
                Session["StudentName"] = student.Name;

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View(model);
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    

    // GET: Register
public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public ActionResult Register(StudentRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check email already exists
            var exists = db.Students.Any(x => x.Email == model.Email);
            if (exists)
            {
                ViewBag.Error = "Email already registered";
                return View(model);
            }

            Student student = new Student
            {
                Name = model.Name,
                Email = model.Email,
                password = model.Password
            };

            db.Students.Add(student);
            db.SaveChanges();

            return RedirectToAction("Login");
        }


    }

    }