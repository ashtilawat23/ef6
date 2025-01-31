using System;
using Backend.Domain.Entities;
using NUnit.Framework;

namespace Backend.Domain.Tests.Entities
{
    [TestFixture]
    public class ProductTests
    {
        private Product _product;

        [SetUp]
        public void SetUp()
        {
            _product = new Product();
        }

        [Test]
        public void Product_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var product = new Product();

            // Assert
            Assert.AreEqual(0, product.Id, "Id should be initialized to 0");
            Assert.AreEqual(string.Empty, product.Name, "Name should be initialized to empty string");
            Assert.AreEqual(string.Empty, product.Description, "Description should be initialized to empty string");
            Assert.AreEqual(0m, product.Price, "Price should be initialized to 0");
            Assert.AreEqual(0, product.StockQuantity, "StockQuantity should be initialized to 0");
            Assert.AreEqual(DateTime.MinValue, product.CreatedAt, "CreatedAt should be initialized to DateTime.MinValue");
            Assert.IsNull(product.UpdatedAt, "UpdatedAt should be initialized to null");
        }

        [Test]
        public void Product_ShouldAllowSettingAndGettingProperties()
        {
            // Arrange
            var expectedId = 1;
            var expectedName = "Test Product";
            var expectedDescription = "Test Description";
            var expectedPrice = 99.99m;
            var expectedStockQuantity = 10;
            var expectedCreatedAt = DateTime.Now;
            var expectedUpdatedAt = DateTime.Now;

            // Act
            _product.Id = expectedId;
            _product.Name = expectedName;
            _product.Description = expectedDescription;
            _product.Price = expectedPrice;
            _product.StockQuantity = expectedStockQuantity;
            _product.CreatedAt = expectedCreatedAt;
            _product.UpdatedAt = expectedUpdatedAt;

            // Assert
            Assert.AreEqual(expectedId, _product.Id, "Id should be set correctly");
            Assert.AreEqual(expectedName, _product.Name, "Name should be set correctly");
            Assert.AreEqual(expectedDescription, _product.Description, "Description should be set correctly");
            Assert.AreEqual(expectedPrice, _product.Price, "Price should be set correctly");
            Assert.AreEqual(expectedStockQuantity, _product.StockQuantity, "StockQuantity should be set correctly");
            Assert.AreEqual(expectedCreatedAt, _product.CreatedAt, "CreatedAt should be set correctly");
            Assert.AreEqual(expectedUpdatedAt, _product.UpdatedAt, "UpdatedAt should be set correctly");
        }

        [Test]
        public void Product_Price_ShouldNotAcceptNegativeValues()
        {
            // Arrange
            var negativePrice = -10m;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _product.Price = negativePrice, "Price should not accept negative values");
        }

        [Test]
        public void Product_StockQuantity_ShouldNotAcceptNegativeValues()
        {
            // Arrange
            var negativeStockQuantity = -5;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _product.StockQuantity = negativeStockQuantity, "StockQuantity should not accept negative values");
        }

        [Test]
        public void Product_UpdatedAt_ShouldAcceptNullValue()
        {
            // Arrange
            DateTime? nullUpdatedAt = null;

            // Act
            _product.UpdatedAt = nullUpdatedAt;

            // Assert
            Assert.IsNull(_product.UpdatedAt, "UpdatedAt should accept null values");
        }

        [Test]
        public void Product_UpdatedAt_ShouldAcceptValidDateTime()
        {
            // Arrange
            var validDateTime = DateTime.Now;

            // Act
            _product.UpdatedAt = validDateTime;

            // Assert
            Assert.AreEqual(validDateTime, _product.UpdatedAt, "UpdatedAt should accept valid DateTime values");
        }
    }
}