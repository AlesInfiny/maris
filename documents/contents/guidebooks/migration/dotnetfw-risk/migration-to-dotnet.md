---
title: .NET Framework のリスク
description: .NET Framework にとどまり続けることで起こりうる リスクについて説明します。
---

# .NET への移行 {#top}

<!-- cSpell:ignore dotnetfw aspnet -->

前章では、 .NET Framework に留まり続けることのリスクについて説明しました。
この章では、 .NET Framework から .NET へ移行することになったときの方針やハードルについて説明します。

## 基本的な方針 {#basic-policy}

.NET Framework と .NET の間には単純互換性がないため、原則手作業でコードを書き直す必要があります。

!!! warning ".NET Upgrade Assistant （非推奨）について"

    .NET Framework → .NET への変換ツールとして、以前は Microsoft が提供する .NET Upgrade Assistant がありました。
    しかし、このツールは [正式に非推奨となってしまった :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/porting/upgrade-assistant-overview){ target=_blank } ため、
    2025 年 11 月現在、 .NET Framework からの移行ツールは存在しません。

## 移行のハードル {#obstacles-to-migration}

### 書き換えが必要な .NET Framework の機能 {#dotnetfw-function-to-rewrite}

以下の .NET Framework 機能は、.NET では廃止または非常に使用しづらいため、.NET の機能へ書き換える必要があります。
なお、前述のとおり、どの機能においても移行方法は手作業でのソースコード書き換えとなります。

#### ASP.NET Web Forms ( 廃止 ) {#aspnet-web-forms}

- 移行先候補： ASP.NET Core MVC や Blazor , SPA ( Vue.js など )

!!! warning "画面設計の見直しも検討すべし"

    ASP.NET Web Forms でサードパーティ製品を使用している場合、同じ画面を再現できない可能性があります。
    設計時から UI 設計のトレンドも大きく変化しているはずですので、画面の再設計を強く推奨します。

#### ASMX Web Services ( 廃止 ) {#asmx-web-services}

- 移行先候補： ASP.NET Core Web API, Core WCF, ASP.NET Core gRPC サービスなど

#### Windows Communication Foundation (WCF) (廃止) {#wcf}

- 移行先候補： Core WCF や ASP.NET Core gRPC サービス

#### DataSet / TableAdapter ( 廃止ではないが使いづらい ) {#dataset-table-adapter}

- 移行先候補： Entity Framework Core

??? note "その他、廃止された機能 ( クリックで展開 )"

    他にも以下の .NET Framework のテクノロジー、機能、 API が廃止されています。

    - アプリケーションドメイン
    - .NET Remoting
    - 透過的セキュリティ
    - COM+ ( System.EnterpriseService )
    - コードアクセスセキュリティ ( CAS )

    これらの影響を受けることは稀ですが、影響確認は必要です。

### 短い製品ライフサイクルへの追従 {#adapt-to-short-product-life-cycles}

.NET は、 .NET Framework に対して製品ライフサイクルが短期化されています。
LTS 版であっても 3 年間のサポートしか提供されません。
つまり、 .NET の場合、これまでの「納品までが仕事」というビジネスモデルは通用しません。
.NET へ移行する場合、 DevOps の導入など、継続的なメンテナンスを前提としたビジネスモデルへの変革が必要となります。

## 結論 {#conclusion}

.NET 関連技術は、大きな変革の時期を迎えています。

- .NET Framework の開発停止
- ASP.NET Web Forms など過去の主要技術の終焉
- .NET Framework にとどまることのリスク増加
- .NET Framework / Core / Mono / Xamarin を統合した .NET

.NET Framework から .NET への移行を進めるべきか、判断しましょう。
なお、コードベースが大きければ大きいほど、移行への道は重く、長くなります。
移行するかどうかに限らず、早期の方針検討・計画化が重要となります。
