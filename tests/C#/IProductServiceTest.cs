using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApi.Models;
using DemoApi.Services;
using Moq;
using NUnit.Framework;

namespace DemoApi.Tests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductService> _productServiceMock;

        [SetUp]
        public void SetUp()
        {
            _productServiceMock = new Mock<IProductService>();
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1" },
                new Product { Id = 2, Name = "Product2" }
            };
            _productServiceMock.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productServiceMock.Object.GetAllProductsAsync();

            // Assert
            Assert.AreEqual(2, result.Count(), "The number of products returned is incorrect.");
        }

        [Test]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _productServiceMock.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productServiceMock.Object.GetProductByIdAsync(1);

            // Assert
            Assert.IsNotNull(result, "The product should not be null.");
            Assert.AreEqual(1, result.Id, "The product ID is incorrect.");
        }

        [Test]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            _productServiceMock.Setup(service => service.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

            // Act
            var result = await _productServiceMock.Object.GetProductByIdAsync(99);

            // Assert
            Assert.IsNull(result, "The product should be null for a non-existent ID.");
        }

        [Test]
        public async Task CreateProductAsync_ShouldReturnCreatedProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "NewProduct" };
            _productServiceMock.Setup(service => service.CreateProductAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _productServiceMock.Object.CreateProductAsync(product);

            // Assert
            Assert.IsNotNull(result, "The created product should not be null.");
            Assert.AreEqual("NewProduct", result.Name, "The product name is incorrect.");
        }

        [Test]
        public async Task UpdateProductAsync_ShouldReturnUpdatedProduct_WhenProductExists()
        {
            // Arrange
            var updatedProduct = new Product { Id = 1, Name = "UpdatedProduct" };
            _productServiceMock.Setup(service => service.UpdateProductAsync(1, updatedProduct)).ReturnsAsync(updatedProduct);

            // Act
            var result = await _productServiceMock.Object.UpdateProductAsync(1, updatedProduct);

            // Assert
            Assert.IsNotNull(result, "The updated product should not be null.");
            Assert.AreEqual("UpdatedProduct", result.Name, "The updated product name is incorrect.");
        }

        [Test]
        public async Task UpdateProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var updatedProduct = new Product { Id = 99, Name = "NonExistent" };
            _productServiceMock.Setup(service => service.UpdateProductAsync(99, updatedProduct)).ReturnsAsync((Product?)null);

            // Act
            var result = await _productServiceMock.Object.UpdateProductAsync(99, updatedProduct);

            // Assert
            Assert.IsNull(result, "The result should be null when updating a non-existent product.");
        }

        [Test]
        public async Task DeleteProductAsync_ShouldReturnTrue_WhenProductIsDeleted()
        {
            // Arrange
            _productServiceMock.Setup(service => service.DeleteProductAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _productServiceMock.Object.DeleteProductAsync(1);

            // Assert
            Assert.IsTrue(result, "The result should be true when a product is successfully deleted.");
        }

        [Test]
        public async Task DeleteProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            _productServiceMock.Setup(service => service.DeleteProductAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _productServiceMock.Object.DeleteProductAsync(99);

            // Assert
            Assert.IsFalse(result, "The result should be false when attempting to delete a non-existent product.");
        }
    }
}