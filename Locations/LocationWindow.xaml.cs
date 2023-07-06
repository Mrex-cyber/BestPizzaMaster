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

namespace BestPizzaMaster.Locations
{
    /// <summary>
    /// Interaction logic for LocationWindow.xaml
    /// </summary>
    public partial class LocationWindow : Window
    {
        private Location? location = null;
        public LocationWindow(Location editingLocation)
        {
            InitializeComponent(); 
            location = editingLocation;
            nameBox.Text = location.City;
        }
        public LocationWindow()
        {
            InitializeComponent();
        }        

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new LocationsContext())
            {
                if (location != null)
                {
                    location.City = nameBox.Text;
                    db.Locations.Update(location);
                }
                else
                {
                    db.Locations.Add(new Location { City = nameBox.Text, Earnings = 0 });
                }
                db.SaveChanges();
            }

            GoToMainPage();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage();
        }

        private void GoToMainPage()
        {
            MainWindow main = new MainWindow();
            main.Show();    
            this.Close();
        }
    }
}
