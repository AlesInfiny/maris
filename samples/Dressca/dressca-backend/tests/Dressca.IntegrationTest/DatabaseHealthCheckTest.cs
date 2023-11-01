using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Dressca.IntegrationTest;

public class DatabaseHealthCheckTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public DatabaseHealthCheckTest(WebApplicationFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task DatabaseConnectionTest()
    {
        // Arrange
        var client = this.factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/health");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("Healthy", response.Content.ReadAsStringAsync().Result);
    }
}
