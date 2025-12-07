---
title: 開発に使用する パッケージ （SSR 編）
description: SSR アプリケーション開発で使用する パッケージの一覧を解説します。
---

# 開発に使用するパッケージ {#top}

<!-- cSpell:ignore bunit -->

## ブランクプロジェクト作成時にインストールされるパッケージ {#packages-installed-on-blank-project}

以下のパッケージはブランクプロジェクト作成時にインストールされます。

- [Microsoft.FluentUI.AspNetCore.Components :material-open-in-new:](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components){ target=_blank }

    .NET のテストプロジェクトをビルドするためのパッケージです。[プロジェクトの作成 - Fluent Blazor Web アプリ](../csr/dotnet/create-project.md#xunit) でプロジェクトを作成すると、自動でインストールされます。

- [Microsoft.FluentUI.AspNetCore.Components.Icons:material-open-in-new:](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Icons){ target=_blank }

    Fluent UI で利用できるアイコン画像のパッケージです。[プロジェクトの作成 - Fluent Blazor Web アプリ](./create-project.md#fluent-blazor-web-app) でプロジェクトを作成すると、自動でインストールされます。

- [Microsoft.NET.Test.Sdk :material-open-in-new:](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk){ target=_blank }

    .NET のテストプロジェクトをビルドするためのパッケージです。[プロジェクトの作成 - xUnit v3 テストプロジェクト](../csr/dotnet/create-project.md#xunit) でプロジェクトを作成すると、自動でインストールされます。

- [xunit.v3 :material-open-in-new:](https://www.nuget.org/packages/xunit.v3/){ target=_blank }

    .NET で利用できるテストフレームワークです。[プロジェクトの作成 - xUnit v3 テストプロジェクト](../csr/dotnet/create-project.md#xunit) でプロジェクトを作成すると、自動でインストールされます。

なお、特定のパッケージをインストールすることで推移的にインストールされるパッケージは記載を省略しています。

!!! info "implicit-using"
    .NET 6 以降では、プロジェクトで使用する SDK の種類に応じて、自動的に `global using`が追加されます。
    たとえば `<Project Sdk="Microsoft.NET.Sdk.Web">` が宣言されている場合、`Microsoft.Extensions.DependencyInjection` が追加されます。
    よって、 明示的に都度 `using` を宣言することなく、 DI コンテナに関するライブラリを参照できます。
    詳細は [暗黙的な using ディレクティブ :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/project-sdk/overview#implicit-using-directives){ target=_blank }を参照してください。

## 追加でインストールするパッケージ {#additional-packages}

以下のパッケージは別途インストールが必要です。

- [Microsoft.EntityFrameworkCore :material-open-in-new:](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore){ target=_blank }

     .NET 環境で広く利用されている O/R マッパーです。

- [Microsoft.EntityFrameworkCore.Design :material-open-in-new:](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/){ target=_blank }

    Entity Framework Core でデータベースへの移行を管理するために必要なパッケージです。

- [Microsoft.EntityFrameworkCore.SqlServer :material-open-in-new:](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/){ target=_blank }

    Entity Framework Core を用いたアプリケーションにおいて、 データベースエンジンとして SQL Server を使用するためのパッケージです。

- [Microsoft.Testing.Extensions.CodeCoverage :material-open-in-new:](https://www.nuget.org/packages/Microsoft.Testing.Extensions.CodeCoverage/){ target=_blank }

    自動テスト結果のコードカバレッジを生成するためのパッケージです。

- [bunit :material-open-in-new:](https://www.nuget.org/packages/bunit/){ target=_blank }

    Blazor コンポーネントの自動テストを実行するためのパッケージです。 xUnit 上に構築でき、シームレスな統合が可能です。

- [Moq :material-open-in-new:](https://www.nuget.org/packages/Moq/){ target=_blank }

    .NET 環境で広く利用されている、自動テスト用のモック作成パッケージです。

- [StyleCop.Analyzers :material-open-in-new:](https://www.nuget.org/packages/StyleCop.Analyzers/){ target=_blank }

    C# のコードを解析し、コーディングルールに則っているかを確認するパッケージです。
