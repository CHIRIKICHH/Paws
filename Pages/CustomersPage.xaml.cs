using Paws.Model;
using Paws.Services;
using Paws.Windows;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Syncfusion.UI.Xaml.Grid.Converters;

namespace Paws.Pages
{
    /// <summary>
    /// Логика взаимодействия для CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();
            if (Employees.CurrentUser.Role != UserRole.Administrator)
            {
                ChangePanel.IsEnabled = false;
                RemovePanel.IsEnabled = false;
            }
            RefreshDataGrid();
        }

        public void RefreshDataGrid()
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    dataGrid.ItemsSource = context.Customers.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка", ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AddCustomer(dataGrid).Show();
        }

        private void dataGrid_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs e)
        {
            if (e.Record != null)
            {
                Customer customer = e.Record as Customer;
                ChangeTextBox.Text = customer.Id.ToString();
                RemoveTextBox.Text = customer.Id.ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ChangeTextBox.Text) && int.TryParse(ChangeTextBox.Text, out int result))
            {
                using (var context = new ApplicationContext())
                {
                    if (context.Customers.Any(x => x.Id == result))
                    {
                        var changeCustomer = context.Customers.FirstOrDefault(x => x.Id == result);
                        var temp = new ChangeCustomer(changeCustomer);
                        temp.Show();
                        temp.Closed += delegate { RefreshDataGrid(); };
                    }
                    else
                    {
                        MessageBox.Show($"Клієнта з ID:{result} не знайдено!", "Помилка");
                    }
                }
            }
            else
            {
                MessageBox.Show($"Поле введення порожнє або Некоректний формат!", "Помилка");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(RemoveTextBox.Text) && int.TryParse(RemoveTextBox.Text, out int result))
            {
                using (var context = new ApplicationContext())
                {
                    if (context.Customers.Any(x => x.Id == result))
                    {
                        var remove = context.Customers.FirstOrDefault(x => x.Id == result);
                        if (MessageBox.Show($"Ви впевнені що хочете видалити клієнта {remove.Id}:{remove.Fullname}?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            context.Remove(remove);
                            context.SaveChanges();
                            RefreshDataGrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Клієнта з ID:{result} не знайдено!", "Помилка");
                    }
                }
            }
            else
            {
                MessageBox.Show($"Поле введення порожнє або Некоректний формат!", "Помилка");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            

            
            MessageBox.Show(new GetPDFReport().GetCustomersPDF(), "Збереження");
        }


    }



}
