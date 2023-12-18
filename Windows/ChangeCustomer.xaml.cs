using Paws.Model;
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

namespace Paws.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeCustomer.xaml
    /// </summary>
    public partial class ChangeCustomer : Window
    {
        Customer _customer;
        public ChangeCustomer(Customer changeCustomer)
        {
            InitializeComponent();
            _customer = changeCustomer;
            FullNameTextBox.Text = changeCustomer.Fullname;
            PhoneTextBox.Text = changeCustomer.Phone;
            EmailTextBox.Text = changeCustomer.Email;
            DiscountTextBox.Text = changeCustomer.DiscountCard;
            AdressTextBox.Text = changeCustomer.Adress;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    _customer.Fullname = FullNameTextBox.Text;
                    _customer.Phone = PhoneTextBox.Text;
                    _customer.Email = EmailTextBox.Text;
                    _customer.Adress = AdressTextBox.Text;
                    _customer.DiscountCard = DiscountTextBox.Text;
                    context.Update(_customer);
                    context.SaveChanges();
                    MessageBox.Show($"Клієнт {_customer.Fullname} успішно відредаговано!", "Успішно!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка Редагування!");
            }
        }

    }
}
