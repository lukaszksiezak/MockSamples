using Microsoft.VisualStudio.TestTools.UnitTesting;
using EF6_DbContextMocking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using System.Data.Entity;

namespace EF6_DbContextMocking.Tests
{
    [TestClass()]
    public class OnlineStoreServiceTests
    {
        /// <summary>
        /// Verification of non-query calls
        /// </summary>
        [TestMethod()]
        public void CustomersWhoBoughtProductTest()
        {
            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<OnlineStoreContext>();
            mockDbContext.Products.Returns(mockDbSet);

            var service = Substitute.For<OnlineStoreService>(mockDbContext);
            service.AddNewProduct("IPhone", "Moblie", 3000);

            mockDbSet.Received(1);
            mockDbContext.Received(1).SaveChanges();
        }
    }
}