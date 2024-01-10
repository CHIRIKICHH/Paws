using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paws.Model
{
    internal class ClientStats
    {
        public Customer Customer { get; set; }
        public string CustomerName
        {
            get => Customer.Fullname;
        }
        public int Orders { get; set; }
        public int Goods { get; set; }
        public decimal? AmountOrders { get; set; }

    }
}
