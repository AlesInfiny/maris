# ブランクプロジェクトの作成

## Vue.jsおよびオプションのインストール ## {: #install-vuejs-and-options }

以下のコマンドを実行して Vue.js をインストールします。

```bash
npm init vue@3
```

create-vue パッケージをインストールする必要があり、続行するかどうかを確認するメッセージが表示されるので、「y」を選択します。

プロジェクト名を入力します。

```bash
√ Project name: ... <project-name>
```

インストールオプションを確認されるので、左右カーソルキーで Yes / No を選択します。サンプルアプリケーションでは以下のように選択しています。

```cmd
√ Add TypeScript? ... Yes
√ Add JSX Support? ... Yes
√ Add Vue Router for Single Page Application development? ... Yes
√ Add Pinia for state management? ... Yes
√ Add Vitest for Unit Testing? ... Yes
√ Add Cypress for End-to-End testing? ... Yes
√ Add ESLint for code quality? ... Yes
√ Add Prettier for code formatting? ... Yes
```

## ブランクプロジェクトのビルドと実行 ## {: #build-and-serve-blank-project }

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
