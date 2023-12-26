---
title: CSR 編
description: クライアントサイドレンダリングを行う Web アプリケーションのアーキテクチャについて解説します。
---

# 全体処理方式 {#top}

クライアントサイドレンダリング方式のアプリケーション全体で考慮すべきアーキテクチャについて、その実装方針を説明します。

## 例外処理方針 {#exception-policy}

（今後追加予定）

## ログ出力方針 {#logging-policy}

（今後追加予定）

<!-- ### トランザクション管理 -->

<!-- ## 入力値検査方針 {#validation-policy} -->

<!-- ### セキュリティ対策 -->

## ヘルスチェック機能の実装方針 {#health-check-implementation}

AlesInfiny Maris において定義しているヘルスチェック機能について説明します。

ヘルスチェック機能の概要については、[ヘルスチェックの必要性](../overview/dotnet-application-processing-system.md#health-check-necessity)を参照してください。

ASP.NET Core を用いた Web アプリケーションでは、`Microsoft.Extensions.Diagnostics.HealthChecks` の機能を利用することでアプリケーションおよびデータベースの死活監視が可能です。

実装方法の詳細については、[ヘルスチェック API の実装](../../guidebooks/how-to-develop/dotnet/health-check-api.md) およびサンプルアプリケーションを参照してください。

AlesInfiny Maris のサンプルアプリケーションにおいて、以下のアドレスへアクセスすることでヘルスチェックを実行します。

<http://localhost:3000/api/health>

上記のアドレスにアクセスするとバックエンドアプリケーションにリクエストが送信され、アプリケーションおよびデータベースの稼働状況が確認されます。
アプリケーションとデータベースが両方とも正常稼働している場合は HTTP 200 のレスポンスをフロントエンドアプリケーションに返却します。
アプリケーションとデータベースのいずれかに異常がある場合は HTTP 503 のレスポンスが返却されます。

|      HealthStatus      | ステータスコード | レスポンスボディ |                   詳細                   |
| ---------------------- | ---------------- | ---------------- | ---------------------------------------- |
| HealthStatus.Healthy   | 200              | Healthy          | サーバーがリクエスト受付可能             |
| HealthStatus.Unhealthy | 503              | Unhealthy        | サーバーがリクエスト受付不可/停止状態   |

[`HealthStatus`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthstatus)  をどのように使い分けるかについては、[HealthStatus の使い分け](../../guidebooks/how-to-develop/dotnet/health-check-api.md#health-status) を参照してください。

また、ロードバランサーによってはヘルスチェック実行時の HTTP メソッドが限られるため、 HTTP GET/HEAD メソッドに対応しています。
