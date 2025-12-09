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

ビューエンジンとして Blazor Server を利用します。
データアクセスには Entity Framework Core を利用します。

![サーバーサイドレンダリング アプリケーションスタック](../../images/app-architecture/overview/server-side-rendering-maris-light.png#only-light){ loading=lazy }
![サーバーサイドレンダリング アプリケーションスタック](../../images/app-architecture/overview/server-side-rendering-maris-dark.png#only-dark){ loading=lazy }

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

1. アプリケーションの設計手法の検討

    アプリケーションの設計手法には、大きく分けてドメインモデルとトランザクションスクリプトがあります。
    ドメインモデルは、複雑な業務を扱うための設計手法です。
    業務ロジックとデータを一体化し、事業活動を表現したオブジェクトを中心にシステムを構成します。
    トランザクションスクリプトは、簡単な業務を扱うための設計手法です。
    リクエストを処理する手順の中に、業務ロジックを埋め込みます。

1. アプリケーションの論理構造を定める

    アプリケーションの保守性を高めることを目的として、サブシステム内をさらに細かく層に構造化します。
    AlesInfiny Maris では、クリーンアーキテクチャを用いて構造化することを推奨します。

    - Web / Web API アプリケーション

      ```mermaid
      block
      columns 2
        presentation["プレゼンテーション層"]:2
        space:2
        infrastructure["インフラストラクチャ層"] tests["単体テスト"]
        space:2
        applicationCore["アプリケーションコア層"]:2
  
        presentation --> infrastructure
        infrastructure --> applicationCore
        tests --> applicationCore
      ```

    - コンソールアプリケーション

      ```mermaid
      block
      columns 2
        entryPoint["エントリーポイント"]:2
        space:2
        infrastructure["インフラストラクチャ層"] tests["単体テスト"]
        space:2
        applicationCore["アプリケーションコア層"]:2
  
        entryPoint --> infrastructure
        infrastructure --> applicationCore
        tests --> applicationCore
      ```

    各層の役割については、以下を参照してください。

    - [CSR アーキテクチャ概要 - アプリケーションアーキテクチャ](../client-side-rendering/csr-architecture-overview.md#application-architecture)
    - [SSR アーキテクチャ概要 - アプリケーションアーキテクチャ](../server-side-rendering/ssr-architecture-overview.md#application-architecture)

    アプリケーションの設計手法にあわせて、プロジェクトの構造を定めます。

    なお .NET のプロジェクト間には、循環参照[^1] の設定ができません。
    アプリケーションアーキテクチャを適切に表現できるプロジェクト構造を検討してください。

1. アプリケーションアーキテクチャをプロジェクト構造に落とし込む

    ここまでに定めた設計手法とアプリケーションの論理構造をもとに、プロジェクトの構造を定めます。
    プロジェクトは原則として業務 / 機能で分割してから、層で分割します。
    また設計手法にあわせて、プロジェクトの粒度を検討します。
    アプリケーションをマイクロサービス化しないのであれば、エントリーポイントのプロジェクトは 1 プロジェクトとすることを推奨します。

    ドメインモデルで設計を進める場合、複雑になりがちな業務ロジックを、うまくモデル化してアプリケーション内に表現することが重要です。
    そのため、業務ロジックの核となるアプリケーションコア層の独立性を保ち、業務ロジックの変更に耐えやすい形を整えます。
    一方トランザクションスクリプトで設計を進める場合、保守性より初期構築のコスト低減が重視されます。
    そのため、テスト容易性を担保しつつ、作成物の量ができる限り少なくなる構造をとります。
    Web アプリケーションにおけるプロジェクト分割の例を示します。

    - ドメインモデルを採用する場合

      ```mermaid
      block
      columns 4
        block:webProject:4
          columns 1
          webProjectLabel["Maris.Web.csproj"]
          presentation["プレゼンテーション層"]
        end
        space:4
        block:infrastructureFuncAProject
          columns 1
          funcAInfraProjectLabel["Maris.FuncA.Infrastructure.csproj"]
          infrastructureA["インフラストラクチャ層(FuncA)"]
        end
        block:testFuncAProject
          columns 1
          testFuncAProjectLabel["Maris.FuncA.UnitTests.csproj"]
          testsA["単体テスト(FuncA)"]
        end
        block:infrastructureFuncBProject
          columns 1
          funcBInfraProjectLabel["Maris.FuncB.Infrastructure.csproj"]
          infrastructureB["インフラストラクチャ層(FuncB)"]
        end
        block:testFuncBProject
          columns 1
          testFuncBProjectLabel["Maris.FuncB.UnitTests.csproj"]
          testsB["単体テスト(FuncB)"]
        end
        space:4
        block:applicationCoreFuncAProject:2
          columns 1
          funcAApplicationCoreProjectLabel["Maris.FuncA.ApplicationCore.csproj"]
          applicationCoreA["アプリケーションコア層(FuncA)"]
        end
        block:applicationCoreFuncBProject:2
          columns 1
          funcBApplicationCoreProjectLabel["Maris.FuncB.ApplicationCore.csproj"]
          applicationCoreB["アプリケーションコア層(FuncB)"]
        end

        webProject --> infrastructureFuncAProject
        webProject --> infrastructureFuncBProject
        infrastructureFuncAProject --> applicationCoreFuncAProject
        infrastructureFuncBProject --> applicationCoreFuncBProject
        testFuncAProject --> applicationCoreFuncAProject
        testFuncBProject --> applicationCoreFuncBProject

        classDef projectNameLabel stroke-width:0px,fill:transparent
        class webProjectLabel,funcAInfraProjectLabel,funcBInfraProjectLabel,testFuncAProjectLabel,testFuncBProjectLabel,funcAApplicationCoreProjectLabel,funcBApplicationCoreProjectLabel projectNameLabel
      ```

    - トランザクションスクリプトを採用する場合

      ```mermaid
      block
      columns 4
        block:testFuncAProject
          columns 1
          testFuncAProjectLabel["Maris.UnitTests.FuncA.csproj"]
          testsA["単体テスト(FuncA)"]
        end
        block:webProject:2
          columns 1
          webProjectLabel["Maris.Web.csproj"]
          presentation["プレゼンテーション層"]
        end
        block:testFuncBProject
          columns 1
          testFuncBProjectLabel["Maris.UnitTests.FuncB.csproj"]
          testsB["単体テスト(FuncB)"]
        end
        space:4
        block:FuncAProject:2
          columns 1
          funcAProjectLabel["Maris.FuncA.csproj"]
          infrastructureA["インフラストラクチャ層(FuncA)"]
          applicationCoreA["アプリケーションコア層(FuncA)"]
        end
        block:FuncBProject:2
          columns 1
          funcBProjectLabel["Maris.FuncB.csproj"]
          infrastructureB["インフラストラクチャ層(FuncB)"]
          applicationCoreB["アプリケーションコア層(FuncB)"]
        end

        webProject --> FuncAProject
        webProject --> FuncBProject
        testFuncAProject --> FuncAProject
        testFuncBProject --> FuncBProject

        classDef projectNameLabel stroke-width:0px,fill:transparent
        class testFuncAProjectLabel,testFuncBProjectLabel,webProjectLabel,funcAProjectLabel,funcBProjectLabel projectNameLabel
      ```

[^1]:
    プロジェクト同士がお互いに参照しあう構造のことを言います。
    .NET ではプロジェクト間の循環参照は許可されていないため、循環参照の発生しない構造を定義しなければなりません。
