---
title: Vue.js 開発手順
description: Vue.js を用いたクライアントサイドアプリケーションの開発手順を説明します。
---

# 静的コード分析とフォーマット {#top}

静的コード分析とフォーマットには .editorconfig 、 ESLint 、 StyleLint 、および Prettier を使用します。

## .editorconfigの追加 {#add-editorconfig}

プロジェクトのルートフォルダーに [.editorconfig](https://docs.microsoft.com/ja-jp/visualstudio/ide/create-portable-custom-editor-options) を追加することで、 IDE (Visual Studio や VSCode) に対してコーディングルールを課すことが可能になります。サンプルアプリケーションでは、.editorconfig によって以下のようなルールを定義しています。

- エンコード（UTF-8 に設定）
- 改行コード（LF に設定）
- 文末に空白行を追加
- インデントのサイズ（2 を設定）
- インデントの形式（空白を設定）
- 行末の空白を削除

.editorconfig の設定には、自動的に適用されるもの（例：エンコードや改行コード）と、違反すると IDE のエディター上に警告が表示されるもの（例：行末の空白）があります。

## Prettier {#prettier}

Prettier は Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

### Prettier の設定 {#settings-prettier}

Prettier の設定ファイルは初期状態では存在しないので、作成するところから始めます。

1. プロジェクトのルートフォルダーに設定ファイル ```./.prettierrc.js``` を作成します。
1. 既定の設定を上書きする場合、設定値を記述します。以下はサンプルアプリケーションでの設定例です。

```javascript
module.exports = {
  semi: true,
  arrowParens: 'always',
  singleQuote: true,
  trailingComma: 'all',
  endOfLine: 'auto',
};
```

全ての設定可能な値は [公式ドキュメント](https://prettier.io/docs/en/options.html) を参照してください。

## ESLint {#eslint}

ESLint は Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

### ESLint の設定 {#settings-eslint}

設定ファイル ```./.eslintrc.cjs``` で行います。このファイルはインストール時に自動的に追加され、デフォルトでは以下のような内容になっています（ESLint バージョン 8.5.0 の場合）。

```javascript
/* eslint-env node */
require('@rushstack/eslint-patch/modern-module-resolution')

module.exports = {
  root: true,
  'extends': [
    'plugin:vue/vue3-essential',
    'eslint:recommended',
    '@vue/eslint-config-typescript',
    '@vue/eslint-config-prettier'
  ],
  overrides: [
    {
      files: [
        'cypress/e2e/**.{cy,spec}.{js,ts,jsx,tsx}'
      ],
      'extends': [
        'plugin:cypress/recommended'
      ]
    }
  ],
  parserOptions: {
    ecmaVersion: 'latest'
  }
}
```

この状態でも静的コード分析は可能ですが、 postCSS の設定ファイルなど、分析する必要のないファイルまで分析対象となってしまうため、以下のように ignorePatterns を追加します（25 行目）。

```javascript
/* eslint-env node */
require('@rushstack/eslint-patch/modern-module-resolution')

module.exports = {
  root: true,
  'extends': [
    'plugin:vue/vue3-essential',
    'eslint:recommended',
    '@vue/eslint-config-typescript',
    '@vue/eslint-config-prettier'
  ],
  overrides: [
    {
      files: [
        'cypress/e2e/**.{cy,spec}.{js,ts,jsx,tsx}'
      ],
      'extends': [
        'plugin:cypress/recommended'
      ]
    }
  ],
  parserOptions: {
    ecmaVersion: 'latest'
  },
  ignorePatterns: ['postcss.config.js', 'tailwind.config.js'],
}
```

### ESLint と Prettier の連携 {#eslint-and-prettier}

Vue.js のブランクプロジェクト作成時に ESLint と Prettier をそれぞれオプションとしてインストールした場合、 ESLint と Prettier を連携させるプラグインが自動的にインストール・設定されます。
したがって、 ESLint と Prettier を連携させるための追加の設定は必要ありません。

## StyleLint {#stylelint}

### Stylelint のインストール {#install-stylelint}

Stylelint および、標準の設定や vue ファイルで使用する設定等をインストールします。サンプルアプリケーションでは以下をインストールしています。

| パッケージ名                      | 使用目的 |
|----------------------------------|----------|
|stylelint                         |cssファイルの構文検証|
|stylelint-config-standard         |Stylelint の標準設定|
|stylelint-config-prettier         |Stylelint の Ptettier 向け設定|
|stylelint-config-recommended-vue  |Stylelint の .vue ファイル向け推奨設定|
|stylelint-prettier                |Stylelint と Prettier の連携プラグイン|

```terminal
npm install -D stylelint \
  stylelint-config-standard \
  stylelint-config-prettier \
  stylelint-config-recommended-vue \
  stylelint-prettier
```

### StyleLint の設定 {#settings-stylelint}

プロジェクトのルートフォルダーに設定ファイル ```./.stylelintrc.js``` を作成し、コードを記述します。

```javascript
module.exports = {
  plugins: ['stylelint-prettier'],
  extends: [
    'stylelint-config-standard',
    'stylelint-config-recommended-vue',
    'stylelint-prettier/recommended',
  ],
  rules: {
    'prettier/prettier': true,
    'at-rule-no-unknown': [
      true,
      { ignoreAtRules: ['tailwind', 'define-mixin'] },
    ],
  },
  ignoreFiles: ['dist/**/*'],
  overrides: [
    {
      files: ['**/*.vue'],
      customSyntax: 'postcss-html',
    },
  ],
};
```

|プロパティ  |説明|
|-----------|---|
|plugins    |使用する外部のプラグインを宣言します。|
|extends    |既存の構成を拡張します。|
|rules      |使用するルールを宣言します。|
|ignoreFiles|分析の対象外とするファイルまたはフォルダーを設定します。|
|overrides  |特定のファイルにのみ別のルールを設定したい場合に使用します。|

## 静的コード分析とフォーマットの実行 {#static-code-analysis-and-format}

```./package.json``` に ESLint 用の script がデフォルトで追加されています。ここに Stylelint も同時に実行するようにコマンドを追加します。追加後の scripts は以下のようになります（関係のないコマンドは省略しています）。

```json
"scripts": {
  "lint": "eslint . --ext .vue,.js,.jsx,.cjs,.mjs,.ts,.tsx,.cts,.mts --fix --ignore-path .gitignore && stylelint **/*.{vue,css} --fix",
}
```

Stylelint を vue ファイルと css ファイルに対して実行するように設定しています。

ターミナルを開き、コマンドを実行します。

```terminal
npm run lint
```

ESLint および Stylelint のオプション引数に ```--fix``` を設定しているため、フォーマットが自動的に実行されます。フォーマットできない違反については、ターミナル上で結果が表示されます。
