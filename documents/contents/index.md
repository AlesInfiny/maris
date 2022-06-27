---
hide:
  - navigation
---

# Maris OSS 版にようこそ

## Maris OSS 版とは ## { #what-is-maris-oss-version }

Maris OSS 版は、 .NET アプリケーションの基本アーキテクチャを実装したサンプルアプリケーションと、その開発ガイドで構成されています。
エンタープライズシステムに求められるアプリケーション構造を誰でも無償で手に入れることができます。
Maris OSS 版の提供物は以下の通りです。

- システム形態別の標準的なアプリケーションアーキテクチャ
- 利用頻度の高い有用な OSS ライブラリ／フレームワークをベースとしたサンプルアプリケーションとその解説
- アプリケーション開発環境／プロジェクト初期構築までのチュートリアル
- 実現したい要件別のサンプルコード、および実現方式の解説

Maris OSS 版は、商用のエンタープライズシステム開発から個人開発の小規模なシステムまで、 Apache License, Version 2.0 にて無償で利用可能です。

## クイックスタート ## { #quick-start }

Maris OSS 版で構築した Web アプリケーションのサンプルを手元で動かしながら確認いただけます。

1. 以下を参照し、開発環境の構築を行ってください

    - 「[ローカル開発環境の構築](guidebooks/how-to-develop/local-environment/index.md)」

1. 以下のリンクから、サンプルアプリケーションをダウンロードしてください。

    - 「[サンプルアプリケーションのダウンロード](samples/downloads/dressca.zip)」

1. ダウンロードした zip ファイルのプロパティを開き、ファイルへのアクセスを許可 ( ブロックを解除 ) してから、任意のディレクトリに展開してください。
   以降の手順では、「dressca」ディレクトリに展開したものとして解説します。

    !!! info "展開先のディレクトリについて"
        展開先のディレクトリは、浅い階層にすることを推奨します。

1. フロントエンドのアプリケーションを実行するためのモジュールを取得します。
   コマンドプロンプトを開き、「dressca\\dressca-frontend」に移動して以下のコマンドを実行します。

    ```winbatch title="フロントエンドアプリケーションの実行に必要なパッケージのインストール"
    npm install
    ```

    !!! info "npm install が失敗した場合"
        `npm install` の途中でエラーや脆弱性情報以外の警告が出た場合、インストールに失敗している可能性があります。
        その場合は、「dressca\\dressca-frontend\\node_modules」ディレクトリを削除し、再度 `npm install` を実行してください。

1. Visual Studio で「dressca\\dressca-backend\\Dressca.sln」を開き、ソリューションをビルドします。

1. データベースの構築を行います。
   コマンドプロンプトを開き、「dressca\\dressca-backend\\src\\Dressca.EfInfrastructure」に移動して以下のコマンドを実行します。

    ```winbatch title="SQL Server のデータベース構築"
    dotnet ef database update
    ```

1. Visual Studio で実行するプロジェクトを選択します。
   ソリューションのプロパティを開き、 [Dressca.Web] プロジェクトをスタートアッププロジェクトに設定します。

    [![Dressca.Web プロジェクトをスタートアッププロジェクトに設定](images/select-startup-project.png){ width="600" loading=lazy }](images/select-startup-project.png)

1. Visual Studio で ++ctrl+f5++ を押下し、アプリケーションを実行します。
   いくつかプロンプト画面が立ち上がった後、ブラウザーが起動し、アプリケーションの実行が開始します。

    [![Dressca トップページ](images/dressca-top.png){ width="600" loading=lazy }](images/dressca-top.png)
