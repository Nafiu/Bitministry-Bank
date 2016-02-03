using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Entities
{
    [Table("BankAccount")]
    public class BankAccount
    {
        public int id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Username")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Password")]
        //[RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d][A-Za-z\d!@#$%^&*()_+]{7,19}$", ErrorMessage = "7-19 character should be  1caps 1small 1num 1symbol")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public Nullable<decimal> AccountBalance { get; set; }
        public virtual ICollection<Transactionsdetail> Transactionsdetails { get; set; }
    }
   
}

   
 