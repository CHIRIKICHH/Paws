using Paws.Model;
using Paws.Pages;
using Paws.Services;
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
using System.Windows.Threading;
using XAct;

namespace Paws
{
    /// <summary>
    /// Логика взаимодействия для MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        DispatcherTimer Timer { get; set; }
        public MainMenuWindow()
        {
            InitializeComponent();
            MainFrame.Content = new OrdersPage();
            CurrentUser.Content = Employees.CurrentUser.Login;
            UserStatusBox.Items.Add(UserStatus.Online);
            UserStatusBox.Items.Add(UserStatus.Break);
            UserStatusBox.Items.Add(UserStatus.Offline);
            if (Employees.CurrentUser.Role != UserRole.Administrator)
            {
                SuppliersButton.IsEnabled = false;
                StatsButton.IsEnabled = false;
                FullPDFReportButton.IsEnabled = false;
            }
            UserStatusBox.SelectedItem = UserStatus.Online;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new OrdersPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new GoodsPage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new CustomersPage();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new EmployeePage();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(new GetPDFReport().GetFullReport(), "Збереження");
        }

        private void StatsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new StatisticPage();
        }

        private void UserStatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var userStatus = (ComboBox)sender;
            Employees.CurrentUser.Status = (UserStatus)userStatus.SelectedItem;
            using(var context = new ApplicationContext())
            {
                context.Update(Employees.CurrentUser);
                context.SaveChanges();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using(var context = new ApplicationContext())
            {
                Employees.CurrentUser.Status = UserStatus.Offline;
                context.Update(Employees.CurrentUser);
                context.SaveChanges();
            }
        }
    }
}
