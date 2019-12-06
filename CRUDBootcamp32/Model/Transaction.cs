using CRUDBootcamp32.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CRUDBootcamp32.Model
{
    [Table("TB_M_Transaction")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int totalPrice { get; set; }
        public DateTimeOffset dateTransaction { get; set; }

        public Transaction() { }
        public Transaction(int totalPrice)
        {
            this.totalPrice = totalPrice;
            this.dateTransaction = DateTimeOffset.Now;
        }
    }
}
