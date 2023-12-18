using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paws.Model
{
    class SalesStats
    {
        public int QuantityOfOrders { get; set; }
        public int QuantityOfSaleGoods { get; set; }
        public decimal? AllOrdersSum { get; set; }
        public int InCashCount { get; set; }
        public int NonCash { get; set; }
        public int ByCardCount { get; set; }
    }
}
