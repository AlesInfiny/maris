using Xunit;

namespace Dressca.Web.Admin.IntegrationTest;

public class DatabaseHealthCheckTest(IntegrationTestWebApplicationFactory<Program> factory)
    : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
{
    private readonly IntegrationTestWebApplicationFactory<Program> factory = factory;

    [Fact]
    public async Task Get_ApiHealth_DBまで含めたヘルスチェックが正常に動作_Healthyを返す()
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
