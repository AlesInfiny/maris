---
title: CSR 編 - テスト
description: フロントエンドアプリケーションのテスト方針について解説します。
---

# フロントエンドアプリケーションのテスト {#top}

フロントエンドアプリケーションのテスト方針について解説します。

## テスト戦略の考え方 {#test-strategy-overview}

テスト戦略を考える際の代表的なモデルとして、テストピラミッドとテスティングトロフィーがあります。
テストピラミッドは単体テストを厚くし、結合テストや E2E テストを少数に絞る考え方です。
一方、テスティングトロフィーは、単体テストよりも結合テストを重視する考え方です。

TODO：それぞれ誰が言い出したのかについて、歴史的経緯を説明します。

[![テストモデル](../../../../images/app-architecture/client-side-rendering/test-model-light.png){ width="800" loading=lazy }](../../../../images/app-architecture/client-side-rendering/test-model-light.png){ align=center }

一般に、バックエンドアプリケーションではテストピラミッドが適している一方で、フロントエンドアプリケーションではテスティングトロフィーのほうが適しています。

TODO:なぜかを説明します。

## 継続的インテグレーション {#continuous-integration}

[継続的インテグレーション](https://developer.mozilla.org/en-US/docs/Glossary/Continuous_integration){ target=_blank }とは、頻繁に本番環境のコードベースに対して変更を加えるソフトウェア開発スタイルです。
継続的インテグレーションを実行する目的は、より短い期間でより多くの機能を本番環境へリリースすることです。
一方で、頻繁なコードベースへの変更には、既存の機能に対するリグレッションのリスクが伴います。
このようなリスクを軽減し、品質を保証したうえで継続的インテグレーションを実現するためには、品質の保証に十分な数の自動テストを継続的にメンテナンスする必要があります。
本章で紹介する下記のテストは、開発者のローカル環境および、 GitHub や Azure DevOps のような CI / CD 環境で自動実行できるようにします。
コードベースへ変更を加える前にこれらの自動テストを実行することによって、リグレッションのリスクの低減とリリーススピードの両立を実現します。

1. 静的解析
1. 単体テスト
1. コンポーネントテスト
1. ビジュアルリグレッションテスト
1. アクセシビリティテスト
1. E2E テスト

## 対象となるアプリケーション構成 {#target-application-structure}

CSR 編で扱うアプリケーションのコードベースは、次のようなフォルダー構成を想定します。
全体像は [フロントエンドアーキテクチャ - フォルダー構成](../../frontend-architecture.md#project-structure) を参照してください。

```text title="src 配下のフォルダー構成" linenums="0"
<project-name>
├─ src/
│  ├─ components/ ------------ 再利用性の高い Vue コンポーネントを格納します。
│  ├─ composables/------------ 状態を持つロジックを再利用するための関数を格納します。
│  ├─ plugins/    ------------ アプリ全体に横断的な機能を格納します。
│  ├─ router/ ---------------- ルーティング定義を格納します。
│  ├─ services/ -------------- ページとストアの処理を中継するサービスを格納します。
│  ├─ stores/ ---------------- ストアの定義を格納します。
│  └─ views/ ----------------- ルーティング定義に対応するページコンポーネントを格納します。
```

以下では、テストの種類ごとに、目的、対象、利用するツールについて述べます。

## 静的解析 {#static-analysis}

プログラムを実行せずにソースコードを解析することで、不具合の原因となる記述や規約違反を検出します。

### 目的 {#stating-analysis-purpose}

- 開発者間の差異を統一し、規約への準拠を保証する
- 不具合を早期に検出する

### 対象 {#static-analysis-targets}

コードベース全体を対象とします。

### 使用ツール {#static-analysis-tools}

- フォーマッター：設定に従ってコードのフォーマットを自動整形します。
    - EditorConfig
    - Prettier
- リンター：規約違反や記述ミス、保守性の低い書き方、潜在的な不具合につながるコードを検出します。一部は自動修正できます。
    - ESLint
    - Stylelint
- 型チェッカー：型の整合性を検証し、型の不一致による問題を静的に検出します。
    - tsc
    - vue-tsc

## 単体テスト {#unit-testing}

アプリケーションを構成するロジックを、個々のモジュール単位で検証します。
データ取得やグローバルな状態など、個々のモジュールの外部に依存する箇所はモック化してテストします。

### 目的 {#unit-testing-purpose}

- ロジックの正しさを高速に検証する
- 異常系や境界値を網羅しやすくする
- 不具合の原因箇所を特定しやすくする

### 対象 {#unit-testing-targets}

- composables
- plugins
- services
- stores
- router

これらはロジックの正しさが重要です。

### 使用ツール {#unit-testing-tools}

- Vitest

## コンポーネントテスト {#component-testing}

最も重要です。
コンポーネントが、 props 、 state 、ユーザー操作に応じて期待通りに描画・動作することを検証します。

### 目的 {#component-testing-purpose}

- UI 部品単位での振る舞いを確認する
- ユーザー操作に対する表示やイベント発火を確認する
- ロジックと描画の結び付きが正しいことを確認する

### 対象 {#component-testing-targets}

- components

これらはロジックに加えて、描画結果や操作に対する振る舞いも重要です。

### 使用ツール {#component-testing-tools}

- Vitest
- Vitest Browser Mode
- Vue Test Utils

## ビジュアルリグレッションテスト {#visual-regression-testing}

画面やコンポーネントの見た目を画像として比較し、意図しない差分を検出します。

### 目的 {#visual-regression-testing-purpose}

- レイアウト崩れやスタイル崩れを検出する

### 対象 {#visual-regression-testing-targets}

- components
- views

### 使用ツール {#visual-regression-testing-tools}

- Vitest Browser Mode を用いたスクリーンショット比較

    - components に対して実行します。

- Playwright

    - views に対して実行します。

## アクセシビリティテスト {#accessibility-testing}

### 目的 {#accessibility-testing-purpose}

### 対象 {#accessibility-testing-targets}

### 使用ツール {#accessibility-testing-targets}

## E2E テスト {#e2e-testing}

実際の利用に近い形でアプリケーション全体を操作し、ユースケース単位で動作を検証します。

### 目的 {#e2e-testing-purpose}

- 画面遷移を含む一連のユーザー操作を確認する
- 認証、 API 通信、状態遷移を含む全体動作を確認する
- 単体テストやコンポーネントテストでは検出できない結合部分の不具合を検出する

### 対象 {#e2e-testing-targets}

- アプリケーション全体

ユースケース単位でテストします。
E2E テストは時間を要するので、対象を必要最小限に絞り込むことが重要です。
不具合が発生した場合に業務上クリティカルな影響が出るシナリオを選定します。
たとえば、 EC サイトでは注文などです。

### 使用ツール {#e2e-testing-tools}

- Playwright
