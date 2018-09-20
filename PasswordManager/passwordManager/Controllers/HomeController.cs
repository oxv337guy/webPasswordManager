using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using passwordManager.Models;

namespace passwordManager.Controllers
{
    public class HomeController : Controller
    {
        PWDBEntities db = new PWDBEntities();

        // GET: Home
        public ActionResult Index()
        {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }
            Session["PageName"] = "MyList";

            HomeIndexViewModel user = new HomeIndexViewModel();
            user.UserId = (int)Session["UserId"];
            user.ShowItemNumber = 10;
            user.ItemStart = 0;
            user.PickUpNewPassword();


            List<ShowList> NewUser = new List<ShowList>();
            foreach (var i in user.ShowNewPassword) {
                NewUser.Add(i);
            }
            
            return View(NewUser);
        }


        [HttpPost]
        public ActionResult Index(int FlipPage) {
            if ((int)Session["UserId"] == 0) {
                return RedirectToAction("Index", "User");
            }

            HomeIndexViewModel user = new HomeIndexViewModel();
            user.UserId = (int)Session["UserId"];
            user.ShowItemNumber = 10;
            user.ComputPageCount();
            int SessionNumber = (int)Session["PageIndex"];
            if ((FlipPage>0) && (SessionNumber +1 >= user.PageCount)) {
                FlipPage = 0;
            }
            if ((FlipPage < 0) && (SessionNumber -1 < 0)) {
                FlipPage = 0;
            }
            Session["PageIndex"] = SessionNumber + FlipPage;
            user.ItemStart = (SessionNumber + FlipPage) * 10;
            user.PickUpNewPassword();


            List<ShowList> NewUser = new List<ShowList>();
            foreach (var i in user.ShowNewPassword) {
                NewUser.Add(i);
            }

            return View(NewUser);
        }

        public ActionResult Edit(int? id) {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }

            if (db.NewPasswords.Find(id) == null)
            {
                return HttpNotFound();
            }
            if (db.NewPasswords.Find(id).UserId != (int)Session["UserId"])
            {
                return HttpNotFound();
            }
            Session["PageName"] = "Edit";

            var query = from o in db.NewPasswords
                        where o.NewPasswordId == id
                        select o;
            NewPassword npw = query.Single();
            return View(npw);
        }

        [HttpPost]
        public ActionResult Edit(NewPassword npw)
        {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }

            if (Request["OkOrCancel"] == "Cancel") {
                return RedirectToAction("Index");
            }

            db.Entry(npw).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }

            if (db.NewPasswords.Find(id) == null)
            {
                return HttpNotFound();
            }
            if (db.NewPasswords.Find(id).UserId != (int)Session["UserId"])
            {
                return HttpNotFound();
            }
            var query = from o in db.NewPasswords
                        where o.NewPasswordId == id
                        select o;
            NewPassword npw = query.Single();
            db.NewPasswords.Remove(npw);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CreatePwText()
        {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }
            Session["PageName"] = "CreatePwText";

            return View();
        }

        [HttpPost]
        public ActionResult CreatePwText(CreatePwTextViewModel SeedAndPwText)
        {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }

            if (string.IsNullOrEmpty(SeedAndPwText.PasswordText)) {
                return View(SeedAndPwText);
            }
            Session["PwText"] = SeedAndPwText.PasswordText;

            //return View("Create");
            return RedirectToAction("Create");
        }

        public ActionResult Create()
        {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }
            Session["PageName"] = "Create";

            NewPassword npw = new NewPassword();
            npw.PasswordText = Session["PwText"].ToString();
            Session["PwText"] = "";
            return View(npw);
        }

        [HttpPost]
        public ActionResult Create(NewPassword npw)
        {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }
            if (Request["OkOrCancel"] == "Cancel") {
                return RedirectToAction("Index");
            }
            db.NewPasswords.Add(npw);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Return(int? id)
        {
            if ((int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "User");
            }
            if (db.NewPasswords.Find(id) == null)
            {
                return HttpNotFound();
            }
            if (db.NewPasswords.Find(id).UserId != (int)Session["UserId"])
            {
                return HttpNotFound();
            }
            Session["PageName"] = "Return";
            var query = from o in db.NewPasswords
                        where o.NewPasswordId == id
                        select o;
            NewPassword npw = query.Single();
            return View(npw);
        }
    }
}