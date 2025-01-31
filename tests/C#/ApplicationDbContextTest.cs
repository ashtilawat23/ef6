using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Backend.Infrastructure.Data;
using Backend.Domain.Entities;

public class ApplicationDbContextTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public ApplicationDbContextTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(_options);
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _context.Products.AddRange(
            new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10.00m },
            new Product { Id = 2, Name = "Product2", Description = "Description2", Price = 20.00m }
        );
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public void AddProduct_ShouldIncreaseProductCount()
    {
        // Arrange
        var newProduct = new Product { Id = 3, Name = "Product3", Description = "Description3", Price = 30.00m };

        // Act
        _context.Products.Add(newProduct);
        _context.SaveChanges();

        // Assert
        Assert.Equal(3, _context.Products.Count());
    }

    [Fact]
    public void AddProduct_WithMissingName_ShouldThrowException()
    {
        // Arrange
        var newProduct = new Product { Id = 3, Description = "Description3", Price = 30.00m };

        // Act & Assert
        Assert.Throws<DbUpdateException>(() =>
        {
            _context.Products.Add(newProduct);
            _context.SaveChanges();
        });
    }

    [Fact]
    public void AddProduct_WithNameExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var newProduct = new Product { Id = 3, Name = new string('A', 101), Description = "Description3", Price = 30.00m };

        // Act & Assert
        Assert.Throws<DbUpdateException>(() =>
        {
            _context.Products.Add(newProduct);
            _context.SaveChanges();
        });
    }

    [Fact]
    public void AddProduct_WithPricePrecisionExceeded_ShouldThrowException()
    {
        // Arrange
        var newProduct = new Product { Id = 3, Name = "Product3", Description = "Description3", Price = 123456789012345.678m };

        // Act & Assert
        Assert.Throws<DbUpdateException>(() =>
        {
            _context.Products.Add(newProduct);
            _context.SaveChanges();
        });
    }

    [Fact]
    public void RetrieveProduct_ShouldReturnCorrectProduct()
    {
        // Arrange
        var expectedProduct = new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10.00m };

        // Act
        var product = _context.Products.Find(1);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(expectedProduct.Name, product.Name);
        Assert.Equal(expectedProduct.Description, product.Description);
        Assert.Equal(expectedProduct.Price, product.Price);
    }

    [Fact]
    public void UpdateProduct_ShouldModifyExistingProduct()
    {
        // Arrange
        var product = _context.Products.Find(1);
        product.Name = "UpdatedProduct1";

        // Act
        _context.Products.Update(product);
        _context.SaveChanges();

        // Assert
        var updatedProduct = _context.Products.Find(1);
        Assert.Equal("UpdatedProduct1", updatedProduct.Name);
    }

    [Fact]
    public void DeleteProduct_ShouldReduceProductCount()
    {
        // Arrange
        var product = _context.Products.Find(1);

        // Act
        _context.Products.Remove(product);
        _context.SaveChanges();

        // Assert
        Assert.Equal(1, _context.Products.Count());
    }
}