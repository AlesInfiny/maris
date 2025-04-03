---
title: CSR 編
description: クライアントサイドレンダリングを行う Web アプリケーションの アーキテクチャについて解説します。
---

# CSR アーキテクチャ概要 {#top}

## 技術スタック {#tech-stack}

AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）を構成する OSS を以下に示します。

![技術スタック](../../images/app-architecture/client-side-rendering/tech-stack-light.png#only-light){ loading=lazy }
![技術スタック](../../images/app-architecture/client-side-rendering/tech-stack-dark.png#only-dark){ loading=lazy }

!!! note ""

    上の図で使用している OSS 製品名およびロゴのクレジット情報は [こちら](../../about-maris/credits.md) を参照してください。

??? note "利用ライブラリ（フロントエンド）"

    - [TypeScript :material-open-in-new:](https://www.typescriptlang.org/){ target=_blank }

          JavaScript を拡張して静的型付にしたプログラミング言語です。
      
    - [Vue.js :material-open-in-new:](https://v3.ja.vuejs.org/){ target=_blank }

          シンプルな設計で拡張性の高い JavaScript のフレームワークです。
      
    - [Vite :material-open-in-new:](https://ja.vitejs.dev/){ target=_blank }

          ES modules を利用してプロジェクトの高速な起動・更新を実現するフロントエンドビルドツールです。
      
    - [Pinia :material-open-in-new:](https://pinia.vuejs.org/){ target=_blank }

          Vue.js 用の状態管理ライブラリです。
      
    - [Vue Router :material-open-in-new:](https://router.vuejs.org/){ target=_blank }

          Vue.js を利用した SPA で、ルーティング制御をするための公式プラグインです。
          
    - [Axios :material-open-in-new:](https://github.com/axios/axios){ target=_blank }

          Vue.js で非同期通信を行うためのプロミスベースの HTTP クライアントです。
          
    - [VeeValidate :material-open-in-new:](https://vee-validate.logaretm.com/){ target=_blank }

          Vue.js 用のリアルタイムバリデーションコンポーネントライブラリです。
          
    - [yup :material-open-in-new:](https://github.com/jquense/yup){ target=_blank }

          JavaScript でフォームのバリデーションルールを宣言的に記述できるライブラリです。

    - [Tailwind CSS :material-open-in-new:](https://tailwindcss.com/){ target=_blank }

          utility class を使って独自のボタンなどを作成する CSS フレームワークです。

    - [Prettier :material-open-in-new:](https://prettier.io/){ target=_blank }

          JavaScript, Vue, CSS, JSON などのコードフォーマッターです。

    - [ESLint :material-open-in-new:](https://eslint.org/){ target=_blank }

          JavaScript の静的検証ツールです。

    - [Stylelint :material-open-in-new:](https://stylelint.io/){ target=_blank }

          CSS の静的検証ツールです。

    - [Vitest :material-open-in-new:](https://vitest.dev/){ target=_blank }

          Vite 環境で動作する高速なテスティングフレームワークです。

    - [Cypress :material-open-in-new:](https://www.cypress.io/){ target=_blank }

          E2E テストツールです。

??? note "利用ライブラリ（バックエンド）"

    - [ASP.NET Core :material-open-in-new:](https://dotnet.microsoft.com/ja-jp/learn/aspnet/what-is-aspnet-core){ target=_blank }

          .NET で利用可能な Web 開発フレームワークです。

    - [Entity Framework Core :material-open-in-new:](https://github.com/dotnet/efcore){ target=_blank }

          .NET で利用可能な O/R マッパーです。

    - [NSwag :material-open-in-new:](https://github.com/RicoSuter/NSwag){ target=_blank }

          実装済みの Web API から OpenAPI 仕様書を生成します。

    - [xUnit v3 :material-open-in-new:](https://xunit.net/){ target=_blank }

          .NET で利用可能なテストフレームワークです。

    - [StyleCopAnalyzers :material-open-in-new:](https://github.com/DotNetAnalyzers/StyleCopAnalyzers){ target=_blank }

          C# のコードを解析し、コーディングルールに則っているかを確認します。

## アプリケーションアーキテクチャ {#application-architecture}

AlesInfiny Maris のアプリケーションアーキテクチャは、クリーンアーキテクチャに基づいています。 アーキテクチャの全体概要は以下の通りです。

![アーキテクチャ概要](../../images/app-architecture/client-side-rendering/csr-architecture-light.png#only-light){ loading=lazy }
![アーキテクチャ概要](../../images/app-architecture/client-side-rendering/csr-architecture-dark.png#only-dark){ loading=lazy }

<!-- ## フロントエンドの構造詳細 {#frontend-structure}

- ビュー
- ビューモデル
- モデル -->

## バックエンドの構造詳細 {#backend-structure}

クライアントサイドレンダリング方式の Web アプリケーションにおける、各層とそれを構成するコンポーネントの役割について、それぞれ説明します。

### アプリケーションコア層 {#application-core}

アプリケーションコア層は、ドメインモデルの定義と業務処理を実装する業務中心の層です。

- ドメインモデル

    ドメインモデルは、業務で扱うドメインをクラスとして表現するエンティティと値オブジェクトで構成するコンポーネントです。 共にモデルのデータ構造と振る舞いを持つことは変わりませんが、エンティティはプロパティが可変である一方、値オブジェクトのプロパティは不変です。
  
- ドメインサービス

    ドメインサービスは、エンティティや値オブジェクトに含めることが適当ではないドメイン固有の処理を実装するコンポーネントです。 ドメイン単位でクラスにまとめ、ドメインに対する処理毎にメソッドとして実装します。 必要に応じてリポジトリを利用して、データベースなどの外部リソースにアクセスします。 アプリケーションコア層のリポジトリインターフェースを利用し、インフラストラクチャ層のリポジトリ実装に依存しないよう注意してください。

- アプリケーションサービス

    アプリケーションサービスは、システムに必要な機能を実装するクラスです。 1 つの Web API の業務処理がアプリケーションサービスの 1 メソッドに対応します。 エンティティや値オブジェクト、リポジトリ ( インターフェース ) を組み合わせて、必要な機能を実現します。 必要に応じてドメインサービスも利用します。

- リポジトリインターフェース

    アプリケーションコア層のリポジトリはインターフェースであり、インフラストラクチャ層のリポジトリで実装されます。 依存関係逆転の法則に従い、アプリケーションコア層の実装がインフラストラクチャ層に依存しないようするためのインターフェースです。

### プレゼンテーション層 {#presentation}

プレゼンテーション層は、主にシステムの利用者とのやり取りを担う層です。 画面を構成するフロントエンドアプリケーションと、バックエンドアプリケーションのインターフェースとなる Web API を配置します。

- コントローラー
  
    コントローラーは ASP.NET Core のコントローラーに対応し、各 Web API の定義と実装を担います。 業務処理であるアプリケーションサービスを呼び出し、その結果からレスポンスデータを生成します。

- API モデル

    Web API のリクエスト／レスポンスの形式を定義するクラスです。 コントローラーが受け取る引数やレスポンスの型を C# のクラスで表現します。

### インフラストラクチャ層 {#infrastructure}

インフラストラクチャ層は、データベースを中心とする外部リソースにアクセスする処理を実現する層です。

- リポジトリ

    インフラストラクチャ層のリポジトリは、アプリケーションコア層のリポジトリインターフェースの実装クラスで、具体的なデータベースアクセス処理を実装します。 DbContext を介してデータベースとデータをやり取りします。

- DbContext (マイグレーション)

    DbContext はデータベース接続するためのクラスです。データベーススキーマをデータベース上に反映するため、 DbContext をもとにマイグレーションを生成します。マイグレーションもインフラストラクチャ層に配置します。
