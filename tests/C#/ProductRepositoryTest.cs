using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class ProductRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly ProductRepository _repository;
    private readonly Mock<DbSet<Product>> _mockSet;

    public ProductRepositoryTests()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockSet = new Mock<DbSet<Product>>();
        _mockContext.Setup(m => m.Products).Returns(_mockSet.Object);
        _repository = new ProductRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1" },
            new Product { Id = 2, Name = "Product2" }
        };
        _mockSet.Setup(m => m.ToListAsync(default)).ReturnsAsync(products);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Name == "Product1");
        Assert.Contains(result, p => p.Name == "Product2");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1" };
        _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Product1", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        // Arrange
        _mockSet.Setup(m => m.FindAsync(2)).ReturnsAsync((Product)null);

        // Act
        var result = await _repository.GetByIdAsync(2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ValidProduct_ShouldAddProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1" };

        // Act
        var result = await _repository.AddAsync(product);

        // Assert
        _mockSet.Verify(m => m.Add(product), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task UpdateAsync_ValidProduct_ShouldUpdateProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "UpdatedProduct" };

        // Act
        var result = await _repository.UpdateAsync(product);

        // Assert
        _mockContext.Verify(m => m.Entry(product).State = EntityState.Modified, Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task DeleteAsync_ValidProduct_ShouldRemoveProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1" };

        // Act
        await _repository.DeleteAsync(product);

        // Assert
        _mockSet.Verify(m => m.Remove(product), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }
}