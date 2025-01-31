using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApi.Models;
using DemoApi.Services;
using NUnit.Framework;

namespace DemoApi.Tests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _productService = new ProductService();
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsEmpty(result, "Result should be an empty list");
        }

        [Test]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.IsNull(result, "Result should be null for non-existent product");
        }

        [Test]
        public async Task CreateProductAsync_ShouldAddProductSuccessfully()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Description = "Description", Price = 10.0m, StockQuantity = 100 };

            // Act
            var createdProduct = await _productService.CreateProductAsync(product);
            var allProducts = await _productService.GetAllProductsAsync();

            // Assert
            Assert.IsNotNull(createdProduct, "Created product should not be null");
            Assert.AreEqual(1, createdProduct.Id, "Product ID should be 1");
            Assert.AreEqual(1, allProducts.Count(), "There should be one product in the list");
        }

        [Test]
        public async Task UpdateProductAsync_ShouldUpdateProductSuccessfully_WhenProductExists()
        {
            // Arrange
            var product = new Product { Name = "Original", Description = "Description", Price = 10.0m, StockQuantity = 100 };
            var createdProduct = await _productService.CreateProductAsync(product);
            var updatedProduct = new Product { Name = "Updated", Description = "New Description", Price = 20.0m, StockQuantity = 200 };

            // Act
            var result = await _productService.UpdateProductAsync(createdProduct.Id, updatedProduct);

            // Assert
            Assert.IsNotNull(result, "Updated product should not be null");
            Assert.AreEqual("Updated", result.Name, "Product name should be updated");
            Assert.AreEqual("New Description", result.Description, "Product description should be updated");
            Assert.AreEqual(20.0m, result.Price, "Product price should be updated");
            Assert.AreEqual(200, result.StockQuantity, "Product stock quantity should be updated");
        }

        [Test]
        public async Task UpdateProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var updatedProduct = new Product { Name = "Updated", Description = "New Description", Price = 20.0m, StockQuantity = 200 };

            // Act
            var result = await _productService.UpdateProductAsync(1, updatedProduct);

            // Assert
            Assert.IsNull(result, "Result should be null for non-existent product");
        }

        [Test]
        public async Task DeleteProductAsync_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Description = "Description", Price = 10.0m, StockQuantity = 100 };
            var createdProduct = await _productService.CreateProductAsync(product);

            // Act
            var result = await _productService.DeleteProductAsync(createdProduct.Id);

            // Assert
            Assert.IsTrue(result, "Delete should return true for existing product");
            var allProducts = await _productService.GetAllProductsAsync();
            Assert.IsEmpty(allProducts, "Product list should be empty after deletion");
        }

        [Test]
        public async Task DeleteProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange

            // Act
            var result = await _productService.DeleteProductAsync(1);

            // Assert
            Assert.IsFalse(result, "Delete should return false for non-existent product");
        }
    }
}