---
title: CSR 編
description: クライアントサイドレンダリングを行う Web アプリケーションのアーキテクチャについて解説します。
---

# 全体処理方式 {#top}

クライアントサイドレンダリング方式のアプリケーション全体で考慮すべきアーキテクチャについて、その実装方針を説明します。

## 例外処理方針 {#exception-policy}

### フロントエンドの例外処理 {#frontend-error-handling}

フロントエンドの例外処理方針は、ユーザーが自身で対応できるか、という観点が重要になります、
たとえばセッションタイムアウトになるといったようなケースではユーザーが再度ログインすることで対処できます。
一方で、 WebAPI サーバー内で予期しない問題が発生するといったケースでは、ユーザーは時間をおいて再度リクエストするなどのことしかできません。
このようにフロントエンドでは、ユーザーが例外を知りどのような対応をするか、ということに注目しハンドリングや通知方法を決定します。

### エラーハンドリング {#client-error-handling}

エラーハンドリングは、以下の 2 種類に分けて考えます。

- クライアントコード内で発生する例外
- API 通信で発生する例外

#### クライアントコード内で発生する例外 {#client-code-error}

TypeScript で記述するコードのハンドリングできる例外については、同期処理は `try-catch` 、 Promise を利用した非同期処理は `catch` メソッドで処理します。

Vue コンポーネント内で例外が発生した場合、それがハンドリングされないと、例外は親コンポーネントに伝播し続けます。そのため、 main.ts ファイルでグローバルエラーハンドリングを行い、例外を処理する必要があります。
グローバルエラーハンドリングには、 Vue.js の `app.config.errorHandler` や JavaScript の `window.onerror` などを利用します。

| 例外の種類 | ハンドリング方法 |
| ---------- | ---------------- |
| Vue コンポーネント内で発生する例外 | `app.config.errorHandler` |
| 一般的な JavaScript エラーや構文エラー | `window.onerror` |
| 非同期処理で発生する例外 | `window.onunhandledrejection` |

#### API 通信で発生する例外 {#api-connection-error}

API 通信で発生する例外についてはいくつかの段階に分けて処理します。なお API 通信には Axios を利用していることを前提としています。

1. レスポンスに対する共通処理

    HTTP ステータスコードに対する共通処理を [axios.interceptors.response :material-open-in-new:](https://axios-http.com/ja/docs/interceptors) に集約します。`axios.interceptors.response` は、レスポンスの受信後、 `then` や `catch` の処理の前に共通処理を挟むことができます。
     ここで行う共通処理については以下のようなものが考えられます。

    - 401 番(認証エラー)であれば、ログイン画面へ遷移する。
    - 404 番(リソースが見つからない)であれば、 Not Found ページへリダイレクトする、もしくはトーストで通知する。
    - 500 番(サーバーエラー)であれば、エラーページへリダイレクトする。

    400 番は、バリデーションエラーなどの業務例外であるため、個別で処理します。

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

2. API レスポンスの業務例外に対する処理

    API 通信で発生する業務例外については、 API 通信のレスポンスハンドリングで個別に処理します。
    リクエストに不備がある、といったユーザーが対応できるようなエラーについては、対応方法をユーザーに通知します。
    通知方法はポップアップやトースト通知などの入力を阻害しない方法を適宜選択することが望ましいです。

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
        I-->>A: Post レスポンス処理
        rect rgba(255, 0, 0, 0.5)
            A-)C: 個別のエラー処理
        end
    ```

3. API 通信のリトライ処理

    クライアントサイドでの自動リトライ処理は原則行いません。例えばポストリクエストに対して、サーバーサイドで処理が実施されているにもかかわらずリトライ処理を行ってしまった場合、重複してデータが登録されうるためです。
    自動でリトライ処理を行ってもいいのは fetch メソッドなど冪等性のあるリクエストのみです。
    その他、リトライ処理が必要な場合はリトライを促すような UI を提供し、ユーザーが手動でリトライを行うようにしましょう。

    429 (Too Many Requests) エラーに対して自動リトライを実装する際は、以下のようにシステムに応じてリトライ回数やリトライ間隔などのリトライポリシーを定めましょう。

    - リトライ回数: 3 回
    - リトライ間隔: 1 秒, 2 秒, 4 秒と指数関数的に増加
    - リトライ回数を超えた場合は、エラーページへリダイレクトする

## ログ出力方針 {#logging-policy}

（今後追加予定）

<!-- ### トランザクション管理 -->

<!-- ## 入力値検査方針 {#validation-policy} -->

<!-- ### セキュリティ対策 -->

## ヘルスチェック機能の実装方針 {#health-check-implementation}

AlesInfiny Maris では、 Web API を通じてシステムが正常稼働中か確認します。
`Microsoft.Extensions.Diagnostics.HealthChecks` の機能を利用してヘルスチェック用の Web API を実装しています。

実装方法の詳細については、[ヘルスチェック API の実装](../../guidebooks/how-to-develop/dotnet/health-check-api.md) およびサンプルアプリケーションを参照してください。
また、ヘルスチェックの全体方針については、[ヘルスチェックの必要性](../overview/dotnet-application-processing-system.md#health-check-necessity)を参照してください。

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
