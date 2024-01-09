---
title: 概要編
description: AlesInfiny Maris のアプリケーションアーキテクチャ概要を解説します。
---

# 構成管理 {#top}

## ソースコード管理 {#source-code-management}

AlesInfiny Maris では、ソースコードの管理に Git を推奨しています。 Git を採用するメリットは以下の通りです。

- 多くの ALM ツールが Git に対応しているため、連携が容易
- 分散型バージョン管理ツールのため、ローカル環境に中央リポジトリのクローンを作成しオフラインでも作業が可能

<!--
### ブランチ戦略 {#branch-strategy}
-->

### 改行コード {#line-break-code}

AlesInfiny Maris では、通常 Git のリモートリポジトリ内の改行コードが LF で統一されることから、ローカルリポジトリの改行コードも LF に統一する方針を採用しています。
各ツール・エディターの設定を以下のようにします。

- Git

    Git はチェックアウトする時、テキストファイルの改行コードを CRLF に自動変換することがあります。
    以下のコマンドを実行し、チェックアウト時の CRLF への自動変換を無効化し、コミット時は LF へ自動変換するように設定します。

    ```bash
    git config --local core.autocrlf input
    ```

    ただしこれらの方法は Git の設定を開発者自身が行わなければいけないため、改行コードが開発者の環境によって混在する可能性があります。
    そのため .gitattributes を利用して、ソースコードの改行コードを強制的に LF に変換するように設定します。

    ```text title=".gitattributes"
    * text=auto eol=lf
    ```

- Visual Studio

    Visual Studio はデフォルトで改行コードを CRLF に変換します。
    これを無効化するために、プロジェクトに editorconfig を作成し `end_of_line = lf` を追加します。

    ```text title=".editorconfig"
    [*]
    end_of_line = lf
    ```

- Visual Studio Code

    プロジェクトで共通の設定をするため、 ワークスペースで設定します。 .code-workspace ファイルへ以下のように設定します。

    ```json title=".code-workspace"
    {
        settings: {
            "files.eol": "\n"
        }
    }
    ```

    またワークスペースを作成しない場合は、フォルダー単位で設定します。対象のフォルダーで .vscode/settings.json ファイルを作成し、以下のように設定します。

    ```json title=".vscode/settings.json"
    {
        "files.eol": "\n"
    }
    ```

- OpenAPI

    OpenAPI の NSwag で生成されるコードの改行コードは、デフォルトで CRLF です。
    そのため、改行コードを LF にするには、 nswag.json へ以下の設定を追加します。

    ```json title="nswag.json"
    {
        "documentGenerator": {
            "aspNetCoreToOpenApi": {
                "newLineBehavior": "LF"
            }
        }
    }
    ```

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
