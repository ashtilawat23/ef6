using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Api.Controllers;
using Backend.Application.Services;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductsController(_mockProductService.Object);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOkResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = 1, Name = "Test Product" } };
        _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnOkResult_WithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Product" };
        _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreatedAtAction_WithCreatedProduct()
    {
        // Arrange
        var product = new Product { Name = "New Product" };
        var createdProduct = new Product { Id = 1, Name = "New Product" };
        _mockProductService.Setup(service => service.CreateProductAsync(product)).ReturnsAsync(createdProduct);

        // Act
        var result = await _controller.CreateProduct(product);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Product>(createdAtActionResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnOkResult_WithUpdatedProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Updated Product" };
        _mockProductService.Setup(service => service.UpdateProductAsync(1, product)).ReturnsAsync(product);

        // Act
        var result = await _controller.UpdateProduct(1, product);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Updated Product", returnValue.Name);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Updated Product" };
        _mockProductService.Setup(service => service.UpdateProductAsync(1, product)).ReturnsAsync((Product)null);

        // Act
        var result = await _controller.UpdateProduct(1, product);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNoContent_WhenDeleteIsSuccessful()
    {
        // Arrange
        _mockProductService.Setup(service => service.DeleteProductAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductService.Setup(service => service.DeleteProductAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}