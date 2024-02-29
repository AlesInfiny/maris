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

## イベントID管理方針 {#event-id-management-policy}

イベント ID は、ログを機械的に識別するために使用します。
適切なイベント ID をログに出力することで、イベントの発生原因や対処手段の特定が容易になり、効率的なサービス運用が可能になります。
イベント ID は、プロジェクトごとにクラスファイルで管理し、管理対象はログレベルが Information 以上のログとします。

- 識別子
  
    プロジェクト内で一意になるような識別子を付与します。

- 名前
  
    プロジェクト内で一意かつ、発生したイベントの意味が理解できる名前を付与します。
  
## メッセージ管理方針 {#message-management-policy}

メッセージ文字列は、表記の統一を図る目的にリソースファイルで管理します。

## 入力検証方針 {#input-validation-policy}

入力値単体や、入力値同士の比較によって検証可能な入力値検証は、入出力インターフェースとなる場所で行います。 入力値単体では検証できず、データストア内のデータとの比較によって検証する場合はアプリケーションコア層の業務ロジックで検証します。

## トランザクション管理方針 {#transaction-management-policy}

トランザクション管理とは、ひとまとまりの処理の ACID 特性を担保することです。
システム内でトランザクション管理方針を定めることで、データ不整合やデッドロックの発生を防ぎ、処理性能を高めます。

### トランザクションの管理単位 {#transaction-unit}

トランザクション管理はアプリケーションコア層のアプリケーションサービスで行います。

#### オンライン処理におけるトランザクションの管理単位 {#online-transaction}

オンライン処理を行うアプリケーションでは、以下の方針でトランザクションを管理します。

- トランザクションの単位をアプリケーションサービス内の各 public メソッドとする
- データアクセスを含む処理を一律でトランザクション管理の対象とする

!!! note "その他のトランザクションの管理単位"
    更新系のデータアクセスを含む処理のまとまりのみトランザクション制御する方針も取ることができます。つまり、参照系のデータアクセスのみ行う処理のまとまりはトランザクション制御しないということです。
    この方針を採用する場合、トランザクション制御していない部分に更新系のデータアクセスが含まれていないことを十分に確認する必要があります。

#### バッチ処理におけるトランザクションの管理単位 {#batch-transaction}

（今後追加予定）

### トランザクションの実現方式 {#implementing-transactions}

TransactionScope を利用してトランザクションの範囲を定めます。以下のように設定します。

