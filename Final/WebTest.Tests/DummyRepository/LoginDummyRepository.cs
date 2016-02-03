using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Entities;
using Repository.Interface;


namespace WebTest.Tests.DummyRepository
{
    public class LoginDummyRepository : Ilogininterface
    {
        private List<BankAccount> loginlist;
        public LoginDummyRepository(List<BankAccount> loginlist)
        {
            this.loginlist = loginlist;
        }

        public string LoginUser(BankAccount l)
        {
            var u = loginlist.Where(a => a.Username.Equals(l.Username) && a.password.Equals(l.password)).FirstOrDefault();
            if (u != null)
            {
               
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }
    }
}
