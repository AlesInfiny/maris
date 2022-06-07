# .NET Framework にとどまることのリスク

--8<-- "includes/abbreviations.md"

最近の .NET ランタイムをめぐる同行を開設したうえで、.NET Framework にとどまり続けることで起こりうるリスクについて説明します。

## 1. .NET ランタイムの最新動向 ## {: #dotnet-runtime-trends}

2022年6月現在、 .NET ランタイムには以下があります。

- .NET Framework
    - Windows 上でのみ動作する
    - Windows OS のコンポーネントとしてプリインストールされている
- .NET （旧称：.NET Core）
    - 様々な OS 上で動作する OSS
    - 大幅な軽量化により動作性能が大きく向上
    - .NET 6 で GUI の開発に強みを持つ Mono / Xamarin を統合完了

![.NET ランタイムの進化と統合](../../../images/guidebooks/migration/dotnetfw-risk/evolution-and-integration-of-dotnet.png)

### 1-1. .NET Frameworkから.NETへ ### {: #dotnetfw-to-dotnet}

.NET Framework と .NET は一見似ていますが、その成り立ちをたどると別の物であることが分かります。

2000年に、Windows PC 向けに Windows アプリケーションや Web アプリケーション向けのライブラリやランタイムを提供する .NET Framework 1.0 がリリースされました。

それから .NET Framework はバージョンを増やしていきますが、一方で技術の革新が進み、クライアントサイドにおいてはスマートフォンアプリや IoT の登場、サーバーサイドにおいては AI 等 SaaS によって提供される高度なサービスの活用など、Windows 製品のみの組み合わせによる単純な Windows アプリケーションや Web アプリケーションではユーザーの需要を満たすシステムを実現することが難しくなりました。

そこで、Microsoft は2016年6月に .NET Core 1.0 をリリースします。これは、Windows 上でも Linux 上でも動作するクロスプラットフォームかつオープンソースの開発プラットフォームです。

また、.NET Core ともに、.NET Standard という概念が登場しました。.NET Standard は、さまざまな .NET 環境で使用可能な API セットを定義した仕様です。.NET (.NET Core) や .NET Framework は、.NET Standard の実装という位置づけになりました。

その後、.NET Core は .NET と改称し、Windows 、Linux の他 mac OS にも対応して、2022年現在は .NET 6.0 がリリースされています（.NET 6.0 は、.NET Standard 2.1 の実装です）。

このように、.NET Framework と .NET は、どちらも .NET Standard の実装であり、C# を使うものの、以下のように異なるものであると言えます。

|                 |ランタイム      |動作環境                                       |
|-----------------|---------------|-----------------------------------------------|
|.NET Framework   |プロプライエタリ|Windowsのみ                                    |
|.NET (.NET Core) |オープンソース  |クロスプラットフォーム（Windows, Linux, mac OS）|

つまり、.NET Framework と .NET の間に単純な互換性はありません。

### 1-2. .NET Frameworkの開発停止 ### {: #stop-development-of-dotnetfw}

2019年5月、Microsoft は .NET Framework の開発停止を宣言しました（[.NET Core is Future of .NET](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/)）。

この宣言では、次のことが明記されています。

- 新しい .NET アプリケーションは .NET Core （現在では .NET） ベースで開発すべきである
- 今後、将来的な投資はすべて .NET Core （現在では .NET）に対して行われる

.NET Framework は 4.8 が最後のバージョンとなり、以降はバグ修正やセキュリティ修正のみが提供されます。

実際に、2022年6月時点の .NET の最新版である .NET 6.0が .NET Standard 2.1 の実装であるのに対し、.NET Framework 4.8 は、.NET Standard 2.0の実装であり、.NET Standard 2.1 で定義されている API は使用できません。

また、上述の Microsoft の宣言により、これまで .NET Framework 向けにサードパーティ製品を提供していた企業も、投資先を .NET へ切り替えています。新製品は .NET 向けに提供され、 .NET Framework をサポートすることはないでしょう。

