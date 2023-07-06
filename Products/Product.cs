using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaMaster
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public int SellingPrice { get; set; }
        public int BuyingPrice { get; set; }
        public int Amount { get; set; }
        public int Earnings { 
            get { return (SellingPrice - BuyingPrice) * Amount; }
        }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
