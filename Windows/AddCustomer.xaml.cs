using Paws.Model;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        private SfDataGrid _grid;
        private ComboBox _box;
        public AddCustomer(SfDataGrid grid)
        {
            InitializeComponent();
            _grid = grid;
        }

        public AddCustomer(ComboBox box)
        {
            InitializeComponent();
            _box = box;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer newCustomer = new Customer
                {
                    Adress = AdressTextBox.Text,
                    Fullname = FullNameTextBox.Text,
                    Email = EmailTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    DiscountCard = DiscountTextBox.Text,
                };
                using (var context = new ApplicationContext())
                {
                    context.Customers.Add(newCustomer);
                    context.SaveChanges();
                    MessageBox.Show($"Клієнт {newCustomer.Fullname} успішно додано!", "Успішно!");
                    Button_Click_1(sender, e);

                    _grid.ItemsSource = context.Customers.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AdressTextBox.Text = "";
            FullNameTextBox.Text = "";
            EmailTextBox.Text = "";
            PhoneTextBox.Text = "";
            DiscountTextBox.Text = "";
        }
    }
}
