using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Entities;
using Repository.Interface;
using Repository.Repositoryclass;
using System.Data.Entity.Validation;

namespace Repository.Unitofwork

{
    public class unitofworkclass :IDisposable
    {
        private DatabaseContext context;
        public unitofworkclass()
        {
            context = new DatabaseContext();
            Ubank = new BankClass(context);
            Ulogin = new LoginClass(context);
        }
        public unitofworkclass(Ilogininterface il)
        {
            Ulogin = il;
        }
        public unitofworkclass(IBankinterface ib)
        {
            Ubank = ib;
        }
       
        public IBankinterface Ubank { get; private set; }
        public Ilogininterface Ulogin { get; private set; }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void save()
        {
            if (context != null)
            {
                context.SaveChanges();
            }
           
        }
    }
}
