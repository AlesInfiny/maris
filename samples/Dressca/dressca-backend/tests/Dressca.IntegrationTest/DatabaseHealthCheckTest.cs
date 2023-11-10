using Xunit;

namespace Dressca.IntegrationTest;

public class DatabaseHealthCheckTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> factory;

    public DatabaseHealthCheckTest(CustomWebApplicationFactory<Program> factory)
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
        var healthCheckResult = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", healthCheckResult);
    }
}
