# 静的コード分析とフォーマット

静的コード分析とフォーマットには、.editorconfig、ESLint、StyleLint、および Prettier を使用します。

## .editorconfigの追加

.editorconfig を追加することで、IDE (Visual Studio や VSCode) に対してコーディングルールを課すことが可能になります。サンプルアプリケーションでは、.editorconfig によって以下のようなルールを定義しています。

- エンコード（UTF-8 に設定）
- 改行コード（LF に設定）
- 文末に空白行を追加
- インデントのサイズ（2 を設定）
- インデントの形式（空白を設定）
- 行末の空白を削除

.editorconfig の概要については [アプリケーション開発手順 .NET編](../dotnet/project-settings/#editorconfig) を参照してください。

## ESLint

ESLint は Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

### ESLint の設定

設定ファイル「.eslintrc.cjs」で行います。デフォルトの状態でも静的コード分析は実施可能です。

### ESLint と Prettier の連携

Vue.js のブランクプロジェクト作成時に ESLint と Prettier をそれぞれオプションとしてインストールした場合、ESLint と Prettier を連携させるプラグインが自動的にインストールされます。
そのため、

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

1. コードを記述します。以下はインストールしたオプションやプラグインを最低限動作させるために必要な設定例です。

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

## Prettier

Prettier は Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

### Prettier の設定

Prettier の設定ファイルは初期状態では存在しないので、作成するところから始めます。

1. 設定ファイル「.prettierrc.js」を作成します。
1. 上書きする設定値を記述します。以下はサンプルアプリケーションでの設定例です。

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

## 静的コード分析とフォーマットの実行

package.json に ESLint 用の script がデフォルトで追加されています。ここに Stylelint も実施するようコマンドを追加します。追加後の script は以下のようになります（以下の例では関係のない部分は省略しています）。

```json
"scripts": {
  "lint": "eslint . --ext .vue,.js,.jsx,.cjs,.mjs,.ts,.tsx,.cts,.mts --fix --ignore-path .gitignore && stylelint **/*.css --fix",
}
```

ターミナルを開き、コマンドを実行します。

```bash
npm run lint
```

ESLint および Stylelint のオプション引数に ```--fix``` を設定しているため、フォーマットが自動的に実行されます。フォーマットできない違反については、ターミナル上で結果が表示されます。
