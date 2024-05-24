---
title: Vue.js 開発手順
description: Vue.js を用いた クライアントサイドアプリケーションの 開発手順を説明します。
---

# プロジェクトの共通設定 {#top}

## TypeScript の設定 {#typescript-settings}

TypeScript で作成されたファイルは、 `tsconfig.json` の設定値をもとにコンパイルが実行されます。
`tsconfig.json` の存在するフォルダーとその配下のフォルダーの該当ファイルに設定が適用されます。

[ブランクプロジェクトの作成](./create-vuejs-blank-project.md) の手順に沿って `create-vue` でプロジェクトを作成すると、以下の `tsconfig.json` および `tsconfig.*.json` が生成されます。
各 `tsconfig.*.json` には `include` に指定したファイル群のコンパイルに関する設定値が定義されています。

```terminal linenums="0"
<project-name>
├ cypress
|  └ tsconfig.json--------- 単体テストの TypeScript として読み込む対象を定義する設定ファイル(Cypress 用)
├ tsconfig.app.json ------- アプリケーションの TypeScript として読み込む対象を定義する設定ファイル
├ tsconfig.node.json ------ TypeScript の設定ファイルとして読み込む対象を定義する設定ファイル
├ tsconfig.json ----------- TypeScript の設定ファイル
└ tsconfig.vitest.json ---- 単体テストの TypeScript として読み込む対象を定義する設定ファイル(vitest 用)
```

自動生成されたルートの `tsconfig.json` では、 Project Reference 機能により `references` に指定された `tsconfig.*.json` を参照します。
つまり、 TypeScript プロジェクトを `references` で指定したファイルに基づいて論理分割しています。
論理分割することにより、以下のような利点があります。

- アプリケーションコードからテストコードを参照するような歪な依存関係を防ぐ
- ビルド時のパフォーマンスを改善する

