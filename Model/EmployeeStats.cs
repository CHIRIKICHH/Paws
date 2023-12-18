using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paws.Model
{
    internal class EmployeeStats
    {
        public Employees Employee { get; set; }
        public string EmployeeName
        {
            get => Employee.Fullname;
        }
        public int Sales { get; set; }
        public decimal? AmountSales { get; set; }
        public UserStatus Status { get => Employee.Status; }
    }
}
