---
title: CSR 編 - テスト
description: フロントエンドアプリケーションのテスト方針について解説します。
---
<!-- cspell:ignore Kent C. Dodds -->
# フロントエンドアプリケーションのテスト {#top}

本章では、フロントエンドアプリケーションのテスト方針について解説します。
AlesInfiny Maris では、継続的インテグレーションを目的として、静的テストから E2E テストの一部までを自動化し、品質と開発スピードの両立を目指します。
本章では自動テストが可能なテストレベルのみを扱い、システムテストや受け入れテストといったテストレベルについては扱いません。

| テストレベル       | 名称             | 自動／手動 | テストツール                           |
| ------------------ | :--------------- | ---------- | :------------------------------------- |
| 単体テスト ( UT0 ) | 静的テスト       | 自動       | リンター、フォーマッター、型チェッカー |
| 単体テスト ( UT0 ) | 単体テスト       | 自動       | Vitest                                 |
| 単体テスト ( UT0 ) | 機能内結合テスト | 自動       | Vitest Browser Mode                    |
| 単体テスト ( UT0 ) | 機能間結合テスト | 自動       | Vitest Browser Mode                    |
| 結合テスト ( ITa ) | E2E テスト       | 自動・手動 | Playwright                             |

テストレベルについては、[テスト方針 - テストレベル](../index.md#test-level) を参照してください。

## テスト戦略のモデル {#test-strategy-model}

<!-- textlint-disable ja-technical-writing/sentence-length -->

テスト戦略の代表的なモデルとして、[テストピラミッド :material-open-in-new:](https://web.dev/articles/ta-strategies?hl=ja#the_classic_the_test_pyramid){ target=_blank }が挙げられます。
テストピラミッドは単体テストを厚くし、結合テストや E2E テストを少数に絞るモデルであり、各テストの量を種類別に積み上げると、下図のようにピラミッド状の三角形を形成します。

 一方で、フロントエンドアプリケーションのテストの文脈でよく挙げられるモデルが、[テスティングトロフィー :material-open-in-new:](https://web.dev/articles/ta-strategies?hl=ja#testing_trophy){ target=_blank }です。
テスティングトロフィーは、[React Testing Library :material-open-in-new:](https://github.com/testing-library/react-testing-library){ target_blank }の開発者として知られる [Kent C. Dodds :material-open-in-new:](https://kentcdodds.com/){ target_blank }により提唱された、単体テストよりも結合テストを重視するモデルです。このことにより、同様に積み上げた場合に下図のように中腹部が膨らんだトロフィー状の形を形成します。

<!-- textlint-enable ja-technical-writing/sentence-length -->

![テストモデル](../../../../images/app-architecture/client-side-rendering/test-model-light.png){ width="800" loading=lazy }

一般に、バックエンドアプリケーションにはテストピラミッドが適し、フロントエンドアプリケーションにはテスティングトロフィーのほうが適している傾向にあると考えられます。というのも、バックエンドアプリケーションで最もテストすべき対象は業務ロジックである一方で、フロントエンドアプリケーションで最もテストすべき対象はユーザー体験を含めたユースケースだと考えられるからです。
バックエンドアプリケーションの業務ロジックは関数やクラス単体に閉じた設計がなされるため、単体テストによって検証可能です。

しかし、フロントエンドアプリケーションのユースケースは、ユーザーからの入力、コンポーネントの見た目の変化、業務ロジックの相互作用によって実現されるので、単体テストではアプリケーションの機能を効果的に検証できません。
よって、フロントエンドアプリケーションでは、コンポーネント・業務ロジック・ユーザーとのインタラクション間の結合テストを重視することによって、アプリケーションの機能をより効果的に検証できます。

以上から、コンポーネント志向のフロントエンドアプリケーションでは、テスティングトロフィーに従い、コンポーネント間、コンポーネントと業務ロジック間、コンポーネントとユーザーとのインタラクション間の結合テストを重視します。バックエンドアプリケーションにおいて用いられるアプリケーションとデータベースとの結合テストとは異なるため、注意してください。フロントエンドアプリケーションとバックエンドアプリケーション間の結合の妥当性は、 E2E テストによって検証します。

!!! warning "テスト戦略のアンチパターン"
    E2E テストおよび手動テストへの依存のリスクを説明します。

## アプリケーションの特性とテスト戦略の選択

アプリケーションの特性がテスト戦略とどのように関連するか述べます。

- 基幹システム
- 企業向け SaaS
- toC 向け Web サービス
  
- 業務領域
    - 金融・公共のような業務領域
    - リリースまでのハードルが高くなります。リリース頻度が少なくなります。

!!! note "継続的インテグレーション"
    [継続的インテグレーション :material-open-in-new:](https://developer.mozilla.org/en-US/docs/Glossary/Continuous_integration){ target=_blank }とは、変更をコードベースに頻繁に統合し、そのたびに自動ビルド・自動テストで検証するソフトウェア開発スタイルです。
    継続的インテグレーションを実行する目的は、より短い期間でより多くの機能を本番環境へリリースすることです。
    一方で、頻繁なコードベースへの変更には、既存の機能に対するリグレッションのリスクが伴います。
    このようなリスクを軽減し、品質を保証したうえで継続的インテグレーションを実現するためには、品質の保証に十分な数の自動テストを継続的にメンテナンスする必要があります。本章で紹介する下記のテストは、開発者のローカル環境および、 GitHub や Azure DevOps のような CI/CD 環境で自動実行できるようにします。コードベースへ変更を加える前にこれらの自動テストを実行することによって、リグレッションのリスクの低減とリリーススピードの両立を実現します。

## テストツール {#testing-tools}

テストの種類と目的に応じて適切なテストツールを採用します。
それぞれのテストツールについて説明します。

- [Prettier :material-open-in-new:](https://prettier.io/){ target_blank }
- [ESLint :material-open-in-new:](https://eslint.org/){ target_blank }
- [Stylelint :material-open-in-new:](https://stylelint.io/){ target_blank }
- [tsc(vue-tsc) :material-open-in-new:](https://github.com/vuejs/language-tools){ target_blank }
- [Vitest :material-open-in-new:](https://vitest.dev/){ target_blank }
- [Vitest Browser Mode :material-open-in-new:](https://vitest.dev/guide/browser/){ target_blank }
- [Vue Test Utils :material-open-in-new:](https://test-utils.vuejs.org/){ target_blank }
- [Playwright :material-open-in-new:](https://playwright.dev/){ target_blank }
- [Lighthouse CI :material-open-in-new:](https://github.com/GoogleChrome/lighthouse-ci/){ target_blank }

## 対象となるアプリケーション構成 {#target-application-structure}

CSR 編で扱うアプリケーションのコードベースは、次のようなフォルダー構成を想定します。
全体像は [フロントエンドアーキテクチャ - フォルダー構成](../../frontend-architecture.md#project-structure) を参照してください。

```text title="src 配下のフォルダー構成" linenums="0"
<project-name>
├─ src/
│  ├─ system-common/
│  ├─ business-common/
│  ├─ feature1/
│  ├─ feature2/
│  ├─ authentication/
│  ├─ basket/
│  ├─ catalog/
│  └─ ordering/ 
```

買い物かご機能、カタログ機能、注文機能はそれぞれ別のチームが開発することを想定します。

```text title="feature 配下のフォルダー構成" linenums="0"
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

たとえば買い物かご機能の機能内結合テストでは、買い物かご内の商品の量の変更をテストします。
買い物かご機能配下にない画面への遷移を伴う操作はテストしません。

機能間結合テストでは、

E2E テストでは、

以下では、テストの種類ごとに、目的、対象、利用するツールについて述べます。

## 静的テスト {#static-analysis}

プログラムを実行せずにソースコードを解析することで、不具合の原因となる記述や規約違反を検出します。

### 目的 {#stating-analysis-purpose}

- フォーマットを統一し、規約への準拠を保証する
- 不具合を早期に検出する

### 対象 {#static-analysis-targets}

コードベース全体を対象とします。

### 使用ツール {#static-analysis-tools}

- フォーマッター：設定に従ってコードのフォーマットを自動整形します。
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

機能を実現するロジックを検証します。
たとえば、分岐条件・バリデーションのルール・バックエンドの API レスポンスから画面へ詰め替える項目・ストアに保存する項目を検証します。

#### ソースコード {#unit-testing-targets-source}

- composables
- services
- stores
- plugins

これらはロジックの正しさが重要です。
そのため、コードカバレッジを重視します。
router は単なるルーティングの設定に近いので、単体テストの効果が低いです。

### 使用ツール {#unit-testing-tools}

- Vitest

## 機能間結合テスト {#integration-testing}

テスティングトロフィーで最も重視すべきテストの種類です。
バックエンドアプリケーションからの API レスポンスはモック化します。

### 目的 {#integration-testing-purpose}

- UI コンポーネント単位での振る舞いを確認する
- ユーザー操作に対する表示やイベント発火を確認する
- ロジックと描画の結び付きが正しいことを確認する

### 対象 {#integration-testing-targets}

ユースケースを実現するユーザーインタラクション・ロジック・コンポーネント間の結合を検証します。

#### ソースコード {#integration-testing-targets-source}

- components
- views

これらはロジックに加えて、描画結果や操作に対する振る舞いが重要です。
そのため、コードカバレッジではなく、ユースケースの網羅率を重視します。

### 使用ツール {#integration-testing-tools}

- Vitest
- Vitest Browser Mode
- Vue Test Utils

## 機能内結合テスト {#integration-testing}

テスティングトロフィーで最も重視すべきテストの種類です。
バックエンドアプリケーションからの API レスポンスはモック化します。

### 目的 {#integration-testing-purpose}

- UI コンポーネント単位での振る舞いを確認する
- ユーザー操作に対する表示やイベント発火を確認する
- ロジックと描画の結び付きが正しいことを確認する

### 対象 {#integration-testing-targets}

ユースケースを実現するユーザーインタラクション・ロジック・コンポーネント間の結合を検証します。

#### ソースコード {#integration-testing-targets-source}

- components
- views

これらはロジックに加えて、描画結果や操作に対する振る舞いが重要です。
そのため、コードカバレッジではなく、ユースケースの網羅率を重視します。

### 使用ツール {#integration-testing-tools}

- Vitest
- Vitest Browser Mode
- Vue Test Utils

## E2E テスト {#e2e-testing}

### 目的 {#e2e-testing-purpose}

- 単体テスト・結合テストでは検出できない不具合を検出する

### 対象 {#e2e-testing-targets}

一部を自動化します。
自動 E2E テストを実行するためには、バックエンドアプリケーションの起動、データベースの準備、外部サービスのモックといった準備が必要です。
そのため、実装・実行・保守ともに高いコストを要します。
よって、対象を必要最小限に絞り込むことが重要です。
対象を絞り込むために、下記の 2 種類[^3]に該当する対象を選定します。

1. ハッピーパス（ゴールデンパス）
テスト対象のアプリケーションにおいて、最も一般的であると判断されるユースケースです。
EC サイトであれば、「会員が商品を購入する」といったユースケースが考えられます。

1. ネガティブパス（Scary Path）
テスト対象のアプリケーションの異常系のユースケースのうち、想定外の動作をした場合にリスクの高いユースケースです。
EC サイトであれば、「会員が商品を購入する」といったユースケースにおいて、外部の決済サービスの不通により購入がエラーになった場合が考えられます。

この 2 種類の共通点は、システムが想定通りの動作をしなかった場合に業務上クリティカルな影響があることです。それゆえ、 自動化に高いコストを支払ったとしても、検証する価値があります。

### 使用ツール {#e2e-testing-tools}

- Playwright

[^3]:[Test paths: Typical kinds of test cases :material-open-in-new:](https://web.dev/articles/ta-test-cases#test_paths_typical_kinds_of_test_cases){ target_blank }
