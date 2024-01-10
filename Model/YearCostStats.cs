using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paws.Model
{
    internal class YearCostStats
    {
        public int MounthCount { get; set; }
        public Year Mounth{ get; set; }
        public int OrdersCount { get; set; }
        public int GoodsCount { get; set; }
        public decimal? Cost { get; set; }
    }
}
