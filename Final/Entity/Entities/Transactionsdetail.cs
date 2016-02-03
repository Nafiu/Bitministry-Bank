using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Entities
{
    [Table("Transactionsdetails")]
    public class Transactionsdetail
    {
        public int id { get; set; }
      
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Currency")]

        [Display(Name = "Name")]
        public string Tousername { get; set; }
        public Nullable<System.DateTime> datetimes { get; set; }
        public Nullable<decimal> debit { get; set; }
        public Nullable<decimal> credit { get; set; }
        [Display(Name = "Type")]
        public string Types { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Currency")]
        [Range(1, int.MaxValue, ErrorMessage = "Please Enter Valid Currency")]
        [DataType(DataType.Currency)]
        public Nullable<decimal> Balance { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }
}
