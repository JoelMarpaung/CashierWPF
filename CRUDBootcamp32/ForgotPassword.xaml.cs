using CRUDBootcamp32.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CRUDBootcamp32
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        MyContext myContext = new MyContext();
        //Random random = new Random();
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (TxtEmailForgot.Text == "")
            {
                MessageBox.Show("Please fill in the email");
            }
            else
            {
                var user = myContext.Users.FirstOrDefault(u => u.email == TxtEmailForgot.Text);
                if (user != null)
                {
                    Guid id = Guid.NewGuid();
                    string resetPwd = id.ToString();
                    user.password = EncryptPassword(resetPwd);
                    myContext.SaveChanges();
                    CreateMailItem(user.username, resetPwd, user.email);
                    RefreshForgot();
                }
                MessageBox.Show("Password has been reset.\nPlease check your email");

            }
        }
        private string EncryptPassword(string pwd)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(pwd));
        }
        private void CreateMailItem(String username, String password, String email)
        {
            Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mailItem.Subject = "Reset Password";
            mailItem.To = email;
            mailItem.Body = "Reset password is successfull.\nUsername : " + username + "\nPassword : " + password + "\nPlease change your password after your login is successfull.";
            mailItem.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;
            mailItem.Display(false);
        }

        private void RefreshForgot()
        {
            TxtEmailForgot.Text = "";
        }
    }
}
