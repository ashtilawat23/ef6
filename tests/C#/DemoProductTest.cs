using System;
using Xunit;
using EFCore.Demo;

namespace EFCore.Demo.Tests
{
    public class DemoProductTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            string name = "Test Product";
            decimal price = 10.99m;

            // Act
            var product = new DemoProduct(name, price);

            // Assert
            Assert.Equal(name, product.Name);
            Assert.Equal(price, product.Price);
            Assert.True(product.IsAvailable);
            Assert.Equal(DateTime.UtcNow.Date, product.CreatedDate.Date);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenNameIsNull()
        {
            // Arrange
            string name = null;
            decimal price = 10.99m;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DemoProduct(name, price));
        }

        [Fact]
        public void UpdatePrice_ShouldUpdatePrice_WhenNewPriceIsValid()
        {
            // Arrange
            var product = new DemoProduct("Test Product", 10.99m);
            decimal newPrice = 15.99m;

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(newPrice, product.Price);
        }

        [Fact]
        public void UpdatePrice_ShouldThrowArgumentException_WhenNewPriceIsZero()
        {
            // Arrange
            var product = new DemoProduct("Test Product", 10.99m);
            decimal newPrice = 0m;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => product.UpdatePrice(newPrice));
            Assert.Equal("Price must be greater than zero", exception.Message);
        }

        [Fact]
        public void UpdatePrice_ShouldThrowArgumentException_WhenNewPriceIsNegative()
        {
            // Arrange
            var product = new DemoProduct("Test Product", 10.99m);
            decimal newPrice = -5m;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => product.UpdatePrice(newPrice));
            Assert.Equal("Price must be greater than zero", exception.Message);
        }

        [Fact]
        public void ToString_ShouldReturnCorrectStringRepresentation()
        {
            // Arrange
            var product = new DemoProduct("Test Product", 10.99m);

            // Act
            var result = product.ToString();

            // Assert
            Assert.Equal("Product: Test Product, Price: $10.99, Available: True", result);
        }

        [Fact]
        public void Name_ShouldNotExceedMaxLength()
        {
            // Arrange
            string longName = new string('A', 101); // Exceeds 100 characters

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => new DemoProduct(longName, 10.99m));
            Assert.Contains("maximum length of 100", exception.Message);
        }

        [Fact]
        public void Price_ShouldBeGreaterThanZero()
        {
            // Arrange
            decimal invalidPrice = 0m;

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => new DemoProduct("Test Product", invalidPrice));
            Assert.Contains("greater than zero", exception.Message);
        }
    }
}