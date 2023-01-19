---
title: フロントエンドのテスト
description: クライアントサイドレンダリングアプリケーションにおけるフロントエンドのテストについて解説します。
---

疑問

- 静的テスト → 単体テスト(関数(util, store)、コンポーネント) → 結合テスト(コンポーネント(主にView, 複合component)) → E2Eテスト
- API の疎通テストはどこで行う？

# フロントエンドのテスト {#top}

フロントエンドも複雑なロジックを持つことが避けられなくなっている。そのためバックエンドと同様にテストを行い、品質を保持することが必要。

テスティングトロフィー

![テスティングトロフィー](../../images/app-architecture/client-side-rendering/testing-trophy-light.png)

## 静的テスト {#static-test}

## ユニットテスト {#unit-test}

### Vueコンポーネントテスト {#component-test}

Vue コンポーネントのテストでは、ロジックを１つずつテストするのではなく、コンポーネントに対する入力と出力にフォーカスを当てテスト対象を決定します。 Vue Test Utils では以下のように説明しています。

コンポーネントのパブリックインターフェイスを検証するテストを作成し、内部をブラックボックスとして扱うことをお勧めします。単一のテストケースでは、コンポーネントに提供された入力（ユーザーのやり取りやプロパティの変更）によって、期待される出力（結果の描画またはカスタムイベントの出力）が行われることが示されます。
[Vue Test Utils - 一般的なヒント](https://v1.test-utils.vuejs.org/ja/guides/common-tips.html#%E4%BD%95%E3%82%92%E3%83%86%E3%82%B9%E3%83%88%E3%81%99%E3%82%8B%E3%81%8B%E3%82%92%E7%9F%A5%E3%82%8B)

一般的な SPA を例にして入力と出力をまとめます。

入力：

| 名称 | 説明 |
| ---- | ---- |
| プロパティ | 親コンポーネントから渡されるもの。Vue の場合 props に相当。 |
| DOM | ユーザーによるUI操作により発生。クリックアクションやフォームの入力など。 |
| Route | URL が変更することにより発生。 |
| Store | Store の変更もコンポーネントへの入力となりうる。 |

出力：

| 名称 | 説明 |
| ---- | ---- |
| イベント | 親コンポーネントへ渡す。Vue の場合 emit に相当。 |
| DOM | 示されたテキスト、要素のOn/Off、スタイル、属性など。 |
| Route | Router へのアクション。 |
| Store | Store の変更、API の呼び出しなど。 |

フロントエンドにおいては、入力はユーザーが行った入力や操作、出力はそれに対する画面に反映する結果ということになります。フロントエンドのテストでは、その間に行われたロジックやデータ・状態の変化はブラックボックスにしてしまうことが理想的とされています。

また、[Angular ドキュメント](https://angular.io/guide/testing-components-scenarios#component-testing-scenarios)では以下のように一般的なテストシナリオとしてテストを行うべきコンポーネントの例が紹介されています。

| テストシナリオ | 説明 |
| ------------- | ---- |
| Component binding | DOM の操作 / 評価 |
| Component with a dependency | 依存関係のあるコンポーネント |
| Component with async service | 非同期処理を扱うサービスを利用しているコンポーネント |
| Component with inputs and outputs | 親コンポーネントから値を受け取る、親コンポーネントにイベントを渡すコンポーネント |
| Routing component | Router を扱うコンポーネント |
| Routed components | ルート設定されているコンポーネント |
| Nested component tests | 他のコンポーネントを内部で利用する、ネストされたコンポーネント |
| Components with RouterLink | Vue の場合 ```<router-link :to="...">``` を使うコンポーネント |
| Use a page object | Page オブジェクトを利用して要素操作に関するロジックを分離する |

どのテストレベルでどのシナリオを行うかの指標が欲しい

### Storeのテスト {#testing-store}

pinia を対象とする。 store では多くの場合、重要なロジックは actions 集まっているため、これをテスト対象とする。このテストはバックエンドの単体テストと同等。

### 非同期のテストについて {#asynchronous-test}

### テストダブルについて {#test-double}

- 何をモックすべきか

## インテグレーションテスト

- ルーティング（URL変更による

## E2Eテスト {#e2e-test}

未検討
cypress (か playwright) を使う。

フロントエンド完結

〇　フロントーバック　自動化（ツール）をおすすめする
