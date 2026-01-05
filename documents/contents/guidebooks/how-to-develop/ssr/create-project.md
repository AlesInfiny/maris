---
title: プロジェクトの作成 （SSR 編）
description: SSR アプリケーション開発における プロジェクトの作成方法を解説します。
---

# プロジェクトの作成 {#top}

<!-- cSpell:ignore fluentblazor -->

## プロジェクトテンプレートの選択 {#select-project-template}

開発するプログラムの種類に応じて、適切なプロジェクトテンプレートを選択しましょう。
以下、利用することの多いプロジェクトテンプレートについて解説します。

### クラスライブラリ {#class-library}

[クラスライブラリ](../csr/dotnet/create-project.md#class-library) を参照してください。

### Blazor Web アプリ {#blazor-web-app}

以下の用途で利用します。

- サーバー側で HTML をレンダリングする Web アプリケーション

??? info "【参考】 .NET CLI を用いて Blazor Web アプリ プロジェクトを作成する方法"
    プロジェクトを作成するフォルダーに移動して、以下のコマンドを利用します。
    プロジェクト名はフォルダー名と同名になります。

    ```shell title="対話型 SSR を使用した Blazor Web アプリプロジェクトの作成コマンド"
    dotnet new blazor
    ```

### Fluent Blazor Web アプリ {#fluent-blazor-web-app}

Blazor Web アプリの利用用途に加えて、以下の用途で利用します。

- UI コンポーネントとして [Fluent UI Blazor :material-open-in-new:](https://www.fluentui-blazor.net/){ target=_blank } コンポーネントを用いた Web アプリケーション

??? info "プロジェクトを作る前に必要な作業"
    Fluent Blazor Web アプリのプロジェクトを作成するには、事前にプロジェクトテンプレートをインストールする必要があります。
    以下のコマンドで [Microsoft.FluentUI.AspNetCore.Templates :material-open-in-new:](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates/){ target=_blank } をインストールできます。

    ```shell title="プロジェクトテンプレートのインストールコマンド"
    dotnet new install Microsoft.FluentUI.AspNetCore.Templates
    ```
<!-- textlint-disable @textlint-ja/no-synonyms -->
??? info "【参考】 .NET CLI を用いて Fluent Blazor Web アプリのプロジェクトを作成する方法"
    プロジェクトを作成するフォルダーに移動して、以下のコマンドを利用します。
    プロジェクト名はフォルダー名と同名になります。

    ```shell title="対話型 SSR を使用した Fluent Blazor Web アプリプロジェクトの作成コマンド"
    dotnet new fluentblazor
    ```
<!-- textlint-enable @textlint-ja/no-synonyms -->

### xUnit v3 テストプロジェクト {#xunit}

[xUnit v3 テストプロジェクト](../csr/dotnet/create-project.md#xunit) を参照してください。

## プロジェクト作成時の追加情報の設定 {#project-additional-information}

<!-- textlint-disable @textlint-ja/no-synonyms -->

Visual Studio で Blazor Web アプリおよび Fluent Blazor Web アプリプロジェクトを作成する場合、追加情報の設定を求められます。
AlesInfiny Maris では以下のように設定しています。

- フレームワーク
  
    その時点で最新の長期的サポートバージョンを選択

- 認証の種類

    「なし」を選択

- HTTPS 用の構成

    チェックする

- 対話型レンダリングモード

    「サーバー」を選択

    SSR アプリケーションの開発では原則として「サーバー」を選択します。

- インタラクティビティの場所

    「グローバル」を選択

    SSR アプリケーションでは原則として「グローバル」を選択します。

- その他のオプション

    「サンプルページを含める」にチェック

    ヘッダー・フッター・ナビゲーション・サンプルページが追加されます。
    画面の構造を細かく制御したい場合はチェックを外してください。

<!-- textlint-enable @textlint-ja/no-synonyms -->

## プロジェクト間の依存関係の設定 {#configure-project-reference}

<!-- textlint-disable @textlint-ja/no-synonyms -->
各プロジェクトを作成後、アーキテクチャに従ってプロジェクト間の依存関係を設定します。
SSR アプリケーションのプロジェクトの依存関係の設定例は、 [SSR アーキテクチャ概要 - アプリケーションアーキテクチャ](../../../app-architecture/server-side-rendering/ssr-architecture-overview.md#application-architecture) を参照してください。

具体的な設定方法については、[プロジェクト内の参照を管理する :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/ide/managing-references-in-a-project){ target=_blank } を参照してください。
<!-- textlint-enable @textlint-ja/no-synonyms -->
