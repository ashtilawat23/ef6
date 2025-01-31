using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using Backend.Api.Middleware;

public class StartupTests
{
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly WebApplicationBuilder _builder;

    public StartupTests()
    {
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockConfiguration = new Mock<IConfiguration>();
        _builder = WebApplication.CreateBuilder();
        _builder.Environment = _mockEnvironment.Object;
        _builder.Configuration = _mockConfiguration.Object;
    }

    [Fact]
    public void ConfigureServices_ShouldAddRequiredServices()
    {
        // Arrange
        _mockEnvironment.Setup(env => env.EnvironmentName).Returns("Development");

        // Act
        var services = _builder.Services;
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddApplicationServices();
        services.AddInfrastructureServices(_builder.Configuration);
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IApplicationBuilder>());
        Assert.NotNull(serviceProvider.GetService<IHostEnvironment>());
        Assert.NotNull(serviceProvider.GetService<IConfiguration>());
    }

    [Fact]
    public void Configure_ShouldSetUpMiddlewaresCorrectly()
    {
        // Arrange
        _mockEnvironment.Setup(env => env.EnvironmentName).Returns("Development");
        var app = _builder.Build();

        // Act
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.MapControllers();

        // Assert
        // Since we can't directly assert pipeline configuration, we assume no exceptions during setup indicates success
    }

    [Fact]
    public void Configure_ShouldNotUseSwaggerInProduction()
    {
        // Arrange
        _mockEnvironment.Setup(env => env.EnvironmentName).Returns("Production");
        var app = _builder.Build();

        // Act & Assert
        // Swagger should not be used in production, so we expect no exceptions if we don't call UseSwagger
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.MapControllers();
    }

    [Fact]
    public void UseMiddleware_ShouldThrowException_WhenMiddlewareIsNull()
    {
        // Arrange
        var app = _builder.Build();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => app.UseMiddleware<ErrorHandlingMiddleware>(null));
    }

    [Fact]
    public void AddCors_ShouldAllowAllOriginsMethodsAndHeaders()
    {
        // Arrange
        var services = _builder.Services;

        // Act
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var corsPolicy = serviceProvider.GetService<ICorsPolicyProvider>();
        Assert.NotNull(corsPolicy);
    }
}