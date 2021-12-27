# Vue.js 編 # { #top }

## Node.jsのインストール

Node.jsをインストールします。

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

プロジェクト名を入力します。

```bash
√ Project name: ... <project-name>
```

インストールオプションを確認されるので、以下のとおり選択します。

```bash
√ Add TypeScript? ... Yes
√ Add JSX Support? ... Yes
√ Add Vue Router for Single Page Application development? ... Yes
√ Add Pinia for state management? ... Yes
√ Add Cypress for testing? ... Yes
√ Add ESLint for code quality? ... Yes
√ Add Prettier for code formatting? ... Yes
```

以下のとおりコマンドを実行し、必要なパッケージをインストールしてアプリケーションを実行します。

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

### Vue.jsと一緒にインストールして開発に使用するコンポーネント

上の手順でインストールしたコンポーネントと使用目的を以下に示します。

|名称|使用目的|
|--|--|
|TypeScript|JavaScriptに省略可能な静的型付けとクラスベースオブジェクト指向を加えたJavaScriptの厳密なスーパーセット（上位互換）|
|JSX|JavaScriptに対してHTML（あるいはXML）のタグのような構文を導入する拡張記法|
|Vue Router|Vue.js でシングルページアプリケーションを構築するためのルーティングを行う|
|Pinia|Vue.js向けの状態管理ライブラリ|
|Cypress||
|ESLint||
|Prettier||

