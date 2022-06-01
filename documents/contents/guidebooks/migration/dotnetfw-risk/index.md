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
    - .NET 5 で GUI の開発に強みを持つ Mono / Xamarin を統合

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

2019年5月、Microsoft は [.NET Framework の開発停止を宣言](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/)しました。

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

## 2. .NET Frameworkの開発停止によるリスク

1章で述べたとおり、.NET Framework の開発はすでに停止しているため、これから始まる開発プロジェクトにおいては、.NET が第一の選択肢となります。
他方、.NET Framework で構築済みの既存のシステムについては、2つの選択肢――.NET へ移行するか、.NET Framework に留まり続けるか――が存在します。

- 移行する：移行パスを考え、コストをかけてリライトする
- 移行しない：リスクを受け入れる

2章では、.NET Framework に留まり続けることを選択した場合、どのような困難が発生するかについて説明します。

## 2-2. 開発を進めることが困難になる

新機能が追加されないとしても .NET Framework 自体は存在しているので、一見開発はできるように思えるかもしれません。しかし、次に述べる理由により、開発は困難となるでしょう。

### (1) 有益な OSS 製品を採用できない

RESTful API を構築するためのフレームワークである Swagger の C# ライブラリ「[Swashbuckle.AspNetCore.Swagger](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Swagger/)」は、.NET Framework 向けには提供されていません。

今後、.NET 向けの 新しい有益な OSS 製品が出たとしても、それらが .NET Framework をサポートする可能性は低いでしょうし、すでに存在する OSS 製品の新しいバージョンが .NET Framework をサポートしなくなる可能性もあります。

### (2) 多くのサードパーティ製品が EOL となり使用できない

Grapecty 社の製品である [InputMan](https://www.grapecity.co.jp/developer/inputmanplus-winforms) の最新版は、.NET Framework 向けには .NET Framework 4.8 版のみ提供されています。また、Infragistics 社の製品である [Ignite UI](https://jp.infragistics.com/support/supported-environments) の最新版は、すでに .NET Framework 3.5 SP1 のサポートを停止しています。

サードパーティ製品は、.NET Framework を広くサポートすることをやめつつあります。
プロジェクトが本番稼働を迎えた直後に使用している製品が EOL を迎えてしまうということも起こりえます。

### (3)ランタイムの進化に追従できない

暗号の利用モードの1つである AEM-GCM を C# で使用できる [System.Security.Cryptography.AesGcm クラス](https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.aesgcm?view=net-6.0)は、.NET Standard 2.1 で追加されたため、.NET Standard 2.0の実装である .NET Framework 4.8 では使用することができません。

このように、すでに .NET Framework は .NET に新しく追加される API からは取り残されつつあります。

### (4) 新しい開発環境が使えない

2022年6月現在最新の Visual Studio である Visual Studio 2022 では、.NET Framework 4.0 から 4.5.1 までの開発がサポートされないため、これらのバージョンをターゲットとしたアプリケーションをビルドすることができません。

今後、IDE でビルドできる .NET Framework のバージョンは減っていくことが考えられます。

### (5) 言語バージョンが更新されないため、モダンなコーディングができない

.NET Framework 4.8 に対応する C# のバージョンは 7.0 です。.NET Framework は 4.8 が最後のバージョンであるため、C# バージョンは 7.0 に固定され、C# 8.0 以降追加されたコード記法は使用できません。

たとえば、WEB 等で見つけたコード例の記法をそのままでは利用できない可能性があります。

### (6) 実行環境が制限される

2022年6月現在、[Azure Web Apps](https://azure.microsoft.com/ja-jp/get-started/web-app/) では、.NET Framework アプリケーションの稼働環境として .NET Framework 3.5 または 4.8 のみ選択可能です（※）。また、Microsoft の公式 docker イメージである [.NET Framework Runtime](https://hub.docker.com/_/microsoft-dotnet-framework-runtime/) も、バージョンとして選択できるのは .NET Framework 3.5 と 4.8 のみです。

このように、.NET Framework はクラウド環境やコンテナー環境から徐々に締め出されつつあります。将来的に、クラウド環境やコンテナー環境で .NET Framework が使用できなくなる可能性は高いです。

※ [Microsoft のクイックスタートガイド](https://docs.microsoft.com/ja-jp/azure/app-service/quickstart-dotnetcore?tabs=netframework48&pivots=development-environment-vs) では、.NET Framework 3.5 が選択できることすら記述されていません。

## 2-3. 開発体制を構築することが困難になる

.NET Framework に留まり続けた場合、開発自体が困難になるだけでなく、開発体制を構築することも困難になります。ここでは、その理由について説明します。

### (1) UI デザイナーとの協業ができない

.NET Framework の代表的な Web アプリケーションである ASP.NET Web Forms は、かつてその開発生産性の高さが評価されていましたが、機能とデザインを完全には分離することができないという欠点があります。

開発者はアプリケーションとしての機能の開発に注力し、UI デザイナーが見栄えの部分を担うという、モダンアプリケーションでは一般的な開発手法は、ASP.NET Web Forms では使用できません。

### (2) .NET Framework に精通した人員確保が困難になる

Youtube 上には [.NET の公式チャンネル](https://www.youtube.com/c/dotNET) が存在しますが、.NET Framework のチャンネルは存在しません。

最新の機能や技術はすべて .NET に追加されるため、勉強熱心な技術者は .NET Framework ではなく .NET を学習するようになるでしょう。また、技術書や研修プログラムといった学習のツールも、新しく提供されるものは .NET Framework ではなく .NET 向けのコンテンツとなります。

これらのことから、社内外を問わず .NET Framework コミュニティーは縮小していきます。それに伴って、.NET Framework に詳しい、特に若手の人材は今後減っていでしょう。開発プロジェクトにおいて、いわゆる .NET Framework の有識者を確保することは困難となります。

## 2-4. 顧客提供価値が低下する

## 2-5. 企業価値が低下する

## 2-6. 移行を急がなくても良いケース

## コラム「.NET Framework は塩漬けに適していたのか？」

## 3. .NETへの移行

### 3-1. 基本的な方針

### 3-2. 移行のハードル

## 4. まとめ
