---
title: CSR 編
description: クライアントサイドレンダリングを行う Web アプリケーションの アーキテクチャについて解説します。
---

# 全体処理方式 {#top}

クライアントサイドレンダリング方式のアプリケーション全体で考慮すべきアーキテクチャについて、その実装方針を説明します。

## 例外処理方針 {#exception-policy}

（今後追加予定）

## ログ出力方針 {#logging-policy}

（今後追加予定）

<!-- ### トランザクション管理 -->

<!-- ## 入力値検査方針 {#validation-policy} -->

## ストア設計方針 {#store-design-policy}

### 永続化方式 {#persistence}

ストアはデフォルトの状態（インメモリ）では、リロードの際にリフレッシュされてしまいます。また別タブにもデータを共有できません。
Web Storage にデータを永続化することで、リロードや別タブでもストアが利用でき、ユーザーの体験向上につながります。
例えば認証情報を Session Storage に永続化することで、画面をリロードしても再ログインを求められないなどのメリットがあります。

以下に、ストアの永続化方式の比較を示します。

| 評価項目                    | インメモリ(デフォルト)           | Session Storage                           | Local Storage            |
| --------------------------- | -------------------------------- | ----------------------------------------- | ------------------------ |
|データ保存先                 | ブラウザメモリ                   | ブラウザキャッシュ一時領域ドメイン単位で保持 | ブラウザキャッシュ永続領域 ドメイン単位で保持  |
| 情報保持期間                | 同一タブ/ウィンドウが開いている間 | 同一タブ/ウィンドウが開いている間           | システム/手動で削除するまで                  |
| ファイルへの保存            | なし                             | なし                                      | あり                                         |
| データ保持の仕様：リロード   | データ削除                       | データ保持                                | データ保持                                     |
| データ保持の仕様：別タブ表示 | 取得不可                         | 取得不可                                  | 取得可                                         |
| 最大容量                    | 1GB程度                         | 5MB                                       | 10MB                                            |
| CSRF脅威                    | 無                              | 有（他の方法で対策可能）                   | 有（他の方法で対策可能）                        |
| XSS脅威                     | 有（対策可能）                   |  有（対策可能）                           | 有（対策可能）                                  |
| 3rd-party JavaScript 読込    | 不可                             | 可能（悪意のあるライブラリ使用時）         |  可能（悪意のあるライブラリ使用時）            |

### セキュリティ上の脅威について {#security-threats}

Web Storage は JavaScript から容易にアクセスできるため、 XSS 攻撃などのセキュリティ上の脅威にさらされる可能性があります。
XSS 攻撃、 CSRF 攻撃などの脅威に対しては、適切な対策を講じることでリスクを軽減できますが、 3rd-party 製 JavaScript ライブラリの使用には注意が必要です。
そのため Web Storage を使用する際には、利便性とセキュリティがトレードオフであることを理解し、 Local Storage には原則秘匿情報を保存しないようにするなど、リスクを軽減しながら利用することが重要です。

<!-- ### セキュリティ対策 -->

## ヘルスチェック機能の実装方針 {#health-check-implementation}

AlesInfiny Maris OSS Edition では、 Web API を通じてシステムが正常稼働中か確認します。
`Microsoft.Extensions.Diagnostics.HealthChecks` の機能を利用してヘルスチェック用の Web API を実装しています。

実装方法の詳細については、[ヘルスチェック API の実装](../../guidebooks/how-to-develop/dotnet/health-check-api.md) およびサンプルアプリケーションを参照してください。
また、ヘルスチェックの全体方針については、[ヘルスチェックの必要性](../overview/dotnet-application-processing-system/health-check-necessity.md) を参照してください。

### API の仕様 {#api-specs}

ヘルスチェック用の Web API にリクエストを送信すると、アプリケーションおよび関連するデータベース等の稼働状況が確認されます。

アプリケーションとデータベース等の外部サービスが全て正常稼働している場合を正常状態とします。
アプリケーションとデータベース等の外部サービスのいずれかに異常がある場合を異常状態とします。

正常状態の場合は、 HTTP 200 のレスポンスを返却し、異常状態の場合は HTTP 503 のレスポンスを返却します。

|      HealthStatus      | ステータスコード | レスポンスボディ |                   詳細                   |
| ---------------------- | ---------------- | ---------------- | ---------------------------------------- |
| HealthStatus.Healthy   | 200              | Healthy          | サーバーがリクエスト受付可能             |
| HealthStatus.Unhealthy | 503              | Unhealthy        | サーバーがリクエスト受付不可/停止状態   |

[`HealthStatus` :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthstatus){ target=_blank } をどのように使い分けるかについては、[HealthStatus の使い分け](../../guidebooks/how-to-develop/dotnet/health-check-api.md#health-status) を参照してください。

また、ロードバランサーによってはヘルスチェック実行時の HTTP メソッドが限られるため、 HTTP GET/HEAD メソッドに対応しています。

### 検証ロジックの追加 {#add-health-check-logic}

ヘルスチェック API は Web プロジェクトのアプリケーションとしての稼働状況と、データベース等の利用/依存しているサービスの稼働状況を取りまとめてレスポンスを返します。
そのため、ヘルスチェック API 実行時に独自の検証ロジックを含める場合は、検証対象の外部サービスに依存するプロジェクトへ実装します。

図のように、外部サービスのヘルスチェックロジックはプレゼンテーション層にあたる Web プロジェクトに直接実装しません。
検証対象の外部サービスに対応するそれぞれのプロジェクトへ分割してロジックを追加し、 Web プロジェクトから参照するようにします。

![検証ロジックの配置](../../images/app-architecture/client-side-rendering/add-health-check-logic-light.png#only-light){ loading=lazy }
![検証ロジックの配置](../../images/app-architecture/client-side-rendering/add-health-check-logic-dark.png#only-dark){ loading=lazy }
