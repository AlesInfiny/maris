---
title: .NET 編（ CSR 編）
description: CSR アプリケーションの サーバーサイドで動作する .NET アプリケーションの 開発手順を解説します。
---

# ヘルスチェック API の実装 {#top}

アプリケーションやデータベースの死活確認のため、 ASP.NET Core の機能を利用してヘルスチェック API を実装できます。

## 基本的な実装方法 {#basic}

ヘルスチェックでは以下の 2 つの状態を区別してアプリケーションの正常性を確認する場合があります。

- 活動性：アプリケーションが正常に起動していること
- 対応性：アプリケーションが正常に起動しており、かつリクエスト受付可能であること

活動性と対応性については [こちら :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks#separate-readiness-and-liveness-probes){ target=_blank }を参照してください。

ヘルスチェック API へのアクセスが非常に多くなる等の事情により、アプリケーションの活動性のみを確認したい場合は以下のように実装します。

```csharp title="Program.cs" hl_lines="4 9"
var builder = WebApplication.CreateBuilder(args);

// ヘルスチェックサービスを追加する
builder.Services.AddHealthChecks();

var app = builder.Build();

// ヘルスチェック API のエンドポイントをマッピングする
app.MapHealthChecks("/health");

app.Run();
```

上記の実装では、`/health` にアクセスすることでアプリケーションが正常に起動していることを確認できます。

ヘルスチェック実行時のレスポンスとして [`HealthStatus` :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthstatus){ target=_blank } がプレーンテキスト形式で返されます。
アプリケーションが起動状態の場合に `Healthy` 、停止状態の場合に `Unhealthy` が返されます。
`HealthStatus` をどのように使い分けるかについては、[HealthStatus の使い分け](#health-status) を参照してください。

## 対応性を確認するための実装方法 {#readiness-probe}

システム全体の対応性を確認するため、データベース等の関連する外部サービスのヘルスチェックを行う際はヘルスチェックロジックを追加します。

[実装方針](../../../../app-architecture/client-side-rendering/global-function/health-check-implementation.md#add-health-check-logic) で説明している通り、 `Program.cs` に直接ヘルスチェックロジックを追加しません。ヘルスチェック対象の外部サービスに依存する各プロジェクトへヘルスチェックロジックを配置します。

プロジェクトごとにヘルスチェックロジックを分割する方法としては以下の 2 通りがあります。

- [`IHealthChecksBuilder` の拡張メソッドを作成する方法](#extension-method)
- [`IHealthCheck` インターフェース実装クラスを作成する方法](#using-interface)

### `IHealthChecksBuilder` の拡張メソッドを作成する方法 {#extension-method}

1. `IHealthChecksBuilder` を戻り値とする `IHealthChecksBuilder` の拡張メソッドを実装

    <!-- textlint-disable ja-technical-writing/sentence-length -->
    `IHealthChecksBuilder` の [`AddCheck` メソッド :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.dependencyinjection.healthchecksbuilderdelegateextensions.addcheck){ target=_blank } や [`AddDbContextCheck` メソッド :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.dependencyinjection.entityframeworkcorehealthchecksbuilderextensions.adddbcontextcheck){ target=_blank } にヘルスチェックロジックを渡すよう拡張メソッドを実装します。
    <!-- textlint-enable ja-technical-writing/sentence-length -->

    <!-- textlint-disable ja-technical-writing/sentence-length -->

    1. Entity Framework Core を利用したアプリケーションにおいてデータベースのヘルスチェックを行う場合
        - NuGet パッケージ [Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore :material-open-in-new:](https://www.nuget.org/packages/Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore ){ target=_blank } の参照を追加

        ```csharp title="DbHealthChecksBuilderExtensions.cs" hl_lines="13 20-30"
        using Microsoft.EntityFrameworkCore;
        using Microsoft.Extensions.DependencyInjection;
        using Microsoft.Extensions.Diagnostics.HealthChecks;

        namespace SampleSystem.Infrastructure;

        public static class DbHealthChecksBuilderExtensions
        {
            public static IHealthChecksBuilder AddSampleDbContextCheck(
                this IHealthChecksBuilder builder, string? name = null, HealthStatus? failureStatus = default, IEnumerable<string>? tags = default)
            {
                // IHealthChecksBuilder.AddDbContextCheck メソッドにヘルスチェックロジックを渡す
                return builder.AddDbContextCheck<SampleDbContext>(
                    name,
                    failureStatus,
                    tags,
                    async (context, token) =>
                    {
                        // ヘルスチェックロジックを実装
                        try
                        {
                            await context.Database.ExecuteSqlRawAsync("SELECT 1", token);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            // ヘルスチェック失敗時の処理

                            return false;
                        }
                    });
            }
        }
        ```

    <!-- textlint-enable ja-technical-writing/sentence-length -->

    1. その他の外部サービスについてヘルスチェックを行う場合

        ```csharp title="SampleHealthChecksBuilderExtensions.cs" hl_lines="12 16-25"
        using Microsoft.Extensions.DependencyInjection;
        using Microsoft.Extensions.Diagnostics.HealthChecks;
        
        namespace SampleSystem.XxxService;

        public static class SampleHealthChecksBuilderExtensions
        {
            public static IHealthChecksBuilder AddSampleHealthCheck(
                this IHealthChecksBuilder builder, string name, IEnumerable<string>? tags = default, TimeSpan? timeout = null)
            {
                // IHealthChecksBuilder.AddCheck メソッドにヘルスチェックロジックを渡す
                return builder.AddCheck(
                    name,
                    () =>
                    {
                        var isHealthy = true;

                        // ヘルスチェックロジックを実装

                        if (isHealthy)
                        {
                            return HealthCheckResult.Healthy();
                        }

                        return HealthCheckResult.Unhealthy();
                    },
                    tags,
                    timeout);
            }
        }
        ```

1. `Program.cs` で作成したヘルスチェックロジックをまとめて登録する

    ```csharp title="Program.cs" hl_lines="7-9"
    using SampleSystem.Infrastructure;
    using SampleSystem.XxxService;

    var builder = WebApplication.CreateBuilder(args);

    // 各プロジェクトに配置したヘルスチェックサービスを登録
    builder.Services.AddHealthChecks()
        .AddSampleDbContextCheck("SampleDatabaseHealthCheck")
        .AddSampleHealthCheck("SampleHealthCheck");

    var app = builder.Build();

    // ヘルスチェック API のエンドポイントをマッピングする
    app.MapHealthChecks("/health");

    app.Run();
    ```

上記の例では、 `/health` にアクセスすることでアプリケーションとデータベース等の外部サービスを含めたヘルスチェックが実行されます。

また、既定では `/health` にアクセスすることで登録されているヘルスチェックが全て実行されます。
アプリケーションと外部サービスのヘルスチェックを行うタイミングを分けたい場合は、[正常性チェックをフィルター処理する :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks#filter-health-checks){ target=_blank } を参照してください。

### `IHealthCheck` インターフェース実装クラスを作成する方法 {#using-interface}

<!-- textlint-disable ja-technical-writing/sentence-length -->
[`IHealthCheck` インターフェースを実装したクラスで、`CheckHealthAsync` メソッドをオーバーライドする :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks#create-health-checks){ target=_blank }ことでヘルスチェックロジックを追加できます。
<!-- textlint-enable ja-technical-writing/sentence-length -->

1. `IHealthCheck` インターフェース実装クラスを作成

    ```csharp title="SampleHealthCheck.cs" hl_lines="9-18"
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    namespace SampleSystem.XxxService;

    public class SampleHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var isHealthy = true;

            // ヘルスチェックロジックを実装

            if (isHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }

            return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus));
        }
    }
    ```

1. `Program.cs` で作成したヘルスチェックロジックを登録する

    ```csharp title="Program.cs" hl_lines="6-7"
    using SampleSystem.XxxService;

    var builder = WebApplication.CreateBuilder(args);

    // 各プロジェクトに配置したヘルスチェックサービスを登録
    builder.Services.AddHealthChecks()
        .AddCheck<SampleHealthCheck>("SampleHealthCheck");

    var app = builder.Build();

    // ヘルスチェック API のエンドポイントをマッピングする
    app.MapHealthChecks("/health");

    app.Run();    
    ```

上記の例では、 `/health` にアクセスすることでアプリケーションとデータベース等の外部サービスを含めたヘルスチェックが実行されます。

また、`IHealthCheck` インターフェース実装クラスを複数作成して `Program.cs` で登録した場合、既定では `/health` にアクセスすることで登録されているヘルスチェックが全て実行されます。
ヘルスチェックを行うタイミングを分けたい場合は、[正常性チェックをフィルター処理する :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks#filter-health-checks){ target=_blank } を参照してください。

## HealthStatus の使い分け {#health-status}

| HealthStatus           | ステータスコード | レスポンスボディ |
| ---------------------- | ---------------- | ---------------- |
| HealthStatus.Healthy   | 200              | Healthy          |
| HealthStatus.Degraded  | 200              | Degraded         |
| HealthStatus.Unhealthy | 503              | Unhealthy        |

既定のヘルスチェック機能ではアプリケーションが起動状態の場合に `Healthy` 、停止状態の場合に `Unhealthy` が返されます。
`HealthStatus` は、 `Healthy, Degraded, Unhealthy` のいずれかの値を取ります。

活動性と対応性を分けてヘルスチェックを行いたい場合に `Degraded` を返すよう実装する場合があります。
例えば、独自に追加したヘルスチェックロジックが正常に動作する場合に `Healthy` 、正常に動作しない場合に `Degraded` を返すよう実装できます。

!!! info "ヘルスチェック失敗時の `HealthStatus` を `Degraded` に指定する"
    ヘルスチェックサービス登録時、 `failureStatus` にヘルスチェック失敗時の `HealthStatus` を指定できます。

    ```csharp title="Program.cs"
    builder.Services.AddHealthChecks()
        .AddCheck<SampleHealthCheck>(
        "Sample",
        failureStatus: HealthStatus.Degraded);
    ```
