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
    [Table("TB_M_Role")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }

        public Role() { }

        public Role(string name)
        {
            this.name = name;
        }
    }
}
