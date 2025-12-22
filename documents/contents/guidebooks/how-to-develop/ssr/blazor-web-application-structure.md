---
title: Blazor Web アプリプロジェクトの構成 （SSR 編）
description: SSR アプリケーション開発における Blazor Web アプリ プロジェクトの構成方法を解説します。
---

# Blazor Web アプリプロジェクトの構成 {#top}

[プロジェクトの作成](./create-project.md) において、 Blazor Web アプリ / Fluent Blazor Web アプリのテンプレートを使用して作成したプロジェクトに対して、必要な設定を追加します。

## 例外ハンドリングの設定 {#exception-handling-configuration}

Blazor Web アプリでは、以下の 2 種類の未処理例外が発生し得ます。

1. Blazor ランタイム内で発生する未処理例外（コンポーネント実行時など）
1. Blazor 起動前に .NET ランタイム内で発生する未処理例外（アプリ起動時など）

これらに共通的に対処するため、`ErrorBoundary` コンポーネントとエラーページを追加します。
具体的な実装方法については、 [集約エラーハンドラーの実装](./centralized-error-handling.md) を参照してください。

## HTTP 通信ログの出力 {#configure-http-communication-log}

実際の開発作業では、意図した通りの HTTP リクエスト / レスポンスが送受信できているか確認することがよくあります。
そのために、開発環境では HTTP 通信ログを出力するよう設定することを推奨します。
[HTTP 通信ログの出力](../csr/dotnet/configure-asp-net-core-web-api-project.md#configure-http-communication-log) に従って、 Blazor Web アプリプロジェクトの Program.cs を変更します。
その後、開発環境の [ログレベルの設定](#configure-log-level) の構成例を参照し、 設定ファイルにキーを追加してください。

## SignalR 通信ログの出力 {#configure-signal-r-communication-log}

Blazor Web アプリでは、 UI 更新やイベント通知に SignalR によるクライアント / サーバー間の双方向通信が行われます。
開発作業中に意図した通信が行われているか確認するために、開発環境では SignalR 通信ログを出力するよう設定することを推奨します。
SignalR 通信ログを出力するためには、 Microsoft.AspNetCore.SignalR と Microsoft.AspNetCore.Http.Connections カテゴリーのログを出力対象に追加する必要があります。
開発環境の [ログレベルの設定](#configure-log-level) の構成例を参照し、 `LogLevel` 要素にキーとログレベルを追加してください。
詳細は [ASP.NET Core SignalR でのログと診断 :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/signalr/diagnostics){ target=_blank }を参照してください。

## ログレベルの設定 {#configure-log-level}

本番環境向けのログレベル設定を、 appsettings.json に、開発環境向けのログレベル設定を appsettings.Development.json にそれぞれ行います。
Information レベル以上のログを出力するように設定しましょう。
ただし、 `Microsoft.AspNetCore` のカテゴリーについては、 Warning 以上のレベルのみ出力するようにします。
開発しているアプリケーションから出力するログのログレベルも、明示的に設定しておくことを推奨します。
例のように、 `LogLevel` 要素にソリューション名と同名のキーを追加し、値としてログレベルを設定します。
通常は Information を設定しましょう。

```json title="appsettings.json の設定例" hl_lines="6"
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "AaaSubSystem": "Information"
    }
  },
}
```

開発環境向けのログレベル設定は appsettings.Development.json に行います。
開発環境では、開発者が設定したデバッグレベルのログも出力できるように設定します。
例のように、 `LogLevel` 要素にソリューション名と同名のキーを追加し、 Debug を設定します。
加えて、下記の例では HTTP 通信ログと SignalR 通信ログが出力されるようにキーとログレベルを設定しています。

```json title="appsettings.Development.json の設定例" hl_lines="5-9"
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.HttpLogging": "Warning",
      "Microsoft.AspNetCore.SignalR": "Information",
      "Microsoft.AspNetCore.Http.Connections": "Information",
      "AaaSubSystem": "Debug"
    }
  },
}
```
