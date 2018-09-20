using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using passwordManager.Models;

namespace passwordManager.Controllers
{
    public class UserController : Controller
    {
        PWDBEntities db = new PWDBEntities();

        // GET: User
        public ActionResult Index()
        {
            Session["PageName"] = "Login";
            UserIndexViewModel user = new UserIndexViewModel();
            return View(user);
        }

        [HttpPost]
        public ActionResult Index(UserIndexViewModel user)
        {
            user.InitialViewModel();

            if (!user.IsAccountValid())
            {
                return View(user);
            }
            if (!user.IsPasswordValid()) {
                return View(user);
            }

            Session["UserId"] = user.UserId;
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateAccount() {
            CreateAccountViewModel NewAccount = new CreateAccountViewModel();

            return View(NewAccount);
        }

        [HttpPost]
        public ActionResult CreateAccount(CreateAccountViewModel NewAccount) {
            
            if (NewAccount.IsAccountNameExist()) {
                return View(NewAccount);
            }
            if (!NewAccount.IsInputPwIdentity()) {
                return View(NewAccount);
            }
            
            MemberUser MemAccount = new MemberUser();
            MemAccount.UserLastName = NewAccount.UserLastName;
            MemAccount.UserFirstName = NewAccount.UserFirstName;
            MemAccount.UserEmail = NewAccount.UserEmail;
            MemAccount.AccountName = NewAccount.AccountName;
            NewAccount.EncryptPw();
            MemAccount.AccountPassword = NewAccount.EncryptPassword;

            db.MemberUsers.Add(MemAccount);
            db.SaveChanges();

            return RedirectToAction("Index", "User");
        }

        public ActionResult Logout() {
            Session["UserId"] = 0;
            Session["PwText"] = "";
            Session["PageIndex"] = 0;
            Session["PageName"] = "Login";
            return RedirectToAction("Index", "User");
        }
    }
}