### 1-3. .NET ランタイムのサポート期限 ### {: #dotnet-runtime-support-expiration}

.NET Framework のサポート期限を以下の表に示します。

| ランタイムの種類                 | サポート期限                 |
| ------------------------------- | ---------------------------- |
| .NET Framework 3.5 SP1          | 2029年1月9日                 |
| .NET Framework 4.0 ～ 4.5.1     | 2016年1月12日（サポート終了） |
| .NET Framework 4.5.2 ～ 4.6.1   | 2022年4月26日（サポート終了） |
| .NET Framework 4.6.2            | OSのサポートライフサイクルに準拠 |
| .NET Framework 4.7              | 同上 |
| .NET Framework 4.7.1            | 同上 |
| .NET Framework 4.7.2            | 同上 |
| .NET Framework 4.8              | 同上 |

2022年6月現在、すでに .NET Framework 4.6.1 以下の .NET Framework 4.x 系は Microsoft のサポートが終了しています。
また、.NET Framework 3.5 SP1 も OS のバージョンによらず、2029年1月9日にはサポートが終了します。

すべての.NET Framework がすぐにサポート停止されるとは考えにくいですが、今後、VB6と同じように、OSの機能としては提供されるものの、新機能は提供されないしドキュメントも更新されない、といった立ち位置になる可能性が高いと考えられます。

## 2. .NET Frameworkの開発停止によるリスク ## {: #risk-of-dotnet-framework}

1章で述べたとおり、.NET Framework の開発はすでに停止しているため、これから始まる開発プロジェクトにおいては、.NET が第一の選択肢となります。
他方、.NET Framework で構築済みの既存のシステムについては、2つの選択肢――.NET へ移行するか、.NET Framework に留まり続けるか――が存在します。

- 移行する：移行パスを考え、コストをかけてリライトする
- 移行しない：リスクを受け入れる

2章では、.NET Framework に留まり続けることを選択した場合、どのような困難が発生するかについて説明します。

### 2-1. 開発を進めることが困難になる ### {: #difficulties-with-developing}

新機能が追加されないとしても .NET Framework 自体は存在しているので、一見開発はできるように思えるかもしれません。しかし、次に述べる理由により、開発は困難となるでしょう。

#### (1) 有益な OSS 製品を採用できない #### {: #cannot-apply-useful-oss}

RESTful API を構築するためのフレームワークである Swagger の C# ライブラリ「[Swashbuckle.AspNetCore.Swagger](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Swagger/)」は、.NET Framework 向けには提供されていません。

今後、.NET 向けの 新しい有益な OSS 製品が出たとしても、それらが .NET Framework をサポートする可能性は低いでしょうし、すでに存在する OSS 製品の新しいバージョンが .NET Framework をサポートしなくなる可能性もあります。

#### (2) 多くのサードパーティ製品が EOL となり使用できない #### {: #3rd-party-products-will-be-EOL}

