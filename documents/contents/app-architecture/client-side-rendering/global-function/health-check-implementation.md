---
title: CSR 編 - 全体処理方式
description: アプリケーション全体で考慮すべきアーキテクチャについて、 その実装方針を説明します。
---

# ヘルスチェック機能の実装方針 {#top}

AlesInfiny Maris OSS Edition では、 Web API を通じてシステムが正常稼働中か確認します。
`Microsoft.Extensions.Diagnostics.HealthChecks` の機能を利用してヘルスチェック用の Web API を実装しています。

実装方法の詳細については、[ヘルスチェック API の実装](../../../guidebooks/how-to-develop/csr/dotnet/health-check-api.md) およびサンプルアプリケーションを参照してください。
また、ヘルスチェックの全体方針については、[ヘルスチェックの必要性](../../overview/dotnet-application-processing-system/health-check-necessity.md) を参照してください。

## API の仕様 {#api-specs}

ヘルスチェック用の Web API にリクエストを送信すると、アプリケーションおよび関連するデータベース等の稼働状況が確認されます。

アプリケーションとデータベース等の外部サービスが全て正常稼働している場合を正常状態とします。
アプリケーションとデータベース等の外部サービスのいずれかに異常がある場合を異常状態とします。

正常状態の場合は、 HTTP 200 のレスポンスを返却し、異常状態の場合は HTTP 503 のレスポンスを返却します。

|      HealthStatus      | ステータスコード | レスポンスボディ |                   詳細                   |
| ---------------------- | ---------------- | ---------------- | ---------------------------------------- |
| HealthStatus.Healthy   | 200              | Healthy          | サーバーがリクエスト受付可能             |
| HealthStatus.Unhealthy | 503              | Unhealthy        | サーバーがリクエスト受付不可/停止状態   |

[`HealthStatus` :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthstatus){ target=_blank } をどのように使い分けるかについては、[HealthStatus の使い分け](../../../guidebooks/how-to-develop/csr/dotnet/health-check-api.md#health-status) を参照してください。

また、監視側の仕様によってヘルスチェック実行時の HTTP メソッドに制限の入ることがあるため、 HTTP GET/HEAD メソッドいずれにも対応できるようにします。

## 検証ロジックの追加 {#add-health-check-logic}

ヘルスチェック API は Web プロジェクトのアプリケーションとしての稼働状況と、データベース等の利用/依存しているサービスの稼働状況を取りまとめてレスポンスを返します。
そのため、ヘルスチェック API 実行時に独自の検証ロジックを含める場合は、検証対象の外部サービスに依存するプロジェクトへ実装します。

図のように、外部サービスのヘルスチェックロジックはプレゼンテーション層にあたる Web プロジェクトに直接実装しません。
検証対象の外部サービスに対応するそれぞれのプロジェクトへ分割してロジックを追加し、 Web プロジェクトから参照するようにします。

![検証ロジックの配置](../../../images/app-architecture/client-side-rendering/add-health-check-logic-light.png#only-light){ loading=lazy }
![検証ロジックの配置](../../../images/app-architecture/client-side-rendering/add-health-check-logic-dark.png#only-dark){ loading=lazy }
