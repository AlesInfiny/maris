---
title: Vue.js 開発手順
description: Vue.js を用いたクライアントサイドアプリケーションの開発手順を説明します。
---

# ブランクプロジェクトの作成 {#top}

## Vue.js およびオプションのインストール {#install-vue-js-and-options}

以下のコマンドを実行して Vue.js をインストールします。

```terminal
npm init vue@3
```

create-vue パッケージをインストールする必要があり、続行するかどうかを確認するメッセージが表示されるので、「y」を選択します。

プロジェクト名を入力します。

```terminal
√ Project name: ... <project-name>
```

インストールオプションを確認されるので、左右カーソルキーで Yes / No を選択します。クライアントサイドのアーキテクチャに基づき、使用するものに対して Yes を選択すると、以下のようになります。

```terminal
√ Add TypeScript? ... Yes
√ Add JSX Support? ... Yes
√ Add Vue Router for Single Page Application development? ... Yes
√ Add Pinia for state management? ... Yes
√ Add Vitest for Unit Testing? ... Yes
√ Add Cypress for End-to-End testing? ... Yes
√ Add ESLint for code quality? ... Yes
√ Add Prettier for code formatting? ... Yes
```

## ブランクプロジェクトのビルドと実行 {#build-and-serve-blank-project}

以下のようにコマンドを実行し、必要なパッケージをインストールしてアプリケーションを実行します。

```terminal
cd <project-name>
npm install
npm run dev
```

`npm run dev` が成功すると以下のように表示されるので、「 Local: 」に表示された URL をブラウザーで表示します。ブランクプロジェクトのランディングページが表示されます。

```terminal
vite v2.7.7 dev server running at:

> Local: http://localhost:3000/
> Network: use `--host` to expose
```
