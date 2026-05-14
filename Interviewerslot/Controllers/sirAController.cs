using Interviewerslot.Models;
using System.Linq;
using System.Web.Mvc;

namespace Interviewerslot.Controllers
{
    public class SirAController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

       
        public ActionResult Index()
        {
            var data = db.Sirs.ToList();
            return View(data);
        }

       
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        public ActionResult Create(Sir model)
        {
            if (ModelState.IsValid)
            {
                db.Sirs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
