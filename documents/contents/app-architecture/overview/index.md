# 概要編

## 構築できるアプリケーション形態 ## {: #application-kind }

Maris OSS 版を利用することで構築できるアプリケーションの概要を、アプリケーション形態ごとに説明します。

- Web アプリケーション（クライアントサイドレンダリング）

    HTML をクライアント側 JavaScript でレンダリングする方式の Web アプリケーションです。
    画面初期表示時にはコンパイル済みの静的ファイルをダウンロードして、 JavaScript で動的に画面をレンダリングします。
    業務データの取得、更新などの処理を行う際は、 Web API 経由でサーバー側の業務ロジックを呼び出します。

    ![クライアントサイドレンダリング](../../images/app-architecture/overview/client-side-rendering-light.png#only-light){ loading=lazy }
    ![クライアントサイドレンダリング](../../images/app-architecture/overview/client-side-rendering-dark.png#only-dark){ loading=lazy }

- Web アプリケーション（サーバーサイドサイドレンダリング）

    サーバーサイドで構築した HTML を表示する方式の Web アプリケーションです。
    （今後追加予定）

- コンソールアプリケーション

    クライアント端末で動作するネイティブアプリケーションです。
    （今後追加予定）
  
## アプリケーション構成 ## {: #application-structure }

Maris OSS 版として、アプリケーション形態ごとに標準的なアプリケーション構成を定義しています。
ここでは主要な構成要素を示します。
詳細はアプリケーション種別ごとの詳細ページ、および、サンプルプリケーションを参照してください。

### Web アプリケーション ( クライアントサイドレンダリング ) ### {: #client-side-rendering }

Vue.js を用いた SPA の構成をとります。
サーバーサイドは .NET 6 以降をベースとした ASP.NET Core の Web API アプリケーションです。
データアクセスには Entity Framework Core を利用します。

![クライアントサイドレンダリング アプリケーションスタック](../../images/app-architecture/overview/client-side-rendering-maris-light.png#only-light){ loading=lazy }
![クライアントサイドレンダリング アプリケーションスタック](../../images/app-architecture/overview/client-side-rendering-maris-dark.png#only-dark){ loading=lazy }

### Web アプリケーション ( サーバーサイドレンダリング ) ### {: #server-side-rendering }

（今後追加予定）

### コンソールアプリケーション ### {: #console-application }

（今後追加予定）

## ソリューション構造 ## {: #solution-structure }

### ソリューションの単位 ### {: #unit-of-solution }

Web アプリケーションやコンソールアプリケーション ( バッチ ) など、 1 つのサブシステムは通常複数のアプリケーションで構成されます。
Maris OSS 版では、 1 サブシステム 1 ソリューションを基本として推奨します。
ただし、複数サブシステム ( 複数ソリューション ) で共用する共通機能を作成する場合は、必要に応じてソリューション分割を検討してください。

ビルド時間が長すぎる場合や、開発者の PC スペックがソリューションの大きさに耐えられない場合は、ソリューションフィルターの機能を活用しましょう。
単一ソリューションを保ちながら、プロジェクト単位でフィルター処理を行うことができます。

[Visual Studio のフィルター処理済みソリューション - ソリューション フィルター ファイル](https://docs.microsoft.com/ja-jp/visualstudio/ide/filtered-solutions?view=vs-2022#solution-filter-files)

### プロジェクトの単位 ### {: #unit-of-project }

プロジェクトは、原則として機能単位、レイヤー単位で分割することを推奨します。
プロジェクトの分割にあたっては、以下の手順で分割を検討してください。

1. 業務分割、機能分割

    サブシステムを業務的な観点で機能に分割します。

1. アプリケーションアーキテクチャの検討

    アプリケーションの内部構造をどのように整理していくか検討します。
    典型的なアプリケーションの内部構造には以下のような種類があります。

    - レイヤードアーキテクチャ
    - クリーンアーキテクチャ

    各アーキテクチャの詳細は、以下を参照してください。

    [一般的な Web アプリケーション アーキテクチャ](https://docs.microsoft.com/ja-jp/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)

1. アプリケーションアーキテクチャをプロジェクト構造に落とし込む

    定めたアプリケーションアーキテクチャをプロジェクト構造に落とし込みます。
    アプリケーションアーキテクチャの定める各コンポーネントをどのようにプロジェクトとして構成するか定めます。
    .NET のプロジェクト間には、循環参照[^1] の設定ができません。
    アプリケーションアーキテクチャを適切に表現できるプロジェクト構造を検討してください。

    最も基本的な構造は、以下の通りです。

    ![代表的なアプリケーションアーキテクチャ](../../images/app-architecture/overview/application-architecture-light.png#only-light){ loading=lazy }
    ![代表的なアプリケーションアーキテクチャ](../../images/app-architecture/overview/application-architecture-dark.png#only-dark){ loading=lazy }

1. 業務 / 機能をプロジェクト構造に当てはめる

    プロジェクトは原則として業務 / 機能で分割を行ってから、レイヤー分割を行います。
    アプリケーションをマイクロサービス化しないのであれば、エントリーポイントのプロジェクトは単一プロジェクトとすることを推奨します。

    ![レイヤードアーキテクチャのプロジェクト分割例](../../images/app-architecture/overview/application-architecture-and-functions-light.png#only-light){ loading=lazy }
    ![レイヤードアーキテクチャのプロジェクト分割例](../../images/app-architecture/overview/application-architecture-and-functions-dark.png#only-dark){ loading=lazy }

[^1]:
    プロジェクト同士がお互いに参照しあう構造のことを言います。
    .NET ではプロジェクト間の循環参照は許可されていないため、循環参照の発生しない構造を定義しなければなりません。
