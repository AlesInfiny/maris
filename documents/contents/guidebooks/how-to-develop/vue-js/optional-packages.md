---
title: Vue.js 開発手順
description: Vue.js を用いたクライアントサイドアプリケーションの開発手順を説明します。
---

# 開発に使用するパッケージ {#top}

## ブランクプロジェクト作成時にオプションとしてインストールされるパッケージ {#optional-packages}

以下のパッケージは Vue.js のブランクプロジェクト作成時にオプションとしてインストールされます。

- [Cypress :material-open-in-new:](https://www.cypress.io/){ target=_blank }

     E2E (End-to-End) テストツール

- [ESLint :material-open-in-new:](https://eslint.org/){ target=_blank }

    JavaScript の静的検証

- [JSX :material-open-in-new:](https://jsx.github.io/){ target=_blank }

    JavaScript ソースコード上で HTML タグを扱えるようにする

- [Pinia :material-open-in-new:](https://pinia.vuejs.org/){ target=_blank }

    Vue.js 用の状態管理ライブラリ

- [Prettier :material-open-in-new:](https://prettier.io/){ target=_blank }

    コードファイルのフォーマット

- [TypeScript :material-open-in-new:](https://www.typescriptlang.org/){ target=_blank }

    JavaScript を拡張して静的型付にしたプログラミング言語

- [Vitest :material-open-in-new:](https://vitest.dev/){ target=_blank }

    Vite 環境で動作する高速テスティングフレームワーク

- [Vue Router :material-open-in-new:](https://router.vuejs.org/){ target=_blank }

    Vue.js を利用した SPA で、ルーティング制御をするための公式プラグイン

なお、特定のパッケージをインストールすることで付随してインストールされるパッケージ（例： ESLint に対する eslint-config-prettier）は記載を省略しています。

## 追加でインストールするパッケージ {#additional-packages}

以下のパッケージは別途インストールが必要です。

- [Axios :material-open-in-new:](https://github.com/axios/axios){ target=_blank }

    Vue.js で非同期通信するためのプロミスベースの HTTP クライアント（[参照](create-api-client-code.md#install-axios)）

- [autoprefixer :material-open-in-new:](https://autoprefixer.github.io/){ target=_blank }

    ベンダープレフィックスを付与するプラグイン（[参照](css.md#install-tailwind-css)）

- [openapi-generator :material-open-in-new:](https://github.com/OpenAPITools/openapi-generator){ target=_blank }

    Web API 仕様からクライアントコードの自動生成（[参照](create-api-client-code.md#install-openapi-generator)）

- [postCSS :material-open-in-new:](https://github.com/postcss/postcss){ target=_blank }

    CSS の最適化（[参照](css.md#install-tailwind-css)）

- [postcss-nesting :material-open-in-new:](https://github.com/csstools/postcss-nesting){ target=_blank }

    スタイルルールを互いに入れ子にするためのプラグイン（[参照](css.md#install-tailwind-css)）

- [stylelint :material-open-in-new:](https://stylelint.io/){ target=_blank }

    CSS の静的検証ツール（[参照](static-verification-and-format.md#install-stylelint)）

- [stylelint-config-standard :material-open-in-new:](https://github.com/stylelint/stylelint-config-standard){ target=_blank }

    Stylelint の標準設定（[参照](static-verification-and-format.md#install-stylelint)）

- [stylelint-config-prettier :material-open-in-new:](https://github.com/prettier/stylelint-config-prettier){ target=_blank }

    Stylelint の Ptettier 向け設定（[参照](static-verification-and-format.md#install-stylelint)）

- [stylelint-config-recommended-vue :material-open-in-new:](https://github.com/ota-meshi/stylelint-config-recommended-vue){ target=_blank }

    Stylelint の .vue ファイル向け推奨設定（[参照](static-verification-and-format.md#install-stylelint)）

- [stylelint-prettier :material-open-in-new:](https://github.com/prettier/stylelint-prettier){ target=_blank }

    Stylelint と Prettier の連携プラグイン（[参照](static-verification-and-format.md#install-stylelint)）

- [Tailwind CSS :material-open-in-new:](https://tailwindcss.com/){ target=_blank }

    CSS フレームワーク（[参照](css.md#install-tailwind-css)）

- [VeeValidate :material-open-in-new:](https://vee-validate.logaretm.com/){ target=_blank }

    Vue.js 用のリアルタイムバリデーションコンポーネントライブラリ（[参照](input-validation.md#install-packages)）

- [yup :material-open-in-new:](https://github.com/jquense/yup){ target=_blank }

    JavaScript でフォームのバリデーションルールを宣言的に記述できるライブラリ（[参照](input-validation.md#install-packages)）
