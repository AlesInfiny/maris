---
title: Vue.js 開発手順 （CSR 編）
description: Vue.js を用いた フロントエンドアプリケーションの 開発手順を説明します。
---

# E2E テストの設定 {#top}

E2E テストの利用により、フロントエンド・バックエンドアプリケーションからデータベースまで一気通貫での動作確認を効率的に行うことができます。
また、実行ブラウザーの指定やテスト証跡取得等の設定も可能です。

E2E テストの実行ツールとして [Playwright :material-open-in-new:](https://playwright.dev/){ target=_blank } を使用します。
[ブランクプロジェクトの作成](./create-vuejs-blank-project.md) の手順に沿って `create-vue` でプロジェクトを作成することで Playwright 自体のインストールは完了しています。
Playwright の設定方法について以下で説明します。

なお、本ページではバックエンドアプリが正常に起動することを前提としているため、 [.NET 編](../dotnet/index.md) の手順に従ってバックエンドアプリの動作確認を事前に行ってください。

## フォルダー構成 {#folder-structure}

E2E テストの設定に関係するフォルダーとファイルは以下の通りです。

```text linenums="0"
<workspace-name>
  ├ e2e/ -------------------- Playwright による End-to-End テスト用のフォルダー
  │ ├ pages/ ---------------- テスト対象の各画面を表すページオブジェクトを配置するフォルダー
  │ ├ fixture/ -------------- テスト実行に必要な前提条件などを定義するフィクスチャーを配置するフォルダー
  │ └ test-scenario/ -------- E2E テストを実行するコードを配置するフォルダー
  ├ package.json ------------ ワークスペースのメタデータ、依存関係、スクリプトなどを定義するファイル
  ├ playwright.config.ts ---- Playwright の設定ファイル
  └ vite.config.ts ---------- Vite の設定ファイル 
```

## 実行スクリプト {#script-settings}

`package.json` に Playwright 実行用のスクリプトを追加します。

```json title="package.json"
"test:e2e": "playwright test",
```

以下のコマンドで Playwright を実行できます。

```shell
npm run test:e2e
```

GitHub Actions などの CI 環境では通常 `CI=true` が設定されるため、後述する `playwright.config.ts` では、環境変数 `CI` の値に応じて設定を切り替えています。
ローカルで同じ設定を確認する場合は、 Playwright の実行前に環境変数 `CI` を設定します。たとえば PowerShell では、次のように設定してからテストを実行します。

```powershell
$env:CI = "true"
npm run test:e2e
```

## Playwright の設定 {#playwright-settings}

### 型チェックの設定 {#type-check-settings}

Playwright 自体は TypeScript の型チェックを行わないため、`tsconfig.json` の設定が必要となります。

`create-vue` でプロジェクト作成時に追加される `e2e/tsconfig.json` を以下のように設定します。

```json title="e2e/tsconfig.json"
--8<-- "samples/Dressca/dressca-frontend/consumer/e2e/tsconfig.json"
```

- `extends`
    - 初期設定の `@tsconfig/node24/tsconfig.json` から `@vue/tsconfig/tsconfig.dom.json` に変更し、ベースとなる設定をアプリケーションコードと合わせます。初期設定の状態ではコンパイル後の JavaScript ファイルの出力要否やモジュール解決の方針に差分があるため、実装時の負担軽減のためにアプリケーションコードと合わせた設定にしています。
- `include`
    - 型チェックの対象とするフォルダーを指定します。
- `compilerOptions.tsBuildInfoFile`
    - .tsbuildinfo ファイルの出力先を一時フォルダーに設定します。
- `compilerOptions.types`
    - E2E テスト作成に必要となる型定義のみ読み込むように設定します。

つづいて、 `npm run type-check` 実行時に Playwright のコードも型チェックされるよう、ルートの `tsconfig.json` に `e2e/tsconfig.json` の設定を追記します。

```ts title="tsconfig.json" hl_lines="13-15"
--8<-- "samples/Dressca/dressca-frontend/consumer/tsconfig.json"
```

`tsconfig.json` の設定後、ワークスペース直下で以下のコマンドを実行して型チェックできることを確認します。

```shell linenums="0"
npm run type-check
```

### テストの設計 {#test-architecture}

Playwright 公式サイトで取り上げられている [Page Object Model（POM） :material-open-in-new:](https://playwright.dev/docs/pom){ target=_blank } に従ってテストコードを作成します。
POM は、 Web アプリケーションの画面をオブジェクトとして表現し、画面の詳細な操作をテストシナリオから分離して保守性を高める設計パターンです。
ページオブジェクトには、テストで使用するセレクターや画面操作を定義します。

サンプルアプリケーション Dressca の Consumer アプリでは以下の構成で [カタログアイテムを注文](../../../../samples/dressca/index.md#consumer-application-features) できることを確認します。

```text linenums="0"
<workspace-name>
  ├ e2e/ -------------------- Playwright による End-to-End テスト用のフォルダー
  │ ├ pages/ ---------------- テスト対象の各画面を表すページオブジェクトを配置するフォルダー
  │ │ ├ base
  │ │ │ └ base-page.ts
  │ │ ├ security
  │ │ │ └ login-page.ts
  │ │ └ shopping
  │ │   ├ basket-page.ts
  │ │   ├ checkout-page.ts
  │ │   ├ display-item-page.ts
  │ │   └ done-page.ts
  │ ├ fixture/ -------------- テスト実行に必要な前提条件などを定義するフィクスチャーを配置するフォルダー
  │ │ └ shopping-test-fixture.ts
  │ └ test-scenario/ -------- E2E テストを実行するコードを配置するフォルダー
  │   └ shopping-scenario.test.ts
  ├ package.json ------------ ワークスペースのメタデータ、依存関係、スクリプトなどを定義するファイル
  ├ playwright.config.ts ---- Playwright の設定ファイル
  └ vite.config.ts ---------- Vite の設定ファイル 
```

??? info "ページオブジェクトの実装例"

    テスト対象となる画面のセレクターに対する操作を定義します。

    ```ts title="display-item-page.ts"
    --8<-- "samples/Dressca/dressca-frontend/consumer/e2e/pages/shopping/display-item-page.ts"
    ```

??? info "フィクスチャーの実装例"

    テスト実行時に必要となるページオブジェクトをまとめて提供します。

    ```ts title="shopping-test-fixture.ts"
    --8<-- "samples/Dressca/dressca-frontend/consumer/e2e/fixture/shopping-test-fixture.ts"
    ```

??? info "テストシナリオの実装例"

    業務の流れに沿って画面を操作するテストシナリオを定義します。

    ```ts title="shopping-scenario.test.ts"
    --8<-- "samples/Dressca/dressca-frontend/consumer/e2e/test-scenario/shopping-scenario.test.ts"
    ```

### テスト実行時の設定 {#test-config-settings}

Playwright では `playwright.config.ts` での設定内容に応じてテストが実行されます。

サンプルアプリケーション Dressca の Consumer アプリでは、以下のように設定します。
初期設定からの変更点をハイライトで示します。

```ts title="playwright.config.ts" hl_lines="16 17 24-27 32-45"
--8<-- "samples/Dressca/dressca-frontend/consumer/playwright.config.ts:14:61"
```

- `use.baseURL`: テスト実行時のベース URL を指定します。
- `use.locale`: テスト実行時のロケールを指定します。
- `projects`: ブラウザー、デバイスなどの組み合わせを定義し、同じテスト群を異なる設定でテストするための実行設定です。
    - 実行例ではブラウザーの種類は chromium のみ設定していますが、必要に応じて他のブラウザーも追加できます。 `projects` で設定可能なブラウザー等については [Projects :material-open-in-new:](https://playwright.dev/docs/test-projects){ target=_blank } を参照してください。
- `webServer`: テスト実行時に起動するサーバーを指定します。
    - Dressca は CSR アプリケーションのため、フロントエンド・バックエンドアプリを起動できるよう設定します。

??? info "プロキシ設定環境下での実行"

    プロキシを設定している環境（環境変数 HTTP_PROXY, HTTPS_PROXY 等を設定している場合）では、 localhost へのアクセスがプロキシ経由となり接続に失敗することがあります。
    そのため Playwright のテスト実行時にプロキシを無効化するために、環境変数 NO_PROXY に localhost と 127.0.0.1 を追加します。
    
    ```ts title="playwright.config.ts"
    process.env.NO_PROXY = process.env.NO_PROXY
      ? `${process.env.NO_PROXY},localhost,127.0.0.1`
      : 'localhost,127.0.0.1'
    ```

### ローカルでの動作確認 {#run-local}

#### ヘッドレスモードでの動作確認 {#headless}

デフォルトでは Playwright はヘッドレスモードで実行され、ブラウザー画面を表示せずにテストが実行されます。
以下のコマンドを実行してヘッドレスモードで実行されます。

```shell
npm run test:e2e
```

テスト実行に成功すると以下のように表示されます。

```text
Running 1 test using 1 worker
  1 passed (41.6s)

To open last HTML report run:

  npx playwright show-report
```

なお、ヘッドレスモードで実行した場合であっても、トレースを取得した場合は以下のコマンドを実行して HTML 実行レポートからテスト実行時のスクリーンショットを確認できます。

```shell
npx playwright show-report
```

[テスト実行時の設定](#test-config-settings) の `playwright.config.ts` では `use.trace` に `'on-first-retry'` を設定しているため、失敗したテストの最初のリトライ時にトレースを取得します。
トレースに関する設定については [Recording a trace :material-open-in-new:](https://playwright.dev/docs/trace-viewer#recording-a-trace){ target=_blank } を参照してください。

#### ブラウザーを起動して動作確認 {#headed}

テスト実行コマンドに `--headed` オプションを指定することで、テスト実行時にブラウザーが起動されます。

`package.json` に以下のようにスクリプトを追加し、テストを実行します。

```json title="package.json"
"test:e2e:headed": "playwright test --headed",
```

```shell
npm run test:e2e:headed
```

正常に実行されるとブラウザーが起動し、以下のように結果が表示されます。

```text
Running 1 test using 1 worker
  1 passed (29.3s)

To open last HTML report run:

  npx playwright show-report
```
