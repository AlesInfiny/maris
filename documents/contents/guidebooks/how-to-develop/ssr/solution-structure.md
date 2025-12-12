---
title: ソリューション構造 の作成 （SSR 編）
description: SSR アプリケーション開発における ソリューション構造の作成方法を解説します。
---

# ソリューション構造の作成 {#top}

ソリューション構造の作成方針および作成方法は、 アプリケーションの形態が CSR であるか SSR によって変わりません。
そのため、 [CSR編 > ソリューション構造の作成](../csr/dotnet/create-solution-structure.md) と同様に、ソリューション構造を作成します。

## ソリューションの作成 {#create-solution}

下記の手順に従い、 Git リポジトリ、ソリューションファイル、物理フォルダーを作成します。

1. [Git リポジトリの作成](../csr/dotnet/create-solution-structure.md#create-git-repository)
1. [ソリューションファイルの作成](../csr/dotnet/create-solution-structure.md#create-solution-file)
1. [ソリューションフォルダーと物理フォルダーの作成](../csr/dotnet/create-solution-structure.md#create-solution-folder-and-physical-folder)

## プロジェクト配置の設計 {#design-project-placement}

[プロジェクト配置の定義](../csr/dotnet/create-solution-structure.md#define-project) に従って、 src フォルダー、 tests フォルダーへ配置するプロジェクトの命名を設計します。

複数のアプリケーションを含む場合や、プロジェクトの数が多くなりすぎた場合は、単一のソリューションで開発を進めることが困難になります。
2 つ以上のアプリケーションを含むソリューションや、プロジェクトの総数が 30 以上のソリューションでは、[ソリューションフィルターの活用](../csr/dotnet/create-solution-structure.md#utilize-solution-filter) を参照し、ソリューションフィルターを導入することを検討してください。

## ソリューションへのプロジェクトの追加 {#add-projects-to-solution}

プロジェクトの命名と配置場所が決まったら、ソリューション内に必要なプロジェクトを追加します。
具体的に追加するプロジェクトの種類と作成方法については [プロジェクトの作成](./create-project.md) で説明します。
