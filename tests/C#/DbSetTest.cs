using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;

namespace System.Data.Entity.Tests
{
    [TestClass]
    public class DbSetTests
    {
        private Mock<DbSet> _mockDbSet;
        private Mock<IInternalSet> _mockInternalSet;

        [TestInitialize]
        public void Setup()
        {
            _mockDbSet = new Mock<DbSet> { CallBase = true };
            _mockInternalSet = new Mock<IInternalSet>();
            _mockDbSet.Setup(m => m.InternalSet).Returns(_mockInternalSet.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Find_ShouldThrowNotImplementedException()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Find(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task FindAsync_ShouldThrowNotImplementedException()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            await dbSet.FindAsync(1);
        }

        [TestMethod]
        public void Attach_ShouldAttachEntity()
        {
            // Arrange
            var entity = new object();
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Attach(entity);

            // Assert
            _mockInternalSet.Verify(m => m.Attach(entity), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Attach_ShouldThrowArgumentNullException_WhenEntityIsNull()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Attach(null);
        }

        [TestMethod]
        public void Add_ShouldAddEntity()
        {
            // Arrange
            var entity = new object();
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Add(entity);

            // Assert
            _mockInternalSet.Verify(m => m.Add(entity), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Add(null);
        }

        [TestMethod]
        public void AddRange_ShouldAddEntities()
        {
            // Arrange
            var entities = new[] { new object(), new object() };
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.AddRange(entities);

            // Assert
            _mockInternalSet.Verify(m => m.AddRange(entities), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRange_ShouldThrowArgumentNullException_WhenEntitiesIsNull()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.AddRange(null);
        }

        [TestMethod]
        public void Remove_ShouldRemoveEntity()
        {
            // Arrange
            var entity = new object();
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Remove(entity);

            // Assert
            _mockInternalSet.Verify(m => m.Remove(entity), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Remove_ShouldThrowArgumentNullException_WhenEntityIsNull()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Remove(null);
        }

        [TestMethod]
        public void RemoveRange_ShouldRemoveEntities()
        {
            // Arrange
            var entities = new[] { new object(), new object() };
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.RemoveRange(entities);

            // Assert
            _mockInternalSet.Verify(m => m.RemoveRange(entities), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveRange_ShouldThrowArgumentNullException_WhenEntitiesIsNull()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.RemoveRange(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Create_ShouldThrowNotImplementedException()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Create();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Create_WithType_ShouldThrowNotImplementedException()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.Create(typeof(object));
        }

        [TestMethod]
        public void Cast_ShouldReturnGenericDbSet()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;
            _mockInternalSet.Setup(m => m.ElementType).Returns(typeof(object));
            _mockInternalSet.Setup(m => m.InternalContext.Set<object>()).Returns(new Mock<DbSet<object>>().Object);

            // Act
            var result = dbSet.Cast<object>();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Cast_ShouldThrowNotSupportedException_WhenInternalSetIsNull()
        {
            // Arrange
            var dbSet = new Mock<DbSet> { CallBase = true }.Object;

            // Act
            dbSet.Cast<object>();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Cast_ShouldThrowInvalidOperationException_WhenTypeMismatch()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;
            _mockInternalSet.Setup(m => m.ElementType).Returns(typeof(string));

            // Act
            dbSet.Cast<object>();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SqlQuery_ShouldThrowNotImplementedException()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            dbSet.SqlQuery("SELECT * FROM Table");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Local_ShouldThrowNotImplementedException()
        {
            // Arrange
            var dbSet = _mockDbSet.Object;

            // Act
            var local = dbSet.Local;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockDbSet = null;
            _mockInternalSet = null;
        }
    }
}