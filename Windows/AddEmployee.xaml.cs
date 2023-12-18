using Paws.Model;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
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
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        SfDataGrid _grid;
        public AddEmployee(SfDataGrid grid)
        {
            InitializeComponent();
            _grid = grid;
            RoleComboBox.Items.Add(UserRole.User);
            RoleComboBox.Items.Add(UserRole.Administrator);
            RoleComboBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                using (var context = new ApplicationContext())
                {
                    if (!string.IsNullOrEmpty(LoginTextBox.Text))
                    {
                        if (!context.Employees.Any(x => x.Login == LoginTextBox.Text)) {
                            if (PasswordTextBox.Text == RepeatPasswordTextBox.Text)
                            {
                                Employees employee = new Employees
                                {
                                    Fullname = FullNameTextBox.Text,
                                    Email = EmailTextBox.Text,
                                    Login = LoginTextBox.Text,
                                    Password = Services.Hash.HashPassword(PasswordTextBox.Text),
                                    Phone = PhoneTextBox.Text,
                                    Position = PositionTextBox.Text,
                                    Role = (UserRole)RoleComboBox.SelectedItem,
                                    Salary = SalaryTextBox.Value,
                                    Status = UserStatus.Offline
                                };
                                context.Employees.Add(employee);
                                context.SaveChanges();
                                MessageBox.Show($"Співробітник {employee.Fullname} - {employee.Position} успішно додано!", "Успішно!");
                                Button_Click_1(sender, e);

                                _grid.ItemsSource = context.Employees.ToList();
                            }
                            else
                            {
                                MessageBox.Show("Паролі не збігаються!", "Помилка введення!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Користувач з таким логіном вже існує", "Помилка введення!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поле логіна порожнє!", "Помилка введення!");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FullNameTextBox.Text = "";
            EmailTextBox.Text = "";
            LoginTextBox.Text = "";
            PasswordTextBox.Text = "";
            RepeatPasswordTextBox.Text = "";
            PhoneTextBox.Text = "";
            PositionTextBox.Text = "";
            RoleComboBox.SelectedIndex = 0;
            SalaryTextBox.Text = "";
        }
    }
}
