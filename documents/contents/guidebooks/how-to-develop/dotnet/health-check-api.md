---
title: .NET 編
description: バックエンドで動作する .NET アプリケーションの開発手順を解説します。
---

# ヘルスチェック API の実装 {#top}

アプリケーションサーバーやデータベースの死活確認のため、 ASP.NET Core の機能を利用してヘルスチェック API を実装できます。

## 基本的な実装方法 {#basic}

以下の実装を `Program.cs` に追加することで、アプリケーションの正常性を確認できます。

``` C# hl_lines="4 9"
var builder = WebApplication.CreateBuilder(args);

// ヘルスチェックサービスを追加する
builder.Services.AddHealthChecks();

var app = builder.Build();

// ヘルスチェック API のエンドポイントをマッピングする
app.MapHealthChecks("/health");

app.Run();
```

上記の実装では、`/health` にアクセスすることでヘルスチェックが実行されます。

ヘルスチェック実行時のレスポンスとして [`HealthStatus`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthstatus) がプレーンテキスト形式で返されます。
サーバーが起動状態の場合に `Healthy` 、停止状態の場合に `Unhealthy` が返されます。

 `HealthStatus` は、 `Healthy, Degraded, Unhealthy` のいずれかの値を取ります。

### HealthStatus の使い分け {#health-status}

|      HealthStatus      | ステータスコード | レスポンスボディ |                   詳細                   |
| ---------------------- | ---------------- | ---------------- | ---------------------------------------- |
| HealthStatus.Healthy   | 200              | Healthy          | サーバーがリクエスト受付可能             |
| HealthStatus.Degraded  | 200              | Degraded         | サーバーが起動済みだがリクエスト受付不可 |
| HealthStatus.Unhealthy | 503              | Unhealthy        | サーバーがリクエスト受付不可(停止状態)   |

既定のヘルスチェック機能ではサーバーが起動状態の場合に `Healthy` 、停止状態の場合に `Unhealthy` が返されます。

活動性と対応性を分けてヘルスチェックを行いたい場合に `Degraded` を返すよう実装する場合があります。
例えば、独自に追加したヘルスチェックロジックが正常に動作する場合に `Healthy` 、正常に動作しない場合に `Degraded` を返すよう実装できます。

