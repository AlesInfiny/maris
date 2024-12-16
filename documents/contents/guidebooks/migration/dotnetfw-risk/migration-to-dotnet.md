---
title: .NET Framework のリスク
description: .NET Framework にとどまり続けることで起こりうる リスクについて説明します。
---

# .NET への移行 {#top}

<!-- cSpell:ignore dotnetfw aspnet -->

前章では、 .NET Framework に留まり続けることのリスクについて説明しました。
この章では、 .NET Framework から .NET へ移行することになったときの方針やハードルについて説明します。

## 基本的な方針 {#basic-policy}

<!-- textlint-disable ja-technical-writing/sentence-length -->

.NET Framework と .NET の間には単純互換性がないため、基本的にはコードを書き直す必要があります。
ただし、 2024 年 11 月現在、以下のアプリケーション形式については、 Microsoft から提供される「[.NET Upgrade Assistant :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/porting/upgrade-assistant-overview){ target=_blank }」という変換ツールが使用できます。

<!-- textlint-enable ja-technical-writing/sentence-length -->

- .NET Framework ASP.NET アプリ
- .NET Framework Windows フォーム アプリ
- .NET Framework WPF アプリ
- .NET Framework サーバーサイド WCF アプリ
- .NET Framework UWP アプリ
- .NET Framework ASP.NET MVC アプリ
- .NET Framework コンソール アプリ
- .NET Framework クラス ライブラリ

.NET Upgrade Assistant を使用すると、 .NET Framework のソリューションやプロジェクトが .NET 形式へ変換されます。
このツールを使用して変換した後、ビルドエラーや実行時エラーに手動で対応します。

??? note "新旧 .NET Upgrade Assistant について（クリックで展開）"

    .NET Upgrade Assistant には、レガシーバージョンと最新バージョンの 2 種類があり、まったく別のアプリケーションと言えるほどの違いがあります。
    .NET Framework サーバーサイド WCF アプリにはレガシーバージョンのみ対応しています。
    また、 ASP.NET Web Forms は最新バージョンのみ対応していますが、機能は限定されています。

    レガシーバージョンと最新バージョンの主な違いは以下のとおりです。

    |                        | レガシーバージョン                      | 最新バージョン                                                                               |
    | ---------------------- | --------------------------------------- | -------------------------------------------------------------------------------------------- |
    | アプリケーションの形態 | コマンドラインツール                    | Visual Studio 拡張機能                                                                       |
    | 実行環境の前提         | .NET 7.0 Runtime （サポート切れ）が必要 | Visual Studio 2022 の最新版が必要                                                            |
    | 実行可能な機能         | .NET Framework から .NET への変換       | 左記のほか、 .NET バージョンのメジャーアップデート対応、 .NET Core → .NET への対応などが追加 |

## 移行のハードル {#obstacles-to-migration}

### 書き換えが必要な .NET Framework の機能 {#dotnetfw-function-to-rewrite}

以下の .NET Framework 機能は、.NET では廃止または非常に使用しづらいため、.NET の機能へ書き換える必要があります。

#### ASP.NET Web Forms ( 廃止 ) {#aspnet-web-forms}

- 移行先候補： ASP.NET Core MVC や Blazor , SPA ( Vue.js など )
- 移行方法：移行ツールは存在せず、手作業でのソースコード書き換えが必要

!!! warning "画面設計の見直しも検討すべし"

    ASP.NET Web Forms でサードパーティ製品を使用している場合、同じ画面を再現できない可能性があります。
    設計時から UI 設計のトレンドも大きく変化しているはずですので、画面の再設計を強く推奨します。

#### ASMX Web Services ( 廃止 ) {#asmx-web-services}

- 移行先候補： ASP.NET Core Web API, Core WCF, ASP.NET Core gRPC サービスなど
- 移行方法：移行ツールは存在せず、手作業でのソースコード書き換えが必要

#### Windows Communication Foundation (WCF) (廃止) {#wcf}

- 移行先候補： Core WCF や ASP.NET Core gRPC サービス
- 移行方法： Core WCF への移行には .NET Upgrade Assistant を使用可能。 ASP.NET Core gRPC サービスの場合、移行ツールは存在せず、手作業でのソースコード書き換えが必要

#### DataSet / TableAdapter ( 廃止ではないが使いづらい ) {#dataset-table-adapter}

- 移行先候補： Entity Framework Core
- 移行方法：移行ツールは存在せず、手作業でのソースコード書き換えが必要

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
