using iTextSharp.text.pdf;
using iTextSharp.text;
using Paws.Model;
using Paws.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Microsoft.Win32;
using Paws.Services;

namespace Paws.Pages
{
    /// <summary>
    /// Логика взаимодействия для GoodsPage.xaml
    /// </summary>
    public partial class GoodsPage : Page
    {
        public GoodsPage()
        {
            InitializeComponent();
            if (Employees.CurrentUser.Role != UserRole.Administrator)
            {
                ChangeButton.IsEnabled = false;
                RemoveButton.IsEnabled = false;
            }
            RefreshDataGrid();
        }

        public void RefreshDataGrid()
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    dataGrid.ItemsSource = context.Goods.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка", ex.Message);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AddGoods(dataGrid).Show();
        }

        private void dataGrid_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs e)
        {
            if (e.Record != null)
            {
                Goods goods = e.Record as Goods;
                ChangeTextBox.Text = goods.Id.ToString();
                RemoveTextBox.Text = goods.Id.ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(new GetPDFReport().GetGoodsPDF(), "Збереження");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ChangeTextBox.Text) && int.TryParse(ChangeTextBox.Text, out int result))
            {
                using (var context = new ApplicationContext())
                {
                    if (context.Goods.Any(x => x.Id == result))
                    {
                        var changeGoods = context.Goods.FirstOrDefault(x => x.Id == result);
                        var temp = new ChangeGoods(changeGoods);
                        temp.Show();
                        temp.Closed += delegate { RefreshDataGrid(); };
                    }
                    else
                    {
                        MessageBox.Show($"Товару з ID:{result} не знайдено!", "Помилка");
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
                    if (context.Goods.Any(x => x.Id == result))
                    {
                        var remove = context.Goods.FirstOrDefault(x => x.Id == result);
                        if (MessageBox.Show($"Ви впевнені що хочете видалити товар {remove.Id}:{remove.ProductName}?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            context.Remove(remove);
                            context.SaveChanges();
                            RefreshDataGrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Товару з ID:{result} не знайдено!", "Помилка");
                    }
                }
            }
            else
            {
                MessageBox.Show($"Поле введення порожнє або Некоректний формат!", "Помилка");
            }
        }
    }
}
