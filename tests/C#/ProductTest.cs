using System;
using Demo.Models;
using Xunit;

namespace Demo.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var product = new Product();

            // Assert
            Assert.NotNull(product);
            Assert.Equal(DateTime.UtcNow.Date, product.CreatedDate.Date);
            Assert.True(product.IsAvailable);
        }

        [Fact]
        public void ApplyDiscount_ShouldApplyValidDiscount()
        {
            // Arrange
            var product = new Product
            {
                Price = 100m
            };
            var discount = 10m;

            // Act
            product.ApplyDiscount(discount);

            // Assert
            Assert.Equal(90m, product.Price);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void ApplyDiscount_ShouldThrowExceptionForInvalidDiscount(decimal discount)
        {
            // Arrange
            var product = new Product
            {
                Price = 100m
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => product.ApplyDiscount(discount));
            Assert.Equal("Discount percentage must be between 0 and 100", exception.Message);
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 99.99m
            };

            // Act
            var result = product.ToString();

            // Assert
            Assert.Equal("Test Product - $99.99", result);
        }

        [Fact]
        public void ProductName_ShouldBeRequired()
        {
            // Arrange
            var product = new Product
            {
                Price = 10m
            };

            // Act
            var validationContext = new ValidationContext(product);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Assert
            Assert.False(Validator.TryValidateObject(product, validationContext, validationResults, true));
        }

        [Fact]
        public void ProductPrice_ShouldBeRequiredAndWithinRange()
        {
            // Arrange
            var product = new Product
            {
                Name = "Valid Name"
            };

            // Act
            var validationContext = new ValidationContext(product);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Assert
            Assert.False(Validator.TryValidateObject(product, validationContext, validationResults, true));

            // Arrange
            product.Price = 0.009m;

            // Act & Assert
            Assert.False(Validator.TryValidateObject(product, validationContext, validationResults, true));

            // Arrange
            product.Price = 10000.01m;

            // Act & Assert
            Assert.False(Validator.TryValidateObject(product, validationContext, validationResults, true));
        }

        [Fact]
        public void ProductDescription_ShouldAllowMaxLength()
        {
            // Arrange
            var product = new Product
            {
                Name = "Valid Name",
                Price = 10m,
                Description = new string('a', 500)
            };

            // Act
            var validationContext = new ValidationContext(product);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Assert
            Assert.True(Validator.TryValidateObject(product, validationContext, validationResults, true));
        }

        [Fact]
        public void ProductDescription_ShouldNotExceedMaxLength()
        {
            // Arrange
            var product = new Product
            {
                Name = "Valid Name",
                Price = 10m,
                Description = new string('a', 501)
            };

            // Act
            var validationContext = new ValidationContext(product);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Assert
            Assert.False(Validator.TryValidateObject(product, validationContext, validationResults, true));
        }
    }
}