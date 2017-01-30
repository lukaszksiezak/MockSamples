using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EF6_DbContextMocking
{
    //To be noted: DbSet properties on the context are marked as virtual.
    public class OnlineStoreContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<OnlineStore> OnlineStore { get; set; }
    }
}
