# CSSの設定とCSSフレームワークの適用

## postCSS

postCSS は、CSS を操作するための JavaScript ベースのプラグインです。このプラグインの API を利用する多くのプラグインが公開されており、後述の autoprefixer もその１つです。

```cmd
npm install -D postcss
```

## autoprefixer

autoprefixer は、CSS に対してベンダープレフィクスを自動的に付与する postCSS のプラグインです。
このプラグインを使用することにより、ベンダープレフィクスを意識する必要が無くなります。

```cmd
npm install -D autoprefixer
```

!!! info "ベンダープレフィクス"
    ベンダープレフィクスとは、ブラウザーのベンダーが独自の拡張機能を実装する際に、
    それがブラウザー独自であることを明示するために付ける識別子のことです。
    たとえば、Chrome 独自の拡張機能の場合、 -webkit- というプレフィックスを使用します。

## postcss-nesting

postcss-nesting は、CSS Nestingの仕様に従って、スタイルルールを互いに入れ子にすることができる postcss のプラグインです。

```cmd
npm install -D postcss-nesting
```

## Tailwind CSS

Tailwind CSS は、Web サイトを構築するための CSS フレームワークです。

### Tailwind CSS のインストール

```cmd
npm install -D tailwindcss
```

### Tailwind CSS の設定

以下のコマンドを入力すると、tailwind.config.js ファイルが作成されます。

```cmd
npx tailwindcss init
```

content に、Tailwind CSS を適用するファイルのパス（ワイルドカード使用可）を設定します。

```javascript
module.exports = {
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {},
  },
  plugins: [],
};
```
