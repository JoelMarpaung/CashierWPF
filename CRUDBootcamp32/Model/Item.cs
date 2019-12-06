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
    [Table("TB_M_Item")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int stock { get; set; }

        public Supplier Supplier { get; set; }
        MyContext myContext = new MyContext();

        public Item() { }
        public Item(String name, int price, int stock, Supplier supplier)
        {
            this.name = name;
            this.price = price;
            this.stock = stock;
            this.Supplier = supplier;
        }

        public void delete(int id)
        {
            var item = myContext.Items.First(s => s.Id == id);
            myContext.Items.Remove(item);
            var result = myContext.SaveChanges();
            if (result > 0)
            {
                MessageBox.Show("Data has been removed");
            }
            else
            {
                MessageBox.Show("Data couldn't been removed");
            }
        }
    }
}
