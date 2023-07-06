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

namespace BestPizzaMaster.Products
{
    /// <summary>
    /// Interaction logic for ProuctListWindow.xaml
    /// </summary>
    public partial class ProductListWindow : Window
    {
        ObservableCollection<Product> products;

        private double sumOfEarning;
        public ProductListWindow()
        {
            InitializeComponent();
            this.Loaded += ProductsListPage_Loaded;

            products = new ObservableCollection<Product>();
        }

        private void ProductsListPage_Loaded(object sender, RoutedEventArgs e)
        {
            titleText.Text = MainWindow.selectedLocation.ToString();
            using (var db = new LocationsContext())
            {
                if (MainWindow.selectedLocation != null)
                {
                    products = new ObservableCollection<Product>(db.Products
                        .Include(pr => pr.Location)
                        .Where<Product>(pr => pr.Location == MainWindow.selectedLocation)
                        .ToList());

                    sumOfEarning = 0;
                    foreach (var item in products)
                    {
                        sumOfEarning += item.Earnings;
                    }
                    ShowEarnings();
                }

                productsList.ItemsSource = products;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.selectedLocation != null)
            {
                Product product = new Product()
                {
                    Name = nameBox.Text,
                    SellingPrice = Int32.Parse(sellingPrice.Text),
                    BuyingPrice = Int32.Parse(buyingPrice.Text),
                    Amount = Int32.Parse(amountBox.Text),
                    Location = MainWindow.selectedLocation,
                };

                using (var db = new LocationsContext())
                {
                    AddEarnings(product);

                    db.Locations.Attach(MainWindow.selectedLocation);
                    db.Products.Add(product);
                    MainWindow.selectedLocation.Products.Add(product);


                    if (db.SaveChanges() > 0)
                    {
                        productsList.ItemsSource = products;
                    }
                }

            }
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (productsList.SelectedItem != null)
            {
                Product product = productsList.SelectedItem as Product;
                if (product != null)
                {
                    using (LocationsContext db = new LocationsContext())
                    {
                        DeleteEarnings(product);

                        db.Products.Remove(product);
                        db.SaveChanges();
                        productsList.ItemsSource = db.Products
                         .Include(pr => pr.Location)
                         .Where<Product>(pr => pr.Location == MainWindow.selectedLocation)
                         .ToList();
                    }
                }
            }
        }
        private void AddEarnings(Product product)
        {
            sumOfEarning += product.Earnings;
            ShowEarnings();
        }
        private void DeleteEarnings(Product product)
        {
            sumOfEarning -= product.Earnings;
            ShowEarnings();
        }

        private void ShowEarnings()
        {
            earnings.Text = "All earnings: " + sumOfEarning.ToString();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
