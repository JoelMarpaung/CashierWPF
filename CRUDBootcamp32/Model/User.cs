using CRUDBootcamp32.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;

namespace CRUDBootcamp32.Model
{
    [Table("TB_M_User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTimeOffset createDate { get; set; }
        public Role Role { get; set; }
        public User() { }
        public User(string username, string email, string password, Role role)
        {
            this.username = username;
            this.email = email;
            this.password = password;
            this.Role = role;
            this.createDate = DateTimeOffset.Now;
        }
    }
}
