---
title: CSR 編
description: クライアントサイドレンダリングを行う Web アプリケーションの アーキテクチャについて解説します。
---

# 全体処理方式 {#top}

クライアントサイドレンダリング方式のアプリケーション全体で考慮すべきアーキテクチャについて、その実装方針を説明します。

## 例外処理方針 {#exception-policy}

<!-- ## サーバーサイドの例外処理 -->

## クライアントサイドの例外処理方針 {#frontend-error-handling}

クライアントサイドは、ユーザーが操作する、ログの取得が難しいなど、サーバーサイドとは異なる特性があります。
しかし例外処理方針としては、サーバーサイドと同様に正常なフローに復帰できるかどうかを最も重要な観点とし、そのためにどこで例外を捕捉しどのようにユーザーへ通知するかを検討します。

### 例外の種類 {#exception-types}

クライアントサイドの例外は、業務例外とシステム例外の 2 種類に分けて考えます。

業務例外は、業務フローで想定されるエラーを表す例外です。システム例外は、業務フロー上は想定されないシステムのエラーを表す例外です。
API 通信においては、ステータスコードが 40x のエラーを業務例外、 50x のエラーをシステム例外として扱います。

### 例外の捕捉 {#catch-exceptions}

クライアントサイドで発生する例外は、以下のようなケースが考えられます。

- ユーザーが操作した際に発生する例外
- API 通信で発生する例外
- システムエラー
- ネットワークエラー

例外の発生が予測できる箇所では、基本的な同期処理は `try-catch` 、非同期処理は `catch` メソッドを利用します。予期せぬ例外が発生した場合は、 Vue.js の `app.config.errorHandler` や JavaScript の `window.onerror` といったグローバルエラーハンドリングで例外を捕捉します。

| 例外の種類                                            | ハンドリング方法              |
| ---------------------------------------------------- | ----------------------------- |
| Vue アプリケーション内で発生する例外                   | `app.config.errorHandler`    |
| JavaScript の構文エラーや Vue アプリケーション外の例外 | `window.onerror`             |
| 非同期処理で発生する例外                              | `window.onunhandledrejection` |

また Vue コンポーネントで例外を個別に捕捉したい場合、 Vue のライフサイクルフックである `errorCaptured` を利用して例外を捕捉します。

HTTP 通信で発生する例外について、ステータスコードに対して共通処理をする場合は、 Axios の `axios.interceptors.response` を利用します。

### 例外の処理 {#error-handling}

クライアントサイドの例外処理では、ユーザーが自身で対応できるか、という観点が重要になります、
たとえばセッションタイムアウトになるといったようなケースではユーザーが再度ログインすることで対処できます。
一方で、 WebAPI サーバー内で予期しない問題が発生するといったケースでは、ユーザー自身では問題を解決できず、開発者に問い合わせをするようなフローが考えられます。

ユーザーに対しては、自身がどのような対応をできるかを考慮し、適切な通知方法を選択します。ポップアップ等の画面遷移を伴わずに通知するか、エラーページへ遷移するかは、ユーザーが操作を継続できるかどうかによって選択します。
一方開発者向けの通知では、エラーの詳細を把握するために必要な情報を収集することが重要です。開発環境であればコンソールログに出力し、本番環境ではログ収集ツールに送信する、などが考えられます。
ただしコンソールログへの出力は原則開発環境のみとします。

#### API 通信で発生する例外処理フロー {#api-connection-error-flow}

クライアントサイドで最も多く発生する例外は、 API 通信で発生する例外です。その要因は、ネットワークエラーやサーバーエラー、ユーザーの入力エラーなど様々であるため、全ての例外に対処することは難しいです。
API 通信で発生する例外についてはいくつかの段階に分けて処理します。なお API 通信には Axios を利用していることを前提としています。

1. API レスポンスに対する共通処理

    HTTP ステータスコードに対する共通処理を [axios.interceptors.response :material-open-in-new:](https://axios-http.com/ja/docs/interceptors) に集約します。`axios.interceptors.response` は、レスポンスの受信後、 `then` や `catch` の処理の前に共通処理を挟むことができます。
    ここで行う共通処理については以下のようなものが考えられます。

    - 401 Unauthorized であれば、ログイン画面へ遷移する。
    - エラー解析モジュールにエラー情報を格納する。

    ```mermaid
        sequenceDiagram
        participant C as Vue コンポーネント
        participant A as Axios.Post
        participant I as Axios.interceptors.response
        participant S as サーバー

        C->>A: API リクエスト呼び出し
        A-)S: API リクエスト
        S--)I: 401 Unauthorized
        rect rgba(255, 0, 0, 0.5)
            I->>C: 共通処理：ログイン画面へ遷移
        end
    ```

1. API レスポンスの例外に対する処理

    API 通信で発生する例外については、 API 通信のレスポンスハンドリングで個別に処理します。
    リクエストに不備がある、といったユーザーが対応できるようなエラーについては、対応方法をユーザーに通知します。
    通知方法はポップアップやトースト通知などの入力を阻害しない方法か、やむを得ずエラー画面に遷移するかを適宜選択することが望ましいです。

    また、発生したエラーを解析するために、以下の方法が考えられます。

    - エラー番号やエラーメッセージを通知し、開発者が問合せるための情報を提供する
    - ユーザーの状況やエラー内容をログ収集ツールに送信する

    ```mermaid
        sequenceDiagram
        participant C as Vue コンポーネント
        participant A as Axios.Post
        participant I as Axios.interceptors.response
        participant S as サーバー

        C->>A: API リクエスト呼び出し
        A-)S: API リクエスト
        S--)I: 400 Bad Request
        I->>A: エラー情報の解析
        rect rgba(255, 0, 0, 0.5)
            A->>C: エラー情報の通知
        end
    ```

## ログ出力方針 {#logging-policy}

（今後追加予定）

<!-- ### トランザクション管理 -->

<!-- ## 入力値検査方針 {#validation-policy} -->

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
