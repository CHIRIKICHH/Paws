using Paws.Model;
using Syncfusion.UI.Xaml.Grid;
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
    /// Логика взаимодействия для AddGoods.xaml
    /// </summary>
    public partial class AddGoods : Window
    {
        SfDataGrid _grid;
        public AddGoods(SfDataGrid grid)
        {
            InitializeComponent();
            _grid = grid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Goods newGood = new Goods
                {
                    ProductName = ProductNameTextBox.Text,
                    ProductCategory = ProductCategoryTextBox.Text,
                    Price = PriceTextBox.Value,
                    Quantity = int.Parse(QuantityTextBox.Text),
                    AvailabilityInStock = AvailableCheckBox.IsChecked,
                };

                using (var context = new ApplicationContext())
                {
                    context.Goods.Add(newGood);
                    context.SaveChanges();
                    MessageBox.Show($"Товар {newGood.ProductName} успішно додано!", "Успішно!");
                    Button_Click_1(sender, e);

                    _grid.ItemsSource = context.Goods.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
