using CRUDBootcamp32.Context;
using CRUDBootcamp32.Model;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace CRUDBootcamp32
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        MyContext myContext = new MyContext();
        public Login()
        {
            InitializeComponent();
            myContext.Roles.ToList();
            myContext.Users.ToList();
        }

        private void Forgot_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TxtUserNameLogin.Text == "" || TxtPasswordLogin.Password == "")
            {
                MessageBox.Show("All fields must be field in");
            }
            else
            {
                string password = EncryptPassword(TxtPasswordLogin.Password);
                var user = myContext.Users.FirstOrDefault(u => u.username == TxtUserNameLogin.Text && u.password == password);
                if (user == null)
                {
                    MessageBox.Show("Username or Password incorrect.");
                    RefreshLogin();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    this.Close();
                }
            }
        }
        private void RefreshLogin()
        {
            TxtUserNameLogin.Text = "";
            TxtPasswordLogin.Password = "";
        }
        private string EncryptPassword(string pwd)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(pwd));
        }

        //private void BtnForgot_Click(object sender, RoutedEventArgs e)
        //{
        //    ForgotPassword forgotPassword = new ForgotPassword();
        //    forgotPassword.Show();
        //    this.Close();
        //}
    }
}
