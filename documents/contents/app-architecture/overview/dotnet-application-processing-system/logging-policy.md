---
title: .NET アプリケーションの 処理方式
description: AlesInfiny Maris OSS Edition で構築する .NET アプリケーションの共通的な処理方式を解説します。
---

# ログ出力方針 {#top}

概要編では、一般的なログの種類とログレベルを定めます。
アプリケーション形態ごとに個別に検討が必要なものについては別途定めます。
アプリケーション形態別のアプリケーションアーキテクチャ解説も、あわせて参照してください。

## ログの種類 {#log-pattern}

AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）で定義するログの種類は以下の通りです。

- 操作ログ

    ユーザーの操作履歴や、アプリケーションに対して行った操作を記録するログを操作ログと呼びます。

- 通信ログ

    アプリケーションがネットワークを介して通信する際、送受信する業務データや、送信先の情報、ヘッダー情報等を記録するログを通信ログと呼びます。

- 監査ログ

    アプリケーションの持つデータに対して行われた CRUD 処理を、誰がいつ実行したか記録するログを監査ログと呼びます。

- アプリケーションログ

    ここまでのログの種類に該当しない、アプリケーションのロジック内から出力する汎用的なログをアプリケーションログと呼びます。

## ログレベル {#log-level}

出力するログには、ログを出力する業務処理内で指定したログレベルを付与します。
ログに出力する情報によって、適切なログレベルを選択します。
ログレベルの定義は以下の通りです。

- Critical

    業務の即時停止につながる可能性のあるログを出力するときに使用するログレベルです。

- Error

    一部の業務が停止する可能性のあるログを出力するときに使用するログレベルです。
    マスターデータの不整合や、原因の不明なエラー発生など、システム運用担当者による確認や対処が必要となる状態を通知する目的に使用します。

- Warning

    業務は継続できるものの、一時的に発生したエラー状態を出力するときに使用するログレベルです。
    業務エラーの記録など、システム運用担当者による対応は不要なものの、システムとして不安定な状態を記録する際使用します。

- Information

    システム運用にあたって必要となる情報を出力するときに使用するログレベルです。
    バッチ処理の開始／終了の記録など、システムの状態を記録する際使用します。

- Debug

    開発者がアプリケーションの開発のために使用するログレベルです。
    各メソッドの入出力データなど、開発目的の情報を記録する際使用します。

- Trace
  
    最も詳細なメッセージを含むログレベルです。
    機密性の高い情報を記録する必要がある場合のみ使用します。

## ログレベルと環境ごとの出力設定 {#configuration-of-log-levels-and-output-per-environment}

システムの実行環境にあわせて、適切なレベルのログを出力するように構成します。

| ログレベル  |      本番環境      |     テスト環境     |  ローカル開発環境  |
| ----------- | :----------------: | :----------------: | :----------------: |
| Critical    | :white_check_mark: | :white_check_mark: | :white_check_mark: |
| Error       | :white_check_mark: | :white_check_mark: | :white_check_mark: |
| Warning     | :white_check_mark: | :white_check_mark: | :white_check_mark: |
| Information | :white_check_mark: | :white_check_mark: | :white_check_mark: |
| Debug       |                    |                    | :white_check_mark: |
| Trace       |                    |                    |                    |

!!! note "ログレベル Trace の使用について"
    ログレベル Trace は、機密性の高い情報を含むことがあり、開発環境であってもログファイル等に出力されることを防ぐため原則使用しません。

## ログに含める標準データ {#standard-log-data}

以下の情報をログに含めます。

- ログ出力日時
- [ログのカテゴリ :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-category){ target=_blank }
- [ログレベル :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-level){ target=_blank }
- [イベントID :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-event-id){ target=_blank }
- [メッセージ :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-message-template){ target=_blank }
- [例外情報（スタックトレースなど） :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-exceptions){ target=_blank }
- [スコープ :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-scopes){ target=_blank }

## ロギングライブラリ {#logging-libraries}

AlesInfiny Maris では、ログ出力に [Microsoft.Extensions.Logging.ILogger :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.logging.ilogger){ target=_blank } インターフェースを使用します。
ログプロバイダーは、アプリケーションの形態に合わせて適切なものを選択します。
なお、構造化ログに対応したログプロバイダーの利用を推奨します。
構造化ログについては以下を参照してください。

[ログメッセージテンプレート :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-message-template){ target=_blank }
