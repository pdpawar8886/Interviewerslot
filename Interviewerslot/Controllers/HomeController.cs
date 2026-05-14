using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interviewerslot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Already logged in?
            if (Session["UserRole"] != null)
            {
                var role = Session["UserRole"].ToString();

                if (role == "Student")
                    return RedirectToAction("Index", "Dashboard");

                if (role == "Sir")
                    return RedirectToAction("Index", "Dashboard");

                if (role == "Admin")
                    return RedirectToAction("Index", "AdminDashboard");
            }

            // Not logged in → Show login choice
            return View();
        }


    }
}