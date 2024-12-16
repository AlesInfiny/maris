---
title: .NET Framework のリスク
description: .NET Framework にとどまり続けることで起こりうる リスクについて説明します。
---

# .NET Framework の開発停止によるリスク {#top}

<!-- cSpell:ignore dotnetfw -->

前章で述べたとおり、 .NET Framework の開発はすでに停止しているため、これから始まる開発プロジェクトにおいては、 .NET が第一の選択肢となります。
他方、 .NET Framework で構築済みの既存のシステムについては、 2 つの選択肢 ―― .NET へ移行するか、 .NET Framework に留まり続けるか―― が存在します。

- 移行する：移行パスを考え、コストをかけてリライトする
- 移行しない：リスクを受け入れる

本章では、 .NET Framework に留まり続けることを選択した場合、どのような困難が発生するかについて説明します。

## 開発を進めることが困難になる {#difficulties-with-developing}

新機能が追加されないとしても .NET Framework 自体は存在しているので、一見開発はできるように思えてしまいます。
しかし、次に述べる理由により、開発を継続するのは徐々に難しくなっていきます。

### 有益な OSS 製品を採用できない {#cannot-apply-useful-oss}

<!-- textlint-disable ja-technical-writing/sentence-length -->

RESTful API の Open API 仕様書を生成する Swagger のライブラリ「 [Swashbuckle.AspNetCore.Swagger :material-open-in-new:](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Swagger/){ target=_blank } 」は、 .NET Framework 向けに提供されていません。
今後、 .NET 向けの有益な OSS 製品が出たとしても、それらが .NET Framework をサポートする可能性は低いでしょう。
またすでに存在する OSS 製品の新しいバージョンが .NET Framework をサポートしなくなる可能性もあります。

<!-- textlint-enable ja-technical-writing/sentence-length -->

### 多くのサードパーティ製品が EOL となり使用できない {#3rd-party-products-will-be-eol}

サードパーティ製品は、 .NET Framework のサポート範囲を徐々に狭めています。
.NET Framework 上でいつまでその製品が利用できるか不透明な状況が訪れています。
プロジェクトが本番稼働を迎えた直後に使用している製品が EOL を迎えてしまうということも起こりえます。

### ランタイムの進化に追従できない {#cannot-keep-up-with-runtime-evolution}

<!-- textlint-disable ja-technical-writing/sentence-length -->

暗号の利用モードの 1 つである AEM-GCM を C# で使用できる [System.Security.Cryptography.AesGcm クラス :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.aesgcm){ target=_blank } は、 .NET Framework 4.8 以降では使用できません。
これは .NET Standard 2.1 で追加されたため、 .NET Standard 2.0 の実装である .NET Framework 4.8 からは使用できないのです。
すでに .NET Framework は .NET に新しく追加される API から取り残されつつあります。

<!-- textlint-enable ja-technical-writing/sentence-length -->

### 新しい開発環境が使えない {#cannot-use-new-ide}

2024 年 11 月現在、最新 Visual Studio 2022 では、 .NET Framework 4.6.2 より古いバージョンでの開発がサポートされません。
よってこれらのバージョンを対象としたアプリケーションをビルドできません。
今後、 Visual Studio でビルドできる .NET Framework のバージョンは減っていくことが考えられます。

### 言語バージョンが更新されないため、モダンなコーディングができない {#without-modern-coding}

.NET Framework 4.8.1 に対応する C# のバージョンは 7.3 です。
.NET Framework は 4.8.1 が最後のバージョンであるため、 C# バージョンは 7.3 に固定されます。
C# 8.0 以降追加された便利で生産性を上げてくれる記法は使用できません。
また Web 等で公開されているコードサンプルをそのままでは利用できない可能性もどんどん高まっていきます。
同じ言語でありながら、時代に取り残される可能性があります。

### 実行環境が制限される {#restriction-of-runtime}

<!-- textlint-disable ja-technical-writing/sentence-length -->

