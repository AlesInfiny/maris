using Xunit;

namespace Dressca.IntegrationTest;

public class DatabaseHealthCheckTest : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
{
    private readonly IntegrationTestWebApplicationFactory<Program> factory;

    public DatabaseHealthCheckTest(IntegrationTestWebApplicationFactory<Program> factory)
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
