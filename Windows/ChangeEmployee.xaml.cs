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

namespace Paws.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeEmployee.xaml
    /// </summary>
    public partial class ChangeEmployee : Window
    {
        Employees _employee;
        public ChangeEmployee(Employees employee)
        {
            InitializeComponent();
            _employee = employee;
            LoginTextBox.IsEnabled = false;

            FullNameTextBox.Text = _employee.Fullname;
            EmailTextBox.Text = _employee.Email;
            PhoneTextBox.Text = _employee.Phone;
            PositionTextBox.Text = _employee.Position;
            SalaryTextBox.Value = _employee.Salary;
            PasswordTextBox.Text = _employee.Password;
            LoginTextBox.Text = _employee.Login;
            RepeatPasswordTextBox.Text = _employee.Password;
            RoleComboBox.Items.Add(UserRole.User);
            RoleComboBox.Items.Add(UserRole.Administrator);
            RoleComboBox.SelectedItem = _employee.Role;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (var context = new ApplicationContext())
                {
                    if (!string.IsNullOrEmpty(LoginTextBox.Text))
                    {
                        if (PasswordTextBox.Text == RepeatPasswordTextBox.Text)
                        {
                            _employee.Fullname = FullNameTextBox.Text;
                            _employee.Phone = PhoneTextBox.Text;
                            _employee.Email = EmailTextBox.Text;
                            _employee.Position = PositionTextBox.Text;
                            _employee.Salary = SalaryTextBox.Value;
                            _employee.Role = (UserRole)RoleComboBox.SelectedItem;
                            _employee.Password = Hash.HashPassword(PasswordTextBox.Text);
                            context.Update(_employee);
                            context.SaveChanges();
                            MessageBox.Show($"Співробітник {_employee.Fullname} - {_employee.Position} успішно відредаговано!", "Успішно!");
                        }
                        else
                        {
                            MessageBox.Show("Паролі не збігаються!", "Помилка введення!");
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


    }
}
