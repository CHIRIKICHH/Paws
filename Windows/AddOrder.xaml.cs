using DevExpress.Mvvm.POCO;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.BC;
using Paws.Model;
using Paws.Services;
using Syncfusion.Data.Extensions;
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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private SfDataGrid _grid;
        public AddOrder(SfDataGrid grid)
        {
            InitializeComponent();
            this._grid = grid;
            UpdateClientsList();
            UpdateGoodsList();
            UpdateEmployeesList();
            ClientComboBox.SelectedItem = ClientComboBox.Items.ToList<Customer>().FirstOrDefault();
            EmployeeComboBox.SelectedItem = EmployeeComboBox.Items.ToList<Employees>().First(x => x.Id == Employees.CurrentUser.Id);
            DeliveryAdressTextBox.Text = (ClientComboBox.SelectedItem as Customer).Adress;
            OderDateTimeBox.DateTime = DateTime.Now;
            PaymentMethodBox.Items.Add(PaymentMethod.NonCash);
            PaymentMethodBox.Items.Add(PaymentMethod.InCash);
            PaymentMethodBox.Items.Add(PaymentMethod.ByCard);
        }
        private void UpdateClientsList()
        {
            using (var context = new ApplicationContext())
            {
                foreach (var item in context.Customers.ToList())
                    ClientComboBox.Items.Add(item);
            }
        }

        private void UpdateEmployeesList()
        {
            using (var context = new ApplicationContext())
            {
                foreach (var item in context.Employees.ToList())
                    EmployeeComboBox.Items.Add(item);
            }
        }
        private void UpdateGoodsList()
        {
            using (var context = new ApplicationContext())
            {
                var goodsList = new List<string>();
                foreach (var item in context.Goods.ToList())
                {
                    if (item.AvailabilityInStock.Value)
                    {
                        GoodsList.Items.Add(item);
                    }
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //new AddEmployee().Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    if (GoodsList.SelectedItems.ToList<object>().Count() > 0)
                    {
                        List<Goods> goodslist = new List<Goods>();
                        foreach (var item in GoodsList.SelectedItems.ToList<Goods>().ToList())
                        {
                            goodslist.Add(context.Goods.First(x => x.Id == item.Id));
                        }

                        Orders newOrder = new Orders
                        {
                            Customer = context.Customers.FirstOrDefault(x => x.Id == ((Customer)ClientComboBox.SelectedItem).Id),
                            Employee = context.Employees.FirstOrDefault(x => x.Id == ((Employees)EmployeeComboBox.SelectedItem).Id),
                            Amount = AmountTextBox.Value,
                            Comment = CommentTextBox.Text,
                            DeliveryAdress = DeliveryAdressTextBox.Text,
                            Goods = goodslist,
                            OrderDateTime = OderDateTimeBox.DateTime.Value.ToUniversalTime(),
                            PaymentMethod = (PaymentMethod)PaymentMethodBox.SelectedItem
                        };


                        context.Orders.Add(newOrder);
                        context.SaveChanges();
                        MessageBox.Show($"Замовлення {newOrder.Id} - {newOrder.CustomerName} - {newOrder.GoodsNames} - {newOrder.Amount}₴ успішно оформлений!", "Успішно!");
                        if(MessageBox.Show($"Надіслати чек на email клієнта? \nEmail: {newOrder.Customer.Email}", "Відправка", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            string result = new EmailService().SendToEmail(newOrder);
                            MessageBox.Show(result,"Result");
                        }
                        Button_Click_1(sender, e);

                        _grid.ItemsSource = context.Orders.Include(u => u.Goods).Include(u => u.Customer).Include(u => u.Employee).ToList();
                        
                    }
                    else
                    {
                        MessageBox.Show("Не вибрано жодного товару", "Помилка Редагування!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void ClientComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DeliveryAdressTextBox.Text = (ClientComboBox.SelectedItem as Customer).Adress;
        }

        private void GoodsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GoodsList.DefaultText = $"Выбрано {GoodsList.SelectedItems.ToList<Goods>().Count()}";
            decimal? temp = 0;
            foreach (var item in GoodsList.SelectedItems.ToList<Goods>())
            {
                temp += item.Price;
            }
            AmountTextBox.Value = temp.Value;
        }
    }
}
