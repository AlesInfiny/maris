---
title: 開発環境構築
description: AlesInfiny Maris OSS Edition のアプリケーション開発で 最低限必要な環境の構築方法を解説します。
---

# ローカル開発環境の構築 {#top}

AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）のアプリケーション開発で最低限必要な環境の構築方法を解説します。

## システム要件 {#system-requirements}

AlesInfiny Maris のアプリケーションを開発するコンピューターが満たすべき要件について解説します。
ここに記載のない環境でも開発できることがありますが、動作確認は行われていません。

### OS 要件 {#os-requirements}

- Windows 11

原則としてサポートされている最新の Windows クライアント OS を対象とします。

### ハードウェア要件 {#hardware-requirements}

- 1.8 GHz 以上、 4 コア以上の 64 ビットプロセッサ ( ARM プロセッサーは対象外です ) が必要です。
    - 8 論理コア ( ハイパースレッディングなど ) 以上を推奨します。
    - 仮想マシンの場合、 4 仮想プロセッサー以上が必要です。 8 仮想プロセッサー以上を推奨します。
- 8 GB 以上の RAM が必要です。 16 GB 以上を推奨します。
- 50 GB 以上の空き容量を持つ SSD が必要です。
- FullHD ( 1920 * 1080 ) 以上の解像度を持つディスプレイが必要です。

### ネットワーク要件 {#network-requirements}

- インターネット接続が必要です。

### ソフトウェア要件 {#software-requirements}

AlesInfiny Maris のアプリケーション開発には、 Visual Studio または VS Code を利用できます。
Vue.js を含む SPA アプリケーションを開発する場合は VS Code を使用します。
Web API アプリケーションやコンソールアプリケーションなど、 .NET アプリケーションを開発する場合は Visual Studio の利用を推奨します。

| 開発するアプリケーション | Visual Studio | VS Code |
| ------------------------ | ------------- | ------- |
| Vue.js アプリケーション  | ×             | 〇      |
| .NET アプリケーション    | 〇            | △ [^1]  |

!!! info "AlesInfiny Maris の前提とする開発環境"
    AlesInfiny Maris の提供する情報は、原則として上記推奨環境についてのみ取り扱います。

## ローカル開発環境の構築手順 {#setup-development-environment}

本節では開発に最低限必要なソフトウェアのインストール方法について解説します。
AlesInfiny Maris の各ドキュメントは、本節に記載されている環境が整っていることを前提に記載されています。

### Visual Studio のインストール {#install-visual-studio}

1. 以下のサイトから、 Visual Studio のインストーラーをダウンロードします。
   エディションは、 Visual Studio のライセンス条項を確認のうえ、ご自身の利用できるものを選択してください。
   バージョンは原則として製品版の最新バージョンを利用してください ( Preview 版は利用しないでください ) 。

    <https://visualstudio.microsoft.com/ja/vs/>

1. ダウンロードしたインストーラーを実行します。
1. ワークロードの選択画面で、 [ASP.NET と Web 開発] 、 [.NET デスクトップ開発] を選択してインストールします。
   これ以外に必要なものがあれば、適宜追加してインストールしてください。
1. Visual Studio が起動できればインストールは完了です。

### VS Code のインストール {#install-vscode}

1. [こちらのサイト :material-open-in-new:](https://code.visualstudio.com/){ target=_blank } から、コンピューターの環境にあった Visual Studio Code(以下 VS Code) のインストーラーをダウンロードします。

1. ダウンロードしたインストーラーを実行します。

1. オプション設定は、ご利用の環境に応じて設定してインストールしてください。

1. インストールが完了したら VS Code を起動します。

1. 以下の拡張機能をインストールします。

    - [Japanese Language Pack for Visual Studio Code :material-open-in-new:](https://marketplace.visualstudio.com/items?itemName=MS-CEINTL.vscode-language-pack-ja){ target=_blank }

        VS Code のユーザーインターフェースを日本語にローカライズする機能を提供します。

    - [EditorConfig for VS Code :material-open-in-new:](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig){ target=_blank }

        アプリケーションのコーディングスタイルを維持する機能を提供します。

    - [IntelliCode :material-open-in-new:](https://marketplace.visualstudio.com/items?itemName=VisualStudioExptTeam.vscodeintellicode){ target=_blank }

        TypeScript 等の入力補完機能を提供します。

    フロントエンドアプリケーションの開発に利用する VS Code の拡張機能は、以下を確認してください。

    - [フロントエンドアプリケーション開発の事前準備](../vue-js/preparation.md#install-extensions)

### Git for Windows のインストール {#install-git-for-windows}

!!!warning "Git for Windows インストールの前に"
    [VS Code のインストール](#install-vscode) を完了させておくことを推奨します。
    Git for Windows のインストール中に、既定のエディターを選択する必要があります。
    VS Code をインストールしている場合、 VS Code を既定のエディターとして設定できます。

1. [こちらのサイト :material-open-in-new:](https://git-scm.com/){ target=_blank } から、コンピューターの環境にあった Git for Windows のインストーラーをダウンロードします。

1. ダウンロードしたインストーラーを実行します。

1. オプション設定は、ご利用の環境に応じて設定してインストールしてください。

1. 以下のコマンドが実行できればインストールは完了です。

    ```shell title="Git for Windows のバージョン確認"
    git --version
    ```

### Node.js のインストール {#install-nodejs}

1. [こちらのサイト :material-open-in-new:](https://nodejs.org/ja/){ target=_blank } からインストーラーを取得します。 Node.js のインストーラーは、原則最新の LTS 版を利用してください。

1. ダウンロードしたインストーラーを実行します。

1. オプション設定は、ご利用の環境に応じて設定してインストールしてください。
   npm のインストールと PATH の追加をするよう設定することを推奨します ( 既定値のままインストールすると npm のインストールと PATH の設定が行われます ) 。

1. 以下のコマンドが実行できればインストールは完了です。

    ```shell title="Node.js と npm のバージョン確認"
    node --version
    npm --version
    ```

### Entity Framework Core Tool のインストール {#install-ef-core-tool}

1. 以下のコマンドを実行します。

    ```shell title="Entity Framework Core Tool のインストール"
    dotnet tool install --global dotnet-ef
    ```

1. 以下のコマンドが実行できればインストールは完了です。

    ```shell title="Entity Framework Core Tool のバージョン確認"
    dotnet ef --version
    ```

[^1]:
    .NET アプリケーションの開発を VS Code で行うことも可能ですが、 Visual Studio を用いたほうが生産性高く開発を進めることができます。
    また GUI ベースの開発ができるため、 Visual Studio のほうが初学者にとって扱いやすい環境です。
