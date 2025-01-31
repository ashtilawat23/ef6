using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class StartupTests
{
    private readonly HttpClient _client;
    private readonly Mock<IProductService> _productServiceMock;

    public StartupTests()
    {
        _productServiceMock = new Mock<IProductService>();

        var host = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost
                    .UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddScoped(_ => _productServiceMock.Object);
                    })
                    .UseStartup<Startup>();
            })
            .Start();

        _client = host.GetTestClient();
    }

    [Fact]
    public async Task GetSwaggerEndpoint_ShouldReturnSwaggerJson()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/swagger/v1/swagger.json");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"title\":\"Demo API\"", content);
    }

    [Fact]
    public async Task GetControllersEndpoint_ShouldReturnSuccess()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/values");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.True(response.IsSuccessStatusCode, "Expected success status code");
    }

    [Fact]
    public void ProductService_ShouldBeRegisteredInDIContainer()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddScoped<IProductService, ProductService>()
            .BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<IProductService>();

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task HttpsRedirection_ShouldRedirectToHttps()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/values");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("https://", response.Headers.Location.ToString());
    }

    [Fact]
    public async Task UseAuthorization_ShouldReturnUnauthorizedForUnauthorizedRequest()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/secure");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}