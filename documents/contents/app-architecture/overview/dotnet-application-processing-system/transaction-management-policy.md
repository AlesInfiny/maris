---
title: .NET アプリケーションの 処理方式
description: AlesInfiny Maris OSS Edition で構築する .NET アプリケーションの共通的な処理方式を解説します。
---

# トランザクション管理方針 {#top}

トランザクション管理とは、ひとまとまりの処理の ACID 特性を担保することです。
システム内でトランザクション管理方針を定めることで、データ不整合やデッドロックの発生を防ぎ、処理性能を高めます。

## トランザクションの管理単位 {#transaction-unit}

トランザクション管理はアプリケーションコア層のアプリケーションサービスで行います。

### オンライン処理におけるトランザクションの管理単位 {#online-transaction}

オンライン処理を行うアプリケーションでは、以下の方針でトランザクションを管理します。

- トランザクションの単位をアプリケーションサービス内の各 public メソッドとする
- データアクセスを含む処理を一律でトランザクション管理の対象とする

!!! note "その他のトランザクションの管理単位"
    更新系のデータアクセスを含む処理のまとまりのみトランザクション制御する方針も取ることができます。つまり、参照系のデータアクセスのみ行う処理のまとまりはトランザクション制御しないということです。
    この方針を採用する場合、トランザクション制御していない部分に更新系のデータアクセスが含まれていないことを十分に確認する必要があります。

### バッチ処理におけるトランザクションの管理単位 {#batch-transaction}

（今後追加予定）

## トランザクションの実現方式 {#implementing-transactions}

TransactionScope を利用してトランザクションの範囲を定めます。以下のように設定します。

- [TransactionOptions.IsolationLevel :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/system.transactions.transactionoptions.isolationlevel){ target=_blank } : ReadCommitted
- [TransactionScopeOption :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/system.transactions.transactionscopeoption){ target=_blank } : Required/RequiresNew
- [TransactionScopeAsyncFlowOption :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/system.transactions.transactionscopeasyncflowoption){ target=_blank } : Enabled

## トランザクションのコミットとロールバック方針 {#commit-and-rollback}

アプリケーションサービスでトランザクションを明示的にコミットした場合に更新内容をデータベースへコミットします。
明示的にコミットせずアプリケーションサービスの処理を抜けた場合、コミットしなかったトランザクション内で行った更新処理をロールバックします（データ不整合により例外が発生した場合なども含みます）。

更新対象データの不整合により発生した例外は、システム例外として集約例外ハンドラーに処理されます。

## トランザクション分離レベル {#isolation-level}

既定値として ReadCommitted を選択します。
コミット済みデータのみを読み取るため、ダーティリードを防ぎます。
また、同一データに対して参照操作が行われても待ち時間が発生しないため、データアクセス処理時間を短縮できます。

!!! note "スナップショット分離オプション"
    SQL Server にはスナップショット分離オプションが存在します。スナップショット分離オプションを有効化すると、トランザクション制御が「ロック方式」から「行のバージョン管理」に変更されます。「行のバージョン管理」では、トランザクション開始時点のデータベースのスナップショットを一時データベースに保存し、スナップショットをもとにトランザクション内の処理が行われます。よって、ロック待ち時間の短縮とデッドロック対策になります。
    スナップショット分離オプションは原則有効化します。

    詳細はこちらを確認してください。
    [SQL Server でのスナップショット分離オプション :material-open-in-new:](https://learn.microsoft.com/ja-jp/sql/connect/ado-net/sql/snapshot-isolation-sql-server){ target=_blank }

## 非同期処理の考慮 {#async-process}

サーバーサイドの実装には非同期処理を多用します。非同期処理を呼び出す場合もトランザクションが適切にコミット/ロールバックされるよう制御します。

!!! note "TransactionScopeAsyncFlowOption の設定"

    TransactionScope の設定値として、 TransactionScopeAsyncFlowOption を Enabled にすることを原則としています。この設定により、非同期処理の呼び出し元の TransactionScope が使用されます。

## 更新競合の対策 {#preventing-update-conflicts}

システムの処理性能を高めるため、原則として楽観同時実行制御を採用します。
更新競合の発生がまれであることを前提として更新対象データにロックをかけず、更新操作の実行時にデータの整合性が取れていることを確認します。
更新競合が発生した場合は例外が発生し、更新内容が取り消されます。

Entity Framework Core を利用して楽観同時実行制御を実装します。詳細は以下を確認してください。

[オプティミスティックコンカレンシー :material-open-in-new:](https://learn.microsoft.com/ja-jp/ef/core/saving/concurrency?tabs=data-annotations#optimistic-concurrency){ target=_blank }

## デッドロック対策 {#preventing-deadlock}

単一のトランザクション内で複数テーブルへのアクセスがある場合、トランザクション間でアクセスする順序を統一します。