- [TransactionOptions.IsolationLevel](https://learn.microsoft.com/ja-jp/dotnet/api/system.transactions.transactionoptions.isolationlevel) : ReadCommitted
- [TransactionScopeOption](https://learn.microsoft.com/ja-jp/dotnet/api/system.transactions.transactionscopeoption) : Required/RequiresNew
- [TransactionScopeAsyncFlowOption](https://learn.microsoft.com/ja-jp/dotnet/api/system.transactions.transactionscopeasyncflowoption) : Enabled

### トランザクションのコミットとロールバック方針 {#commit-and-rollback}

アプリケーションサービスでトランザクションを明示的にコミットした場合に更新内容をデータベースへコミットします。
明示的にコミットせずアプリケーションサービスの処理を抜けた場合、コミットしなかったトランザクション内で行った更新処理をロールバックします（データ不整合により例外が発生した場合なども含みます）。

更新対象データの不整合により発生した例外は、システム例外として集約例外ハンドラーに処理されます。

### トランザクション分離レベル {#isolation-level}

既定値として ReadCommitted を選択します。
コミット済みデータのみを読み取るため、ダーティリードを防ぎます。
また、同一データに対して参照操作が行われても待ち時間が発生しないため、データアクセス処理時間を短縮できます。

!!! note "スナップショット分離オプション"
    SQL Server にはスナップショット分離オプションが存在します。スナップショット分離オプションを有効化すると、トランザクション制御が「ロック方式」から「行のバージョン管理」に変更されます。「行のバージョン管理」では、トランザクション開始時点のデータベースのスナップショットを一時データベースに保存し、スナップショットをもとにトランザクション内の処理が行われます。よって、ロック待ち時間の短縮とデッドロック対策になります。
    スナップショット分離オプションは原則有効化します。

    詳細はこちらを確認してください。
    [SQL Server でのスナップショット分離オプション :material-open-in-new:](https://learn.microsoft.com/ja-jp/sql/connect/ado-net/sql/snapshot-isolation-sql-server){ target=_blank }

### 非同期処理の考慮 {#async-process}

サーバーサイドの実装には非同期処理を多用します。非同期処理を呼び出す場合もトランザクションが適切にコミット/ロールバックされるよう制御します。

!!! note "TransactionScopeAsyncFlowOption の設定"

    TransactionScope の設定値として、 TransactionScopeAsyncFlowOption を Enabled にすることを原則としています。この設定により、非同期処理の呼び出し元の TransactionScope が使用されます。

### 更新競合の対策 {#preventing-update-conflicts}

システムの処理性能を高めるため、原則として楽観同時実行制御を採用します。
更新競合の発生がまれであることを前提として更新対象データにロックをかけず、更新操作の実行時にデータの整合性が取れていることを確認します。
更新競合が発生した場合は例外が発生し、更新内容が取り消されます。

Entity Framework Core を利用して楽観同時実行制御を実装します。詳細は以下を確認してください。

[オプティミスティックコンカレンシー :material-open-in-new:](https://learn.microsoft.com/ja-jp/ef/core/saving/concurrency?tabs=data-annotations#optimistic-concurrency){ target=_blank }

### デッドロック対策 {#preventing-deadlock}

単一のトランザクション内で複数テーブルへのアクセスがある場合、トランザクション間でアクセスする順序を統一します。

## ヘルスチェックの必要性 {#health-check-necessity}

### 目的・背景 {#health-check-background}

ヘルスチェックの目的は、ロードバランサー（以降 LB ）がシステムの稼働状況を監視することです。
ヘルスチェック機能の実装により以下を期待できます。

- 負荷分散

    各サーバーがリクエスト受付可能かどうかを確認の上、リクエストを複数のサーバーに割り振ることで負荷分散を行います。
    これによりレスポンスの低下を防ぎ、サービスを継続的かつ効率的に運用できます。

- 可用性向上

    ヘルスチェック機能によりサーバーやアプリケーションの稼働状況を監視することで、異常を検知できます。
    サーバーの異常を検知した際、正常なサーバーに自動的に動作を引き継ぐことで、システムの運用停止を防ぎます。

### 基本方針 {#health-check-policy}

LB が行うヘルスチェックは、使用するプロトコルレイヤーの違いによって、以下のように分類できます。

| プロトコルレイヤー |                                                           詳細                                                           |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------ |
| Layer 3          | ネットワーク層で動作を監視します。ICMP echo リクエストを送信し、 echo リプライが帰ってくるかどうかを確認します。            |
| Layer 4          | トランスポート層で動作を監視します。TCP のハンドシェイクを行い、サーバーの動作を確認します。                             |
| Layer 7          | アプリケーション層で動作を監視します。 HTTP リクエストを送信し、サーバーの HTTP レスポンスの確認により動作を確認します。 |

Layer 3 や Layer 4 で行うヘルスチェックは、作成した Web アプリケーションの稼働状況まで確認できません。
AlesInfiny Maris では、監視対象のサーバーやアプリケーション、関連するデータベースなどのサービスを包含し、システムとしての正常性を Layer 7 で監視する方針とします。

### レスポンス形式 {#health-check-response}

ヘルスチェックを確認する HTTP レスポンスとして、サーバーの正常値と異常値を表すステータスコードとレスポンスボディを定義します。

- ステータスコード

    AlesInfiny Maris では正常値を 200 、異常値を 503 で統一します。

- レスポンスボディ

    レスポンスボディの内容は、頻繁な状態監視による通信量の増加に対応するため、簡潔な形に固定する必要があります。
    従って、サーバーの動作状態を表す内容のみをレスポンスボディとします。
