using BestPizzaMaster.Employees;
using BestPizzaMaster.Locations;
using BestPizzaMaster.Products;
using Microsoft.EntityFrameworkCore;
using PizzaMaster;
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

namespace BestPizzaMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Location selectedLocation;
        public double sumOfLocationEarnings;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;

            
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (LocationsContext db = new LocationsContext())
            {
                locationsList.ItemsSource = db.Locations.ToList();
                var locations = db.Locations
                    .Include(l => l.Products)
                    .Include(l => l.Employees)
                    .ToList();

                foreach (var location in locations)
                {
                    sumOfLocationEarnings = 0;
                    foreach (var product in location.Products)
                    {
                        sumOfLocationEarnings += product.Earnings;
                    }
                    location.Earnings = sumOfLocationEarnings;
                }

                foreach (var location in locations)
                {
                    location.Employees = location.Employees;
                }

            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            LocationWindow locationWindow = new LocationWindow();
            locationWindow.Show();
            this.Close();
        }
        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {

            if (locationsList.SelectedItem != null)
            {
                Location location = locationsList.SelectedItem as Location;
                

                var resultOfDialog = MessageBox.Show("Delete the " + location + " location", "Are you sure of deleting?", MessageBoxButton.YesNo);

                if (location != null && resultOfDialog == MessageBoxResult.Yes)
                {
                    using (LocationsContext db = new LocationsContext())
                    {
                        db.Locations.Remove(location);
                        db.SaveChanges();
                        locationsList.ItemsSource = db.Locations.ToList();
                    }
                }
            }
        }

        private void EmployeesList_Click(object sender, RoutedEventArgs e)
        {
            if (locationsList.SelectedItem != null)
            {
                selectedLocation = locationsList.SelectedItem as Location;
                if (selectedLocation != null)
                {
                    EmployeeListWindow employeeWindow = new EmployeeListWindow();
                    employeeWindow.Show();
                    this.Close();
                }
            }
        }

        private void ProductsList_Click(object sender, RoutedEventArgs e)
        {
            if (locationsList.SelectedItem != null)
            {
                selectedLocation = locationsList.SelectedItem as Location;
                if (selectedLocation != null)
                {
                    ProductListWindow employeeWindow = new ProductListWindow();
                    employeeWindow.Show();
                    this.Close();
                }
            }
        }
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (locationsList.SelectedItem != null)
            {
                selectedLocation = locationsList.SelectedItem as Location;
                LocationWindow locationWindow = new LocationWindow(selectedLocation);
                locationWindow.Show();
                this.Close();
            }
        }
    }
}
