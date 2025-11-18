---
title: 概要編
description: AlesInfiny Maris OSS Edition を利用することで構築できる アプリケーションの概要を説明します。
---

# アプリケーション構成 {#top}

AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）として、アプリケーション形態ごとに標準的なアプリケーション構成を定義しています。
ここでは主要な構成要素を示します。
<!-- （CSR編など、アプリケーション種別ごとのアーキテクチャ解説ができたら右記の文を差しこむ。）詳細はアプリケーション種別ごとの詳細ページ、および、サンプルアプリケーションを参照してください。 -->

## Web アプリケーション ( クライアントサイドレンダリング ) {#client-side-rendering}

Vue.js を用いた SPA の構成をとります。
サーバーサイドは .NET 10 以降の ASP.NET Core の Web API アプリケーションです。
データアクセスには Entity Framework Core を利用します。

![クライアントサイドレンダリング アプリケーションスタック](../../images/app-architecture/overview/client-side-rendering-maris-light.png#only-light){ loading=lazy }
![クライアントサイドレンダリング アプリケーションスタック](../../images/app-architecture/overview/client-side-rendering-maris-dark.png#only-dark){ loading=lazy }

!!! note ""

    上の図で使用している OSS 製品名およびロゴのクレジット情報は [こちら](../../about-maris/credits.md) を参照してください。

## Web アプリケーション ( サーバーサイドレンダリング ) {#server-side-rendering}

（今後追加予定）

## コンソールアプリケーション {#console-application}

コンソールアプリケーションでは、汎用ホスト [Microsoft.Extensions.Hosting :material-open-in-new:](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/){ target=_blank } を用いて構築します。データアクセスには Entity Framework Core を利用します。

サンプルのダウンロードおよび解説については、 [コンソールアプリケーションで DI を利用する](../../samples/console-app-with-di/index.md) を参照してください。

![コンソールアプリケーション アプリケーションスタック](../../images/app-architecture/overview/console-application-maris-light.png#only-light){ loading=lazy }
![コンソールアプリケーション アプリケーションスタック](../../images/app-architecture/overview/console-application-maris-dark.png#only-dark){ loading=lazy }

## ソリューションの構造 {#solution-structure}

### ソリューションの単位 {#unit-of-solution}

Web アプリケーションやコンソールアプリケーション ( バッチ ) など、 1 つのサブシステムは通常複数のアプリケーションで構成されます。
AlesInfiny Maris では、 1 サブシステム 1 ソリューションを基本として推奨します。
ただし、複数サブシステム ( 複数ソリューション ) で共用する共通機能を作成する場合は、必要に応じてソリューション分割を検討してください。

ビルド時間が長すぎる場合や、開発者の PC スペックがソリューションの大きさに耐えられない場合は、ソリューションフィルターの機能を活用しましょう。
1 ソリューションを保ちながら、プロジェクト単位でフィルター処理を行うことができます。

[Visual Studio のフィルター処理済みソリューション - ソリューション フィルター ファイル :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/ide/filtered-solutions#solution-filter-files){ target=_blank }

### プロジェクトの単位 {#unit-of-project}

プロジェクトは、原則として機能単位、層単位で分割することを推奨します。
プロジェクトの分割にあたっては、以下の手順で分割を検討してください。

1. 業務分割、機能分割

    サブシステムを業務的な観点で機能に分割します。

1. アプリケーションアーキテクチャの検討

    アプリケーションの内部構造をどのように整理していくか検討します。
    典型的なアプリケーションの内部構造には以下のような種類があります。

    - レイヤードアーキテクチャ
    - クリーンアーキテクチャ

    各アーキテクチャの詳細は、以下を参照してください。

    [一般的な Web アプリケーション アーキテクチャ :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures){ target=_blank }

1. アプリケーションアーキテクチャをプロジェクト構造に落とし込む

    定めたアプリケーションアーキテクチャをプロジェクト構造に落とし込みます。
    アプリケーションアーキテクチャの定める各コンポーネントをどのようにプロジェクトとして構成するか定めます。
    .NET のプロジェクト間には、循環参照[^1] の設定ができません。
    アプリケーションアーキテクチャを適切に表現できるプロジェクト構造を検討してください。

    最も基本的な構造は、以下の通りです。

    - クライアントサイドレンダリング

    ![代表的なアプリケーションアーキテクチャ](../../images/app-architecture/overview/application-architecture-light.png#only-light){ loading=lazy }
    ![代表的なアプリケーションアーキテクチャ](../../images/app-architecture/overview/application-architecture-dark.png#only-dark){ loading=lazy }

    - コンソールアプリケーション

    ![代表的なアプリケーションアーキテクチャ](../../images/app-architecture/overview/console-application-architecture-light.png#only-light){ loading=lazy }
    ![代表的なアプリケーションアーキテクチャ](../../images/app-architecture/overview/console-application-architecture-dark.png#only-dark){ loading=lazy }

1. 業務 / 機能をプロジェクト構造に当てはめる

    プロジェクトは原則として業務 / 機能で分割してから、層で分割します。
    アプリケーションをマイクロサービス化しないのであれば、エントリーポイントのプロジェクトは 1 プロジェクトとすることを推奨します。

    ![レイヤードアーキテクチャのプロジェクト分割例](../../images/app-architecture/overview/application-architecture-and-functions-light.png#only-light){ loading=lazy }
    ![レイヤードアーキテクチャのプロジェクト分割例](../../images/app-architecture/overview/application-architecture-and-functions-dark.png#only-dark){ loading=lazy }

[^1]:
    プロジェクト同士がお互いに参照しあう構造のことを言います。
    .NET ではプロジェクト間の循環参照は許可されていないため、循環参照の発生しない構造を定義しなければなりません。
