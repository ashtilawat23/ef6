using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Backend.Api.Middleware.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ILogger<ErrorHandlingMiddleware>> _mockLogger;
        private readonly ErrorHandlingMiddleware _middleware;

        public ErrorHandlingMiddlewareTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<ErrorHandlingMiddleware>>();
            _middleware = new ErrorHandlingMiddleware(_mockNext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task InvokeAsync_ShouldCallNextDelegate_WhenNoExceptionThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ShouldHandleException_WhenExceptionThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _mockNext.Setup(next => next(It.IsAny<HttpContext>())).Throws(new Exception("Test exception"));

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.Is<string>(s => s.Contains("An unhandled exception occurred"))), Times.Once);
            Assert.Equal("application/json", context.Response.ContentType);
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        [Fact]
        public async Task HandleExceptionAsync_ShouldReturnCorrectResponseFormat_WhenExceptionThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var exception = new Exception("Test exception");

            // Act
            await ErrorHandlingMiddleware.HandleExceptionAsync(context, exception);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
            var expectedResponse = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An internal server error occurred.",
                DetailedMessage = "Test exception"
            };
            var expectedJson = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Assert.Equal(expectedJson, responseBody);
        }

        [Fact]
        public async Task HandleExceptionAsync_ShouldSetResponseContentTypeToJson_WhenCalled()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var exception = new Exception("Test exception");

            // Act
            await ErrorHandlingMiddleware.HandleExceptionAsync(context, exception);

            // Assert
            Assert.Equal("application/json", context.Response.ContentType);
        }

        [Fact]
        public async Task HandleExceptionAsync_ShouldSetResponseStatusCodeToInternalServerError_WhenCalled()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var exception = new Exception("Test exception");

            // Act
            await ErrorHandlingMiddleware.HandleExceptionAsync(context, exception);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }
    }
}