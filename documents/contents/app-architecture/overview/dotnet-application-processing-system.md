---
title: 概要編
description: AlesInfiny Maris のアプリケーションアーキテクチャ概要を解説します。
---

# .NETアプリケーションの処理方式 {#top}

AlesInfiny Maris で構築する .NET アプリケーションの共通的な処理方式を解説します。

## 例外処理方針 {#exception-handling-policy}

アプリケーション全体での例外処理方針を定めます。

### 例外の種類 {#exception-type}

AlesInfiny Maris では、アプリケーションで発生する例外を[業務例外](#business-exception)と[システム例外](#system-exception)の 2 つに分類します。
それぞれの例外の意味や処理方針を以下に示します。

#### 業務例外 {#business-exception}

- 意味

    業務フロー上、想定されるエラーを表す例外です。

- 発生箇所

    アプリケーションコア層の業務ロジック内で、明示的にスローします。

- 処理方針

    業務ロジックの最も外側で集約し、アプリケーション形態・業務フローに応じた処理を行います。

#### システム例外 {#system-exception}

- 意味

    業務フロー上は想定されないシステムのエラーを表す例外です。
    実行ランタイムまでキャッチされずに到達した例外は、すべてシステム例外として扱います。

- 発生箇所

    .NET の実行ランタイム内からスローされます。
    またアプリケーションとして想定していない状態となったとき、業務ロジック内から明示的にスローすることがあります。

- 処理方針

    集約例外ハンドラー内でキャッチして、システムエラーの処理フローを実行します。

### 業務例外とシステム例外の使い分け {#business-exception-and-system-exception}

原則として、システム例外は業務ロジック内からスローしないようにします。 何らかのエラー状態を業務ロジックとして検出するのであれば、そのエラーを回復するための手段を業務フローとして設計し、業務例外として処理します。

## ログ出力方針 {#logging-policy}

概要編では、一般的なログの種類とログレベルを定めます。
アプリケーション形態ごとに個別に検討が必要なものについては別途定めます。
アプリケーション形態別のアプリケーションアーキテクチャ解説も、あわせて参照してください。

### ログの種類 {#log-pattern}

AlesInfiny Maris で定義するログの種類は以下の通りです。

- 操作ログ

    ユーザーの操作履歴や、アプリケーションに対して行った操作を記録するログを操作ログと呼びます。

- 通信ログ

    アプリケーションがネットワークを介して通信する際、送受信する業務データや、送信先の情報、ヘッダー情報等を記録するログを通信ログと呼びます。

- 監査ログ

    アプリケーションの持つデータに対して行われた CRUD 処理を、誰がいつ実行したか記録するログを監査ログと呼びます。

- アプリケーションログ

    ここまでのログの種類に該当しない、アプリケーションのロジック内から出力する汎用的なログをアプリケーションログと呼びます。

### ログレベル {#log-level}

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

### ログレベルと環境ごとの出力設定 {#configuration-of-log-levels-and-output-per-environment}

システムの実行環境にあわせて、適切なレベルのログを出力するように構成します。

| ログレベル   |      本番環境      |     テスト環境     |  ローカル開発環境  |
| ----------- | :----------------: | :----------------: | :----------------: |
| Critical    | :white_check_mark: | :white_check_mark: | :white_check_mark: |
| Error       | :white_check_mark: | :white_check_mark: | :white_check_mark: |
| Warning     | :white_check_mark: | :white_check_mark: | :white_check_mark: |
| Information | :white_check_mark: | :white_check_mark: | :white_check_mark: |
| Debug       |                    |                    | :white_check_mark: |
| Trace       |                    |                    |                    |

!!! note "ログレベル Trace の使用について"
    ログレベル Trace は、機密性の高い情報を含むことがあり、開発環境であってもログファイル等に出力されることを防ぐため原則使用しません。

### ログに含める標準データ {#standard-log-data}

以下の情報をログに含めます。

- ログ出力日時
- [ログのカテゴリ](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-category)
- [ログレベル](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-level)
- [イベントID](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-event-id)
- [メッセージ](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-message-template)
- [例外情報（スタックトレースなど）](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-exceptions)
- [スコープ](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-scopes)

### ロギングライブラリ {#logging-libraries}

AlesInfiny Maris では、ログ出力に [Microsoft.Extensions.Logging.ILogger](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.logging.ilogger) インターフェースを使用します。ログプロバイダーは、アプリケーションの形態に合わせて適切なものを選択します。

## メッセージ管理方針 {#message-management-policy}

メッセージ文字列は、表記の統一を図る目的にリソースファイルで管理します。

## 入力検証方針 {#input-validation-policy}

入力値単体や、入力値同士の比較によって検証可能な入力値検証は、入出力インターフェースとなる場所で行います。 入力値単体では検証できず、データストア内のデータとの比較によって検証する場合はアプリケーションコア層の業務ロジックで検証します。
