﻿using System.Net;
using System.Net.Http.Headers;
using Xunit;

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

        // Act
        // 認証せずにリクエスト送信
        var response = await client.GetAsync("api/users");

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

        // Act
        var response = await client.GetAsync("api/users");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("{\"userId\":\"testUser\"}", result);
    }

    [Fact]
    public async Task Get_認証不要なAPI_サーバー時間を返す()
    {
        // Arrange
        var client = this.factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/servertime");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        // "{"serverTime":"2024/04/12 13:32:59"}"
        Assert.StartsWith("{\"serverTime\":", result);
    }
}
