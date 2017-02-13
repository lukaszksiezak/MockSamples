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
    public class DbEntityFake: DbEntitySample { }

    [TestClass()]
    public class DbEntitySampleTests
    {
        [TestMethod()]
        public void CreateEntityTest()
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
        [TestMethod()]
        public void ModifyEntityTest()
        {
            //Arrange
            var mockEntityFactory = new WestCoastCustoms();
            DbEntitySample fakeEntity = new DbEntitySample();

            //Act
            Assert.IsNull(fakeEntity.Name);
            mockEntityFactory.ModifyEntity(ref fakeEntity, "someName");
            //Assert
            Assert.IsTrue(fakeEntity.Name.Equals("someName"));

        }
    }
}