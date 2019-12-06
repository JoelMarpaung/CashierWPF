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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyContext myContext = new MyContext();
        int supplier_id;
        int role_id;
        int item_id;
        string old_email;
        int cart_id;
        int total_price;
        List<TransactionItem> cart = new List<TransactionItem>();
        User user = new User();

        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            //Initialize Data Grid
            ShowDataGridSupplier();
            ShowDataGridItem();
            ShowDataCart();
            ShowUsersData();
            //Initialize Data Combobox
            ShowDataComboBoxSupplier();
            //Initialize Data List
            ShowItemList();
            ShowRoleList();
            btnRemoveShop.IsEnabled = false;
            btnSubmit.IsEnabled = false;
            btnClear.IsEnabled = false;
            txtPay.IsEnabled = false;
            if (this.user.Role.Id != 1)
            {
                TabUser.Visibility = Visibility.Hidden;
            }

        }

        #region Supplier
        private void BtnSubmit(object sender, RoutedEventArgs e)
        {
            if (TxtName.Text == "" || TxtEmail.Text == "")
            {
                MessageBox.Show("All fields must be filled in");
            }
            else
            {
                var data = myContext.Suppliers.Where(s => s.email == TxtEmail.Text).FirstOrDefault();
                if (data == null)
                {
                    var push = new Supplier(TxtName.Text, TxtEmail.Text);
                    myContext.Suppliers.Add(push);
                    var result = myContext.SaveChanges();
                    refreshSupplier();
                    if (result > 0)
                    {
                        MessageBox.Show("Data has been saved");
                        CreateMailItem(TxtName.Text, TxtEmail.Text);
                    }
                    else
                    {
                        MessageBox.Show("Data couldn't been saved");
                    }
                }
                else
                {
                    MessageBox.Show("Email already registered");
                }
            }

        }

        private void CreateMailItem(String name, String email)
        {
            Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mailItem.Subject = "Latihan Pengiriman menggunakan WPF";
            mailItem.To = email;
            mailItem.Body = "Penambahan data dengan nama: " + name + " dan email: " + email + " sudah berhasil disimpan ke database";
            mailItem.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;
            mailItem.Display(false);
        }

        private void ShowDataGridSupplier()
        {
            DataSupplier.ItemsSource = myContext.Suppliers.ToList();
        }

        private void DataSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var data = DataSupplier.SelectedItem;
                TxtId.Text = (DataSupplier.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                TxtName.Text = (DataSupplier.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                TxtEmail.Text = (DataSupplier.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
                old_email = (DataSupplier.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            }
            catch (Exception) { }
        }

        private void EmailValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9.@]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (TxtId.Text == "")
            {
                MessageBox.Show("No data selected");
            }
            else if (TxtName.Text == "" || TxtEmail.Text == "")
            {
                MessageBox.Show("All data must filled in");
            }
            else
            {
                if (old_email == TxtEmail.Text)
                {
                    int id = Convert.ToInt32(TxtId.Text);
                    var supplier = myContext.Suppliers.First(s => s.Id == id);
                    supplier.name = TxtName.Text;
                    supplier.email = TxtEmail.Text;
                    var result = myContext.SaveChanges();
                    if (result > 0)
                    {
                        MessageBox.Show("Data has been updated");
                    }
                    else
                    {
                        MessageBox.Show("Data couldn't been saved");
                    }
                }
                else
                {
                    var data = myContext.Suppliers.Where(s => s.email == TxtEmail.Text).FirstOrDefault();
                    if (data == null)
                    {
                        int id = Convert.ToInt32(TxtId.Text);
                        var supplier = myContext.Suppliers.First(s => s.Id == id);
                        supplier.name = TxtName.Text;
                        supplier.email = TxtEmail.Text;
                        var result = myContext.SaveChanges();
                        if (result > 0)
                        {
                            MessageBox.Show("Data has been updated");
                        }
                        else
                        {
                            MessageBox.Show("Data couldn't been saved");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email already registered");
                    }
                }
            }
            refreshSupplier();
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            if (TxtId.Text == "")
            {
                MessageBox.Show("No data selected");
            }
            else
            {
                var selectedOption = MessageBox.Show("Are you sure want to delete supplier ?", "Delete Supplier", MessageBoxButton.YesNo);
                if (selectedOption == MessageBoxResult.Yes)
                {
                    try
                    {
                        var delete = new Supplier();
                        delete.delete(Convert.ToInt32(TxtId.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Data couldn't deleted because used by Item data");
                    }
                }
                refreshSupplier();
            }
        }

        private void btnRefSupplier_Click(object sender, RoutedEventArgs e)
        {
            refreshSupplier();
        }

        private void refreshSupplier()
        {
            TxtId.Text = "";
            TxtName.Text = "";
            TxtEmail.Text = "";
            ShowDataGridSupplier();
            ShowDataGridItem();
            ShowDataComboBoxSupplier();
        }

        #endregion 

        #region Item
        private void NameValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9!]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SupplierList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                supplier_id = Convert.ToInt32(SupplierList.SelectedValue.ToString());
            }
            catch (Exception) { }
        }

        public void ShowDataGridItem()
        {
            DataItem.ItemsSource = myContext.Items.ToList();
        }

        public void ShowDataComboBoxSupplier()
        {
            SupplierList.ItemsSource = myContext.Suppliers.ToList();
        }

        private void SubmitItem_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNameItem.Text == "" || TxtPriceItem.Text == "" || TxtStockItem.Text == "" || supplier_id == 0)
            {
                MessageBox.Show("All fields must filled in");
            }
            else
            {
                var item = myContext.Items.Where(i => i.name.ToLower() == TxtNameItem.Text.ToLower()).FirstOrDefault();
                if (item == null)
                {
                    var supplier = myContext.Suppliers.Where(s => s.Id == supplier_id).FirstOrDefault();
                    var push = new Item(TxtNameItem.Text, Convert.ToInt32(TxtPriceItem.Text), Convert.ToInt32(TxtStockItem.Text), supplier);
                    myContext.Items.Add(push);
                    var result = myContext.SaveChanges();
                    if (result > 0)
                    {
                        MessageBox.Show("Data has been saved");
                    }
                    else
                    {
                        MessageBox.Show("Data couldn't been saved");
                    }
                }
                else
                {
                    item.stock += Convert.ToInt32(TxtStockItem.Text);
                    var result = myContext.SaveChanges();
                    if (result > 0)
                    {
                        MessageBox.Show("Data has been Updated");
                    }
                    else
                    {
                        MessageBox.Show("Data couldn't been saved");
                    }
                }
                refreshItem();
                RefreshShop();
            }
        }

        private void DataItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var data = DataItem.SelectedItem;
                TxtIdItem.Text = (DataItem.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                TxtNameItem.Text = (DataItem.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                TxtPriceItem.Text = (DataItem.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
                TxtStockItem.Text = (DataItem.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
                SupplierList.Text = (DataItem.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text;
                int itemId = Convert.ToInt32(TxtIdItem.Text);
                var item = myContext.Items.First(i => i.Id == itemId);
                supplier_id = item.Supplier.Id;
            }
            catch (Exception) { }
        }

        private void updateItem_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNameItem.Text == "" || TxtPriceItem.Text == "" || TxtStockItem.Text == "")
            {
                MessageBox.Show("All fields must be filled in");
            }
            else
            {
                var supplier = myContext.Suppliers.First(s => s.Id == supplier_id);
                int id = Convert.ToInt32(TxtIdItem.Text);
                var item = myContext.Items.First(s => s.Id == id);
                item.name = TxtNameItem.Text;
                item.price = Convert.ToInt32(TxtPriceItem.Text);
                item.stock = Convert.ToInt32(TxtStockItem.Text);
                item.Supplier = supplier;
                var result = myContext.SaveChanges();
                if (result > 0)
                {
                    refreshItem();
                    RefreshShop();
                    MessageBox.Show("Data has been updated");
                }
                else
                {
                    MessageBox.Show("Data couldn't been saved");
                }
            }
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            if (TxtIdItem.Text == "")
            {
                MessageBox.Show("No data selected");
            }
            else
            {
                var selectedOption = MessageBox.Show("Are you sure want to delete item ?", "Delete Item", MessageBoxButton.YesNo);
                if (selectedOption == MessageBoxResult.Yes)
                {
                    try
                    {
                        var delete = new Item();
                        delete.delete(Convert.ToInt32(TxtIdItem.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Data couldn't deleted because used by Transaction data");
                    }
                }
                refreshItem();
                RefreshShop();
            }

        }

        private void btnRefItem_Click(object sender, RoutedEventArgs e)
        {
            refreshItem();
        }

        private void refreshItem()
        {
            TxtIdItem.Text = "";
            TxtNameItem.Text = "";
            TxtPriceItem.Text = "";
            TxtStockItem.Text = "";
            ShowDataGridItem();
            ShowDataComboBoxSupplier();
            SupplierList.Text = "--Choose Supplier--";
            supplier_id = 0;
            ShowItemList();
        }

        #endregion Item

        #region Shop
        private void ShowItemList()
        {
            listItem.ItemsSource = myContext.Items.ToList();
        }

        private void ShowDataCart()
        {
            dataCart.ItemsSource = cart.ToList();
        }


        private void listItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                item_id = Convert.ToInt32(listItem.SelectedValue.ToString());
                var item = myContext.Items.Find(item_id);
                txtStockShop.Text = item.stock.ToString();
                txtPriceShop.Text = item.price.ToString();
                txtQuantityShop.Text = "";
            }
            catch (Exception) { }
        }

        private void btnAddCart_Click(object sender, RoutedEventArgs e)
        {
            if (txtQuantityShop.Text == "" || txtPriceShop.Text == "" || txtStockShop.Text == "")
            {
                MessageBox.Show("All fields must be filled in");
            }
            else if (Convert.ToInt32(txtQuantityShop.Text) <= 0)
            {
                MessageBox.Show("Quantity must be one or greater");
            }
            else if (Convert.ToInt32(txtQuantityShop.Text) > Convert.ToInt32(txtStockShop.Text))
            {
                MessageBox.Show("Quantity to buy is more than stock");
            }
            else
            {

                var data = cart.Find(c => c.Item.Id == item_id);
                var item = myContext.Items.Find(item_id);
                if (data != null)
                {
                    int max = item.stock - data.quantity;
                    if (Convert.ToInt32(txtQuantityShop.Text) > max)
                    {
                        MessageBox.Show("Quantity to buy + quantity already in cart of the same item is more than stock");
                    }
                    else
                    {
                        cart.Add(new TransactionItem() { Item = item, price = Convert.ToInt32(txtPriceShop.Text) * (Convert.ToInt32(txtQuantityShop.Text) + data.quantity), quantity = Convert.ToInt32(txtQuantityShop.Text) + data.quantity });
                        cart.Remove(data);
                    }
                }
                else
                {
                    cart.Add(new TransactionItem() { Item = item, price = Convert.ToInt32(txtPriceShop.Text) * Convert.ToInt32(txtQuantityShop.Text), quantity = Convert.ToInt32(txtQuantityShop.Text) });
                }
                ShowDataCart();
                btnRemoveShop.IsEnabled = true;
                btnSubmit.IsEnabled = true;
                btnClear.IsEnabled = true;
                txtPay.IsEnabled = true;
                countTotalPrice();
                RefreshShop();
            }

        }

        private void dataCart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cart_id = dataCart.SelectedIndex;
        }

        private void btnRemoveShop_Click(object sender, RoutedEventArgs e)
        {
            cart.RemoveAt(cart_id);
            ShowDataCart();
            if (!cart.Any())
            {
                btnRemoveShop.IsEnabled = false;
                btnSubmit.IsEnabled = false;
                btnClear.IsEnabled = false;
                txtPay.IsEnabled = false;
            }
            countTotalPrice();
            RefreshShop();
        }

        private void countTotalPrice()
        {
            total_price = 0;
            foreach (var data in cart)
            {
                total_price += data.price;
            }
            txtTotalPrice.Text = total_price.ToString();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearCart();
            RefreshShop();
        }

        private void ClearCart()
        {
            cart.RemoveRange(0, cart.Count);
            ShowDataCart();
            if (!cart.Any())
            {
                btnRemoveShop.IsEnabled = false;
                btnSubmit.IsEnabled = false;
                btnClear.IsEnabled = false;
                txtPay.IsEnabled = false;
            }
        }

        private void RefreshShop()
        {
            countTotalPrice();
            txtStockShop.Text = "";
            txtPriceShop.Text = "";
            txtQuantityShop.Text = "";
            txtPay.Text = "";
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtPay.Text == "")
            {
                MessageBox.Show("Please fill the pay to submit the transaction");
            }
            else if (Convert.ToInt32(txtPay.Text) < Convert.ToInt32(txtTotalPrice.Text))
            {
                MessageBox.Show("Pay must be more than total price.");
            }
            else
            {
                var push = new Transaction(Convert.ToInt32(txtTotalPrice.Text));
                myContext.Transaction.Add(push);
                myContext.SaveChanges();
                var transaction = myContext.Transaction.OrderByDescending(t => t.Id).FirstOrDefault();
                foreach (var data in cart)
                {
                    var item = myContext.Items.Find(data.Item.Id);
                    var push2 = new TransactionItem(data.quantity, data.price, transaction, item);
                    myContext.TransactionItem.Add(push2);
                    myContext.SaveChanges();

                    item.stock = item.stock - data.quantity;
                    myContext.SaveChanges();
                }
                int returnMoney = Convert.ToInt32(txtPay.Text) - Convert.ToInt32(txtTotalPrice.Text);
                ClearCart();
                RefreshShop();
                refreshItem();
                ShowItemList();
                ShowDataGridItem();
                var selectedOption = MessageBox.Show("Transaction success. The return = " + returnMoney + "\nDo you want to print the transaction report", "Transaction success", MessageBoxButton.YesNo);
                if (selectedOption == MessageBoxResult.Yes)
                {
                    try
                    {
                        TransactionReportForm form = new TransactionReportForm(transaction.Id);
                        form.Show();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Transaction couldn't be printed");
                    }
                }
            }
        }
        #endregion

        #region User
        private void ShowUsersData()
        {
            DataGridUsers.ItemsSource = myContext.Users.ToList();
        }
        private void ShowRoleList()
        {
            RoleList.ItemsSource = myContext.Roles.ToList();
        }

        private void RoleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                role_id = Convert.ToInt32(RoleList.SelectedValue.ToString());
            }
            catch (Exception) { }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (TxtUserNameUser.Text == "" || TxtEmailUser.Text == "" || role_id == 0)
            {
                MessageBox.Show("All fields must be filled in");
            }
            else
            {

                var dataByName = myContext.Users.FirstOrDefault(u => u.username.ToLower() == TxtUserNameUser.Text.ToLower());
                var dataByEmail = myContext.Users.FirstOrDefault(u => u.email.ToLower() == TxtEmailUser.Text.ToLower());
                if (dataByName != null)
                {
                    MessageBox.Show("Username already registered.\n Please choose another username");
                }
                else if (dataByEmail != null)
                {
                    MessageBox.Show("Email already registered.\n Please choose another email");
                }
                else
                {
                    var role = myContext.Roles.Find(role_id);
                    Guid id = Guid.NewGuid();
                    string password = EncryptPassword(id.ToString());
                    var push = new User(TxtUserNameUser.Text, TxtEmailUser.Text, password, role);
                    myContext.Users.Add(push);
                    var result = myContext.SaveChanges();
                    if (result > 0)
                    {
                        MessageBox.Show("User succesfully registered");
                        CreateMailItem(TxtUserNameUser.Text, id.ToString(), TxtEmailUser.Text);
                    }
                    else
                    {
                        MessageBox.Show("Data couldn't been saved");
                    }
                    refreshUser();
                }

            }
        }

        private void CreateMailItem(String username, String password, String email)
        {
            Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mailItem.Subject = "New user created";
            mailItem.To = email;
            mailItem.Body = "Your account has been registered successfull.\nUsername : " + username + "\nPassword : " + password + "\nPlease change your password after your login is successfull.";
            mailItem.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;
            mailItem.Display(false);
        }

        private void refreshUser()
        {
            TxtUserNameUser.Text = "";
            TxtEmailUser.Text = "";
            TxtChangeOld.Password = "";
            TxtChangeNew.Password = "";
            TxtChangeConfirm.Password = "";
            ShowUsersData();
            ShowRoleList();
            RoleList.Text = "--Choose Role--";
            role_id = 0;
        }

        private string EncryptPassword(string pwd)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(pwd));
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string password = EncryptPassword(TxtChangeOld.Password);
            var userChange = myContext.Users.First(s => s.Id == user.Id);
            if (TxtChangeOld.Password == "" || TxtChangeNew.Password == "" || TxtChangeConfirm.Password == "")
            {
                MessageBox.Show("All fields must be filled in");
            }
            else if (TxtChangeNew.Password != TxtChangeConfirm.Password)
            {
                MessageBox.Show("New password and confirm password doesn't match");
            }
            else if (password != userChange.password)
            {
                MessageBox.Show("Your old password is not incorrect.\nPlease type the corect password");
            }
            else
            {
                userChange.password = EncryptPassword(TxtChangeNew.Password);
                var result = myContext.SaveChanges();
                MessageBox.Show("Password succesfully changed");
                refreshUser();
            }
        }
        #endregion
    }
}