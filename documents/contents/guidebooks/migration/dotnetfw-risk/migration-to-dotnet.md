---
title: .NET Framework のリスク
description: .NET Framework にとどまり続けることで起こりうる リスクについて説明します。
---

# .NET への移行 {#top}

前章では、 .NET Framework に留まり続けることのリスクについて説明しました。
この章では、 .NET Framework から .NET へ移行することになったときの方針やハードルについて説明します。

## 基本的な方針 {#basic-policy}

.NET Framework と .NET の間には単純な互換性がないため、移行にあたっては、プロジェクトファイルや依存ライブラリの変更、 API の置き換え、アプリケーションの構成変更などが必要になります。

こうした移行作業には、 Microsoft が提供する [GitHub Copilot アップグレード :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/porting/github-copilot-upgrade/overview){ target=_blank } を活用できます。

GitHub Copilot アップグレードは、 AI エージェントによって既存のプロジェクトや依存関係を分析し、アップグレード計画の作成からコードの変更、ビルドやテストによる検証までを支援するツールです。 .NET Framework から .NET への移行にも対応しており、従来のプロジェクト形式から SDK スタイルへの変換や、ライブラリの置き換え、アプリケーションアーキテクチャの移行などを支援します。

ただし、 GitHub Copilot アップグレードによって、すべての移行作業が自動的に完了するわけではありません。生成された変更内容や移行計画を開発者が確認し、必要に応じて修正するとともに、移行後のアプリケーションが正常に動作することを十分に検証する必要があります。

具体的な利用方法や対応する移行シナリオについては、 Microsoft の公式ドキュメントを参照してください。

- [GitHub Copilot アップグレードの概要 :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/porting/github-copilot-upgrade/overview){ target=_blank }
- [GitHub Copilot アップグレードを使用して .NET アプリをアップグレードする方法 :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/porting/github-copilot-upgrade/dotnet-how-to-upgrade-with-github-copilot){ target=_blank }

## 移行のハードル {#obstacles-to-migration}

### 書き換えが必要な .NET Framework の機能 {#dotnetfw-function-to-rewrite}

以下の .NET Framework の機能は、 .NET では廃止されている、または同等の機能を利用することが難しいため、 .NET で利用可能な技術や機能への置き換えが必要になります。

これらの機能の移行にあたっては、 GitHub Copilot アップグレードを活用することで、既存コードの分析や移行先の技術への置き換えを支援できます。ただし、移行先の技術選定や設計の見直しが必要になる場合もあるため、生成されたコードをそのまま利用できるとは限りません。

各機能の具体的な移行方法については、 Microsoft の公式ドキュメントも参照してください。

#### ASP.NET Web Forms ( 廃止 ) {#aspnet-web-forms}

- 移行先候補: Blazor (Microsoft 推奨 ) , SPA ( Vue.js など )

!!! info "GitHub Copilot 向けのスキル"

    Microsoft は GitHub Copilot 向けに ASP.NET Web Forms から Blazor へのアップグレード用スキルを公開しています。

!!! warning "画面設計の見直しも検討すべし"

    ASP.NET Web Forms でサードパーティ製品を使用している場合、同じ画面を再現できない可能性があります。
    設計時から UI 設計のトレンドも大きく変化しているはずですので、画面の再設計を強く推奨します。

#### ASMX Web Services ( 廃止 ) {#asmx-web-services}

- 移行先候補: ASP.NET Core Web API, Core WCF, ASP.NET Core gRPC サービスなど

#### Windows Communication Foundation (WCF) (廃止) {#wcf}

- 移行先候補: Core WCF や ASP.NET Core gRPC サービス

#### DataSet / TableAdapter ( 廃止ではないが使いづらい ) {#dataset-table-adapter}

- 移行先候補: Entity Framework Core

??? note "その他、廃止された機能 ( クリックで展開 )"

    他にも以下の .NET Framework のテクノロジー、機能、 API が廃止されています。

    - アプリケーションドメイン
    - .NET Remoting
    - 透過的セキュリティ
    - COM+ ( System.EnterpriseService )
    - コードアクセスセキュリティ ( CAS )

    これらの影響を受けることは稀ですが、影響確認は必要です。

### 短い製品ライフサイクルへの追従 {#adapt-to-short-product-life-cycles}

<!-- textlint-disable @textlint-ja/no-synonyms -->
.NET は、 .NET Framework に対して製品ライフサイクルが短期化されています。
LTS 版であっても 3 年間のサポートしか提供されません。
つまり、 .NET の場合、これまでの「納品までが仕事」というビジネスモデルは通用しません。
.NET へ移行する場合、 DevOps の導入など、継続的なメンテナンスを前提としたビジネスモデルへの変革が必要となります。

## 結論 {#conclusion}

.NET 関連技術は、大きな変革の時期を迎えています。
<!-- textlint-enable @textlint-ja/no-synonyms -->

- .NET Framework の開発停止
- ASP.NET Web Forms など過去の主要技術の終焉
- .NET Framework にとどまることのリスク増加
- .NET Framework / Core / Mono / Xamarin を統合した .NET

.NET Framework から .NET への移行を進めるべきか、判断しましょう。
なお、コードベースが大きければ大きいほど、移行への道は重く、長くなります。
移行するかどうかに限らず、早期の方針検討・計画化が重要となります。
