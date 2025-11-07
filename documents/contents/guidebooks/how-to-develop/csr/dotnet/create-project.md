---
title: .NET 編（CSR 編）
description: CSR アプリケーションの サーバーサイドで動作する .NET アプリケーションの 開発手順を解説します。
---

# プロジェクトの作成 {#top}

<!-- cSpell:ignore classlib webapi -->

## プロジェクトテンプレートの選択 {#select-project-template}

開発するプログラムの種類に応じて、適切なプロジェクトテンプレートを選択しましょう。
以下、利用することの多いプロジェクトテンプレートについて解説します。

### クラスライブラリ {#class-library}

以下の用途で利用します。

- エントリーポイント以外のビジネスロジック ( アプリケーションコア層 ) やデータアクセス処理 ( インフラストラクチャ層 ) を提供するプロジェクト
- DTO を管理するプロジェクト
- システム共通処理を提供するプロジェクト

??? info "【参考】 .NET CLI を用いてクラスライブラリプロジェクトを作成する方法"
    プロジェクトを作成するフォルダーに移動して、以下のコマンドを利用します。
    プロジェクト名はフォルダー名と同名になります。

    ```shell title="クラスライブラリプロジェクトの作成コマンド"
    dotnet new classlib
    ```

### コンソールアプリ {#console-application}

以下の用途で利用します。

- バッチアプリケーションのエントリーポイントとなるプロジェクト

??? info "【参考】 .NET CLI を用いてコンソールアプリケーションプロジェクトを作成する方法"
    プロジェクトを作成するフォルダーに移動して、以下のコマンドを利用します。
    プロジェクト名はフォルダー名と同名になります。

    ```shell title="コンソールアプリケーションプロジェクトの作成コマンド"
    dotnet new console
    ```

### ASP.NET Core Web API {#web-api}

以下の用途で利用します。

- Web API だけを公開する Web アプリケーション

??? info "【参考】 .NET CLI を用いて ASP.NET Core Web API プロジェクトを作成する方法"
    プロジェクトを作成するフォルダーに移動して、以下のコマンドを利用します。
    プロジェクト名はフォルダー名と同名になります。

    ```shell title="ASP.NET Core Web API プロジェクトの作成コマンド"
    dotnet new webapi
    ```

### ASP.NET Core with Vite {#spa-app}

以下の用途で利用します。

- Vue.js などの JavaScript ベースのクライアントアプリケーションから接続する Web API を提供する Web アプリケーション ( SPA のバックエンドアプリケーション )

??? info "プロジェクトを作る前に必要な作業"
    ASP.NET Core with Vite のプロジェクトテンプレートは、 Visual Studio をインストールしただけでは利用できません。
    [JohannDev.DotNet.Web.Spa.ProjectTemplates :material-open-in-new:](https://www.nuget.org/packages/JohannDev.DotNet.Web.Spa.ProjectTemplates/){ target=_blank } の NuGet パッケージを事前にインストールしてください。

??? info "【参考】 .NET CLI を用いて ASP.NET Core with Vite プロジェクトを作成する方法"
    プロジェクトを作成するフォルダーに移動して、以下のコマンドを利用します。
    プロジェクト名はフォルダー名と同名になります。

    ```shell title="ASP.NET Core with Vite プロジェクトの作成コマンド"
    dotnet new vite
    ```

### xUnit v3 テストプロジェクト {#xunit}

以下の用途で利用します。

- 単体 / 結合 / E2E テストドライバー

??? info "プロジェクトを作る前に必要な作業"
    xUnit v3 の単体テストプロジェクトを作成するには、事前にプロジェクトテンプレートをインストールする必要があります。
    以下のコマンドで [xunit.v3.templates :material-open-in-new:](https://www.nuget.org/packages/xunit.v3.templates/){ target=_blank } をインストールできます。

    ```shell title="プロジェクトテンプレートのインストールコマンド"
    dotnet new install xunit.v3.templates
    ```

??? info "【参考】 .NET CLI を用いて xUnit v3 テストプロジェクトを作成する方法"
    プロジェクトを作成するフォルダーに移動して、以下のコマンドを利用します。
    プロジェクト名はフォルダー名と同名になります。

    ```shell title="xUnit v3 テストプロジェクトの作成コマンド"
    dotnet new xunit3
    ```

## プロジェクトの命名 {#project-naming-rule}

プロジェクト名は、原則としてプロジェクトのルートフォルダー名と同名にします。
一般的な命名規則は以下の通りです。

- *ソリューション名*.*業務名*.*層の名前*
- *ソリューション名*.*層の名前*

ソリューションの規模が大きい場合は、業務名や機能名をプロジェクト名に含めて、同一層のプロジェクトを複数配置するようにします。
業務や機能で分割するほどプロジェクトが大きくない場合は、プロジェクト名にそれらを含めなくてもかまいません。

??? example "規模の小さなソリューションにおける構成例"

    ![規模の小さなソリューションにおける構成](../../../../images/guidebooks/how-to-develop/csr/dotnet/project-structure-light.png#only-light){ loading=lazy }
    ![規模の小さなソリューションにおける構成](../../../../images/guidebooks/how-to-develop/csr/dotnet/project-structure-dark.png#only-dark){ loading=lazy }

    規模の小さいソリューションにおけるクリーンアーキテクチャに則ったプロジェクトの配置と命名例を示します。
    まず、物理的なフォルダー配置と、ソリューション内の配置を揃えることで、何がどこにあるかをすぐに判断できるよう整理しています。

    Infrastructure 層のプロジェクトは、実装の具体的な方式をプロジェクト名の末尾や接頭辞として付けることを推奨します。
    このように命名することで、各プロジェクト内に様々な実装テクノロジーが混在することを避け、適切な機能配置を実現できます。
    以下に例を示します。
    
    - AaaSubSystem.EFInfrastructure : Entity Framework Coreを使った Repository の実装 ( 別案 : AaaSubSystem.Infrastructure.EFCore )
    - AaaSubSystem.Store.StaticFiles : 静的ファイルを用いたストアの実装

    ASP.NET Core Web API のプロジェクトを作成する場合は、 Web API の入出力インターフェースを管理する DTO を集めたプロジェクトを作りましょう。
    今後 Web API の呼び出し側を .NET のテクノロジーで開発する場合、 DTO を抜き出しておくことでコード共有できるようになり、開発が便利に進められます。

## プロジェクト間の依存関係の設定 {#configure-project-reference}

<!-- textlint-disable @textlint-ja/no-synonyms -->
各プロジェクトを作成後、アーキテクチャに従ってプロジェクト間の依存関係を設定します。
アーキテクチャごとのプロジェクトの依存関係の設定例は、 [プロジェクトの単位](../../../../app-architecture/overview/application-structure.md#unit-of-project) を参照してください。

具体的な設定方法については、[プロジェクト内の参照を管理する :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/ide/managing-references-in-a-project){ target=_blank } を参照してください。
<!-- textlint-enable @textlint-ja/no-synonyms -->
