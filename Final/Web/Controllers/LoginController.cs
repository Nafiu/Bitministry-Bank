using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Unitofwork;
using Entity.Entities;
using System.Web.Security;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        private unitofworkclass uow;
        public LoginController() : this(new unitofworkclass()) { }
        public LoginController(unitofworkclass uow)
        {
            this.uow = uow;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(BankAccount l)
        {
            if (ModelState.IsValid)
            {
                var s= uow.Ulogin.LoginUser(l);
                if (s == "Success")
                {
                    return RedirectToAction("MyProfile", "Home");
                }
            }
            ViewBag.forpas = "Worng Username And Password";
            return View();
        }
        public ActionResult Views()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Views");
        }
        protected override void Dispose(bool disposing)
        {
            uow.Dispose();
            base.Dispose(disposing);
        }
    }
}