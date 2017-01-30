using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6_DbContextMocking
{
    public class OnlineStore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Person Owner { get; set; }
        public virtual List<Person> Customers { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
