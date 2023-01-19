---
title: CSR 編
description: クライアントサイドレンダリングを行う Web アプリケーションのアーキテクチャについて解説します。
---

# フロントエンドアーキテクチャ {#top}

## 技術スタック {#tech-stack}

AlesInfiny Maris を構成する OSS を以下に示します。

![OSS構成要素](../../images/app-architecture/client-side-rendering/oss-components-light.png#only-light){ loading=lazy }
![OSS構成要素](../../images/app-architecture/client-side-rendering/oss-components-dark.png#only-dark){ loading=lazy }

- [TypeScript :material-open-in-new:](https://www.typescriptlang.org/){ target=_blank }

      JavaScript を拡張して静的型付にしたプログラミング言語。

- [Vue.js :material-open-in-new:](https://v3.ja.vuejs.org/){ target=_blank }

      シンプルな設計で拡張性の高い JavaScript のフレームワーク。

- [Vite :material-open-in-new:](https://ja.vitejs.dev/){ target=_blank }

      ES modules を利用してプロジェクトの高速な起動・更新を実現するフロントエンドビルドツール。

- [Pinia :material-open-in-new:](https://pinia.vuejs.org/){ target=_blank }

      Vue.js 用の状態管理ライブラリ。

- [Vue Router :material-open-in-new:](https://router.vuejs.org/){ target=_blank }

      Vue.js を利用した SPA で、ルーティング制御をするための公式プラグイン。

- [Axios :material-open-in-new:](https://github.com/axios/axios){ target=_blank }

      Vue.js で非同期通信を行うためのプロミスベースの HTTP クライアント。

- [VeeValidate :material-open-in-new:](https://vee-validate.logaretm.com/){ target=_blank }

      Vue.js 用のリアルタイムバリデーションコンポーネントライブラリ。

- [yup :material-open-in-new:](https://github.com/jquense/yup){ target=_blank }

      JavaScript でフォームのバリデーションルールを宣言的に記述できるライブラリ。

- [Tailwind CSS :material-open-in-new:](https://tailwindcss.com/){ target=_blank }

      utility class を使って独自のボタンなどを作成する CSS フレームワーク

- [Prettier :material-open-in-new:](https://prettier.io/){ target=_blank }

      JavaScript, Vue, CSS, JSON などのコードフォーマッター。

- [ESLint :material-open-in-new:](https://eslint.org/){ target=_blank }

      JavaScript の静的検証ツール。

- [Stylelint :material-open-in-new:](https://stylelint.io/){ target=_blank }

      CSS の静的検証ツール。

- [Vitest :material-open-in-new:](https://vitest.dev/){ target=_blank }

      Vite 環境で動作する高速テスティングフレームワーク。

- [Cypress :material-open-in-new:](https://www.cypress.io/){ target=_blank }

      E2E テストツール。

## アーキテクチャ {#frontend-architecture}

### MVVMパターン {#mvvm-pattern}

AlesInfiny Maris で採用している Vue.js のソフトウェア・アーキテクチャは MVVM パターンに分類されます。
以下にアーキテクチャを示します。
![フロントエンド コンポーネント構成](../../images/app-architecture/client-side-rendering/frontend-architecture-light.png#only-light){ loading=lazy }
![フロントエンド コンポーネント構成](../../images/app-architecture/client-side-rendering/frontend-architecture-dark.png#only-dark){ loading=lazy }

<!-- markdownlint-disable-next-line no-emphasis-as-heading -->
**ビュー**

:  ブラウザへのレンダリングおよびブラウザからのイベントの待ち受けを役割として担います。ビューには UI の構造やスタイルを定義します。

<!-- markdownlint-disable-next-line no-emphasis-as-heading -->
**ビューモデル**

:  ブラウザからのイベントを受け、プレゼンテーションロジックを実行します。ビューモデルのプレゼンテーションロジックには、レンダリングに必要な処理や入力チェック、モデルを通じたデータの取得や更新などの処理を実装します。

<!-- markdownlint-disable-next-line no-emphasis-as-heading -->
**モデル**

:  ビジネスロジックとして状態管理やブラウザ外部との入出力を担います。モデルのビジネスロジックには、データ構造やデータの状態管理、 Web API 呼び出しや Web API 呼び出し結果のハンドリングなどの処理を実装します。モデルは後述する Pinia のアーキテクチャに従って実装します。

<!-- textlint-disable -->
Vue.js ではビューとビューモデルを [単一ファイルコンポーネント(SFC) :material-open-in-new:](https://v3.ja.vuejs.org/guide/single-file-component.html#単一ファイルコンポーネント){ target=_blank } と呼ばれる同一のファイル(拡張子.vue)に記述できるので、図ではビュー&ビューモデルと表現しています。

### ビュー＆ビューモデル コンポーネント {#view-and-viewmodel-component}

![MVVMパターン ビュー＆ビューモデル](../../images/app-architecture/client-side-rendering/view%26viewmodel-component-light.png#only-light){ loading=lazy }
![MVVMパターン ビュー＆ビューモデル](../../images/app-architecture/client-side-rendering/view%26viewmodel-component-dark.png#only-dark){ loading=lazy }

ビューとビューモデルはそれぞれブラウザへのレンダリングとそのブラウザから受けたイベントに対するプレゼンテーションロジックなどを行うコンポーネントです。
ブラウザに表示する画面は Component という複数の画面構成要素と View というそれらを組み合わせたページから構成されます。
これらの画面コンポーネントが、デザインやデータバインドなどの画面表示（ビュー）と、イベント処理や入力処理などの画面要素に対する処理（ビューモデル）を持っています。

#### 画面コンポーネント {#screen-components}

Vue.js はコンポーネント指向のフレームワークであることから画面要素を Component という再利用可能な単位で分割し、複数の画面コンポーネントを組み合わせることによってひとつの画面(View)を構成します。
View がルーティングによって遷移される画面として指定されます。画面コンポーネントは実際の画面では以下のようなイメージになります。

![画面コンポーネント イメージ](../../images/app-architecture/client-side-rendering/screen-component-detail-light.png#only-light){ loading=lazy }
![画面コンポーネント イメージ](../../images/app-architecture/client-side-rendering/screen-component-detail-dark.png#only-dark){ loading=lazy }

#### 画面遷移 {#screen-transition}

画面遷移には、 Vue Router という Vue.js の拡張ライブラリを利用します。 Vue Router はルーティング定義に基づいて遷移先の画面コンポーネントを特定し、表示する画面コンポーネントを切り替えることで画面遷移を実現します。 Vue Router による画面遷移はフロントエンドのみで完結するためバックエンドへ通信しません。また AlesInfiny Maris では、「View」を切り替えの単位としています。

Vue Router : [公式ドキュメント :material-open-in-new:](https://router.vuejs.org/introduction.html){ target=_blank }

![Vue Router によるルーティング](../../images/app-architecture/client-side-rendering/routing-by-vue-router-light.png#only-light){ loading=lazy }

![Vue Router によるルーティング](../../images/app-architecture/client-side-rendering/routing-by-vue-router-dark.png#only-dark){ loading=lazy }

#### モデルコンポーネントとの連携 {#linkage-with-model-component}

Vue.js ではバックエンドのアプリケーションとの連携をモデルが行います。そのため、ユーザーが行う画面コンポーネントからの処理や入力情報をモデルに連携する必要があります。この連携ではビューモデルのプレゼンテーションロジックから、後述するモデルコンポーネントの Store が持つ Action を呼び出して行います。

#### フロント入力チェック {#input-validation}

文字種や文字数などの入力チェックは、ビューモデルで行い、不要なバックエンドとの通信の発生を防止します。  AlesInfiny Maris では VeeValidate と yup という OSS ライブラリを利用します。 VeeValidate はフォームや入力コンポーネントを監視し、 yup は検証スキームを定義する OSS です。

![VeeValidation と yup による入力チェック](../../images/app-architecture/client-side-rendering/input-validation-light.png#only-light){ loading=lazy }
![VeeValidation と yup による入力チェック](../../images/app-architecture/client-side-rendering/input-validation-dark.png#only-dark){ loading=lazy }

### モデルコンポーネント {#model-component}

![MVVMパターン モデル](../../images/app-architecture/client-side-rendering/model-component-light.png#only-light){ loading=lazy }
![MVVMパターン モデル](../../images/app-architecture/client-side-rendering/model-component-dark.png#only-dark){ loading=lazy }

モデルはフロントエンドアプリケーションのビジネスロジックとして、扱うデータの状態管理や画面(ビュー)へのデータ連携、 Web API の呼び出しおよびハンドリングなどの役割を持つコンポーネントです。またフロントエンドで扱うデータモデルと API モデルとの乖離を吸収し、扱いやすい状態に加工する役割も持ちます。

このフロントエンドで扱う状態を保持するコンテナのことを Store と呼び、 Maris では Pinia という Vue.js の Store ライブラリを利用して管理します。

Pinia : [公式ドキュメント :material-open-in-new:](https://pinia.vuejs.org/introduction.html){ target=_blank }

#### Storeの構成要素 {#store-structure}

Pinia における Store は、 State・Getter・Action という 3 つの要素から構成されています。

![Pinia のアーキテクチャ](../../images/app-architecture/client-side-rendering/pinia-architecture-light.png#only-light){ loading=lazy }
![Pinia のアーキテクチャ](../../images/app-architecture/client-side-rendering/pinia-architecture-dark.png#only-dark){ loading=lazy }

<!-- markdownlint-disable-next-line no-emphasis-as-heading -->
**State**
:  Store で管理するデータそのもの。

<!-- markdownlint-disable-next-line no-emphasis-as-heading -->
**Getter**
:  Store で管理しているデータである State を画面コンポーネント(ビュー & ビューモデル)に返すもの。

<!-- markdownlint-disable-next-line no-emphasis-as-heading -->
**Action**
<!-- textlint-disable -->
:  Store で管理しているデータである State に対して変更を行うもの。また API の呼び出しや API のレスポンスのハンドリングを行うもの。

#### APIの呼び出しについて {#about-invoke-api}

API の呼び出しは Action で行います。 AlesInfiny Maris では、 Promise ベースでリクエストの設定が容易な axios という OSS を利用します。

axios : [github :material-open-in-new:](https://github.com/axios/axios){ target=_blank }

#### バックエンドとのAPI連携 {#communicate-with-backend}

AlesInfiny Maris では API 仕様を OpenAPI を用いて作成します。ここには API の機能が説明されており、フロントエンドエンジニアとバックエンドエンジニアの間で API 設計に乖離が生じないようにします。

OpenAPI 仕様 : [公式ドキュメント :material-open-in-new:](https://swagger.io/specification/){ target=_blank }

![OpenAPIを利用したバックエンドとの連携](../../images/app-architecture/overview/client-side-rendering-maris-light.png#only-light){ loading-lazy }
![OpenAPIを利用したバックエンドとの連携](../../images/app-architecture/overview/client-side-rendering-maris-dark.png#only-dark){ loading-lazy }

<!-- バックエンド編のAPIドキュメントへリンク -->

<!-- ### OpenAPI generator -->

## フォルダー構成 {#project-structure}

Vue.js プロジェクトのフォルダー構成は、ブランクプロジェクト作成時のデフォルトの構成を基に以下のように行います。なおこのフォルダー配下の構成については、コンポーネント設計方法に依存するため、各プロジェクトの方針に従います。

``` text title="プロジェクトのフォルダー構成全体像" linenums="0"
<project-name>
├─ cypress/ ------------------ cypress による E2E テストに関するファイルを格納します。
├─ public/ ------------------- メディアファイルや favicon など静的な資産を格納します。
├─ src/
│  ├─ assets/ ---------------- コードや動的ファイルが必要とするCSSや画像などのアセットを格納します。
│  ├─ components/ ------------ 単体で自己完結している再利用性の高い vue コンポーネントなどを格納します。
│  ├─ config/ ---------------- 設定ファイルを格納します。
│  ├─ router/ ---------------- ルーティング定義を格納します。
│  ├─ stores/ ---------------- store に関するファイルを格納します。
│  ├─ views/ ----------------- ルーティングで指定される vue ファイルを格納します。またページ固有の挙動などもここに含めます。
│  ├─ App.vue
│  └─ main.ts
├─ index.html
└─ package.json
```

### views フォルダー {#views-directory}

views フォルダーはルーティングで指定される vue ファイルを格納します。そのためこの下層のフォルダー構造はサイト構造を意識して作成することを推奨します。以下の例で Login.vue なら ```https://xxxx.com/account/login``` と設定します。

``` text title="views フォルダー" linenums="0"
src/
└─ views/
   ├─ account/
   │  ├─ LoginView.vue
   │  └─ LogoutView.vue
   ├─ catalog/
   └─ order/
```

!!! note "Vue Router の設定"
      Vue Router では URL のパスと対象のファイルを指定することで、ルーティングを設定します。以下は `https://xxxx.com/account/login` という URL に対して上記の `LoginView.vue` を設定している例です。

      ``` TypeScript title="index.ts"
      import { createRouter, createWebHistory } from "vue-router";

      const router = createRouter({
         history: createWebHistory(import.meta.env.BASE_URL),
         routes: [
            {
               path: "/account/login",
               name: "account/login",
               component: () => import('@/views/account/LoginView.vue'),
            },
         ],
      });
      ```

### components フォルダー {#components-directory}

components フォルダーは主に、再利用性の高い vue コンポーネントファイルを格納します。さらにこの下層フォルダーはドメインで分割し、それを操作するコンポーネントを格納します。こうすることで再利用性を活かすために、どのドメインを対象にしたコンポーネントなのかを明確にします。また vue ファイルに限らずプロジェクト内で再利用性の高いもの（icon など）もこちらに格納します。

``` text title="components フォルダー" linenums="0"
src/
└─ components/
   ├─ account/
   │  ├─ LoginForm.vue
   │  └─ LogoutMessage.vue
   ├─ product/
   │  ├─ ProductDetail.vue
   │  └─ ProductList.vue
   └─ icon/
```

上記の拡張として Atomic Design でコンポーネント設計をする場合は、 atoms, molecules, organisms でフォルダーを構成します。この際 atoms と molecules は同一フォルダーにコンポーネント構成パーツとしてまとめ、 organisms との区別を「store へのアクセスの有無」として行うことでドメイン分割が容易になります。

!!! note "Atomic Design"
      Atomic Design とは UI の構成要素を 5 段階に分けてパーツ単位で UI デザインを設計する方法のことです。最も小さい単位である Atoms パーツを組み合わせた Molecules, さらにそれらを組み合わせた Organism, というように要素を細分化し、それらを組み合わせて画面を作成します。コンポーネントの再利用性やデザイン変更の反映のしやすさといったメリットがあります。

      - [Atomic Design by Brad Frost :material-open-in-new:](https://atomicdesign.bradfrost.com/){ target=_blank }

``` text title="components フォルダー by Atomic Design" linenums="0"
src/
└─ components/
   ├─ atoms&molecules/
   │  ├─ Button.vue
   │  ├─ Input.vue
   │  └─ Form.vue
   │
   ├─ organisms/
   │  ├─ account/
   │  │  ├─ LoginForm.vue
   │  │  └─ LogoutMessage.vue
   │  └─ product/
   │     ├─ ProductDetail.vue
   │     └─ ProductList.vue
   │
   └─ icon/
```

!!! note "URL とドメイン"
    views フォルダーは URL 本位、 components フォルダーはドメイン本位で構成するため、下層フォルダー構造は一致しません。（部分的に一致することはあります。）

<!--
#### テストファイル

テストファイルは ``` __test__ ``` フォルダーを作らず、対象コンポーネントの隣に配置します。
-->