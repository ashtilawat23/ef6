using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Application.Services;
using Backend.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace Backend.Tests.Application.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductService> _productServiceMock;
        private Product _sampleProduct;
        private IEnumerable<Product> _sampleProductList;

        [SetUp]
        public void SetUp()
        {
            _productServiceMock = new Mock<IProductService>();
            _sampleProduct = new Product { Id = 1, Name = "Sample Product", Price = 10.0 };
            _sampleProductList = new List<Product> { _sampleProduct };
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            _productServiceMock.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(_sampleProductList);

            // Act
            var result = await _productServiceMock.Object.GetAllProductsAsync();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(_sampleProductList, result, "Returned product list should match the sample product list");
        }

        [Test]
        public async Task GetProductByIdAsync_ExistingId_ReturnsProduct()
        {
            // Arrange
            int productId = 1;
            _productServiceMock.Setup(service => service.GetProductByIdAsync(productId)).ReturnsAsync(_sampleProduct);

            // Act
            var result = await _productServiceMock.Object.GetProductByIdAsync(productId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(_sampleProduct, result, "Returned product should match the sample product");
        }

        [Test]
        public async Task GetProductByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int productId = 99;
            _productServiceMock.Setup(service => service.GetProductByIdAsync(productId)).ReturnsAsync((Product?)null);

            // Act
            var result = await _productServiceMock.Object.GetProductByIdAsync(productId);

            // Assert
            Assert.IsNull(result, "Result should be null for non-existing product ID");
        }

        [Test]
        public async Task CreateProductAsync_ValidProduct_ReturnsCreatedProduct()
        {
            // Arrange
            _productServiceMock.Setup(service => service.CreateProductAsync(_sampleProduct)).ReturnsAsync(_sampleProduct);

            // Act
            var result = await _productServiceMock.Object.CreateProductAsync(_sampleProduct);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(_sampleProduct, result, "Returned product should match the sample product");
        }

        [Test]
        public void CreateProductAsync_InvalidProduct_ThrowsException()
        {
            // Arrange
            Product invalidProduct = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _productServiceMock.Object.CreateProductAsync(invalidProduct),
                "Creating a null product should throw an ArgumentNullException");
        }

        [Test]
        public async Task UpdateProductAsync_ExistingId_ValidProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            int productId = 1;
            _productServiceMock.Setup(service => service.UpdateProductAsync(productId, _sampleProduct)).ReturnsAsync(_sampleProduct);

            // Act
            var result = await _productServiceMock.Object.UpdateProductAsync(productId, _sampleProduct);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(_sampleProduct, result, "Returned product should match the sample product");
        }

        [Test]
        public async Task UpdateProductAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int productId = 99;
            _productServiceMock.Setup(service => service.UpdateProductAsync(productId, _sampleProduct)).ReturnsAsync((Product?)null);

            // Act
            var result = await _productServiceMock.Object.UpdateProductAsync(productId, _sampleProduct);

            // Assert
            Assert.IsNull(result, "Result should be null for non-existing product ID");
        }

        [Test]
        public async Task DeleteProductAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            int productId = 1;
            _productServiceMock.Setup(service => service.DeleteProductAsync(productId)).ReturnsAsync(true);

            // Act
            var result = await _productServiceMock.Object.DeleteProductAsync(productId);

            // Assert
            Assert.IsTrue(result, "Result should be true for successful deletion");
        }

        [Test]
        public async Task DeleteProductAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            int productId = 99;
            _productServiceMock.Setup(service => service.DeleteProductAsync(productId)).ReturnsAsync(false);

            // Act
            var result = await _productServiceMock.Object.DeleteProductAsync(productId);

            // Assert
            Assert.IsFalse(result, "Result should be false for non-existing product ID");
        }
    }
}