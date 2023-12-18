using Microsoft.EntityFrameworkCore;
using Paws.Model;
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

namespace Paws
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> Logins = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                using(var context = new ApplicationContext())
                {
                    Logins = context.Employees.Select(x => x.Login).ToList();
                    LoginBox.ItemsSource = Logins;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Помилка підключення до БД\n" + ex.Message, "Помилка підключення до БД");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(LoginBox.Text))
                {
                    MessageBox.Show("Поле логіна порожнє!");
                }
                else if (string.IsNullOrEmpty(PasswordBox.Text))
                {
                    MessageBox.Show("Поле пароля порожнє!");
                }
                else
                {
                    using (var context = new ApplicationContext())
                    {
                        string password = Hash.HashPassword(PasswordBox.Text);
                        if (context.Employees.Any(x => x.Login == LoginBox.Text && x.Password == password))
                        {
                            Employees.CurrentUser = context.Employees.First(x => x.Login == LoginBox.Text && x.Password == password);
                            new MainMenuWindow().Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Користувач із зазначеним логіном і паролем не знайдений!");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка входу!");
            }
        }

        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
                    
        }
    }
}
