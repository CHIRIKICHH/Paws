using DevExpress.Mvvm.Native;
using Paws.Model;
using Paws.Services;
using Paws.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paws.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        public ObservableCollection<Employees> EmployeesList { get; set; } = new ObservableCollection<Employees>();
        public EmployeePage()
        {
            InitializeComponent();
            if (Employees.CurrentUser.Role != UserRole.Administrator)
            {
                AddButton.IsEnabled = false;
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
                    
                    dataGrid.ItemsSource = context.Employees.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AddEmployee(dataGrid).Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ChangeTextBox.Text) && int.TryParse(ChangeTextBox.Text, out int result))
            {
                using (var context = new ApplicationContext())
                {
                    if (context.Employees.Any(x => x.Id == result))
                    {
                        var changeEmployees = context.Employees.FirstOrDefault(x => x.Id == result);
                        if (changeEmployees.Status == UserStatus.Offline)
                        {
                            var temp = new ChangeEmployee(changeEmployees);
                            temp.Show();
                            temp.Closed += delegate { RefreshDataGrid(); };
                        }
                        else
                        {
                            MessageBox.Show("Редагування не доступно поки користувач знаходиться в програмі!", "Помилка");
                        }
                    }
                    else
                    {
                        MessageBox.Show( $"Клієнта з ID:{result} не знайдено!", "Помилка");
                    }
                }
            }
            else
            {
                MessageBox.Show($"Поле введення порожнє або Некоректний формат!", "Помилка");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(RemoveTextBox.Text) && int.TryParse(RemoveTextBox.Text, out int result))
            {
                using (var context = new ApplicationContext())
                {
                    if (context.Employees.Any(x => x.Id == result))
                    {
                        var remove = context.Employees.FirstOrDefault(x => x.Id == result);
                        if (MessageBox.Show($"Ви впевнені що хочете видалити співробітника {remove.Id}:{remove.Fullname}?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            context.Remove(remove);
                            context.SaveChanges();
                            RefreshDataGrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Клієнта з ID:{result} не знайдено!", "Помилка");
                    }
                }
            }
            else
            {
                MessageBox.Show($"Поле введення порожнє або Некоректний формат!", "Помилка");
            }
        }

        private void dataGrid_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs e)
        {
            if (e.Record != null)
            {
                Employees employee = e.Record as Employees;
                ChangeTextBox.Text = employee.Id.ToString();
                RemoveTextBox.Text = employee.Id.ToString();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(new GetPDFReport().GetEmployeesPDF());
        }
    }
}
