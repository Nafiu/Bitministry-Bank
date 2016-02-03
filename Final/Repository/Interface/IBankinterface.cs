using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Entities;

namespace Repository.Interface
{
    public interface IBankinterface
    {
        BankAccount profile(string l);
        string Transfer(Transactionsdetail ta);
        string Withdraw(Transactionsdetail ta);
        void Deposite(Transactionsdetail ta);       
        IEnumerable<Transactionsdetail> Getstatement(DateTime fromd, DateTime tod);
    }
}
