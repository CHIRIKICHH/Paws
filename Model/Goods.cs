using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace Paws.Model
{
    public class Goods
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public bool? AvailabilityInStock { get; set; }

        public List<Orders> Orders { get; set; } = new List<Orders>();
        public override string ToString()
        {
            return $"{Id} - {ProductName} - {ProductCategory} - {Price}₴";
        }
    }
}
