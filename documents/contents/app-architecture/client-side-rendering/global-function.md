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

`Program.cs` で、アプリケーションにヘルスチェックサービスを登録し、ヘルスチェック実行用のエンドポイントを作成することでヘルスチェック機能を実現できます。
詳しい実装方法については、[ヘルスチェック API の実装](../../guidebooks/how-to-develop/dotnet/health-check-api.md) およびサンプルアプリケーションを参照してください。

AlesInfiny Maris のサンプルアプリケーションにおいて、以下のアドレスへアクセスすることでヘルスチェックを実行できます。

<http://localhost:3000/api/health>

ヘルスチェック実行時のレスポンスとして以下の [`HealthStatus`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthstatus) のいずれかが返されます。

|      HealthStatus      | ステータスコード | レスポンスボディ |                   詳細                   |
| ---------------------- | ---------------- | ---------------- | ---------------------------------------- |
| HealthStatus.Healthy   | 200              | Healthy          | サーバーがリクエスト受付可能             |
| HealthStatus.Unhealthy | 503              | Unhealthy        | サーバーがリクエスト受付不可/停止状態   |

`HealthStatus` の使い分けについては、[HealthStatus の使い分け](../../guidebooks/how-to-develop/dotnet/health-check-api.md#healthstatus) を参照してください。
