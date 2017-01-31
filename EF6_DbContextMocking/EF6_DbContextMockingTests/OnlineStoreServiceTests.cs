using Microsoft.VisualStudio.TestTools.UnitTesting;
using EF6_DbContextMocking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading;

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

        /// <summary>
        /// verification of a query
        /// </summary>
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

            var mockDbSet = Substitute.For<DbSet<Product>, IQueryable<Product>>();
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

        //Verification of async query
        [TestMethod()]
        public async Task GetAllProductsTest()
        {
            //Arrange
            var dbData = new List<Product>(){
                new Product() {Name="IPhone", Price=100 },
                new Product() {Name="Htc", Price=150 },
                new Product() {Name="Motorola", Price = 50 },
                new Product() {Name="Nokia", Price=120 }
            }.AsQueryable();

            var fakeAsyncEnumerable = new TestDbAsyncEnumerable<Product>(dbData);

            var mockDbSet = Substitute.For<DbSet<Product>,IQueryable<Product>, IDbAsyncEnumerable<Product>>();

            mockDbSet.AsQueryable().Provider.Returns(fakeAsyncEnumerable.AsQueryable().Provider);
            mockDbSet.AsQueryable().Expression.Returns(fakeAsyncEnumerable.AsQueryable().Expression);
            mockDbSet.AsQueryable().ElementType.Returns(fakeAsyncEnumerable.AsQueryable().ElementType);          
            ((IDbAsyncEnumerable<Product>)mockDbSet).GetAsyncEnumerator()
                .Returns(((IDbAsyncEnumerable<Product>)fakeAsyncEnumerable).GetAsyncEnumerator());

            var mockContext = Substitute.For<OnlineStoreContext>();
            mockContext.Products.Returns(mockDbSet);

            var service = Substitute.For<OnlineStoreService>(mockContext);

            //Act
            var products = await service.GetAllProducts();

            //Assert
            Assert.AreEqual(products.Count, 4);
            Assert.AreEqual(products[0].Name, "Motorola");
            Assert.AreEqual(products[3].Name, "Htc");
        }
    }

    /// <summary>
    /// Mandatory classes to test async query. Test required class which was implementing IDbAsyncEnumerable and saying that, also IDbAsyncEnumerator and IDbAsyncQueryProvider
    /// https://msdn.microsoft.com/en-us/library/dn314429(v=vs.113).aspx
    /// </summary>

    public class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }
    public class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _localEnumerator;

        public TestDbAsyncEnumerator(IEnumerator<T> localEnumerator)
        {
            _localEnumerator = localEnumerator;
        }

        public void Dispose()
        {
            _localEnumerator.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_localEnumerator.MoveNext());
        }

        public T Current
        {
            get { return _localEnumerator.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
    public class TestDbAsyncQueryProvider<T> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _localQueryProvider;

        internal TestDbAsyncQueryProvider(IQueryProvider localQueryProvider)
        {
            _localQueryProvider = localQueryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _localQueryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _localQueryProvider.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }
}



