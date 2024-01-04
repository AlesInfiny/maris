---
title: .NET 編
description: バックエンドで動作する .NET アプリケーションの開発手順を解説します。
---

# ヘルスチェック API の実装 {#top}

アプリケーションやデータベースの死活確認のため、 ASP.NET Core の機能を利用してヘルスチェック API を実装できます。

## 基本的な実装方法 {#basic}

アプリケーションおよび関連するデータベースのヘルスチェックを行う場合の実装方法を説明します。

Entity Framework Core を利用したアプリケーションの場合、以下の手順でデータベースを含めたヘルスチェックを行うことができます。

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
アプリケーションとデータベースのヘルスチェックを行うタイミングを分けたい場合は、[正常性チェックをフィルター処理する](https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks#filter-health-checks) を参照してください。

ヘルスチェックの際に実行するロジックを追加したい場合は、[ヘルスチェックロジックをカスタムする場合](#customize-health-check-logic)を参照してください。

AlesInfiny Maris のサンプルアプリケーションでは、対象のデータベースに対応するプロジェクトへヘルスチェックロジックを配置する都合上、 `Program.cs` に直接ヘルスチェックロジックを追加していません。
 `IHealthChecksBuilder` の拡張メソッドとして処理を切り出し、`AddDbContextCheck` メソッドにヘルスチェックロジックを渡しています。

ヘルスチェック実行時のレスポンスとして [`HealthStatus`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthstatus) がプレーンテキスト形式で返されます。
サーバーが起動状態の場合に `Healthy` 、停止状態の場合に `Unhealthy` が返されます。
`HealthStatus` は、 `Healthy, Degraded, Unhealthy` のいずれかの値を取ります。

`HealthStatus` をどのように使い分けるかについては、[HealthStatus の使い分け](#health-status) を参照してください。

### 活動性の確認 {#liveness-probe}

ヘルスチェックでは以下の 2 つの状態を区別してアプリケーションの正常性を確認する場合があります。

- 活動性：アプリケーションが正常に起動していること
- 対応性：アプリケーションが正常に起動しており、かつリクエスト受付可能であること

活動性と対応性については[こちら](https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks#separate-readiness-and-liveness-probes)を参照してください。

ヘルスチェック API へのアクセスが非常に多くなる等、アプリケーションの活動性のみを確認したい場合は以下の実装を `Program.cs` に追加します。

``` C# title="Program.cs" hl_lines="4 9"
var builder = WebApplication.CreateBuilder(args);

// ヘルスチェックサービスを追加する
builder.Services.AddHealthChecks();

var app = builder.Build();

// ヘルスチェック API のエンドポイントをマッピングする
app.MapHealthChecks("/health");

app.Run();
```

上記の実装では、`/health` にアクセスすることでアプリケーションが正常に起動していることを確認できます。

### HealthStatus の使い分け {#health-status}

|      HealthStatus      | ステータスコード | レスポンスボディ |                   詳細                   |
| ---------------------- | ---------------- | ---------------- | ---------------------------------------- |
| HealthStatus.Healthy   | 200              | Healthy          | サーバーがリクエスト受付可能             |
| HealthStatus.Degraded  | 200              | Degraded         | サーバーが起動済みだがリクエスト受付不可 |
| HealthStatus.Unhealthy | 503              | Unhealthy        | サーバーがリクエスト受付不可(停止状態)   |

既定のヘルスチェック機能ではサーバーが起動状態の場合に `Healthy` 、停止状態の場合に `Unhealthy` が返されます。

活動性と対応性を分けてヘルスチェックを行いたい場合に `Degraded` を返すよう実装する場合があります。
例えば、独自に追加したヘルスチェックロジックが正常に動作する場合に `Healthy` 、正常に動作しない場合に `Degraded` を返すよう実装できます。

!!! info "ヘルスチェック失敗時の `HealthStatus` を `Degraded` に指定する"
    ヘルスチェックサービス登録時、 `failureStatus` にヘルスチェック失敗時の `HealthStatus` を指定できます。

    ``` C# title="Program.cs"
    builder.Services.AddHealthChecks()
        .AddCheck<SampleHealthCheck>(
        "Sample",
        failureStatus: HealthStatus.Degraded);
    ```

## ヘルスチェックロジックをカスタムする場合 {#customize-health-check-logic}

ヘルスチェックロジックを追加する場合、以下のいずれかの方法で実装します。

- [`IHealthCheck` インターフェースを実装したクラスで、`CheckHealthAsync` メソッドをオーバーライドする](https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks#create-health-checks)
- `AddCheck` メソッドや `AddDbContextCheck` メソッドのオーバーロードにヘルスチェックロジックを渡す
    - [`AddCheck` メソッド](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.dependencyinjection.healthchecksbuilderdelegateextensions.addcheck)
    - [`AddDbContextCheck` メソッド](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.dependencyinjection.entityframeworkcorehealthchecksbuilderextensions.adddbcontextcheck)
