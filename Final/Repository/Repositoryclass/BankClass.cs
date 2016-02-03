using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Entity.Entities;
using Repository.Interface;
using System.Web.Security;
using System.Web;
using System.Transactions;

namespace Repository.Repositoryclass
{
    public class BankClass : IBankinterface
    {
        private DatabaseContext context;
        public BankClass(DatabaseContext context)
        {
            this.context = context;          
        }   
        public BankAccount profile(string user)
        {
            BankAccount b = context.Generaltable.Where(a => a.Username.Equals(user)).FirstOrDefault();
            return b;
        }
        public void Deposite(Transactionsdetail ta)
        {
            var userdeposite = profile(ta.Username);
            BankAccount ba = userdeposite;
            ba.AccountBalance = ba.AccountBalance + ta.Balance;   

            Transactionsdetail td = new Transactionsdetail();   
            td.Username = ta.Username;
            td.Types = "Deposit";
            td.Tousername = ta.Tousername;
            td.credit = ta.Balance;
            td.datetimes = DateTime.Now;
            td.Balance = ba.AccountBalance;
            context.transdetails.Add(td);
        }

        public string Withdraw(Transactionsdetail ta)
        {
            var userwithdraw = profile(ta.Username);
            if (userwithdraw.AccountBalance >= ta.Balance)            {
                BankAccount ba = userwithdraw;                    
                ba.AccountBalance = ba.AccountBalance - ta.Balance;
                Transactionsdetail td = new Transactionsdetail();
                td.Username = ta.Username;
                td.Tousername = ta.Tousername;
                td.Types = "Withdraw";
                td.debit = ta.Balance;
                td.Balance = ba.AccountBalance;
                td.datetimes = DateTime.Now;
                context.transdetails.Add(td);
                return "Completed";
            }
            return "Failed";
        }
        public string Transfer(Transactionsdetail ta)
        {
           
            bool debitmoney = false;
            bool creditmoney = false;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var from = profile(ta.Username);
                    var to = context.Generaltable.Where(a => a.Username.Equals(ta.Tousername)).FirstOrDefault();
                    if (from.AccountBalance >= ta.Balance && to != null)
                    {
                        BankAccount bafrom = from;             
                        bafrom.AccountBalance = bafrom.AccountBalance - ta.Balance;

                        BankAccount bato = to;         
                        bato.AccountBalance = bato.AccountBalance + ta.Balance;

                        Transactionsdetail tddebit = new Transactionsdetail();
                        tddebit.Username = from.Username;
                        tddebit.Tousername = ta.Tousername;
                        tddebit.datetimes = DateTime.Now;
                        tddebit.debit = ta.Balance;
                        tddebit.credit = null;
                        tddebit.Balance = bafrom.AccountBalance;
                        tddebit.Types = "Transaction";
                        context.transdetails.Add(tddebit);

                        Transactionsdetail tdcredit = new Transactionsdetail();
                        tdcredit.Username = ta.Tousername;
                        tdcredit.Tousername = from.Username;
                        tdcredit.datetimes = DateTime.Now;
                        tdcredit.debit = null;
                        tdcredit.credit = ta.Balance;
                        tdcredit.Types = "Transaction";
                        tdcredit.Balance = bato.AccountBalance;
                        context.transdetails.Add(tdcredit);
                        debitmoney = true;
                        creditmoney = true;
                        if (debitmoney && creditmoney) 
                        {
                            ts.Complete();
                            return "completed";
                        }
                    }
                }
            }
            catch
            {
                return "Failed";          
            }
            return "NoTransaction";        
    }

        public IEnumerable<Transactionsdetail> Getstatement(DateTime fromd,DateTime tod)
        {
           var ss = context.transdetails.Where(a => a.Username.Equals(HttpContext.Current.User.Identity.Name) &&
          (a.datetimes >= fromd && a.datetimes <= tod)).ToList();
            return ss;
        }
    }
}
