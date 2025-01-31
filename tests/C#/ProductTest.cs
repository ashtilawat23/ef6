using NUnit.Framework;
using DemoApi.Models;

namespace DemoApi.Tests.Models
{
    [TestFixture]
    public class ProductTests
    {
        private Product _product;

        [SetUp]
        public void Setup()
        {
            _product = new Product();
        }

        [Test]
        public void Product_DefaultConstructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var product = new Product();

            // Assert
            Assert.AreEqual(0, product.Id, "Default Id should be 0");
            Assert.AreEqual(string.Empty, product.Name, "Default Name should be an empty string");
            Assert.AreEqual(string.Empty, product.Description, "Default Description should be an empty string");
            Assert.AreEqual(0m, product.Price, "Default Price should be 0");
            Assert.AreEqual(0, product.StockQuantity, "Default StockQuantity should be 0");
        }

        [Test]
        public void Product_SetValidId_ShouldUpdateId()
        {
            // Arrange
            var expectedId = 123;

            // Act
            _product.Id = expectedId;

            // Assert
            Assert.AreEqual(expectedId, _product.Id, "Id should be set to the expected value");
        }

        [Test]
        public void Product_SetValidName_ShouldUpdateName()
        {
            // Arrange
            var expectedName = "Test Product";

            // Act
            _product.Name = expectedName;

            // Assert
            Assert.AreEqual(expectedName, _product.Name, "Name should be set to the expected value");
        }

        [Test]
        public void Product_SetValidDescription_ShouldUpdateDescription()
        {
            // Arrange
            var expectedDescription = "This is a test product.";

            // Act
            _product.Description = expectedDescription;

            // Assert
            Assert.AreEqual(expectedDescription, _product.Description, "Description should be set to the expected value");
        }

        [Test]
        public void Product_SetValidPrice_ShouldUpdatePrice()
        {
            // Arrange
            var expectedPrice = 29.99m;

            // Act
            _product.Price = expectedPrice;

            // Assert
            Assert.AreEqual(expectedPrice, _product.Price, "Price should be set to the expected value");
        }

        [Test]
        public void Product_SetValidStockQuantity_ShouldUpdateStockQuantity()
        {
            // Arrange
            var expectedStockQuantity = 50;

            // Act
            _product.StockQuantity = expectedStockQuantity;

            // Assert
            Assert.AreEqual(expectedStockQuantity, _product.StockQuantity, "StockQuantity should be set to the expected value");
        }

        [Test]
        public void Product_SetNegativePrice_ShouldAllowNegativePrice()
        {
            // Arrange
            var negativePrice = -10.99m;

            // Act
            _product.Price = negativePrice;

            // Assert
            Assert.AreEqual(negativePrice, _product.Price, "Price should allow negative values");
        }

        [Test]
        public void Product_SetNegativeStockQuantity_ShouldAllowNegativeStockQuantity()
        {
            // Arrange
            var negativeStockQuantity = -5;

            // Act
            _product.StockQuantity = negativeStockQuantity;

            // Assert
            Assert.AreEqual(negativeStockQuantity, _product.StockQuantity, "StockQuantity should allow negative values");
        }
    }
}