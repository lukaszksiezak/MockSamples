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
            //Arrange
            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<OnlineStoreContext>();
            mockDbContext.Products.Returns(mockDbSet);
            //Act
            var service = Substitute.For<OnlineStoreService>(mockDbContext);
            service.AddNewProduct("IPhone", "Moblie", 3000);
            //Assert
            mockDbSet.Received(1);
            mockDbContext.Received(1).SaveChanges();
        }

        [TestMethod()]
        public void AllShopProductsOrderedByPriceTest()
        {
            //Arrange
            var dbData = new List<Product>(){
                new Product() {Name="IPhone", Price=100 },
                new Product() {Name="Htc", Price=150 },
                new Product() {Name="Motorola", Price = 50 },
                new Product() {Name="Nokia", Price=120 }
            }.AsQueryable();

            ///Without passing interface in substitute.for - exception was thrown. Explanation from SO:
            ///NSubstitute calls the Provider's getter, then it specifies the return value. 
            ///This getter call isn't intercepted by the substitute and you get an exception.
            ///It happens because of explicit implementation of IQueryable.Provider 
            ///property in DbQuery class.
            ///You can explicitly create substitutes for multiple interfaces with NSub, 
            ///and it creates a proxy which covers all specified interfaces.
            ///Then calls to the interfaces will be intercepted by the substitute.

            var mockDbSet = Substitute.For<DbSet<Product>,IQueryable<Product>>();
            mockDbSet.AsQueryable().Provider.Returns(dbData.Provider);
            mockDbSet.AsQueryable().Expression.Returns(dbData.Expression);
            mockDbSet.AsQueryable().ElementType.Returns(dbData.ElementType); //works without it
            mockDbSet.AsQueryable().GetEnumerator().Returns(dbData.GetEnumerator()); //works without it
                        
            var mockContext = Substitute.For<OnlineStoreContext>();
            mockContext.Products.Returns(mockDbSet);

            var service = Substitute.For<OnlineStoreService>(mockContext);

            //Act
            var products = service.AllShopProductsOrderedByPrice();

            //Assert
            Assert.AreEqual(products.Count, 4);
            Assert.AreEqual(products[0].Name, "Motorola");
            Assert.AreEqual(products[3].Name, "Htc");
        }        
    }
}



