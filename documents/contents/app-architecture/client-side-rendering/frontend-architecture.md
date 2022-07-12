# フロントエンド

## 技術スタック ## {: #tech-stack }

Maris OSS 版を構成するOSSは以下のようになります。

![OSS構成要素](../../images/app-architecture/client-side-rendering/oss-components.png)

## アーキテクチャ ## {: #frontend-architecture }

### MVVMパターン ### {: #mvvm-pattern }

Maris OSS 版で採用しているVueのソフトウェア・アーキテクチャは MVVM パターンに分類されます。
レイヤー構成は以下のようになります。
![フロントエンド コンポーネント構成](../../images/app-architecture/client-side-rendering/frontend-architecture.png)

| 名称 | 説明 |
| ---- | ---- |
| ビュー           | ブラウザへのレンダリングおよびブラウザからのイベントの待ち受けを行います。Vue.jsの単一ファイルコンポーネントにテンプレートとして実装します。 |
| ビューモデル     | ブラウザからのイベントを受けて、レンダリングのための処理や入力チェックを行い、データ取得や更新処理をModelの呼び出しを通じて行います。Vue.jsの単一ファイルコンポーネントに実装します。 |
| モデル            | View間の引継ぎ情報を保存し、WebAPI呼び出しやWebAPI呼び出し結果のハンドリングを行います。Piniaアーキテクチャに従って実装する。 |

### ビュー（コンポーネント）＆ビューモデル（コンポーネント）### {: #view-and-viewmodel-component }

![MVVMパターン ビュー＆ビューモデル](../../images/app-architecture/client-side-rendering/mvvm-pattern.png)

フロントエンドのアーキテクチャのうち、ビューとビューモデルはそれぞれ画面要素の描画やその画面要素が実行するイベントの発火などを行うコンポーネントです。
画面は Frame / Page / Section / UI Parts という複数の画面コンポーネントから構成されます。
これらの画面コンポーネントが、デザインやデータバインドなどの画面表示と、イベント処理や入力処理などの画面要素に対する処理を持っています。

#### 画面コンポーネント #### {: #screen-components }

Vue.jsはコンポーネント指向のFWであることから画面要素をコンポーネントという再利用可能な単位で分割し、複数の画面コンポーネントを組み合わせることによって一つの画面を構成します。Maris OSS 版では、画面要素を役割ごとに「フレーム」「ページ」「セクション」「UI パーツ」という四つの画面コンポーネントに分割します。

| 名称 | 説明 |
| ---- | ---- |
| フレーム | ヘッダーやサイドバーなど画面レイアウト要素。 |
| ページ | 業務処理で切り替わる画面。セクションを参照。 |
| セクション | Pageを構成する画面要素のまとまり。UIパーツを参照。粒度の大きなセクションがより粒度の小さいセクションを参照することが可能。 |
| UI パーツ | 複数のページやセクションで利用するボタンやテキストボックスなどの汎用的な画面要素。 |

各画面コンポーネントは実際の画面では以下のようなイメージになります。

![画面コンポーネント イメージ](../../images/app-architecture/client-side-rendering/screen-component-detail.png)

#### モデル（コンポーネント）との連携 #### {: #linkage-with-model-component }

Vue ではバックエンドのアプリケーションとの連携をモデルが行います。そのため、ユーザーが行うビュー・ビューモデルからの処理や入力情報をモデルに連携する必要があります。この連携ではビューモデルのロジックから、モデルの Store の Getter や Action を呼び出して行います。

#### 画面遷移 #### {: #screen-transition }

画面遷移には、Vue Router という Vue.js の拡張ライブラリを利用します。Vue Router はルーティング定義に基づいて遷移先の画面コンポーネントを特定し、表示する画面コンポーネントを切り替えることで画面遷移を実現します。Vue Router による画面遷移はフロントエンドのみで完結するためバックエンドへの通信を行いません。またMaris OSS 版では、「ページ」を切り替えの単位としています。

![Vue Router によるルーティング](../../images/app-architecture/client-side-rendering/routing-by-vue-router.png)

#### 入力チェック #### {: #input-validation }

入力チェックには、 VeeValidate と yup というライブラリを利用します。
| 名前 | 説明 |
| ---- | ---- |
| VeeValidate | Vue.js用のリアルタイムバリデーションコンポーネントライブラリ。 |
| yup | JavaScriptでフォームのバリデーションルールを宣言的に記述することのできるライブラリ。 |



### モデル（コンポーネント） ### {: #model-component }

![MVVMパターン モデル](../../images/app-architecture/client-side-rendering/mvvm-pattern.png)

フロントエンドのアーキテクチャのうち、モデルはフロントエンドアプリケーションで扱うデータの状態管理や画面(ビュー)へデータを連携する役割を持つコンポーネントです。 Maris では Pinia という Vue.js の拡張ライブラリを利用します。

#### Storeの構成 #### {: #store-structure }

Pinia は入力途中の一時データや認証情報、エラー情報などアプリケーションで利用するデータを管理する領域である Store と、State・Getter・Actionという三つの要素から構成されています。

| 名称 | 説明 |
| ---- | ---- |
| State | Store で管理するデータそのもの。 |
| Getter | Store で管理しているデータである State を画面コンポーネント(ビュー & ビューモデル)に返すもの。 |
| Action | Store で管理しているデータである State に対して変更を行うもの。また API の呼び出しや API のレスポンスのハンドリングを行うもの。 |

#### APIの呼び出しについて ####{: #about-invoke-api }

## プロジェクト構成