2024 年 11 月現在、 [Azure Web Apps :material-open-in-new:](https://learn.microsoft.com/ja-JP/azure/app-service/){ target=_blank } では、 .NET Framework アプリケーションの稼働環境として .NET Framework 3.5 または 4.8 のみ選択可能です[^1]。
また、 Microsoft の公式 docker イメージである [.NET Framework Runtime :material-open-in-new:](https://hub.docker.com/_/microsoft-dotnet-framework-runtime/){ target=_blank } も、バージョンとして選択できるのは .NET Framework 3.5, 4.8, 4.8.1 のみです。
.NET Framework は、クラウド環境やコンテナー環境から徐々に締め出されつつあります。
将来的に、クラウド環境やコンテナー環境で .NET Framework が使用できなくなる可能性は高いです。

<!-- textlint-enable ja-technical-writing/sentence-length -->

## 開発体制を構築することが困難になる {#difficult-to-establish-a-development-team}

.NET Framework に留まり続けた場合、開発自体が困難になるだけでなく、開発体制を構築することも困難になります。
ここでは、その理由について説明します。

### UI デザイナーとの協業ができない {#cannot-work-with-ui-designers}

.NET Framework の代表的な Web アプリケーションフレームワークである ASP.NET Web Forms は、かつてその開発生産性の高さが評価されていました。
しかし、開発時にデザインと画面機能を完全には分離できないという欠点があります。
開発者はアプリケーションとしての機能の開発に注力し、 UI デザイナーが見栄えの部分を担うという、モダンアプリケーションでは一般的な開発方法は、 ASP.NET Web Forms では使用できません。

### .NET Framework に精通した人員確保が困難になる {#cannot-get-experts-of-dotnetfw}

かつて .NET Framework で構築された Azure のクラウドサービスは .NET へ移行されています。

- [The Azure Cosmos DB Journey to .NET 6 :material-open-in-new:](https://devblogs.microsoft.com/dotnet/the-azure-cosmos-db-journey-to-net-6/){ target=_blank }
- [Azure Active Directory’s gateway is on .NET Core 3.1! :material-open-in-new:](https://devblogs.microsoft.com/dotnet/azure-active-directorys-gateway-service-is-on-net-core-3-1/){ target=_blank }

また、無償の動画コンテンツ ( [.NET の公式チャンネル :material-open-in-new:](https://www.youtube.com/c/dotNET){ target=_blank } ) が存在しますが、 .NET Framework のコンテンツは存在しません。

このように、社外では .NET の技術者や技術者コミュニティーが拡大すると同時に、 .NET Framework のコミュニティーは縮小しています。
最新の機能や技術はすべて .NET に追加されるため、技術者は .NET Framework ではなく .NET を学習するようになるでしょう。
また、技術書や研修プログラムといった学習のツールも、新しく提供される物は .NET Framework ではなく .NET 向けのコンテンツとなります。

これらのことから、社内外を問わず .NET Framework コミュニティーは縮小していきます。
それに伴って、 .NET Framework に詳しい、特に若手の人材は今後減っていくでしょう。
開発プロジェクトにおいて .NET Framework の有識者を確保することは困難となります。

## 顧客提供価値が低下する {#decrease-in-customer-value}

ここまで説明したとおり、新しい機能や最新の技術は .NET にのみ追加され、.NET Framework は取り残されます。
「[ランタイムの進化に追従できない](#cannot-keep-up-with-runtime-evolution)」 で解説した通り、暗号などのセキュリティに関連するものであっても、新しい機能は .NET Framework には追加されません。

今後、 .NET には、新たに定義されたプロトコルである HTTP/3 や Windows 11 の関連機能、新たな入力デバイスに対応する機能といった重要な新機能が追加されると予想されます。
しかし、.NET Framework ではそれらの機能は利用できません。

また、.NET Framework で最もよく利用される ASP.NET Web Forms は、生産性の向上を重視したフレームワークであったため、優れた UI / UX を実現することは非常に困難です。
新しい技術は利用できず、 UI / UX の点でも見劣りする .NET Framework で作成したアプリケーションでは、顧客に提供できる価値は低下します。

## 企業価値が低下する {#decrease-in-corporate-value}

ここまで、 .NET Framework に留まり続けた場合には、新しい機能・技術や最新の UI / UX から取り残され、結果として顧客に提供する価値が低下するというリスクを説明しました。
顧客に提供できる価値が低下するということは、すなわち、企業としての価値の低下に直結します。
価値の低いアプリケーションしか提供できない企業に誰が価値を見出すでしょうか。
また、企業価値が低下するのは顧客にとってだけではありません。
新しいことに挑戦しない企業は、優秀なエンジニアにとって魅力がないものに映るでしょう。
.NET Framework にいつまでも留まり続けることで、業界内だけでなく、就職・転職市場においても、企業の価値が低下することは避けられません。

## 移行を急がなくても良い例 {#cases-no-need-to-rush-the-migration}

ここまで、 .NET Framework に留まり続けることのリスクについて解説しました。
しかし、移行を急ぐ必要があまりないアプリケーションも、数は少ないものの存在します。
以下の条件を満たす、いわゆる「塩漬けシステム」は、現時点で移行を急ぐ必要はありません。

- インターネットに公開されていないオンプレミスのシステム
- 長く要件が変わらず、新しい機能追加も行われていないシステム
- 同じシステムを 5 年以上、一切手を加えずに使い続ける前提のシステム
- セキュリティリスクを受け入れることができる ( または、他の方法で担保できる ) システム

ただし、 .NET Framework 3.5 SP1 以前を使用しているシステムは、上掲の条件をすべて満たしていたとしても移行を検討すべきです。
.NET Framework 3.5 SP1 は OS のバージョンによらず、 2029 年 1 月にサポートが終了するためです。

???+ note "コラム：.NET Framework は塩漬けシステムに適していたのか？"

    「[移行を急がなくても良いケース](#cases-no-need-to-rush-the-migration)」では、「塩漬けシステム」の移行を急ぐ必要は無いと記載しました。
    しかし本当に .NET Framework はそのようなシステムに適していたのでしょうか？
    .NET Framework は、インストール後バージョンを固定できるようなものではなく、サーバー OS の更新時に新しいバージョンへと自動でアップデートされます。
    .NET Framework には強い後方互換性があるため、テストを再実施しなくても非互換が発生しないパターンが多かっただけといえるでしょう。

[^1]: [Microsoft のクイックスタートガイド :material-open-in-new:](https://learn.microsoft.com/ja-jp/azure/app-service/quickstart-dotnetcore?tabs=netframework48&pivots=development-environment-vs){ target=_blank } では、 .NET Framework 3.5 が選択できることすら説明されていません。
