using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Entities;
using Repository.Interface;
using System.Web.Security;

namespace Repository.Repositoryclass
{
    public class LoginClass : Ilogininterface
    {
        private DatabaseContext context;

        public LoginClass()
        {
        }

        public LoginClass(DatabaseContext context)
        {
            this.context = context;
        }



        public string LoginUser(BankAccount l)
        {
            string encryptpass = FormsAuthentication.HashPasswordForStoringInConfigFile(l.password, "SHA1");
            var u = context.Generaltable.Where(a => a.Username.Equals(l.Username) && a.password.Equals(encryptpass)).FirstOrDefault();
            if (u != null)
            {
                FormsAuthentication.SetAuthCookie(u.Username, false);
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }
    }
}
