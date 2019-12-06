using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDBootcamp32.Model
{
    [Table("TB_M_Transaction_Item")]
    public class TransactionItem
    {
        [Key]
        public int Id { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public Transaction Transaction { get; set; }
        public Item Item { get; set; }

        public TransactionItem() { }
        public TransactionItem(int quantity, int price, Transaction transaction, Item item)
        {
            this.quantity = quantity;
            this.price = price;
            this.Transaction = transaction;
            this.Item = item;
        }
    }
}