Grapecty 社の製品である [InputMan](https://www.grapecity.co.jp/developer/inputmanplus-winforms) の最新版は、.NET Framework 向けには .NET Framework 4.8 版のみ提供されています。また、Infragistics 社の製品である [Ignite UI](https://jp.infragistics.com/support/supported-environments) の最新版は、すでに .NET Framework 3.5 SP1 のサポートを停止しています。

サードパーティ製品は、.NET Framework を広くサポートすることをやめつつあります。
プロジェクトが本番稼働を迎えた直後に使用している製品が EOL を迎えてしまうということも起こりえます。

#### (3) ランタイムの進化に追従できない #### {: #cannot-keep-up-with-runtime-evolution}

暗号の利用モードの1つである AEM-GCM を C# で使用できる [System.Security.Cryptography.AesGcm クラス](https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.aesgcm?view=net-6.0)は、.NET Standard 2.1 で追加されたため、.NET Standard 2.0の実装である .NET Framework 4.8 では使用することができません。

このように、すでに .NET Framework は .NET に新しく追加される API からは取り残されつつあります。

#### (4) 新しい開発環境が使えない #### {: #cannot-use-new-IDE}

2022年6月現在最新の Visual Studio である Visual Studio 2022 では、.NET Framework 4.0 から 4.5.1 までの開発がサポートされないため、これらのバージョンをターゲットとしたアプリケーションをビルドすることができません。

今後、IDE でビルドできる .NET Framework のバージョンは減っていくことが考えられます。

#### (5) 言語バージョンが更新されないため、モダンなコーディングができない #### {: #without-modern-coding}

.NET Framework 4.8 に対応する C# のバージョンは 7.0 です。.NET Framework は 4.8 が最後のバージョンであるため、C# バージョンは 7.0 に固定され、C# 8.0 以降追加されたコード記法は使用できません。

たとえば、WEB 等で見つけたコード例の記法をそのままでは利用できない可能性があります。

#### (6) 実行環境が制限される #### {: #restriction-of-runtime}

2022年6月現在、[Azure Web Apps](https://azure.microsoft.com/ja-jp/get-started/web-app/) では、.NET Framework アプリケーションの稼働環境として .NET Framework 3.5 または 4.8 のみ選択可能です（※）。また、Microsoft の公式 docker イメージである [.NET Framework Runtime](https://hub.docker.com/_/microsoft-dotnet-framework-runtime/) も、バージョンとして選択できるのは .NET Framework 3.5 と 4.8 のみです。

このように、.NET Framework はクラウド環境やコンテナー環境から徐々に締め出されつつあります。将来的に、クラウド環境やコンテナー環境で .NET Framework が使用できなくなる可能性は高いです。

※ [Microsoft のクイックスタートガイド](https://docs.microsoft.com/ja-jp/azure/app-service/quickstart-dotnetcore?tabs=netframework48&pivots=development-environment-vs) では、.NET Framework 3.5 が選択できることすら記述されていません。

### 2-2. 開発体制を構築することが困難になる ### {: #difficult-to-establish-a-development-team}

.NET Framework に留まり続けた場合、開発自体が困難になるだけでなく、開発体制を構築することも困難になります。ここでは、その理由について説明します。

#### (1) UI デザイナーとの協業ができない #### {: #cannot-work-with-UI-designers}

.NET Framework の代表的な Web アプリケーションである ASP.NET Web Forms は、かつてその開発生産性の高さが評価されていましたが、機能とデザインを完全には分離することができないという欠点があります。

開発者はアプリケーションとしての機能の開発に注力し、UI デザイナーが見栄えの部分を担うという、モダンアプリケーションでは一般的な開発手法は、ASP.NET Web Forms では使用できません。

#### (2) .NET Framework に精通した人員確保が困難になる #### {: #cannot-get-experts-of-dotnetfw}

かつて .NET Framework で構築されたAzure のクラウドサービスは .NET へ移行されています。

- [The Azure Cosmos DB Journey to .NET 6](https://devblogs.microsoft.com/dotnet/the-azure-cosmos-db-journey-to-net-6/)
- [Azure Active Directory’s gateway is on .NET Core 3.1!](https://devblogs.microsoft.com/dotnet/azure-active-directorys-gateway-service-is-on-net-core-3-1/)

また、Youtube 上には [.NET の公式チャンネル](https://www.youtube.com/c/dotNET) が存在しますが、.NET Framework のチャンネルは存在しません。

このように、社外では .NET の技術者や技術者コミュニティが拡大すると同時に、.NET Framework のコミュニティは縮小しています。

最新の機能や技術はすべて .NET に追加されるため、勉強熱心な技術者は .NET Framework ではなく .NET を学習するようになるでしょう。また、技術書や研修プログラムといった学習のツールも、新しく提供されるものは .NET Framework ではなく .NET 向けのコンテンツとなります。

これらのことから、社内外を問わず .NET Framework コミュニティーは縮小していきます。それに伴って、.NET Framework に詳しい、特に若手の人材は今後減っていでしょう。開発プロジェクトにおいて、いわゆる .NET Framework の有識者を確保することは困難となります。

### 2-3. 顧客提供価値が低下する ### {: #decrease-in-customer-value}

ここまで説明したとおり、新しい機能や最新の技術は .NET にのみ追加され、.NET Framework は取り残されます。

[2-1 (3)](#cannot-keep-up-with-runtime-evolution) に記述のとおり、暗号などのセキュリティに関連するものであっても、新しい機能は .NET Framework には追加されません。

今後、.NET Standard ならびに .NET には重要な新機能、たとえば新たに定義されたプロトコルである HTTP/3 や Windows 11 の関連機能、新たな入力デバイスに対応する機能といったものが追加されると予想されますが、.NET Framework ではそれらの機能は利用できません。

また、[2-2 (1)](#cannot-work-with-UI-designers) で述べたように、ASP.NET Web Forms は機能とデザインを完全に分離することができないため、最新の優れた UI/UX という点では、SPA (Single Page Application) 等には遠く及びません。

新しい技術は利用できず、UI/UX の点でも見劣りする .NET Framework で作成したアプリケーションでは、顧客に提供できる価値は低下します。

### 2-4. 企業価値が低下する ### {: #decrease-in-corporate-value}

ここまで、.NET Framework に留まり続けた場合には新しい機能・技術や 最新の UI/UX から取り残され、結果として顧客に提供する価値が低下するというリスクを説明しました。

顧客に提供できる価値が低下するということは、すなわち、企業としての価値の低下につながります。価値の低いアプリケーションしか提供できない企業に誰が価値を見出すでしょうか。

また、企業価値が低下するのは顧客にとってだけではありません。新しいことに挑戦しない企業は、優秀なエンジニアにとって魅力がないものに映るでしょう。

.NET Framework にいつまでも留まり続けることで、業界内だけでなく、就職・転職市場においても、企業の価値が低下することは避けられません。

### 2-5. 移行を急がなくても良いケース ### {: #cases-no-need-to-rush-the-migration}

ここまで、.NET Framework に留まり続けることのリスクについて解説しましたが、実のところ移行を急ぐ必要があまりないアプリケーションも存在します。

以下の条件を満たす、いわゆる「塩漬けシステム」は、現時点で移行を急ぐ必要はありません。

- インターネットに公開されていないオンプレミスのシステム
- 長く要件が変わらず、新規機能追加も行われていないシステム
- 同じシステムを5年以上、一切手を加えずに使い続ける前提である
- セキュリティリスクを受け入れることができる（または、他の方法で担保できる）

ただし、.NET Framework 3.5 SP1 以前を使用しているシステムは、上掲の条件をすべて満たしていたとしても移行を検討すべきです。.NET Framework 3.5 SP1 は OS のバージョンによらず、2029年1月にサポートが終了するためです。

???+ note "コラム：.NET Framework は塩漬けシステムに適していたのか？"

    2-5 ではいわゆる「塩漬けシステム」は移行を急ぐ必要は無いと記載しましたが、
    .NET Framework はそのようなシステムに適していたのでしょうか？

    実際には、.NET Framework は一度インストールすればバージョンを固定できるようなものではなく、
    サーバー OS の更新時には新しいバージョンへアップデートする必要があります。

    .NET Framework には強い後方互換性があるため、テストを再実施しなくても
    非互換が発生しないパターンが多かっただけといえるでしょう。

## 3. .NETへの移行 ## {: #migration-to-dotnet}

前章では、.NET Framework に留まり続けることのリスクについて説明しました。この章では、.NET Framework から .NET へ移行することになったときの方針やハードルについて説明します。

### 3-1. 基本的な方針 ### {: #basic-policy}

.NET Framework と .NET の間には単純互換性がないため、基本的にはコードを書き直す必要があります。
ただし、以下のアプリケーション形式については、Microsoft から提供される「[.NET Upgrade Assistant](https://docs.microsoft.com/ja-jp/dotnet/core/porting/upgrade-assistant-overview)」という変換ツールが使用できます。

- .NET Framework Windows フォーム アプリ
- .NET Framework WPF アプリ
- .NET Framework UWP アプリ
- .NET Framework ASP.NET MVC アプリ
- .NET Framework コンソール アプリ
- .NET Framework クラス ライブラリ

.NET Upgrade Assistant を使用すると、.NET Framework のソリューションやプロジェクトが .NET 形式へ変換されます。このツールを使用して変換した後、ビルドエラーや実行時エラーに手動で対応します。

ASP.NET Web Forms はこのツールに対応していないため、手動でコードを書き直す必要があります。

### 3-2. 移行のハードル ### {: #obstacles-to-migration}

#### (1) 書き換えが必要な .NET Framework の機能 #### {: #dotnetfw-function-to-rewrite}

以下の .NET Framework 機能は、.NET では廃止または非常に使用しづらいため、.NET の機能へ書き換える必要があります。

##### ASP.NET Web Forms（廃止） ##### {: #aspnet-web-forms}

- 移行先候補：ASP.NET MVC や Single Page Application (Vue.js, Blazor)
- 移行方法：移行ツールは存在せず、手作業でのソースコード書き換えが必要

!!! warning

    ただし、ASP.NET Web Forms でサードパーティ製品を使用している場合、同じ画面を再現できない可能性があります。

##### ASMX Web Services （廃止） ##### {: #asmx-web-services}

- 移行先候補：ASP.NET Web API
- 移行方法：移行ツールは存在せず、手作業でのソースコード書き換えが必要

!!! warning

    サーバーサイドの SOAP サービスを構築する手段は .NET にはありません。SOAP 以外のプロトコルを検討する必要があります。

##### DataSet / TableAdapter （廃止ではないが使いづらい） ##### {: #dataset-tableadapter}

- 移行先候補：Entity Framework Core
- 移行方法：移行ツールは存在せず、手作業でのソースコード書き換えが必要

??? note "その他、廃止された機能"

    他にも以下の .NET Framework のテクノロジー、機能、API が廃止されています。

    - アプリケーションドメイン
    - .NET Remoting
    - 透過的セキュリティ
    - COM+ (System.EnterpriseService)
    - コードアクセスセキュリティ (CAS)

    大半のケースでこれらの影響を受けることはありませんが、影響確認は必要です。

    なお、API レベルでの影響調査であれば、ツールを用いてある程度は自動的に実施可能です。

    - [.NET Portablity Analyzer](https://marketplace.visualstudio.com/items?itemName=ConnieYau.NETPortabilityAnalyzer)

#### (2) 短い製品ライフサイクルへの追従 #### {: #adapt-to-short-product-life-cycles}

.NET は、.NET Framework に対して製品ライフサイクルが短期化されています。具体的には、LTS 版であっても3年間のサポートしか提供されません。

つまり、.NET の場合、これまでの「納品までが仕事」というビジネスモデルは通用しません。

.NET へ移行する場合、DevOpsの導入など、継続的メンテナンスを行うことを前提としたビジネスモデルへの変革が必要となります。

## 4. まとめ ## {: #conclusion}

.NET 関連技術は、大きな変革の時期を迎えています。

- .NET Framework の開発停止
- Web Forms など過去の主要技術の終焉
- .NET Framework にとどまることのリスク増加
- .NET Framework / Core / Mono / Xamarin を統合した .NET

.NET Framework から .NET への移行を進めるかどうか、判断すべきです。
なお、コードベースが大きければ大きいほど、移行への道は重く／長くなります。
移行するかどうかに限らず、早期の方針検討・計画化が重要となります。
