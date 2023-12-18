using Paws.Model;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using XAct;

namespace Paws.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeOrder.xaml
    /// </summary>
    public partial class ChangeOrder : Window
    {
        private Orders _changeOrder;
        public ChangeOrder(Orders changeOrder)
        {
            InitializeComponent();
            _changeOrder = changeOrder;
            UpdateClientsList();
            UpdateGoodsList();
            UpdateEmployeesList();
            ClientComboBox.SelectedItem = ClientComboBox.Items.ToList<Customer>().First(x => x.Id == _changeOrder.Customer.Id);
            EmployeeComboBox.SelectedItem = EmployeeComboBox.Items.ToList<Employees>().First(x => x.Id == _changeOrder.Employee.Id);
            DeliveryAdressTextBox.Text = _changeOrder.Customer.Adress;
            OderDateTimeBox.DateTime = _changeOrder.OrderDateTime;
            PaymentMethodBox.Items.Add(PaymentMethod.NonCash);
            PaymentMethodBox.Items.Add(PaymentMethod.InCash);
            PaymentMethodBox.Items.Add(PaymentMethod.ByCard);
            PaymentMethodBox.SelectedItem = _changeOrder.PaymentMethod;
            AmountTextBox.Value = _changeOrder.Amount;
            CommentTextBox.Text = _changeOrder.Comment;
            DeliveryAdressTextBox.Text = _changeOrder.DeliveryAdress;

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
            //try
            //{
                
                using (var context = new ApplicationContext())
                {
                    List<Goods> goodslist = new List<Goods>();
                    if (GoodsList.SelectedItems.ToList<object>().Count() > 0)
                    {
                        foreach (var item in GoodsList.SelectedItems.ToList<Goods>().ToList())
                        {
                            goodslist.Add(context.Goods.First(x => x.Id == item.Id));
                        }

                        _changeOrder.Customer = ClientComboBox.SelectedItem as Customer;
                        _changeOrder.Employee = EmployeeComboBox.SelectedItem as Employees;
                        _changeOrder.DeliveryAdress = DeliveryAdressTextBox.Text;
                        _changeOrder.Amount = AmountTextBox.Value;
                        _changeOrder.Comment = CommentTextBox.Text;
                        _changeOrder.OrderDateTime = OderDateTimeBox.DateTime.Value.ToUniversalTime();
                        _changeOrder.PaymentMethod = (PaymentMethod)PaymentMethodBox.SelectedItem;
                        _changeOrder.Goods = goodslist;
                        context.Orders.Update(_changeOrder);
                        context.SaveChanges();
                        MessageBox.Show($"Замовлення {_changeOrder.Id} - {_changeOrder.Customer.Fullname} успішно відредаговано!", "Успішно!");
                    }
                    else
                    {
                        MessageBox.Show("Не вибрано жодного товару", "Помилка Редагування!");
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Помилка Редагування!");
            //}
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

