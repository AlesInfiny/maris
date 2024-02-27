---
title: .NET 編
description: バックエンドで動作する .NET アプリケーションの開発手順を解説します。
---

# 結合テストプロジェクトの構成 {#top}

## 結合テストプロジェクトの設定 {#test-project-settings}

結合テスト用に Xunit のテストプロジェクトを作成し、以下を設定します。

- テスト対象プロジェクトを参照
- 結合テストプロジェクト実行に必要な NuGet パッケージのインストール
    - [xunit :material-open-in-new:](https://www.nuget.org/packages/xunit){ target=_blank }
    - [xunit.runner.visualstudio :material-open-in-new:](https://www.nuget.org/packages/xunit.runner.visualstudio){ target=_blank }
    - [Microsoft.AspNetCore.Mvc.Testing :material-open-in-new:](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing){ target=_blank }

## テスト対象プロジェクトの設定 {#target-project-settings}

テスト対象のプロジェクトの `Program.cs` を以下のいずれかの方法でテストプロジェクトに公開します。

- テスト対象プロジェクトの internal メンバーをテストプロジェクトから参照できるようにする
- 部分クラス宣言を利用して `Program.cs` を public にする

詳細は以下を参照してください。
[既定の WebApplicationFactory を使用した基本的なテスト](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#basic-tests-with-the-default-webapplicationfactory)

## テストコード作成 {#writing-test-codes}

### 基本のテスト {#basic}

テストクラスが `IClassFixture<WebApplicationFactory<Program>>` インターフェースを実装する形にします。
これにより、テスト対象アプリケーションを模した結合テスト用の `TestServer` インスタンスを各テストケースごとに生成できます。

``` C#
public class BasicTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    [InlineData("/About")]
    [InlineData("/Privacy")]
    [InlineData("/Contact")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
    }
}
```

詳細は以下を参照してください。
[既定の WebApplicationFactory を使用した基本的なテスト](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#basic-tests-with-the-default-webapplicationfactory)

### テスト対象アプリケーションのカスタマイズ {#customize-target-app}

テスト対象アプリケーションの構成を変更してテストを行いたい場合は、 `WebApplicationFactory` クラスを継承したクラスを作成します。

``` C#
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });

        builder.UseEnvironment("Development");
    }
}
```

テストクラスで実装する `IClassFixture` インターフェースの型引数を `CustomWebApplicationFactory` とします。

``` C#
public class IndexPageTests :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program>
        _factory;

    public IndexPageTests(
        CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
```

詳細は以下を参照してください。
[WebApplicationFactory のカスタマイズ](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#customize-webapplicationfactory)
