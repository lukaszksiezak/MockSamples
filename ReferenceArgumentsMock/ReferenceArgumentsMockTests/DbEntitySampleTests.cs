using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferenceArgumentsMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace ReferenceArgumentsMock.Tests
{
    public class DbEntityFake: DbEntitySampleTests
    {
        public Guid Guid { get; set; }
    }

    [TestClass()]
    public class DbEntitySampleTests
    {
        [TestMethod()]
        public void CreateEntityAndAssignNameTest()
        {
            //Arrange
            var mockEntityFactory = Substitute.For<DbEntityFactory<DbEntityFake>>();
            DbEntityFake fakeEntity;
            //Act
            mockEntityFactory.CreateEntity(out fakeEntity);
            //Assert
            Assert.IsNotNull(fakeEntity);
            Assert.IsNotNull(fakeEntity.Guid);
        }
    }
}