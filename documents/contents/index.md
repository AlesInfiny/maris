---
title: AlesInfiny Maris OSS Edition に ようこそ
description: AlesInfiny Maris OSS Edition の概要を説明します。
hide:
  - navigation
---

# AlesInfiny Maris OSS Edition にようこそ {#top}

## AlesInfiny Maris OSS Edition とは {#what-is-alesinfiny-maris-version}

AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris）では、 .NET アプリケーションの一般的なアーキテクチャや方式設計のためのドキュメントおよびサンプルアプリケーションを提供します。
AlesInfiny Maris の提供物は以下の通りです。

- システム形態別の標準的なアプリケーションアーキテクチャ
- 利用頻度の高い有用な OSS ライブラリ／フレームワークを基盤としたサンプルアプリケーションおよびその解説
- アプリケーション開発環境／プロジェクト初期構築までのチュートリアル
- 実現したい要件別のサンプルコード、および実現方式の解説

AlesInfiny Maris は、クリエイティブ・コモンズ表示 4.0 国際ライセンスおよび Apache License, Version 2.0 にて使用可能です。
商用のエンタープライズシステム開発から個人開発の小規模なシステムまで、ライセンスの条項に従う限り、個人、商用とも無料で使用できます。
ライセンスについての詳細は「[利用規約 - ライセンス](about-maris/terms.md#license)」を参照してください。

## クイックスタート {#quick-start}

AlesInfiny Maris で構築した Web アプリケーションのサンプルを手元で動かしながら確認いただけます。

1. 以下を参照し、開発環境を構築してください

    - 「[ローカル開発環境の構築](guidebooks/how-to-develop/local-environment/index.md)」

1. 以下のリンクから、サンプルアプリケーションをダウンロードしてください。

    - 「[サンプルアプリケーションのダウンロード](samples/downloads/dressca.zip)」

1. ダウンロードした zip ファイルのプロパティを開き、ファイルへのアクセスを許可 ( ブロックを解除 ) してから、任意のフォルダーに展開してください。
   以降の手順では、「dressca」フォルダーに展開したものとして解説します。

    !!! info "展開先のフォルダーについて"
        展開先のフォルダーは、浅い階層にすることを推奨します。

1. フロントエンドアプリケーションを実行するためのモジュールを取得します。
   コマンドプロンプトを開き、「dressca\\dressca-frontend」に移動して以下のコマンドを実行します。

    ```winbatch title="フロントエンドアプリケーションの実行に必要なパッケージのインストール"
    npm ci
    ```

    !!! info "npm ci が失敗した場合"
        `npm ci` の途中でエラーや脆弱性情報以外の警告が出た場合、インストールに失敗している可能性があります。
        その場合は、「dressca\\dressca-frontend\\node_modules」、
        「dressca\\dressca-frontend\\consumer\\node_modules」フォルダーをそれぞれ削除し、再度 `npm ci` を実行してください。

1. Visual Studio で「dressca\\dressca-backend\\Dressca.sln」を開き、ソリューションをビルドします。

1. データベースを構築します。
   コマンドプロンプトを開き、「dressca\\dressca-backend\\src\\Dressca.EfInfrastructure」に移動して以下のコマンドを実行します。

    ```winbatch title="SQL Server のデータベース構築"
    dotnet ef database update
    ```

1. Visual Studio で実行するプロジェクトを選択します。
   ソリューションのプロパティを開き、 [Dressca.Web.Consumer] プロジェクトをスタートアッププロジェクトに設定します。

    [![Dressca.Web.Consumer プロジェクトをスタートアッププロジェクトに設定](images/select-startup-project.png){ width="600" loading=lazy }](images/select-startup-project.png)

1. Visual Studio で ++ctrl+f5++ を押下し、アプリケーションを実行します。
   いくつかプロンプト画面が立ち上がった後、ブラウザーが起動し、アプリケーションの実行が開始します。

    [![Dressca トップページ](images/dressca-top.png){ width="600" loading=lazy }](images/dressca-top.png)
