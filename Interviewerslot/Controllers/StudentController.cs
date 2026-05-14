using Interviewerslot.Models;
using System.Linq;
using System.Web.Mvc;

namespace Interviewerslot.Controllers
{
    public class StudentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

       
        public ActionResult Index()
        {
            var students = db.Students.ToList();
            return View(students);
        }

        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(Student model)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            var student = db.Students.FirstOrDefault(s => s.StudentId == id);

            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // POST: Student/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student model)
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.FirstOrDefault(s => s.StudentId == model.StudentId);

                if (student == null)
                    return HttpNotFound();

                student.Name = model.Name;
                student.Email = model.Email;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}
