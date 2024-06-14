---
title: コーディング規約
description: AlesInfiny Maris OSS Edition のコーディング規約に関する方針を示します。
---

# コーディング規約 {#top}

AlesInfiny Maris OSS Edition では、静的コード分析ツールの既定のルール等をコーディング規約として採用し、必要に応じてカスタマイズしています。これによりコーディング規約の作成を省力化し、チェックを自動化しています。

.NET アプリケーション、 Vue.js アプリケーションそれぞれで以下の内容をコーディング規約の参考としています。

- .NET アプリケーション
    - .editorconfig

        .editorconfig を Visual Studio で生成した際の既定値
        [コードスタイルルールのオプション](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/code-style-rule-options)

    - StyleCopAnalyzers
  
        StyleCopAnalyzers の既定値

- Vue.js アプリケーション

    - .editorconfig

        [Vue.js の .editorconfig](https://github.com/vuejs/vue/blob/main/.editorconfig)

    - Prettier
  
        Vue.js ブランクプロジェクト作成時の既定値

    - ES Lint

        Vue.js ブランクプロジェクト作成時の既定値

    - StyleLint

        StyleLint の標準設定および vue ファイル向けの推奨設定（stylelint-config-standard, stylelint-config-recommended-vue）

コーディング規約の内容および静的コード分析ツールの詳しい設定方法については、以下のページとサンプルアプリの実装を参照してください。

- [静的コード解析用パッケージと設定ファイルの導入(.NET)](../how-to-develop/dotnet/project-settings.md#setup-static-code-testing)
- [静的コード分析とフォーマット(Vue.js)](../how-to-develop/vue-js/static-verification-and-format.md)
