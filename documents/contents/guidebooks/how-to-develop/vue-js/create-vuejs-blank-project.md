# ブランクプロジェクトの作成からビルドと実行

## ローカル開発環境の構築

ローカル開発環境の構築について [ローカル開発環境の構築](../local-environment/index.md) を参照し、最低限必要なソフトウェアをインストールしてください。

## Vue.jsおよびオプションのインストール

以下のコマンドを実行して Vue.js をインストールします。

```bash
npm init vue@3
```

以下のように必要なパッケージをインストールするかどうか確認されるので「y」を入力します。

```bash
Need to install the following packages:
  create-vue@3
Ok to proceed? (y)
```

任意のプロジェクト名を入力します。

```bash
√ Project name: ... <project-name>
```

インストールオプションを確認されるので選択します。サンプルアプリケーションでは以下のように選択しています。

```cmd
√ Add TypeScript? ... Yes
√ Add JSX Support? ... Yes
√ Add Vue Router for Single Page Application development? ... Yes
√ Add Pinia for state management? ... Yes
√ Add Cypress for testing? ... Yes
√ Add ESLint for code quality? ... Yes
√ Add Prettier for code formatting? ... Yes
```

上の例でインストールしたコンポーネントと使用目的を以下に示します。

|名称      |使用目的|
|----------|-------|
|TypeScript|JavaScriptに省略可能な静的型付けとクラスベースオブジェクト指向を加えたJavaScriptの厳密なスーパーセット（上位互換）|
|JSX       |JavaScriptに対してHTML（あるいはXML）のタグのような構文を導入する拡張記法|
|Vue Router|Vue.js でシングルページアプリケーションを構築するためのルーティングを行う|
|Pinia     |Vue.js向けの状態管理ライブラリ|
|Cypress   ||
|ESLint    ||
|Prettier  ||

## ブランクプロジェクトのビルドと実行

以下のようにコマンドを実行し、必要なパッケージをインストールしてアプリケーションを実行します。

```bash
cd <project-name>
npm install
npm run dev
```

`npm run dev` が成功すると以下のように表示されるので、「Local:」に表示されたURLをブラウザーで表示します。ブランクプロジェクトのランディングページが表示されます。

```bash
vite v2.7.7 dev server running at:

> Local: http://localhost:3000/
> Network: use `--host` to expose
```
