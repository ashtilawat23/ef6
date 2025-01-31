using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApi.Controllers;
using DemoApi.Models;
using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DemoApi.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController(_productServiceMock.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Product1" } };
            _productServiceMock.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Single(returnProducts);
        }

        [Fact]
        public async Task GetProduct_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _productServiceMock.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.Id, returnProduct.Id);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _productServiceMock.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedAtAction_WithCreatedProduct()
        {
            // Arrange
            var product = new Product { Name = "NewProduct" };
            var createdProduct = new Product { Id = 1, Name = "NewProduct" };
            _productServiceMock.Setup(service => service.CreateProductAsync(product)).ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.CreateProduct(product);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnProduct = Assert.IsType<Product>(createdAtActionResult.Value);
            Assert.Equal(createdProduct.Id, returnProduct.Id);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOkResult_WithUpdatedProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "UpdatedProduct" };
            _productServiceMock.Setup(service => service.UpdateProductAsync(1, product)).ReturnsAsync(product);

            // Act
            var result = await _controller.UpdateProduct(1, product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.Name, returnProduct.Name);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "UpdatedProduct" };
            _productServiceMock.Setup(service => service.UpdateProductAsync(1, product)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.UpdateProduct(1, product);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_WhenProductIsDeleted()
        {
            // Arrange
            _productServiceMock.Setup(service => service.DeleteProductAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _productServiceMock.Setup(service => service.DeleteProductAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}