Project Reference 機能については[Project References :material-open-in-new:](https://www.typescriptlang.org/docs/handbook/project-references.html){ target=_blank }を参照してください。

なお、 `tsconfig.app.json` `tsconfig.node.json` には npm パッケージで提供されている tsconfig を継承するように設定されているため、継承元の設定値が存在します。
`extends` に定義されている継承元ファイルを参照して実際の設定値を確認できます。
また、 `references` で参照されているファイルでは `compilerOptions.composite` を `true` に設定する必要があります。

![tsconfigの継承関係](../../../images/guidebooks/how-to-develop/vue-js/vue-tsconfig-light.png#only-light){ loading=lazy }
<!-- ![tsconfigの継承関係](../../../images/guidebooks/how-to-develop/vue-js/vue-tsconfig-dark.png#only-dark){ loading=lazy } -->

### tsconfig の設定値の解説 {#tsconfig}

- `compilerOptions.noEmit`

    コンパイルの結果を出力しないよう設定するプロパティです。デフォルト値の `true` を設定します。
    ビルドで利用する package.json に定義されたスクリプトでは Vite でトランスパイルを行い、型チェックには `vue-tsc` が利用されるためです。
    Vite では `esbuild` または `rollup` でトランスパイルを行い、`tsc` を型チェックのみに利用するため、トランスパイルの結果を出力しません。

- `compilerOptions.tsBuildInfoFile`
  
    ビルド結果の差分を示す `.tsbuildinfo` ファイルの出力先を指定するプロパティです。
    `noEmit` を `true` としていても、出力先を明示しない場合は tsconfig.json と同じフォルダーに `.tsbuildinfo` ファイルが出力されます。
    そのため、デフォルト値である node_modules 配下の一時フォルダーを指定し、不要なファイルの出力を防いでいます。

- `compilerOptions.module`
  
    コンパイルしたファイルのモジュールシステムを設定するプロパティです。
    tsconfig.node.json で `ESNext` 、 tsconfig.json で `NodeNext` がデフォルトで設定されます。
    Cypress が内部で利用している `ts-node` の挙動の都合上、 tsconfig.json に `compilerOptions.module` が設定されています。

- `compilerOptions.moduleResolution`
  
    モジュール解決の方針を設定するプロパティです。
    tsconfig.node.json ではデフォルトで Vite での利用が推奨されている `Bundler` に設定されています。`Bundler` についての詳細は[--moduleResolution bundler :material-open-in-new:](https://www.typescriptlang.org/docs/handbook/release-notes/typescript-5-0.html#--moduleresolution-bundler){ target=_blank }を参照してください。

??? note "tsconfig.json の設定例"

    ``` json title="tsconfig.json"
    {
      "files": [],
      "references": [
        {
          "path": "./tsconfig.node.json"
        },
        {
          "path": "./tsconfig.app.json"
        },
        {
          "path": "./tsconfig.vitest.json"
        }
      ],
      "compilerOptions": {
        "module": "NodeNext"
      }
    }
    ```

??? note "tsconfig.app.json の設定例"

    - verbatimModuleSyntax
        
        デフォルトの設定では `true` が設定されており、型のみを import する場合に import type 構文またはインライン type 修飾子が付いていないとエラーになります。
        このオプションは元々`importsNotUsedAsValues` `preserveValueImports` `isolatedModules` の3つのオプションを用いて制御していた挙動を簡略化したものです。
        詳細は [Verbatim Module Syntax :material-open-in-new:](https://www.typescriptlang.org/tsconfig/#verbatimModuleSyntax){ target=_blank } を参照してください。
        しかし、現在 `openapi-generator-cli` で自動生成されたコードでは `import` と `import type` が区別されないため、暫定的に `false` を設定しています。

    ``` json title="tsconfig.app.json"　hl_lines="13"
    {
      "extends": "@vue/tsconfig/tsconfig.dom.json",
      "include": ["env.d.ts", "src/**/*", "src/**/*.vue", "mock/**/*"],
      "exclude": ["src/**/__tests__/*"],
      "compilerOptions": {
        "composite": true,
        "tsBuildInfoFile": "./node_modules/.tmp/tsconfig.app.tsbuildinfo",
        "baseUrl": ".",
        "paths": {
        "@/*": ["./src/*"]
        },
        "preserveValueImports": false,
        "verbatimModuleSyntax": false
      }
    }
    ```

??? note "tsconfig.node.json の設定例"

    AlesInfiny Maris では、フロントエンドアプリを mock モードでビルドする際のソースコードを `mock` 配下に含みます。
    本来 tsconfig.node.json は設定ファイルとして読み込む対象を定義していますが、vite.config.ts の参照先で mock を参照している都合上、 `"mock/**/*"` を include の対象にしています。

    ``` json title="tsconfig.node.json" hl_lines="4"
    {
      "extends": ["@tsconfig/node20/tsconfig.json"],
      "include": ["vite.config.*", "vitest.config.*", "cypress.config.*",
      "src/generated/api-client/**/*","mock/**/*","vite-plugins/*"],
      "compilerOptions": {
        "composite": true,
        "noEmit": true,
        "tsBuildInfoFile": "./node_modules/.tmp/tsconfig.node.tsbuildinfo",
        "module": "ESNext",
        "moduleResolution": "Bundler",
        "types": ["node"],
      }
    }
    ```

??? note "tsconfig.vitest.json の設定例"

    ``` json title="tsconfig.vitest.json"
    {
      "extends": "./tsconfig.app.json",
      "exclude": [],
      "compilerOptions": {
        "composite": true,
        "tsBuildInfoFile": "./node_modules/.tmp/tsconfig.vitest.tsbuildinfo",
        "lib": [],
        "types": ["node", "jsdom"],
      }
    }
    ```

## Vite の設定 {#vite-settings}

[ブランクプロジェクトの作成](./create-vuejs-blank-project.md) の手順に沿って `create-vue` でプロジェクトを作成すると、プロジェクトルートに `vite.config.ts` が生成されます。
`vite.config.ts` に設定を追加することでビルド時の設定が定義できます。
`vite` コマンドを実行する際、プロジェクトルートの `vite.config.ts` の設定値が自動的に読み込まれます。

### vite.config の設定値の解説 {#vite-config}

- [条件付き設定 :material-open-in-new:](https://ja.vitejs.dev/config/#%E6%9D%A1%E4%BB%B6%E4%BB%98%E3%81%8D%E8%A8%AD%E5%AE%9A){ target=_blank }

    コマンドやモードに応じて異なる設定を適用する場合、関数を export して設定します。

    ``` ts
    export default defineConfig(({ command, mode, isSsrBuild, isPreview }) => {
      if (command === 'serve') {
        return {
        // 固有の設定
        }
      } else {
        // ...
      }
    })
    ```

    設定例では mock モードでビルドした際に、デフォルトのプラグインに加えてモック用に定義したプラグインを読み込んでいます。

    なお、条件付き設定のために関数を export する際は `vitest.config.ts` の実装も変更が必要です。
    `vitest.config.ts` の設定については[Managing Vitest config file :material-open-in-new:](https://vitest.dev/config/file.html){ target=_blank }を参照してください。

- `loadEnv()`

    モードに応じた `.env.*` ファイルを読み込み、環境変数を取得します。
    詳しくは[環境変数を設定に使用する :material-open-in-new:](https://ja.vitejs.dev/config/#%E7%92%B0%E5%A2%83%E5%A4%89%E6%95%B0%E3%82%92%E8%A8%AD%E5%AE%9A%E3%81%AB%E4%BD%BF%E7%94%A8%E3%81%99%E3%82%8B){ target=_blank }を参照してください。

- [resolve.alias :material-open-in-new:](https://ja.vitejs.dev/config/shared-options.html#resolve-alias){ target=_blank }

    パスエイリアスを設定して、 import するパスの指定を簡潔にできます。

- [server.proxy :material-open-in-new:](https://ja.vitejs.dev/config/server-options.html#server-proxy){ target=_blank }

    特定のパスで始まるリクエストの振り分け先を指定できます。
    設定例では `/api`, `/swagger` で始まるパスのリクエストをバックエンドアプリで処理するよう指定しています。

??? note "vite.config.ts の設定例"

    ``` ts title="vite.config.ts"
    import { fileURLToPath, URL } from 'url';

    import { defineConfig, loadEnv } from 'vite';
    import vue from '@vitejs/plugin-vue';
    import vueJsx from '@vitejs/plugin-vue-jsx';
    import { setupMockPlugin } from './vite-plugins/setup-mock';

    // https://vitejs.dev/config/
    export default defineConfig(({ mode }) => {
      const plugins = [vue(), vueJsx()];
      const env = loadEnv(mode, process.cwd());

      return {
        plugins: mode === 'mock' ? [...plugins, setupMockPlugin()] : plugins,
        resolve: {
          alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url)),
          },
        },
        server: {
          proxy: {
            '/api': {
              target: env.VITE_PROXY_ENDPOINT_ORIGIN,
              changeOrigin: true,
              configure: (proxy, options) => {
                options.autoRewrite = true;
                options.secure = false;
              },
            },
            '/swagger': {
              target: env.VITE_PROXY_ENDPOINT_ORIGIN,
              changeOrigin: true,
              secure: false,
            },
          },
        },
      };
    });

    ```
