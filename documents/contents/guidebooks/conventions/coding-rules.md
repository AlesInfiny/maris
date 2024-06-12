---
title: コーディング規約
description: AlesInfiny Maris OSS Edition のコーディング規約に関する方針を示します。
---

# コーディング規約 {#top}

AlesInfiny Maris OSS Edition では、静的コード分析ツールを利用してコーディング規約のチェックを自動化しています。
.NET アプリケーションでは .editorconfig と StyleCopAnalyzers 、 Vue.js アプリケーションでは Prettier 、 ES Lint 、 StyleLint を利用しています。詳しい設定方法については、アプリケーション開発手順の以下のページを参照してください。

- [静的コード分析とフォーマット(Vue.js)](../how-to-develop/vue-js/static-verification-and-format.md)
- [静的コード解析用パッケージと設定ファイルの導入(.NET)](../how-to-develop/dotnet/project-settings.md#setup-static-code-testing)

また、.NET アプリケーションでは .editorconfig や StyleCopAnalyzers を VisualStudio で導入した際の既定値をコーディング規約として採用しています。 Vue.js アプリケーションでのコーディング規約および静的コード分析ツールの詳しい設定方法については、上記のアプリケーション開発手順のページとサンプルアプリの実装を参照してください。
