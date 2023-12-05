---
title: .NET 編
description: バックエンドで動作する .NET アプリケーションの開発手順を解説します。
---

# ヘルスチェック API の実装 {#top}

アプリケーションサーバーやデータベースの死活確認のため、ASP.NET Core の機能を利用してヘルスチェック API を実装できます。

## 基本的な実装方法 {#basic}

以下の実装を `Program.cs` に追加することで、アプリケーションがリクエストに応答可能かどうかが判定されます。

``` C# hl_lines="4 9"
var builder = WebApplication.CreateBuilder(args);

// ヘルスチェックサービスを追加する
builder.Services.AddHealthChecks();

var app = builder.Build();

// ヘルスチェック API のエンドポイントをマッピングする
app.MapHealthChecks("/healthz");

app.Run();
```

上記の実装では、`/healthz` にアクセスすることでヘルスチェックが実施されます。
ヘルスチェックの結果である [`HealthStatus`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthstatus) の値がレスポンスとして返されます。 `HealthStatus` は、 `Healthy, Degraded, Unhealthy` のいずれかの値を取ります。

## ヘルスチェックロジックをカスタムする場合 {#custom-health-check-logic}

アプリケーションがリクエストに応答可能か以外の条件も併せてヘルスチェックをしたい場合、`IHealthCheck` インターフェースを実装したクラスを作成します。

- クラスの実装例

`IHealthCheck` インターフェースを実装したクラスで、
`CheckHealthAsync` メソッドをオーバーライドしてヘルスチェックのロジックを追加します。

``` C#
public class SampleHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // ここに独自のヘルスチェックロジックを実装

        return Task.FromResult(
            new HealthCheckResult(
                context.Registration.FailureStatus, "An unhealthy result."));
    }
}
```

- Program.csの実装

`AddCheck` メソッドを利用して `SampleHealthCheck` クラスで実装したヘルスチェックを登録します。

``` C#
builder.Services.AddHealthChecks()
    .AddCheck<SampleHealthCheck>("Sample");
```

これによってヘルスチェックAPI呼び出しの際に独自に実装したヘルスチェックロジックが実行されます。

!!! info "`AddCheck` メソッドにラムダ式を渡す"

    `AddCheck` メソッドに `HealthCheckResult` を戻り値とするラムダ式を渡すことでヘルスチェックロジックを追加できます。

    ``` C#
    builder.Services.AddHealthChecks()
        .AddCheck("Sample", () => HealthCheckResult.Healthy("A healthy result."));
    ```

    複雑なロジックを追加する場合は、`IHealthChecksBuilder` の拡張メソッドとして別のクラスに処理を切り出すか、`IHealthCheck` インターフェースを実装したクラスを作成することを推奨します。

## データベースのヘルスチェックを追加する場合 {#database-health-check}

Entity Framework Core を利用したアプリケーションの場合、以下の手順でデータベースのヘルスチェックを行うことができます。

- NuGet パッケージ [Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore) の参照を追加
- DbContextのヘルスチェックをサービスに追加

``` C# hl_lines="4-6 9 10"
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
app.MapHealthChecks("/healthz");

app.Run();
```

上記の例では、`/healthz` にアクセスすることでアプリケーションとデータベースのヘルスチェックが実施されます。
アプリケーションとデータベースのヘルスチェックを行うタイミングを分けたい場合は、[複数のヘルスチェックを別々に実行する場合](#複数のヘルスチェックを別に実行する場合) を参照してください。

データベースのヘルスチェックの際に独自のロジックを実行したい場合は、[ヘルスチェックロジックをカスタムする場合](#ヘルスチェックロジックをカスタムする場合)と同様に、`IHealthCheck` インターフェースを実装したクラスを作成します。

## 複数のヘルスチェックを別々に実行する場合 {#execute-health-checks-separately}

以下のようにヘルスチェックを複数登録し、ヘルスチェックAPIのエンドポイントが1つの場合、既定ではヘルスチェックAPIが呼び出された際に全てのヘルスチェックが実行されます。

``` C#
// IHealthCheck実装クラスで定義したヘルスチェックを登録
builder.Services.AddHealthChecks()
    .AddCheck<SampleServerCheck>("SampleServerCheck");
// DbContextのヘルスチェックを登録
builder.Services.AddHealthChecks()
    .AddDbContextCheck<SampleDbContextCheck>("SampleDbContextCheck");

app.MapHealthChecks("/api/health");
```

サーバーとデータベースのヘルスチェックを個別に行いたい場合等、別々のタイミングでヘルスチェックを行いたい場合は以下の手順で実装します。

- ヘルスチェックをサービスに登録する際にタグ付けする
- サーバーとDBのヘルスチェック機能を別々のエンドポイントにマッピングする

``` C# hl_lines="8 10-18"
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

ヘルスチェックをサービスに登録する際にタグを付けることができます。タグを利用して API 呼び出しで実行されるヘルスチェックをフィルター処理します。
エンドポイントをマッピングする際、 `HealthCheckOptions` の `Predicate` オプションに `bool` 値を返す関数を指定することで特定の正常性チェックを実行できます。

上記のコード例では、`db` タグが付いているかどうかでヘルスチェックが実行されるエンドポイントを分けています。これにより、 `/api/health/server` を呼び出すと `SampleServerCheck` が実行され、 `/api/health/db` を呼び出すと `SampleDbCheck` が実行されます。
`SampleServerCheck` が登録されていなかった場合、 `/api/health/server` を呼び出すと、アプリケーションに対する既定のヘルスチェックが実行されます。
