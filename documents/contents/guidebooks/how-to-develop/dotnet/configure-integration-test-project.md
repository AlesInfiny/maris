---
title: .NET 編
description: バックエンドで動作する .NET アプリケーションの開発手順を解説します。
---

# 結合テストプロジェクトの構成 {#top}

## 結合テストプロジェクトの設定 {#test-project-settings}

結合テスト用に xUnit のテストプロジェクトを作成し、以下を設定します。

- テスト対象プロジェクトを参照
- 結合テストプロジェクト実行に必要な NuGet パッケージのインストール
    - [xunit :material-open-in-new:](https://www.nuget.org/packages/xunit){ target=_blank }
    - [xunit.runner.visualstudio :material-open-in-new:](https://www.nuget.org/packages/xunit.runner.visualstudio){ target=_blank }
    - [Microsoft.AspNetCore.Mvc.Testing :material-open-in-new:](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing){ target=_blank }

## テスト対象プロジェクトの設定 {#target-project-settings}

テスト対象プロジェクトの `Program.cs` を以下のいずれかの方法でテストプロジェクトに公開します。

- テスト対象プロジェクトの internal メンバーをテストプロジェクトから参照できるようにする
- 部分クラス宣言を利用して `Program.cs` を public にする

詳細は以下を参照してください。
[既定の WebApplicationFactory を使用した基本的なテスト](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#basic-tests-with-the-default-webapplicationfactory)

## テストコード作成 {#writing-test-codes}

### 基本のテスト {#basic}

テストクラスに [`IClassFixture<TFixture>`](https://xunit.net/docs/shared-context#class-fixture) インターフェースを実装し、 `WebApplicationFactory<Program>` のインスタンスをテストクラスから利用します。
`WebApplicationFactory<Program>` はテスト対象アプリケーションの `TestServer` インスタンスを提供します。

``` C# hl_lines="2"
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
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        // TestServerにリクエストを送信するHttpClientを取得
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
}
```

上記のコードでは、テスト対象のページにアクセスした際のステータスコードと Content-Type ヘッダーの内容を確認しています。

詳細は以下を参照してください。

[既定の WebApplicationFactory を使用した基本的なテスト](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#basic-tests-with-the-default-webapplicationfactory)

### テスト対象アプリケーションのカスタマイズ {#customize-target-app}

テスト対象アプリケーションの構成を変更してテストを行いたい場合は、 `WebApplicationFactory<TEntryPoint>` クラスを継承したクラスを作成します。
以下の手順では、結合テスト用にデータベースコンテキストを差し替えています。

1. `WebApplicationFactory<TEntryPoint>` クラスを継承したクラスでテスト対象アプリケーションの構成を変更する。

    ``` C#  hl_lines="2"
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? Environments.Development;

            builder.ConfigureServices(services =>
            {
                // DbContextの設定を差し替えるためにサービス登録をいったん削除する
                services.RemoveAll<DbContextOptions<SampleDbContext>>();

                // テスト用の構成を読み込む
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .Build();
                var connectionString = config.GetConnectionString(ConnectionStringName);

                // テスト用の設定を使用してDbContextをサービス登録
                services.AddDbContext<SampleDbContext>(option =>
                {
                    option.UseSqlServer(connectionString);
                });
            });
        }
    }
    ```

    上記のコードでは、環境変数の値に応じて構成ファイルを読み込み、アプリケーションのデータベース接続先を変更しています。

1. テストクラスで実装する `IClassFixture` インターフェースの型引数を `CustomWebApplicationFactory<Program>` とします。

    ``` C# hl_lines="2"
    public class IndexPageTests :
    IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public IndexPageTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_EndpointsReturnSuccess()
        {
            // Arrange & Act
            var indexPage = await client.GetAsync("/Index");

            // Assert
            Assert.Equal(HttpStatusCode.OK, indexPage.StatusCode);
        }
    }
    ```

詳細は以下を参照してください。
[WebApplicationFactory のカスタマイズ](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#customize-webapplicationfactory)
