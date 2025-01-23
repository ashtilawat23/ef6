using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Models;
using Demo.Services;
using Moq;

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
        public void AddProduct_ShouldAddProductSuccessfully()
        {
            // Arrange
            string name = "Laptop";
            decimal price = 999.99m;
            string description = "High-performance laptop";

            // Act
            var product = _productService.AddProduct(name, price, description);

            // Assert
            Assert.IsNotNull(product, "Product should not be null");
            Assert.AreEqual(name, product.Name, "Product name should match");
            Assert.AreEqual(price, product.Price, "Product price should match");
            Assert.AreEqual(description, product.Description, "Product description should match");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddProduct_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            string name = "";
            decimal price = 999.99m;
            string description = "High-performance laptop";

            // Act
            _productService.AddProduct(name, price, description);
        }

        [TestMethod]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            _productService.AddProduct("Laptop", 999.99m, "High-performance laptop");
            _productService.AddProduct("Smartphone", 599.99m, "Latest model smartphone");

            // Act
            var products = _productService.GetAllProducts();

            // Assert
            Assert.AreEqual(2, products.Count, "There should be two products");
        }

        [TestMethod]
        public void UpdateProduct_ShouldUpdateProductPrice()
        {
            // Arrange
            var product = _productService.AddProduct("Laptop", 999.99m, "High-performance laptop");
            decimal newPrice = 899.99m;

            // Act
            _productService.UpdateProduct(product.Id, price: newPrice);
            var updatedProduct = _productService.GetProduct(product.Id);

            // Assert
            Assert.AreEqual(newPrice, updatedProduct.Price, "Product price should be updated");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void UpdateProduct_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            Guid nonExistentProductId = Guid.NewGuid();
            decimal newPrice = 899.99m;

            // Act
            _productService.UpdateProduct(nonExistentProductId, price: newPrice);
        }

        [TestMethod]
        public void ApplyDiscountToAll_ShouldApplyDiscountCorrectly()
        {
            // Arrange
            _productService.AddProduct("Laptop", 1000m, "High-performance laptop");
            _productService.AddProduct("Smartphone", 500m, "Latest model smartphone");
            decimal discountPercentage = 10;

            // Act
            _productService.ApplyDiscountToAll(discountPercentage);

            // Assert
            foreach (var product in _productService.GetAllProducts())
            {
                Assert.AreEqual(product.OriginalPrice * (1 - discountPercentage / 100), product.Price, "Discount should be applied correctly");
            }
        }

        [TestMethod]
        public void DeleteProduct_ShouldRemoveProductSuccessfully()
        {
            // Arrange
            var product = _productService.AddProduct("Tablet", 299.99m, "10-inch tablet");

            // Act
            _productService.DeleteProduct(product.Id);
            var products = _productService.GetAllProducts();

            // Assert
            Assert.IsFalse(products.Contains(product), "Product should be removed from the list");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void DeleteProduct_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            Guid nonExistentProductId = Guid.NewGuid();

            // Act
            _productService.DeleteProduct(nonExistentProductId);
        }
    }
}