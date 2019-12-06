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
    [Table("TB_M_Supplier")]
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTimeOffset createDate { get; set; }

        MyContext myContext = new MyContext();

        public Supplier() { }
        public Supplier(String name, String email)
        {
            this.name = name;
            this.email = email;
            this.createDate = DateTimeOffset.Now;
        }

        public void delete(int id)
        {
            var supplier = myContext.Suppliers.First(s => s.Id == id);
            myContext.Suppliers.Remove(supplier);
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
