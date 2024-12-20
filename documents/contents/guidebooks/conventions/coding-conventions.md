---
title: コーディング規約
description: AlesInfiny Maris OSS Edition のコーディング規約に関する方針を示します。
---

# コーディング規約 {#top}

AlesInfiny Maris OSS Edition （以下 AlesInfiny Maris ）では、一般に広く採用されている規約に準拠し、必要に応じて最低限のカスタムルールを加えることを基本方針とします。
ゼロから独自規約を作成することは、以下のような問題があるため推奨しません。

- 規約作成にかかる負荷が大きい
- 必要な規約の漏れが発生しやすい
- 機械的なチェックの仕組みを作りにくい

## AlesInfiny Maris で採用している規約 {#style-guide}

.NET アプリケーション、 Vue.js アプリケーションそれぞれで以下の内容を基本のコーディング規約としています。

- .NET アプリケーション
    - .editorconfig

        .editorconfig を Visual Studio で生成した際の既定値

        [コードスタイルルールのオプション :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/code-style-rule-options){ target=_blank }

        [コードスタイルの規則 :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/style-rules/){ target=_blank }

    - StyleCopAnalyzers

        [StyleCopAnalyzers 既定で適用されるルール :material-open-in-new:](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/DOCUMENTATION.md){ target=_blank }

- Vue.js アプリケーション
    - [Airbnb JavaScript Style Guide :material-open-in-new:](https://github.com/airbnb/javascript){ target=_blank }

        Airbnb が GitHub 上に公開している JavaScript のスタイルガイドです。コードのフォーマットに対する規約および、バグを防止するための規約を定めています。あわせて、これらの規約に従うための ESLint の設定が公開されています。

    - [Vue.js スタイルガイド :material-open-in-new:](https://ja.vuejs.org/style-guide/){ target=_blank }

        Vue.js が公式に提供するスタイルガイドです。 JavaScript に対する規約ではカバーできない Vue 固有の記法について、エラーの発生やアンチパターンを避けるための規約を優先度別に定めています。 これらの規約への違反を検出するための ESLint のプラグインも公式に提供されています。

    - [CSS specifications :material-open-in-new:](https://www.w3.org/Style/CSS/current-work){ target=_blank }

        W3C が策定する CSS の標準仕様です。 Stylelint では、この標準仕様に従うための設定が公開されています。

上記のコーディング規約は静的コード解析ツールによって自動的にチェックできるようにします。
バックエンド側では Visual Studio でのコーディング中やビルド時に .NET コンパイラーによる自動チェックが行われます。
フロントエンド側では Prettier 、 ES Lint 、 Stylelint を利用してコーディング規約の自動チェックを行っています。
コーディング規約の内容および静的コード解析ツールの詳しい設定方法については、以下のページとサンプルアプリの実装を確認してください。

- [静的コード解析用パッケージと設定ファイルの導入(.NET)](../how-to-develop/dotnet/project-settings.md#setup-static-code-testing)
- [静的コード分析とフォーマット(Vue.js)](../how-to-develop/vue-js/static-verification-and-format.md)
