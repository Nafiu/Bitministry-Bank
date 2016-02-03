using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Entity.Entities
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(): base("name=AppConnString")
        {

        }


        public DbSet<BankAccount> Generaltable { get; set; }
        public DbSet<Transactionsdetail> transdetails { get; set; }
    }
}
