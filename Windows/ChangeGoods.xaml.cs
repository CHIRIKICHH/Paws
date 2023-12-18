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
    /// Логика взаимодействия для ChangeGoods.xaml
    /// </summary>
    public partial class ChangeGoods : Window
    {
        private Goods _changeGood;
        public ChangeGoods(Goods changeGood)
        {
            InitializeComponent();
            _changeGood = changeGood;
            ProductNameTextBox.Text = _changeGood.ProductName;
            ProductCategoryTextBox.Text = _changeGood.ProductCategory;
            PriceTextBox.Value = _changeGood.Price;
            AvailableCheckBox.IsChecked = _changeGood.AvailabilityInStock;
            QuantityTextBox.Text = _changeGood.Quantity.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (var context = new ApplicationContext())
                {

                    _changeGood.ProductName = ProductNameTextBox.Text;
                    _changeGood.ProductCategory = ProductCategoryTextBox.Text;
                    _changeGood.Price = PriceTextBox.Value;
                    _changeGood.AvailabilityInStock = AvailableCheckBox.IsChecked;
                    _changeGood.Quantity = int.Parse(QuantityTextBox.Text);
                    context.Update(_changeGood);
                    context.SaveChanges();
                    MessageBox.Show($"Товар {_changeGood.Id} - {_changeGood.ProductName} успішно відредаговано!", "Успішно!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!");
            }
        }
    }
}

