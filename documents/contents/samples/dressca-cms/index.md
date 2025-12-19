---
title: サンプル解説
description: AlesInfiny Maris OSS Edition の提供するサンプルの 解説ドキュメント集。
---

# Dressca-CMS {#top}

## 概要 {#overview}

## クイックスタート {#quick-start}

1. 以下を参照し、開発環境を構築してください。

    - 「[ローカル開発環境の構築](../../guidebooks/how-to-develop/ssr/local-development.md)」

1. 以下のリンクから、サンプルアプリケーションをダウンロードしてください。

    - 「[サンプルアプリケーションのダウンロード](../downloads/dressca-cms.zip)」

1. ダウンロードした zip ファイルのプロパティを開き、ファイルへのアクセスを許可 ( ブロックを解除 ) してから、任意のフォルダーに展開してください。
   以降の手順では、「dressca-cms」フォルダーに展開したものとして解説します。

    !!! info "展開先のフォルダーについて"
        展開先のフォルダーは、浅い階層にすることを推奨します。

1. Visual Studio で「dressca-cms\\DresscaCMS.slnx」を開き、ソリューションをビルドします。

1. データベースを構築します。
   コマンドプロンプトを開き、「dressca-cms」に移動して以下のコマンドを実行します。

    ```powershell linenums="0" title="SQL Server のデータベース構築"
    dotnet ef database update --projext .\src\DresscaCMS.Announcement\
    dotnet ef database update --project .\src\DresscaCMS.Authentication\
    ```

1. Visual Studio で実行するプロジェクトを選択します。
   ソリューションのプロパティを開き、 [DresscaCMS.Web] プロジェクトをスタートアッププロジェクトに設定します。

1. Visual Studio で ++ctrl+f5++ を押下し、アプリケーションを実行します。
   いくつかプロンプト画面が立ち上がった後、ブラウザーが起動し、アプリケーションの実行が開始します。
