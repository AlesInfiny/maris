using System;
using System.Collections.Generic;
using System.Text;

namespace Dressca.Web.Admin.IntegrationTest;

public class SecuritySettingsHeaderTest(IntegrationTestWebApplicationFactory<Program> factory)
    : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
{
    private readonly IntegrationTestWebApplicationFactory<Program> factory = factory;

    [Fact]
    public async Task Get_ヘルスチェックAPI_SecuritySettingsHeaderが正しく設定されている()
    {
        // Arrange
        var client = this.factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var response = await client.GetAsync("api/health", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.True(response.Headers.Contains("X-Content-Type-Options"));
        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").SingleOrDefault());
        Assert.True(response.Headers.Contains("X-Frame-Options"));
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").SingleOrDefault());
        Assert.True(response.Headers.Contains("Content-Security-Policy"));
        Assert.Equal("frame-ancestors 'none'", response.Headers.GetValues("Content-Security-Policy").SingleOrDefault());
    }

    [Fact]
    public async Task Get_存在しないAPI_SecuritySettingsHeaderが正しく設定されている()
    {
        // Arrange
        var client = this.factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var response = await client.GetAsync("api/nonexistent", cancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        Assert.True(response.Headers.Contains("X-Content-Type-Options"));
        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").SingleOrDefault());
        Assert.True(response.Headers.Contains("X-Frame-Options"));
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").SingleOrDefault());
        Assert.True(response.Headers.Contains("Content-Security-Policy"));
        Assert.Equal("frame-ancestors 'none'", response.Headers.GetValues("Content-Security-Policy").SingleOrDefault());
    }
}
