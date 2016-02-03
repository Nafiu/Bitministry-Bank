using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Entities;
using Repository.Interface;
using System.Transactions;

namespace WebTest.Tests
{
    public class BankDummyRepository : IBankinterface
    {
        private List<BankAccount> banklist;
        private List<Transactionsdetail> translist;
       

        public BankDummyRepository(List<Transactionsdetail> translist, List<BankAccount> banklist)
        {
            this.translist = translist;
            this.banklist = banklist;
        }
        string use;
        public BankAccount profile(string username)
        {
            use = username;
            BankAccount b = banklist.Where(a => a.Username.Equals(use)).FirstOrDefault();
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
            translist.Add(td);
        }
        public string Withdraw(Transactionsdetail ta)
        {
            var userwithdraw = profile(ta.Username);
            if (userwithdraw.AccountBalance >= ta.Balance)
            {
                BankAccount ba = userwithdraw;
                ba.AccountBalance = ba.AccountBalance - ta.Balance;
                Transactionsdetail td = new Transactionsdetail();
                td.Username = ta.Username;
                td.Tousername = ta.Tousername;
                td.Types = "Withdraw";
                td.debit = ta.Balance;
                td.Balance = ba.AccountBalance;
                td.datetimes = DateTime.Now;
                translist.Add(td);
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
                    var to = banklist.Where(a => a.Username.Equals(ta.Tousername)).FirstOrDefault();
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
                        translist.Add(tddebit);

                        Transactionsdetail tdcredit = new Transactionsdetail();
                        tdcredit.Username = ta.Tousername;
                        tdcredit.Tousername = from.Username;
                        tdcredit.datetimes = DateTime.Now;
                        tdcredit.debit = null;
                        tdcredit.credit = ta.Balance;
                        tdcredit.Types = "Transaction";
                        tdcredit.Balance = bato.AccountBalance;
                        translist.Add(tdcredit);
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
            return "no";
        }

        public IEnumerable<Transactionsdetail> Getstatement(DateTime fromd, DateTime tod)
        {
            var ss = translist.Where(a => a.Username.Equals("John") &&
          (a.datetimes >= fromd && a.datetimes <= tod)).ToList();
            return ss;
        }
    }
}
