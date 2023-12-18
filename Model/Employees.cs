using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paws.Model
{
    public class Employees
    {
        internal static Employees CurrentUser;

        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string Position { get; set; }
        public decimal? Salary { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Login} - {Position}";
        }
    }
}
