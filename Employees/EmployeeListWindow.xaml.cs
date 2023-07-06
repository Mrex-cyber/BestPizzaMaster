using Microsoft.EntityFrameworkCore;
using PizzaMaster;
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

namespace BestPizzaMaster.Employees
{
    /// <summary>
    /// Interaction logic for EmployeeListWindow.xaml
    /// </summary>
    public partial class EmployeeListWindow : Window
    {
        ObservableCollection<Employee> employees;
        public EmployeeListWindow()
        {
            InitializeComponent();

            this.Loaded += EmployeesListPage_Loaded;
        }

        private void EmployeesListPage_Loaded(object sender, RoutedEventArgs e)
        {
            titleText.Text = MainWindow.selectedLocation.ToString();
            using (var db = new LocationsContext())
            {
                if (MainWindow.selectedLocation != null)
                {
                    employees = new ObservableCollection<Employee>(db.Employees
                        .Include(emp => emp.Location)
                        .Where<Employee>(emp => emp.Location == MainWindow.selectedLocation)
                        .ToList());
                }
            }
            employeesList.ItemsSource = employees;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.selectedLocation != null)
            {
                Employee employee = new Employee()
                {
                    FullName = nameBox.Text,
                    Ages = Int32.Parse(agesBox.Text),
                    Salary = Int32.Parse(salaryBox.Text),
                    Position = "Employee",
                    Location = MainWindow.selectedLocation,
                };

                using (var db = new LocationsContext())
                {
                    db.Locations.Attach(MainWindow.selectedLocation);
                    db.Employees.Add(employee);

                    if (db.SaveChanges() > 0)
                    {
                        employees.Add(employee);
                    }
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeesList.SelectedItem != null)
            {
                Employee employee = employeesList.SelectedItem as Employee;
                if (employee != null)
                {
                    using (LocationsContext db = new LocationsContext())
                    {
                        db.Employees.Remove(employee);
                        db.SaveChanges();

                        employeesList.ItemsSource = db.Employees.ToList();
                    }
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
