using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paws.Model
{
    public class Orders
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        [NotMapped]
        public string CustomerName { get { return Customer.Fullname; }  }
        public Employees Employee { get; set; }
        [NotMapped]
        public string EmployeeName { get { return Employee.Fullname; } }
        public List<Goods> Goods { get; set; } = new List<Goods>();
        [NotMapped]
        public string GoodsNames { get
            {
                return string.Join("\n",Goods.Select(x => x.ProductName).ToArray());
            } }
        [NotMapped]
        internal string GoodsNamesForEmail
        {
            get
            {
                return string.Join("\n<br>", Goods.Select(x => x.ProductName).ToArray());
            }
        }
        public DateTime? OrderDateTime { get; set; }
        public decimal? Amount { get; set; }
        public string DeliveryAdress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Comment { get; set; }

    }

    public enum PaymentMethod
    {
        InCash = 0,
        NonCash = 1,
        ByCard = 2
    }
}
