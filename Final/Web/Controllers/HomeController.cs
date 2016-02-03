using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Entity.Entities;
using Repository.Unitofwork;
using PagedList;
using PagedList.Mvc;
using Web.CustomActionfilters;

namespace Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class HomeController : Controller
    {
        private unitofworkclass uow;
        public HomeController() : this(new unitofworkclass()) { }
        public HomeController(unitofworkclass uow)
        {
            this.uow = uow;
        }
        public ActionResult MyProfile()
        {
            string s = User.Identity.Name;
            List<BankAccount> b = new List<BankAccount>();
            b.Add(uow.Ubank.profile(s));
            return View(b);
        }
        [HttpGet]
        public ActionResult Deposite()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposite(Transactionsdetail ta)
        {
            if (ModelState.IsValid)
            {
                uow.Ubank.Deposite(ta);
                uow.save();
                ViewBag.headi = "Deposite";
                ViewBag.transfer = "Deposited sucessfully";
                return View("Partialviews");
            }
            return View();
        }


        [HttpGet]
        public ActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(Transactionsdetail ta)
        {
            if (ModelState.IsValid)
            {
                var i = uow.Ubank.Withdraw(ta);
                if (i == "Completed")
                {
                    uow.save();
                    ViewBag.headi = "Withdraw";
                    ViewBag.transfer = "Withdraw sucessfully";
                    return View("Partialviews");
                }
                ViewBag.headi = "Withdraw";
                ViewBag.transfer = "Withdraw Failed Check your account name and your balance";
                return View("ErrorPartialViews");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer(Transactionsdetail ta)
        {
            if (ModelState.IsValid)
            {
                var i = uow.Ubank.Transfer(ta);
                ModelState.Remove("Amount");
                ModelState.Remove("To");
                if (i == "completed")
                {
                    uow.save();                   
                    ViewBag.headi = "Transaction";
                    ViewBag.transfer = "Transaction completed Sucessfully";
                    return View("Partialviews");
                }
                if (i == "Failed")
                {
                    ModelState.Remove("Amount");
                    ModelState.Remove("To");                
                    ViewBag.headi = "Transaction";
                    ViewBag.transfer = "Transaction Failed";
                    return View("ErrorPartialViews");
                }              
                ViewBag.headi = "Transaction";
                ViewBag.transfer = "Transaction Failed due to receiver account is not available (or) you not having the balance";
                ModelState.Remove("Amount");
                ModelState.Remove("To");
                return View("ErrorPartialViews");
            }
            return View();
        }

        public ActionResult Statement(string fromd, string tod, int? page)
        {
            DateTime f = Convert.ToDateTime(fromd, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            DateTime t = Convert.ToDateTime(tod, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            var stu = uow.Ubank.Getstatement(f, t).ToList();
            ViewBag.fd = fromd; ViewBag.td = tod;

            if ((stu.Count == 0) && fromd != null)
            {
                if (f > t)
                {
                    ViewBag.dat = "Choose correct Date";
                }
                else if (stu.Count == 0)
                {
                    ViewBag.er = "No Transaction Occured";
                }
            }
            return View(stu.ToPagedList(page ?? 1, 5));
        }   

        protected override void Dispose(bool disposing)
        {
            uow.Dispose();
            base.Dispose(disposing);
        }
    }
}