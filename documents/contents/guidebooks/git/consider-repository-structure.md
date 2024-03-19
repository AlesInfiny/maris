---
title: Git 構築ガイド
description: Git リポジトリの構築に関するガイドラインを示します。
---

# リポジトリ構造の検討 {#top}

Git リポジトリの管理方式として、「mono-repo」と「poly-repo ( multi-repo ) 」の 2 種類があります。
本章では、リポジトリの各管理方法とメリットについて簡単に解説します。

## mono-repo とは {#about-mono-repo}

![mono-repo の構造例](../../images/guidebooks/git/mono-repo-structure-light.png#only-light){ align=right loading=lazy }
![mono-repo の構造例](../../images/guidebooks/git/mono-repo-structure-dark.png#only-dark){ align=right loading=lazy }

すべてのソースコードを単一の Git リポジトリで管理するパターンを「mono-repo」と呼びます。
様々なアプリケーションのソースコードをサブフォルダーに分割して、 1 つの Git リポジトリに格納します。

mono-repo とはいえ、すべてのソースコードを 1 つのリポジトリに含める必要はありません。
全く異なるシステムであれば、システム単位でリポジトリを分割します。

リポジトリのルートフォルダーには、サブシステム単位でフォルダーを作成して整理します。
サブシステム間で共用するライブラリも、同じリポジトリ内に配置して管理します。

## poly-repo とは {#about-poly-repo}

![poly-repo の構造例](../../images/guidebooks/git/poly-repo-structure-light.png#only-light){ align=right loading=lazy }
![poly-repo の構造例](../../images/guidebooks/git/poly-repo-structure-dark.png#only-dark){ align=right loading=lazy }

ソースコードを複数の Git リポジトリで管理するパターンを「poly-repo」と呼びます。
リポジトリの作成単位は特に定められていませんが、多くの場合リリースをかける単位でリポジトリを分割します。
マイクロサービスアーキテクチャのシステムであれば、各サービスをバラバラのリポジトリに格納します。

リポジトリ間で共有するライブラリがある場合も、他の機能同様、別のリポジトリとして管理します。
共有ライブラリは、通常パッケージ管理システムを通して、他のリポジトリに取り込みます。

## リポジトリ管理方法の比較 {#repo-management-method-comparison}

mono-repo のメリットは以下の通りです。

- サブシステム間のリソースの共有や、横断した変更が容易
- サブシステムを横断したテストが容易
- 全体の把握が容易

対して、 poly-repo のメリットは以下の通りです。

- 開発者各自の見るリポジトリが限定されるため、開発効率が向上
- リポジトリの肥大化を防止できる
- リポジトリ間で異なるテクノロジーを容易に適用できる

AlesInfiny Maris OSS Edition では、 mono-repo の構成を推奨します。
ただし、 Git リポジトリホスティングサービスの CI / CD 機能が、サブフォルダー単位で変更を検知でき、変更のあったサブシステムのビルドパイプラインのみ実行できることが前提です。
また、 1 つ 1 つのサブシステムが非常に大きく、かつそれぞのれサブシステムの独立性が極めて高い場合は、サブシステム単位でリポジトリ分割することも検討してください。
