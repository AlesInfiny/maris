using System.Net;
using System.Net.Http.Headers;

namespace Dressca.IntegrationTest;

public class ApiTest(ApiTestWebApplicationFactory<Program> factory)
    : IClassFixture<ApiTestWebApplicationFactory<Program>>
{
    private readonly ApiTestWebApplicationFactory<Program> factory = factory;

    [Fact]
    public async Task Get_認証必要なAPI_認証エラー_Status401を返す()
    {
        // Arrange
        var client = this.factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        // 認証せずにリクエスト送信
        var response = await client.GetAsync("api/users", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_認証必要なAPI_認証成功_UserIDを返す()
    {
        // Arrange
        var client = this.factory.CreateClient();

        // ログイン成功時に取得するJWTをヘッダーに追加
        var token = this.factory.CreateToken("testUser");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var response = await client.GetAsync("api/users", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(cancellationToken);
        Assert.Equal("{\"userId\":\"testUser\"}", result);
    }

    [Fact]
    public async Task Get_認証不要なAPI_サーバー時間を返す()
    {
        // Arrange
        var client = this.factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var response = await client.GetAsync("api/serverTime", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(cancellationToken);

        // "{"serverTime":"2024/04/12 13:32:59"}"
        Assert.StartsWith("{\"serverTime\":", result);
    }
}
