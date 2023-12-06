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

## ヘルスチェック機能の実装 {#health-check-implementation}

AlesInfiny Maris において定義しているヘルスチェック機能について説明します。

ヘルスチェック機能の概要については、[ヘルスチェックの必要性](../overview/dotnet-application-processing-system.md#health-check)を参照してください。

### 実装方針 {#implementation-policy}

ASP.NET Core を用いた Web アプリケーションでは、`Microsoft.Extensions.Diagnostics.HealthChecks` の機能を利用することでアプリケーションおよびデータベースの死活監視が可能です。

`Program.cs` で、アプリケーションにヘルスチェックサービスを登録し、エンドポイントを作成することでヘルスチェック機能を実現できます。
詳しい実装方法については、[ヘルスチェック API の実装](../../guidebooks/how-to-develop/dotnet/health-check-api.md) およびサンプルアプリケーションの実装内容を参照してください。

AlesInfiny Maris のサンプルアプリケーションにおいて、ヘルスチェックの内容は以下のアドレスで確認できます。

<http://localhost:3000/api/health>

ヘルスチェック実行時のレスポンスとして以下の `HealthStatus` のいずれかが返されます。既定ではレスポンスボディが `HealthStatus` のプレーンテキストとなります。

|      HealthStatus      | ステータスコード | レスポンスボディ |                   詳細                   |
| ---------------------- | ---------------- | ---------------- | ---------------------------------------- |
| HealthStatus.Healthy   | 200              | Healthy          | サーバーがリクエスト受付可能             |
| HealthStatus.Degraded  | 200              | Degraded         | サーバーが起動済みだがリクエスト受付不可 |
| HealthStatus.Unhealthy | 503              | Unhealthy        | サーバーがリクエスト受付不可(停止状態)   |

既定のヘルスチェック機能ではサーバーが起動状態の場合に `Healthy` 、サーバーが停止状態の場合に `Unhealthy` が返されます。

活動性（アプリケーションが起動しているかどうか）と対応性（アプリケーションが起動しており、かつリクエスト受付可能かどうか）を分けてヘルスチェックを行いたい場合に `Degraded` を返すよう実装する場合があります。
例えば、独自に追加したヘルスチェックロジックが正常に動作する場合に `Healthy` 、正常に動作しない場合に `Degraded` を返すよう実装することができます。
