---
title: CSR 編 - テスト
description: フロントエンドアプリケーションのテスト方針について解説します。
---

# フロントエンドアプリケーションのテスト {#top}

フロントエンドアプリケーションでは、以下のテストを行います。
いずれのテストも、開発者のローカル環境と CI 環境で自動実行します。

| テストレベル | テスト名称           | 確認内容                                                         | 利用するツール                                  |
| ------------ | -------------------- | ---------------------------------------------------------------- | ----------------------------------------------- |
| UT0          | 静的テスト           | 不具合の原因となる記述や規約違反がないこと                       | Prettier 、 ESLint 、 Stylelint 、 vue-tsc      |
| UT0          | 単体テスト           | 分岐条件や入力値検証などのロジックが設計通りに動作すること       | Vitest                                          |
| UT1          | コンポーネントテスト | UI 、ロジック、子コンポーネントを結合したコンポーネントの成立    | Vitest 、 Vitest Browser Mode 、 Vue Test Utils |
| UT1          | 画面テスト           | モジュールを結合した画面の成立と、画面のレイアウト               | Vitest 、 Vitest Browser Mode 、 Vue Test Utils |
| UT1          | 画面遷移テスト       | ルーターやグローバル状態と結合した、画面遷移を伴うイベントの処理 | Vitest 、 Vitest Browser Mode 、 Vue Test Utils |
| ITa          | E2E テスト           | フロントエンドからバックエンドまで一気通貫したユースケースの実現 | Playwright                                      |

## 単体テストフェーズのテスト方針 {#ut-phase-policy}

- 単体テストとコンポーネントテストでは、コードカバレッジを重視します。
- Web API 呼び出しやグローバルな状態など、テスト対象の外部に依存する箇所はモック化します。
- コンポーネントテスト以降は、 Vitest Browser Mode の [Context API :material-open-in-new:](https://vitest.dev/api/browser/context.html#context-api){ target=_blank } を用いて、実ブラウザー上の操作に近い形で検証します。
