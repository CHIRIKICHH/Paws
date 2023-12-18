using Microsoft.EntityFrameworkCore;
using Paws.Model;
using Paws.Services;
using Paws.Windows;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paws.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            RefreshDataGrid();
            if(Employees.CurrentUser.Role == UserRole.User)
            {
                DeleButton.IsEnabled = false;
                ChangeButton.IsEnabled = false;
            }
        }

        public void RefreshDataGrid()
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    dataGrid.ItemsSource = context.Orders.Include(u => u.Goods).Include(u => u.Customer).Include(u => u.Employee).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка", ex.Message);
            }
        }

        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        double autoHeight = double.NaN;

        private void datagrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (e.RowIndex > 0 && dataGrid.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
            {
                //if (autoHeight <= 24)
                //{
                e.Height = autoHeight + 40;
                e.Handled = true;
                //}
            }
        }
        


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AddOrder(dataGrid).Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(new GetPDFReport().GetOrdersPDF(), "");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ChangeTextBox.Text) && int.TryParse(ChangeTextBox.Text, out int result))
            {
                using (var context = new ApplicationContext())
                {
                    if (context.Orders.Any(x => x.Id == result))
                    {
                        var changeOrder = context.Orders.Include(u => u.Goods).Include(u => u.Customer).Include(u => u.Employee).FirstOrDefault(x => x.Id == result);
                        var temp = new ChangeOrder(changeOrder);
                        temp.Show();
                        temp.Closed += delegate { RefreshDataGrid(); };
                    }
                    else
                    {
                        MessageBox.Show($"Замовлення з ID:{result} не знайдено!", "Помилка");
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
            if (!string.IsNullOrEmpty(RemoveTextBox.Text) && int.TryParse(RemoveTextBox.Text, out int result))
            {
                using (var context = new ApplicationContext())
                {
                    if (context.Orders.Any(x => x.Id == result))
                    {
                        var remove = context.Orders.Include(u => u.Customer).FirstOrDefault(x => x.Id == result);
                        if (MessageBox.Show($"Ви впевнені що хочете видалити Замовлення {remove.Id}:{remove.CustomerName}?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            context.Remove(remove);
                            context.SaveChanges();
                            RefreshDataGrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Замовлення з ID:{result} не знайдено!", "Помилка");
                    }
                }
            }
            else
            {
                MessageBox.Show($"Поле введення порожнє або Некоректний формат!", "Помилка");
            }
        }

        private void dataGrid_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs e)
        {
            if (e.Record != null)
            {
                Orders order = e.Record as Orders;
                ChangeTextBox.Text = order.Id.ToString();
                RemoveTextBox.Text = order.Id.ToString();
            }
        }
    }

}

