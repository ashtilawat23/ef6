using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Application.Repositories;
using Backend.Application.Services;
using Backend.Domain.Entities;
using Moq;
using Xunit;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1" },
            new Product { Id = 2, Name = "Product 2" }
        };
        _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.Equal(products, result);
    }

    [Fact]
    public async Task GetProductByIdAsync_ExistingId_ReturnsProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product 1" };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task GetProductByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.GetProductByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProductAsync_ValidProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var product = new Product { Name = "New Product" };
        var createdProduct = new Product { Id = 1, Name = "New Product", CreatedAt = DateTime.UtcNow };
        _productRepositoryMock.Setup(repo => repo.AddAsync(product)).ReturnsAsync(createdProduct);

        // Act
        var result = await _productService.CreateProductAsync(product);

        // Assert
        Assert.Equal(createdProduct, result);
        Assert.Equal(createdProduct.CreatedAt, product.CreatedAt);
    }

    [Fact]
    public async Task UpdateProductAsync_ExistingProduct_UpdatesAndReturnsProduct()
    {
        // Arrange
        var existingProduct = new Product { Id = 1, Name = "Old Product" };
        var updatedProduct = new Product { Id = 1, Name = "Updated Product" };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduct);
        _productRepositoryMock.Setup(repo => repo.UpdateAsync(existingProduct)).ReturnsAsync(updatedProduct);

        // Act
        var result = await _productService.UpdateProductAsync(1, updatedProduct);

        // Assert
        Assert.Equal(updatedProduct, result);
    }

    [Fact]
    public async Task UpdateProductAsync_NonExistingProduct_ReturnsNull()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.UpdateProductAsync(999, new Product());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteProductAsync_ExistingProduct_ReturnsTrue()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product to Delete" };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
        _productRepositoryMock.Setup(repo => repo.DeleteAsync(product)).Returns(Task.CompletedTask);

        // Act
        var result = await _productService.DeleteProductAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteProductAsync_NonExistingProduct_ReturnsFalse()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.DeleteProductAsync(999);

        // Assert
        Assert.False(result);
    }
}