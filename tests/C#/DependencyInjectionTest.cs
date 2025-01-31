using System;
using Backend.Application;
using Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Backend.Tests.Application
{
    [TestClass]
    public class DependencyInjectionTests
    {
        private IServiceCollection _services;

        [TestInitialize]
        public void Setup()
        {
            _services = new ServiceCollection();
        }

        [TestMethod]
        public void AddApplicationServices_ShouldRegisterIProductService()
        {
            // Arrange
            // Act
            _services.AddApplicationServices();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            var productService = serviceProvider.GetService<IProductService>();
            Assert.IsNotNull(productService, "IProductService should be registered in the service collection.");
            Assert.IsInstanceOfType(productService, typeof(ProductService), "Registered service should be of type ProductService.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddApplicationServices_ShouldThrowExceptionWhenServiceNotRegistered()
        {
            // Arrange
            // Act
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            var productService = serviceProvider.GetService<IProductService>();
            Assert.IsNull(productService, "IProductService should not be registered before AddApplicationServices is called.");
        }

        [TestMethod]
        public void AddApplicationServices_ShouldRegisterServiceAsScoped()
        {
            // Arrange
            _services.AddApplicationServices();
            var serviceProvider = _services.BuildServiceProvider();

            // Act
            var firstInstance = serviceProvider.GetService<IProductService>();
            var secondInstance = serviceProvider.GetService<IProductService>();

            // Assert
            Assert.AreNotSame(firstInstance, secondInstance, "IProductService should be registered with a Scoped lifetime.");
        }

        [TestMethod]
        public void AddApplicationServices_ShouldNotThrowExceptionOnMultipleCalls()
        {
            // Arrange
            // Act
            _services.AddApplicationServices();
            _services.AddApplicationServices(); // Calling again to ensure no exception is thrown

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var productService = serviceProvider.GetService<IProductService>();
            Assert.IsNotNull(productService, "IProductService should be registered even after multiple calls to AddApplicationServices.");
        }
    }
}