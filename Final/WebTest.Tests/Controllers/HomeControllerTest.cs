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
using PagedList;

namespace WebTest.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        BankAccount ba1 = null;
        BankAccount ba2 = null;
        Transactionsdetail td1 = null;
        Transactionsdetail td2 = null; Transactionsdetail td3 = null;
        List<BankAccount> banklist = null;
        List<Transactionsdetail> translist = null;
        BankDummyRepository dummyrepo=null;
        unitofworkclass uow=null;
        HomeController controller = null;
      
        public HomeControllerTest()
        {
            ba1 = new BankAccount { id = 1, Username = "John", password = "John@123", AccountBalance = 25000 };
            ba2 = new BankAccount { id = 2, Username = "Raja", password = "Raja@123", AccountBalance = 20000 };

            td1 = new Transactionsdetail
            {id = 1,Username = "John",Tousername = "John",datetimes = DateTime.Now,debit = null,credit = 25000,
                Types = "Deposit",Balance = 25000 };

            td2 = new Transactionsdetail
            { id = 2, Username = "Raja", Tousername = "Raja", datetimes = DateTime.Now,debit = null,credit = 20000,
                Types = "Deposit", Balance = 20000 };

            td3 = new Transactionsdetail
            {id = 3,Username = "John",Tousername = "John",datetimes = DateTime.Now,debit = 1000,credit = null,
                Types = "Withdraw",Balance = 20000};

            banklist = new List<BankAccount> { ba1, ba2 };
            translist = new List<Transactionsdetail> { td1, td2,td3 };
            dummyrepo = new BankDummyRepository(translist, banklist);
            uow = new unitofworkclass(dummyrepo);
            controller = new HomeController(uow);
        }

        [TestMethod]
        public void Deposite()
        {            
            Transactionsdetail t = new Transactionsdetail { Username = "John", Balance = 500 };
            var result = controller.Deposite(t) as ViewResult;
            Assert.AreEqual("Deposited sucessfully", result.ViewBag.transfer);
        }
        [TestMethod]
        public void Withdraw()
        {
            Transactionsdetail t = new Transactionsdetail { Username = "Raja", Balance = 1500 };
            var result = controller.Withdraw(t) as ViewResult;
            Assert.AreEqual("Withdraw sucessfully", result.ViewBag.transfer);
        }

        [TestMethod]
        public void Withdraw_money_more_than_your_account()
        {
            Transactionsdetail t = new Transactionsdetail { Username = "Raja", Balance = 1500000 };
            var result = controller.Withdraw(t) as ViewResult;
            Assert.AreEqual("Withdraw Failed Check your account name and your balance", result.ViewBag.transfer);
        }

        [TestMethod]
        public void Transfer()
        {
            Transactionsdetail t = new Transactionsdetail { Username = "Raja",Tousername="John", Balance = 1500 };
            var result = controller.Transfer(t) as ViewResult;
            Assert.AreEqual("Transaction completed Sucessfully", result.ViewBag.transfer);
        }

        [TestMethod]
        public void Transfer_to_not_having_account()
        {
            Transactionsdetail t = new Transactionsdetail { Username = "Raja", Tousername = "Sam", Balance = 1500 };
            var result = controller.Transfer(t) as ViewResult;
            Assert.AreEqual("Transaction Failed due to receiver account is not available (or) you not having the balance", result.ViewBag.transfer);
        }

        [TestMethod]
        public void GetStatement()
        {
            var result = controller.Statement("01-01-2015", "01-12-2018",null) as ViewResult;
            var model = (PagedList<Transactionsdetail>)result.ViewData.Model;
            var list = model.ToList();
            CollectionAssert.Contains(list, td1);
            CollectionAssert.Contains(list, td3);
        }
        [TestMethod]
        public void GetStatement_on_Not_Transfered()
        {
            var result = controller.Statement("01-01-2014", "01-01-2015", null) as ViewResult;
            Assert.AreEqual("No Transaction Occured", result.ViewBag.er);
        }
    }
}
