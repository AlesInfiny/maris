---
title: .NET 編（CSR 編）
description: CSR アプリケーションの サーバーサイドで動作する .NET アプリケーションの 開発手順を解説します。
---

# E2E テストのバックエンド設定 {#top}

E2E テストの利用により、フロントエンド・バックエンドアプリケーションからデータベースまで一気通貫での動作確認を効率的に行うことができます。
E2E テストの実行ツールとして [Playwright :material-open-in-new:](https://playwright.dev/){ target=_blank } を使用します。

本ページでは、 E2E テストを実行する際のバックエンドアプリ関連の設定について説明します。
Playwright のインストール方法等については、 [E2E テストの設定](../vue-js/e2e-test-settings.md) を参照してください。

## CI 環境でのバックエンド設定 {#backend-settings-on-ci}

Playwright は GitHub Actions などの CI 環境で実行できます。
CI 環境のランナーには、ブラウザーやデータベースなどテストに必要な環境が存在しないため、ワークフロー内で準備する必要があります。

バックエンドアプリに依存しない設定については、 [CI 環境での実行](../vue-js/e2e-test-settings.md#run-on-ci) を参照してください。

以降の設定内容は GitHub Actions を前提としています。

??? info "CI パイプラインの設定例"

    ```yml title=".github/workflows/samples-dressca-consumer-e2e.ci.yml"
    --8<-- ".github/workflows/samples-dressca-consumer-e2e.ci.yml:4:120"
    ```

### データベースの準備 {#prepare-database-on-ci}

CI 環境ではローカル開発用のデータベースを使用できないため、ワークフロー内で SQL Server のコンテナを起動し、マイグレーションを適用してテスト用のデータベースを準備します。

[テスト実行時の設定](../vue-js/e2e-test-settings.md#test-config-settings) の `webServer` で起動するバックエンドアプリは、既定では開発環境の設定で起動するため、ローカル開発用の接続文字列でデータベースへ接続しようとします。
そのため、環境変数 `ConnectionStrings__DresscaDbContext` に SQL Server のコンテナを指定して接続先を上書きします。
.NET の構成プロバイダーは環境変数の値を `appsettings.{環境名}.json` より優先するため、アプリケーションの設定ファイルを変更することなく、コンテナ上の SQL Server へ接続できます。

```yml title=".github/workflows/samples-dressca-consumer-e2e.ci.yml"
--8<-- ".github/workflows/samples-dressca-consumer-e2e.ci.yml:60:94"
```

### HTTPS 開発証明書の作成 {#create-dev-certs-on-ci}

`webServer` で起動するバックエンドアプリは HTTPS で待ち受けるため、サーバー証明書が必要です。
CI 環境のランナーに証明書が存在しない場合に備え、ワークフロー内で以下のコマンドを実行して HTTPS 開発証明書を作成します。

```shell linenums="0"
dotnet dev-certs https
```

なお、 [テスト実行時の設定](../vue-js/e2e-test-settings.md#test-config-settings) の `ignoreHTTPSErrors` は、開発証明書が OS に信頼されていないことによるエラーを無視する設定です。
サーバー証明書そのものの代わりにはならないため、開発証明書の作成とあわせて設定します。
