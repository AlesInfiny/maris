---
title: .NET 編
description: バックエンドで動作する .NET アプリケーションの 開発手順を解説します。
---

# ソリューション構造の作成 {#top}

## Git リポジトリの作成 {#create-git-repository}

まずは Git リポジトリを構築しましょう。
Git リポジトリの構築については、以下を参照してください。

- [Git リポジトリ構築ガイド](../../git/index.md)

## ソリューションの作成 {#create-solution}

作成した Git リポジトリ内に、ソリューションを作成していきます。
原則としてソリューションはサブシステムごとに作成します。
ソリューション全体構造の概要は「[アプリケーションアーキテクチャ概要編 - ソリューション構造](../../../app-architecture/overview/application-structure.md#solution-structure)」を参照してください。
本節では、単一のソリューション内の構造について、更に詳細に解説します。

### ソリューション構成と物理フォルダーの関係 {#solution-and-physical-folder-relationships}

Visual Studio でソリューションを開いたとき、ソリューションエクスプローラーに表示される構造と、物理的なフォルダーの構造は一致しないことがあります。
不一致を引き起こす最大の原因は、 Visual Studio の機能である「[ソリューションフォルダー :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/ide/solutions-and-projects-in-visual-studio#solution-folder){ target=_blank }」の存在です。

ソリューションフォルダーは、 Visual Studio のソリューションエクスプローラー内にのみ存在する仮想的なフォルダーです。
AlesInfiny Maris OSS Edition では、ソリューションフォルダーと物理的なフォルダーを完全に一致させて管理することを推奨します。
ソリューションエクスプローラー内の配置と、物理的な配置を一致させることで、ファイルの検索性が大きく向上します。

### ソリューションファイルの作成 {#create-solution-file}

バックエンドアプリケーションを配置するフォルダー内に、 Visual Studio を利用して [空のソリューションファイルを作成 :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/ide/creating-solutions-and-projects#create-empty-solutions){ target=_blank } します。
ソリューションファイルの名前は、以下のいずれかのパターンに従って命名します。

| #   | パターン                                   |
| --- | ------------------------------------------ |
| 1   | *会社名*.*システム名*.*サブシステム名*.sln |
| 2   | *システム名*.*サブシステム名*.sln          |
| 3   | *サブシステム名*.sln                       |

ソリューション名は、これから開発するシステムのルート名前空間と一致します。
そのため、変更の可能性が少なく、階層が少なくなるような命名を検討します。
システムの規模が大きくなることを見越す場合は、会社名やシステム名を含んだソリューション名にすることを推奨します。

ソリューション名が長くなると、ルート名前空間もあわせて長くなります。
ルート名前空間が長くなると、ソースコードの文字数が増え、視認性の低下や無駄な記述の増加を招きます。
会社名やシステム名、サブシステム名は、略称や開発コードネームを付与してもかまいません。
開発メンバーの共通認識の取れる適切な単語や略語があれば、それを利用して文字数が削減できないか検討してください。

### ソリューションフォルダーと物理フォルダーの作成 {#create-solution-folder-and-physical-folder}

![ソリューションの物理フォルダー構造](../../../images/guidebooks/how-to-develop/dotnet/solution-root-folders-light.png#only-light){ loading=lazy align=right }
![ソリューションの物理フォルダー構造](../../../images/guidebooks/how-to-develop/dotnet/solution-root-folders-dark.png#only-dark){ loading=lazy align=right }

ソリューション内は、本番環境に配置するソースコード ( 以降プロダクションコード ) と、テストに使用するソースコード ( 以降テストコード ) を分割して配置します。
まずソリューションファイルを配置したフォルダーに「src」フォルダーと「tests」フォルダーを作成します。
src フォルダーにはプロダクションコードを、 tests フォルダーにはテストコードを配置していきます。

この物理フォルダーと対応するように、 Visual Studio のソリューションエクスプローラー上で「src」ソリューションフォルダーと「tests」ソリューションフォルダーを作成します。
この後作成するプロジェクトやソースコードは、 src ソリューションフォルダーまたは tests ソリューションフォルダーに物理フォルダーと同じように配置します。

<!-- textlint-disable ja-technical-writing/sentence-length -->

!!! note "プロダクションコードとテストコードのフォルダーを最初に分割する理由"
    .NET アプリケーションの開発では、静的コード分析やコーディングルールの設定に [.editorconfig ファイル :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/ide/create-portable-custom-editor-options){ target=_blank } を用います。
    .editorconfig ファイルは、ファイルを配置したフォルダー以下すべてのファイルに対して、設定した値が影響を与えます。
    プロダクションコードとテストコードでは、通常コーディングルールの厳しさに濃淡をつけます[^1]。
    そのため、プロダクションコードを配置するフォルダーとテストコードを配置するフォルダーを最初に分類しておくことで、各コード向けの .editorconfig ファイルの適用が簡単になります。

<!-- textlint-enable ja-technical-writing/sentence-length -->

    またフォルダー構造に基づく設定は、他にもいくつか存在します。

    - [Directory.Build.props と Directory.Build.targets :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/msbuild/customize-your-build#directorybuildprops-and-directorybuildtargets){ target=_blank }
    - [Central Package Management :material-open-in-new:](https://devblogs.microsoft.com/nuget/introducing-central-package-management/){ target=_blank }

    プロダクションコードを配置するフォルダーとテストコードを配置するフォルダーの分割は、これらの機能を活用するためにも役立ちます。

## プロジェクト配置の定義 {#define-project}

src フォルダー、 tests フォルダーには、プロジェクトを格納するフォルダーをフラットに並べます。
プロジェクトを格納するフォルダーは、各プロジェクトのルート名前空間の名前を付けます。
これは通常プロジェクト名と一致します。

src フォルダーにどのようなプロジェクトを作成するかは、採用するアーキテクチャや、業務の構造・規模にあわせて柔軟に検討します。
詳細は「[アプリケーションアーキテクチャ概要編 - プロジェクトの単位](../../../app-architecture/overview/application-structure.md#unit-of-project)」を参照してください。
tests フォルダー内は、テストの目的にあわせてテストプロジェクトを分割しましょう。
単体機能確認、結合機能確認、性能確認などが、よくあるテストプロジェクトの分割単位です。

## ソリューションフィルターの活用 {#utilize-solution-filter}

複数のアプリケーションを含む場合や、プロジェクトの数が多くなりすぎた場合は、単一のソリューションで開発を進めることが困難になります。
その場合は、[ソリューションフィルター :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/msbuild/solution-filters#solution-filter-files){ target=_blank } を用いて業務や機能、スタートアッププロジェクトの単位でプロジェクトをフィルタリングします。
2 つ以上のアプリケーションを含む場合や、プロジェクトの総数が 30 以上のソリューションの場合は、ソリューションフィルターの活用を検討しましょう。
プロジェクトを業務単位で分割しておくことで、ソリューションフィルターが効果的に活用できます。
また、ソリューションフィルターを使用する場合は、単体機能確認のためのテストプロジェクトも業務単位で分割しましょう。

![ソリューションフィルター](../../../images/guidebooks/how-to-develop/dotnet/solution-filter-light.png#only-light){ loading=lazy }
![ソリューションフィルター](../../../images/guidebooks/how-to-develop/dotnet/solution-filter-dark.png#only-dark){ loading=lazy }

[^1]: プロダクションコードのみ XML コメントの記入を必須にする、といったルールの調整が良く行われます。
