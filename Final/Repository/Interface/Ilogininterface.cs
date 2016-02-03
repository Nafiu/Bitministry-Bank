using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Entities;

namespace Repository.Interface
{
    public interface Ilogininterface 
    {
       string LoginUser(BankAccount l);
    }
}
