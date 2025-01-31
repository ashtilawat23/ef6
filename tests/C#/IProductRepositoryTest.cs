using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Application.Repositories;
using Backend.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace Backend.Tests.Repositories
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private List<Product> _productList;

        [SetUp]
        public void SetUp()
        {
            _mockProductRepository = new Mock<IProductRepository>();

            _productList = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 10.0 },
                new Product { Id = 2, Name = "Product2", Price = 20.0 }
            };

            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_productList);
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => _productList.FirstOrDefault(p => p.Id == id));
            _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => 
                {
                    _productList.Add(product);
                    return product;
                });
            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => 
                {
                    var existingProduct = _productList.FirstOrDefault(p => p.Id == product.Id);
                    if (existingProduct != null)
                    {
                        existingProduct.Name = product.Name;
                        existingProduct.Price = product.Price;
                    }
                    return existingProduct;
                });
            _mockProductRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Product>()))
                .Callback((Product product) => _productList.Remove(product));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Act
            var result = await _mockProductRepository.Object.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count(), "GetAllAsync should return all products.");
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ShouldReturnProduct()
        {
            // Act
            var result = await _mockProductRepository.Object.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result, "GetByIdAsync should return a product for a valid ID.");
            Assert.AreEqual(1, result.Id, "GetByIdAsync should return the correct product.");
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _mockProductRepository.Object.GetByIdAsync(99);

            // Assert
            Assert.IsNull(result, "GetByIdAsync should return null for an invalid ID.");
        }

        [Test]
        public async Task AddAsync_ValidProduct_ShouldAddAndReturnProduct()
        {
            // Arrange
            var newProduct = new Product { Id = 3, Name = "Product3", Price = 30.0 };

            // Act
            var result = await _mockProductRepository.Object.AddAsync(newProduct);

            // Assert
            Assert.AreEqual(newProduct, result, "AddAsync should return the added product.");
            Assert.AreEqual(3, _productList.Count, "AddAsync should add the product to the list.");
        }

        [Test]
        public async Task UpdateAsync_ExistingProduct_ShouldUpdateAndReturnProduct()
        {
            // Arrange
            var updatedProduct = new Product { Id = 1, Name = "UpdatedProduct1", Price = 15.0 };

            // Act
            var result = await _mockProductRepository.Object.UpdateAsync(updatedProduct);

            // Assert
            Assert.IsNotNull(result, "UpdateAsync should return the updated product.");
            Assert.AreEqual("UpdatedProduct1", result.Name, "UpdateAsync should update the product name.");
        }

        [Test]
        public async Task UpdateAsync_NonExistingProduct_ShouldReturnNull()
        {
            // Arrange
            var nonExistingProduct = new Product { Id = 99, Name = "NonExistent", Price = 50.0 };

            // Act
            var result = await _mockProductRepository.Object.UpdateAsync(nonExistingProduct);

            // Assert
            Assert.IsNull(result, "UpdateAsync should return null for a non-existing product.");
        }

        [Test]
        public void DeleteAsync_ExistingProduct_ShouldRemoveProduct()
        {
            // Arrange
            var productToDelete = _productList.First();

            // Act
            _mockProductRepository.Object.DeleteAsync(productToDelete);

            // Assert
            Assert.AreEqual(1, _productList.Count, "DeleteAsync should remove the product from the list.");
            Assert.IsFalse(_productList.Contains(productToDelete), "DeleteAsync should remove the correct product.");
        }

        [Test]
        public void DeleteAsync_NonExistingProduct_ShouldNotThrow()
        {
            // Arrange
            var nonExistingProduct = new Product { Id = 99, Name = "NonExistent", Price = 50.0 };

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _mockProductRepository.Object.DeleteAsync(nonExistingProduct),
                "DeleteAsync should not throw an exception for a non-existing product.");
        }
    }
}