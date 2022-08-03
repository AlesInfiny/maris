# 静的コード分析とフォーマット

静的コード分析とフォーマットには、.editorconfig、ESLint、StyleLint、および Prettier を使用します。

## .editorconfigの追加

プロジェクトのルートフォルダーに [.editorconfig](https://docs.microsoft.com/ja-jp/visualstudio/ide/create-portable-custom-editor-options) を追加することで、IDE (Visual Studio や VSCode) に対してコーディングルールを課すことが可能になります。サンプルアプリケーションでは、.editorconfig によって以下のようなルールを定義しています。

- エンコード（UTF-8 に設定）
- 改行コード（LF に設定）
- 文末に空白行を追加
- インデントのサイズ（2 を設定）
- インデントの形式（空白を設定）
- 行末の空白を削除

## Prettier

Prettier は Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

### Prettier の設定

Prettier の設定ファイルは初期状態では存在しないので、作成するところから始めます。

1. 設定ファイル「.prettierrc.js」を作成します。
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

## ESLint

ESLint は Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

### ESLint の設定

設定ファイル「.eslintrc.cjs」で行います。このファイルはデフォルトで追加済みであり、変更しなくても静的コード分析は実施可能です。他のパッケージとの連携など、必要に応じて変更してください。

### ESLint と Prettier の連携

Vue.js のブランクプロジェクト作成時に ESLint と Prettier をそれぞれオプションとしてインストールした場合、ESLint と Prettier を連携させるプラグインが自動的にインストール・設定されます。
したがって、ESLint と Prettier を連携させるための追加の設定は必要ありません。

## StyleLint

### Stylelint のインストール

Stylelint および、標準の設定や vue ファイルで使用する設定等をインストールします。サンプルアプリケーションでは以下をインストールしています。

| パッケージ名                      | 使用目的 |
|----------------------------------|----------|
|stylelint                         |cssファイルの構文検証|
|stylelint-config-standard         |Stylelint の標準設定|
|stylelint-config-prettier         |Stylelint の Ptettier 向け設定|
|stylelint-config-recommended-vue  |Stylelint の .vue ファイル向け推奨設定|
|stylelint-prettier                |Stylelint と Prettier の連携プラグイン|

```bash
npm install --save-dev stylelint \
  stylelint-config-standard \
  stylelint-config-prettier \
  stylelint-config-recommended-vue \
  stylelint-prettier
```

### StyleLint の設定

Stylelint の設定ファイルは初期状態では存在しないので、作成するところから始めます。

1. 設定ファイル「.stylelintrc.js」 を作成します。

1. コードを記述します。以下はインストールしたオプションやプラグインを最低限動作させるために必要な設定例です。他のパッケージとの連携など、必要に応じて設定を変更してください。

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
    },
};
```

## 静的コード分析とフォーマットの実行

package.json に ESLint 用の script がデフォルトで追加されています。ここに Stylelint も同時に実行するようコマンドを追加します。追加後の script は以下のようになります。

```json
"scripts": {
  "lint": "eslint . --ext .vue,.js,.jsx,.cjs,.mjs,.ts,.tsx,.cts,.mts --fix --ignore-path .gitignore && stylelint **/*.{vue,css} --fix",
}
```

上の例では、関係のないコマンドは省略しています。また、Stylelintを vue ファイルと css ファイルに対して実行するように設定しています。

ターミナルを開き、コマンドを実行します。

```bash
npm run lint
```

ESLint および Stylelint のオプション引数に ```--fix``` を設定しているため、フォーマットが自動的に実行されます。フォーマットできない違反については、ターミナル上で結果が表示されます。
