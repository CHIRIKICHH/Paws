using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace Paws.Model
{
    public class Customer
    {
        public int Id { get; set; }
        
        public string Fullname { get; set; }
        
        public string Adress { get; set; }
        
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DiscountCard { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Fullname} - {Phone}";
        }
    }
}
