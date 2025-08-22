---
title: Git 構築ガイド
description: Git リポジトリの構築に関するガイドラインを示します。
---

# Git の基本設定 {#top}

本章では、 Git の基本設定に関するガイドラインを示します。

## 改行コード {#line-break-code}

AlesInfiny Maris OSS Edition では、通常 Git のリモートリポジトリ内の改行コードが LF で統一されることから、ローカルリポジトリの改行コードも LF に統一する方針を採用しています。
各ツール・エディターの設定を以下のようにします。

- Git

    Git はチェックアウトする時、テキストファイルの改行コードを CRLF に自動変換することがあります。
    以下のコマンドを実行し、チェックアウト時の CRLF への自動変換を無効化し、コミット時は LF へ自動変換するように設定します。

    ```shell
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