詳しくは、[ヘルスチェックロジックをカスタムする場合](#customize-health-check-logic) を参照してください。

## ヘルスチェックロジックをカスタムする場合 {#customize-health-check-logic}

アプリケーションの起動以外でリクエスト受付に必要な条件がある場合、以下の手順でヘルスチェックロジックを追加します。

- `IHealthCheck` インターフェースを実装したクラスで、`CheckHealthAsync` メソッドをオーバーライドする
- `Program.cs` でヘルスチェックサービスを登録する

``` C# title="SampleHealthCheck.cs"
public class SampleHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        // 独自のヘルスチェックロジックを実装

        if (isHealthy)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("A healthy result."));
        }

        return Task.FromResult(
            new HealthCheckResult(
                context.Registration.FailureStatus, "An unhealthy result."));
    }
}
```

``` C# title="Program.cs"
builder.Services.AddHealthChecks()
    .AddCheck<SampleHealthCheck>("Sample");
```

これにより、ヘルスチェック API 呼び出しの際に独自に実装したヘルスチェックロジックが実行されます。

!!! info "`AddCheck` メソッドにラムダ式を渡す"
    `AddCheck` メソッドに `HealthCheckResult` を戻り値とするラムダ式を渡すことでヘルスチェックロジックを追加できます。

    ``` C#
    builder.Services.AddHealthChecks()
        .AddCheck("Sample", () => HealthCheckResult.Healthy("A healthy result."));
    ```

    複雑なロジックを追加する場合は、`IHealthChecksBuilder` の拡張メソッドとして別のクラスに処理を切り出すか、`IHealthCheck` インターフェースを実装したクラスを作成することを推奨します。

!!! info "ヘルスチェック失敗時の `HealthStatus` を `Degraded` に指定する"
    ヘルスチェックサービスを登録する際、 `failureStatus` にヘルスチェック失敗時の `HealthStatus` を指定できます。

    ``` C#
    builder.Services.AddHealthChecks()
        .AddCheck<SampleHealthCheck>(
        "Sample",
        failureStatus: HealthStatus.Degraded);
    ```

## データベースのヘルスチェックを追加する場合 {#database-health-check}

Entity Framework Core を利用したアプリケーションの場合、以下の手順でデータベースのヘルスチェックを行うことができます。

- NuGet パッケージ [Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore) の参照を追加
- DbContext のヘルスチェックをサービスに追加

``` C# title="Program.cs" hl_lines="4-6 9 10"
var builder = WebApplication.CreateBuilder(args);

// DbContext の登録
builder.Services.AddDbContext<SampleDbContext>(options =>
   options.UseSqlServer(
      builder.Configuration.GetConnectionString("DefaultConnection")));

// DbContext のヘルスチェックサービスを追加する
builder.Services.AddHealthChecks()
   .AddDbContextCheck<SampleDbContext>();

var app = builder.Build();

// ヘルスチェック API のエンドポイントをマッピングする
app.MapHealthChecks("/health");

app.Run();
```

上記の例では、`/health` にアクセスすることでアプリケーションとデータベースのヘルスチェックが実行されます。
アプリケーションとデータベースのヘルスチェックを行うタイミングを分けたい場合は、[複数のヘルスチェックを別々に実行する場合](#execute-health-checks-separately) を参照してください。

データベースのヘルスチェックの際に独自のロジックを実行したい場合は、[ヘルスチェックロジックをカスタムする場合](#customize-health-check-logic)と同様に、`IHealthCheck` インターフェースを実装したクラスを作成します。

## 複数のヘルスチェックを別々に実行する場合 {#execute-health-checks-separately}

以下のようにヘルスチェックを複数登録し、ヘルスチェック API のエンドポイントが 1 つの場合、既定ではヘルスチェック API が呼び出された際に全てのヘルスチェックが実行されます。

``` C# title="Program.cs"
// IHealthCheck実装クラスで定義したヘルスチェックを登録
builder.Services.AddHealthChecks()
    .AddCheck<SampleServerCheck>("SampleServerCheck");
// DbContextのヘルスチェックを登録
builder.Services.AddHealthChecks()
    .AddDbContextCheck<SampleDbContextCheck>("SampleDbContextCheck");

app.MapHealthChecks("/api/health");
```

サーバーとデータベースのヘルスチェックを個別に行いたい場合等、別々のタイミングでヘルスチェックを行いたい場合は以下の手順で実装します。

- ヘルスチェックをサービス登録時にタグ付けする
- サーバーと DB のヘルスチェック機能を別々のエンドポイントにマッピングする

``` C# title="Program.cs" hl_lines="8 10-18"
builder.Services.AddHealthChecks()
    .AddCheck<SampleServerCheck>("SampleServerCheck");

// タグ付け
builder.Services.AddHealthChecks()
    .AddDbContextCheck<SampleDbContextCheck>(
        "SampleDbContextCheck",
        tags: new[] { "db" });

app.MapHealthChecks("/api/health/server", new HealthCheckOptions
{
    Predicate = healthCheck => !healthCheck.Tags.Contains("db")
});

app.MapHealthChecks("/api/health/db", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("db")
});
```

ヘルスチェックのサービス登録時にタグを付けることができます。タグを利用して API 呼び出しで実行されるヘルスチェックをフィルター処理します。
エンドポイントをマッピングする際、 `HealthCheckOptions` の `Predicate` オプションに `bool` 値を返す関数を指定することで特定の正常性チェックを実行できます。

上記のコード例では、`db` タグが付いているかどうかでヘルスチェックが実行されるエンドポイントを分けています。これにより、 `/api/health/server` を呼び出すと `SampleServerCheck` が実行され、 `/api/health/db` を呼び出すと `SampleDbCheck` が実行されます。
`SampleServerCheck` が登録されていなかった場合、 `/api/health/server` を呼び出すと、アプリケーションに対する既定のヘルスチェックが実行されます。
