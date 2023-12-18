using Microsoft.EntityFrameworkCore;
using Paws.Model;
using Syncfusion.Data.Extensions;
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
using XAct;

namespace Paws.Pages
{
    /// <summary>
    /// Логика взаимодействия для StatisticPage.xaml
    /// </summary>
    public partial class StatisticPage : Page
    {
        public StatisticPage()
        {
            InitializeComponent();
            RadioButton_Checked(TodayRadioButton, null);
            StatsTypeComboBox.Items.Add("Статистика по співробітникам");
            StatsTypeComboBox.Items.Add("Статистика з продажу");
            StatsTypeComboBox.SelectedIndex = 0;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                RadioButton selected = (RadioButton)sender;
                if (selected.Content.ToString() == "За період")
                {
                    SelectDateGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    SelectDateGrid.Visibility = Visibility.Hidden;
                }


                switch (selected.Content)
                {
                    case "За сьогодні": StartDate.DateTime = DateTime.Now.Date; EndDate.DateTime = DateTime.Now.AddDays(1).Date; break;
                    case "За вчора": StartDate.DateTime = DateTime.Now.AddDays(-1).Date; EndDate.DateTime = DateTime.Now.Date; break;
                    case "За 3 дні": StartDate.DateTime = DateTime.Now.AddDays(-2).Date; EndDate.DateTime = DateTime.Now.AddDays(1).Date; break;
                    case "За тиждень": StartDate.DateTime = DateTime.Now.AddDays(-6).Date; EndDate.DateTime = DateTime.Now.AddDays(1).Date; break;
                    case "За період":  break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (StatsTypeComboBox.SelectedItem.ToString() == "Статистика по співробітникам")
            {
                EmployeeStatsDataGrid.ItemsSource = GetStatistics();
                OrdersStatsDataGrid.Visibility = Visibility.Hidden;
                EmployeeStatsDataGrid.Visibility = Visibility.Visible;
            }
            else
            {
                OrdersStatsDataGrid.ItemsSource = GetSalesStats();
                OrdersStatsDataGrid.Visibility = Visibility.Visible;
                EmployeeStatsDataGrid.Visibility = Visibility.Hidden;
            }
        }
        private List<EmployeeStats> GetStatistics()
        {
            var statistics = new List<EmployeeStats>();
            try
            {
                using (var context = new ApplicationContext())
                {
                    foreach (var item in context.Employees.ToList())
                    {
                        statistics.Add(
                            new EmployeeStats()
                            {
                                Employee = item,
                                Sales = context.Orders.Include(u => u.Employee).Where(x =>StartDate.DateTime.Value.ToUniversalTime() <= x.OrderDateTime.Value.ToUniversalTime() && EndDate.DateTime.Value.ToUniversalTime() >= x.OrderDateTime.Value.ToUniversalTime()).Where(x => x.Employee.Id == item.Id).Count(),
                                AmountSales = context.Orders.Include(u => u.Employee).Where(x => x.Employee.Id == item.Id && StartDate.DateTime.Value.ToUniversalTime() <= x.OrderDateTime.Value.ToUniversalTime() && EndDate.DateTime.Value.ToUniversalTime() >= x.OrderDateTime.Value.ToUniversalTime()).Where(x => x.Employee.Id == item.Id).Sum(x => x.Amount.Value)
                            }
                            ) ;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
            return statistics;
        }
        private List<SalesStats> GetSalesStats()
        {
            var salesStats = new List<SalesStats>();
            try
            {
                using(var context = new ApplicationContext())
                {
                    var OrdersInDateRange = context.Orders.Include(x => x.Goods).Where(x => StartDate.DateTime.Value.ToUniversalTime() <= x.OrderDateTime.Value.ToUniversalTime() && EndDate.DateTime.Value.ToUniversalTime() >= x.OrderDateTime.Value.ToUniversalTime());
                    salesStats.Add(new SalesStats()
                    {
                        QuantityOfOrders = OrdersInDateRange.Count(),
                        QuantityOfSaleGoods = OrdersInDateRange.Select(x => x.Goods.Count).Sum(),
                        AllOrdersSum = OrdersInDateRange.Select(x => x.Amount).Sum(),
                        InCashCount = OrdersInDateRange.Where(x => x.PaymentMethod == PaymentMethod.InCash).Count(),
                        ByCardCount = OrdersInDateRange.Where(x => x.PaymentMethod == PaymentMethod.ByCard).Count(),
                        NonCash  = OrdersInDateRange.Where(x => x.PaymentMethod == PaymentMethod.NonCash).Count()
                    });
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
            return salesStats;
        }
        
    }
}

