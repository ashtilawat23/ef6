using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Services;
using Demo.Models;

namespace Demo.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        private ProductService _productService;

        [TestInitialize]
        public void Setup()
        {
            _productService = new ProductService();
        }

        [TestMethod]
        public void AddProduct_ShouldAddProduct_WhenValidDataProvided()
        {
            // Arrange
            string name = "Test Product";
            decimal price = 10.99m;
            string description = "Test Description";

            // Act
            var result = _productService.AddProduct(name, price, description);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(price, result.Price);
            Assert.AreEqual(description, result.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Product name cannot be empty")]
        public void AddProduct_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            string name = " ";
            decimal price = 10.99m;

            // Act
            _productService.AddProduct(name, price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Price must be greater than zero")]
        public void AddProduct_ShouldThrowException_WhenPriceIsZeroOrNegative()
        {
            // Arrange
            string name = "Test Product";
            decimal price = 0;

            // Act
            _productService.AddProduct(name, price);
        }

        [TestMethod]
        public void GetProduct_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = _productService.AddProduct("Test Product", 10.99m);

            // Act
            var result = _productService.GetProduct(product.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.Id, result.Id);
        }

        [TestMethod]
        public void GetProduct_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Act
            var result = _productService.GetProduct(999);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            _productService.AddProduct("Product 1", 10.99m);
            _productService.AddProduct("Product 2", 20.99m);

            // Act
            var result = _productService.GetAllProducts();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetAvailableProducts_ShouldReturnOnlyAvailableProducts()
        {
            // Arrange
            var product1 = _productService.AddProduct("Product 1", 10.99m);
            var product2 = _productService.AddProduct("Product 2", 20.99m);
            product2.IsAvailable = false;

            // Act
            var result = _productService.GetAvailableProducts();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(product1.Id, result.First().Id);
        }

        [TestMethod]
        public void UpdateProduct_ShouldUpdateProduct_WhenValidDataProvided()
        {
            // Arrange
            var product = _productService.AddProduct("Product", 10.99m);
            string newName = "Updated Product";
            decimal newPrice = 15.99m;
            string newDescription = "Updated Description";

            // Act
            var result = _productService.UpdateProduct(product.Id, newName, newPrice, newDescription);

            // Assert
            Assert.IsTrue(result);
            var updatedProduct = _productService.GetProduct(product.Id);
            Assert.AreEqual(newName, updatedProduct.Name);
            Assert.AreEqual(newPrice, updatedProduct.Price);
            Assert.AreEqual(newDescription, updatedProduct.Description);
        }

        [TestMethod]
        public void UpdateProduct_ShouldNotUpdate_WhenProductDoesNotExist()
        {
            // Act
            var result = _productService.UpdateProduct(999, "New Name", 10.99m);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Price must be greater than zero")]
        public void UpdateProduct_ShouldThrowException_WhenNewPriceIsZeroOrNegative()
        {
            // Arrange
            var product = _productService.AddProduct("Product", 10.99m);

            // Act
            _productService.UpdateProduct(product.Id, price: 0);
        }

        [TestMethod]
        public void DeleteProduct_ShouldRemoveProduct_WhenProductExists()
        {
            // Arrange
            var product = _productService.AddProduct("Product", 10.99m);

            // Act
            var result = _productService.DeleteProduct(product.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(_productService.GetProduct(product.Id));
        }

        [TestMethod]
        public void DeleteProduct_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Act
            var result = _productService.DeleteProduct(999);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ApplyDiscountToAll_ShouldApplyDiscountToAllProducts()
        {
            // Arrange
            var product1 = _productService.AddProduct("Product 1", 100m);
            var product2 = _productService.AddProduct("Product 2", 200m);
            decimal discountPercentage = 10;

            // Act
            _productService.ApplyDiscountToAll(discountPercentage);

            // Assert
            Assert.AreEqual(90m, product1.Price);
            Assert.AreEqual(180m, product2.Price);
        }
    }
}