using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web;
using Web.Controllers;
using Entity.Entities;
using Repository.Repositoryclass;
using Repository.Unitofwork;
using WebTest.Tests.DummyRepository;
using Repository.Interface;

namespace WebTest.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTest
    {

        BankAccount ba1 = null;
        BankAccount ba2 = null;
        List<BankAccount> loginlist = null;
        LoginDummyRepository dummyrepo = null;
        unitofworkclass uow = null;
        LoginController controller = null;

        public LoginControllerTest()
        {
            ba1 = new BankAccount { id = 1, Username = "John", password = "John@123", AccountBalance = 25000 };
            ba2 = new BankAccount { id = 2, Username = "Raja", password = "Raja@123", AccountBalance = 20000 };
            loginlist = new List<BankAccount> { ba1, ba2 };
            dummyrepo = new LoginDummyRepository(loginlist);
            uow = new unitofworkclass(dummyrepo);
            controller = new LoginController(uow);
        }
      

        [TestMethod]
        public void Login()
        {
            BankAccount login = new BankAccount { Username = "John", password = "John@123" };
            var result = (RedirectToRouteResult)controller.Login(login);
            Assert.AreEqual("MyProfile", result.RouteValues["action"]);
        }
        
        //If login fail it will never allow user to MyProfile
        //This TestMethod will fail because password was wrong
        [TestMethod]
        public void LoginFail()
        {
            BankAccount login = new BankAccount { Username = "John", password = "joh@23" };
            ActionResult result = controller.Login(login);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        //If Login Fail Message shows
        [TestMethod]
        public void Loginfailmessage()
        {
            BankAccount login = new BankAccount { Username = "Raj", password = "Raja@123" };
            var result = controller.Login(login) as ViewResult;
            Assert.AreEqual("Worng Username And Password", result.ViewBag.forpas);
        }
    }
}
