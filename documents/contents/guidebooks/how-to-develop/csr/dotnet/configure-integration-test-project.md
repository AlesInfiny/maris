---
title: .NET 編（CSR 編）
description: CSR アプリケーションの サーバーサイドで動作する .NET アプリケーションの 開発手順を解説します。
---

<!-- cspell:ignore localdb mssqllocaldb -->

# 結合テストプロジェクトの構成 {#top}

## 結合テストプロジェクトの設定 {#test-project-settings}

結合テスト用に xUnit v3 のテストプロジェクトを作成し、以下を設定します。

- テスト対象プロジェクトを参照
- 結合テストプロジェクト実行に必要な NuGet パッケージのインストール
    - [xunit.v3 :material-open-in-new:](https://www.nuget.org/packages/xunit.v3){ target=_blank }
    - [xunit.runner.visualstudio :material-open-in-new:](https://www.nuget.org/packages/xunit.runner.visualstudio){ target=_blank }
    - [Microsoft.AspNetCore.Mvc.Testing :material-open-in-new:](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing){ target=_blank }

## テスト対象プロジェクトの設定 {#target-project-settings}

テスト対象プロジェクトの `Program.cs` を部分クラス宣言を利用してテストプロジェクトに公開します。

```csharp title="Program.cs" hl_lines="4"
var builder = WebApplication.CreateBuilder(args);
// ...
app.Run();
public partial class Program {}
```

!!! note "internal メンバーをテストプロジェクトに公開する"
    テスト対象プロジェクトの internal メンバーをテストプロジェクトから参照可能にした場合にも `Program.cs` を公開できます。
    詳細は以下を参照してください。

    [既定の WebApplicationFactory を使用した基本的なテスト :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#basic-tests-with-the-default-webapplicationfactory){ target=_blank }

## テストコード作成 {#writing-test-codes}

### 基本のテスト {#basic}

<!-- textlint-disable ja-technical-writing/sentence-length -->

テストクラスに [`IClassFixture<TFixture>` :material-open-in-new:](https://xunit.net/docs/shared-context#class-fixture){ target=_blank } インターフェースを実装し、 [`WebApplicationFactory<TEntryPoint>` :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1){ target=_blank }のインスタンスをテストクラスから利用します。
`WebApplicationFactory<TEntryPoint>` はテスト対象アプリケーションの `TestServer` インスタンスを提供します。

<!-- textlint-enable ja-technical-writing/sentence-length -->

```csharp hl_lines="2"
public class BasicTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        // TestServerにリクエストを送信するHttpClientを取得
        var client = this.factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
}
```

上記のコードでは、テスト対象のページにアクセスした際のステータスコードと Content-Type ヘッダーの内容を確認しています。

詳細は以下を参照してください。

[既定の WebApplicationFactory を使用した基本的なテスト :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#basic-tests-with-the-default-webapplicationfactory){ target=_blank }

### テスト対象アプリケーションの構成設定 {#change-settings-of-target-app}

システム構成が異なるローカル環境と CI 環境でテストを行う場合等、テスト対象アプリケーションの構成を変更してテストを行う際は `WebApplicationFactory<TEntryPoint>` クラスを継承したクラスを作成します。

以下の手順では、結合テスト用にデータベースコンテキストを差し替えています。

1. `WebApplicationFactory<TEntryPoint>` クラスを継承したクラスでテスト対象アプリケーションの構成を変更する。

    ```csharp hl_lines="2"
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
                var connectionString = config.GetConnectionString("SampleDbContext");

                // テスト用の設定を使用してDbContextをサービス登録
                services.AddDbContext<SampleDbContext>(option =>
                {
                    option.UseSqlServer(connectionString);
                });
            });
        }
    }
    ```

    ??? example "appsettings.{env}.json の設定例"

        ```json title="appsettings.Development.json"
        {
            "Logging": {
                "LogLevel": {
                    "Default": "Information",
                    "Microsoft.AspNetCore": "Warning",
                    "Microsoft.AspNetCore.HttpLogging": "Information",
                    "Microsoft.Extensions.Diagnostics.HealthChecks": "Debug",
                    "SampleApp": "Debug"
                    }
            },
            "ConnectionStrings": {
                "SampleDbContext": "Data Source=(localdb)\\mssqllocaldb;Database=SampleDb;Integrated Security=True"
            }
        }
        ```
        ```json title="appsettings.IntegrationTest.json"
        {
            "Logging": {
                "LogLevel": {
                    "Default": "Information",
                    "Microsoft.AspNetCore": "Warning",
                    "SampleApp": "Information"
                }
            },
            "ConnectionStrings": {
                "SampleDbContext": "Server=localhost,1433;Database=SampleDb;User=<ユーザー名>;Password=<パスワード>;TrustServerCertificate=true;"
            },
            "AllowedHosts": "*"
        }
        ```

1. テストクラスで実装する `IClassFixture` インターフェースの型引数を `CustomWebApplicationFactory<Program>` とする。

    ```csharp hl_lines="2"
    public class IndexPageTests
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        private readonly CustomWebApplicationFactory<Program> factory;

        public IndexPageTests(CustomWebApplicationFactory<Program> factory)
        {
            this.factory = factory;
            this.client = this.factory.CreateClient();
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

    上記の実装により、環境変数 `TEST_ENVIRONMENT` の値に応じて構成ファイルを読み込み、アプリケーションのデータベース接続先を変更しています。
    ビルドマシンごとに異なる `TEST_ENVIRONMENT` の値を設定し、 `appsettings.{TEST_ENVIRONMENT}.json` を切り替えることができます。また、 `IClassFixture` インターフェースの型引数を `CustomWebApplicationFactory<Program>` とすることで、構成を変更したテスト対象アプリケーションの設定でテストを実行します。

詳細は以下を参照してください。

[WebApplicationFactory のカスタマイズ :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests#customize-webapplicationfactory){ target=_blank }
