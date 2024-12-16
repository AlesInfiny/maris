---
title: 概要編
description: AlesInfiny Maris OSS Edition を利用することで構築できる アプリケーションの概要を説明します。
---

# 構成管理 {#top}

## ソースコード管理 {#source-code-management}

AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）では、ソースコードの管理に Git を推奨しています。 Git を採用するメリットは以下の通りです。

- 多くの ALM ツールが Git に対応しているため、連携が容易
- 分散型バージョン管理ツールのため、ローカル環境に中央リポジトリのクローンを作成しオフラインでも作業が可能

### ブランチ戦略 {#branch-strategy}

<!-- textlint-disable ja-technical-writing/sentence-length -->

ブランチ運用方針は、プロジェクトの規模やチームの体制によって多種多様です。そのため適したブランチ運用をすることが、開発効率やコードの品質、可用性を高めることにつながります。
代表的なブランチ戦略として、[git-flow :material-open-in-new:](https://nvie.com/posts/a-successful-git-branching-model/){ target=_blank }
 、[GitHub Flow :material-open-in-new:](https://docs.github.com/ja/get-started/quickstart/github-flow){ target=_blank }などが挙げられます。
これらをベースとして、以下のような視点でプロジェクトに適したブランチ戦略を定めましょう。

<!-- textlint-enable ja-technical-writing/sentence-length -->

- 機能開発による差分管理の要否

    誰がどの機能に対していつどのように修正したかの整理や差分が必要な場合は、開発する機能ごとにブランチを作成します。 ALM ツールを採用している場合、このブランチにチケットを紐づけることで、修正が必要になった原因や背景を詳細に記録できます。

- リリースバージョンの管理

    アプリケーションの形態やリリース頻度によって、リリースバージョンの管理方法を定めます。  

    - 結合・システムテストが完了している安定したバージョンをリリースする

        リリースするバージョン単位で develop ブランチを作成し開発・修正内容をマージします。 develop ブランチでの結合・システムテストの完了後に main ブランチへマージすることで常に main ブランチを安定した状態で保つことができます。

        ```mermaid
        gitGraph
            commit
            branch developA
            checkout developA
            commit
            branch featureA
            branch featureB
            checkout featureA
            commit
            commit
            checkout developA
            merge featureA
            checkout featureB
            commit
            checkout developA
            merge featureB
            checkout main
            merge developA
            commit id: "Release" tag: "v1.0.0"
        ```

        develop ブランチを作成しない方針の場合、システムテスト完了後の main ブランチからリリースブランチを作成することで、安定したリリース版として保つことが可能です。この方法では各リリースバージョンをそれぞれ管理できます。

        ```mermaid
        gitGraph
            commit
            commit
            branch featureA
            branch featureB
            checkout featureA
            commit
            commit
            checkout main
            merge featureA
            checkout featureB
            commit
            checkout main
            merge featureB
            commit type: HIGHLIGHT
            branch releaseA
            checkout releaseA
            commit
            checkout main
            commit
        ```

        常に最新のプログラムのみをリリースする場合は、リリースブランチにシステムテスト完了後の main ブランチを統合する方法も考えられます。

        ```mermaid
        gitGraph
            commit
            branch release
            checkout main
            commit
            branch developA
            commit
            checkout main
            merge developA
            commit
            checkout release
            merge main
            commit type: HIGHLIGHT id: "v1.0.0"
            checkout main
            commit
            branch developB
            commit
            checkout main
            merge developB
            commit
            checkout release
            merge main
            commit type: HIGHLIGHT id: "v2.0.0"
            checkout main
            commit
        ```

    - 常に main ブランチをリリースバージョンとする

        プログラムの安定性よりも、新機能の開発・既存機能の修正への対応が重視されておりリリース頻度が高い場合、 main ブランチと作業用のブランチで運用するシンプルなブランチ戦略を取ることも考えられます。この場合は、リリース時点での main ブランチの状態に対して、タグを付与してバージョン管理します。

        ```mermaid
        gitGraph
            commit
            commit type: HIGHLIGHT tag: "v0.9.0"
            branch featureA
            branch featureB
            checkout featureA
            commit
            commit
            checkout main
            merge featureA
            checkout featureB
            commit
            checkout main
            merge featureB
            commit type: HIGHLIGHT tag: "v1.0.0"
            branch featureC
            checkout featureC
            commit
            checkout main
            merge featureC
            commit type: HIGHLIGHT tag: "v1.0.1"
        ```

- main ブランチや develop ブランチの破壊的な変更への対策の要否

    main ブランチや develop ブランチへのマージには、開発者の意図に関わらず破壊的な変更が含まれる、という可能性があります。多くの ALM ツールにはこれを防止するために、マージする際に他のメンバーに承認を求めるプルリクエストを発行する機能があります。プルリクエストでは以下のような項目のブランチポリシーを設定し、コードの品質を高く保ちます。

    - レビュアーの数：リポジトリの管理者〇人
    - コードレビューのコメントがすべて解決済みになっている
    - ビルドマシンでコードのビルドができる
    - ビルドマシンで単体テストが全件通過する

### 改行コード {#line-break-code}

AlesInfiny Maris では、通常 Git のリモートリポジトリ内の改行コードが LF で統一されることから、ローカルリポジトリの改行コードも LF に統一する方針を採用しています。
詳細な設定方法は [Git の基本設定](../../guidebooks/git/git-settings.md#line-break-code) を参照してください。

## 推奨するリポジトリ構造 {#repository-structure}

1 つのシステムの中に複数のプロジェクトやパッケージが存在する場合、リポジトリ管理の方針を定めます。
単一のリポジトリで管理する mono-repo とそれぞれ個別のリポジトリで管理する poly-repo ( multi-repo ) という管理方法があります。

- mono-repo のメリット
    - プロジェクト間のリソースの共有、すなわち横断した変更が容易
    - プロジェクトを横断したテストが容易
    - システムの全体把握が容易
- poly-repo のメリット
    - 開発者自身が作業するリポジトリに集中できるため開発効率が向上  
    - リポジトリの肥大化の防止

AlesInfiny Maris では mono-repo 構造を推奨します。
マイクロサービス開発など、各プロジェクトの独立性が高く、プロジェクトごとに採用する技術要素が全く異なる場合は、 poly-repo を検討してください。
