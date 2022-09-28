# 開発に使用するパッケージ

## ブランクプロジェクト作成時にオプションとしてインストールされるパッケージ ## {: #optional-packages }

以下のパッケージは Vue.js のブランクプロジェクト作成時にオプションとしてインストールされます。

|パッケージ名 |説明    　　　　　　　　　　　　　　　　　　　　　　　　            |
|------------|------------------------------------------------------------------|
|Cypress     |E2E (End-to-End) テストツール                                      |
|ESLint      |JavaScript の静的検証                                              |
|JSX         |JavaScript ソースコード上で HTML タグを扱えるようにする             |
|Pinia       |Vue.js 用の状態管理ライブラリ                                      |
|Prettier    |コードファイルのフォーマット                                       |
|TypeScript  |JavaScript を拡張して静的型付にしたプログラミング言語               |
|Vitest      |Vite 環境で動作する高速テスティングフレームワーク                   |
|Vue Router  |Vue.js を利用した SPA で、ルーティング制御をするための公式プラグイン |

なお、上の表では、特定のパッケージをインストールすることで付随してインストールされるパッケージ（例： ESLint に対する eslint-config-prettier）は省略しています。

## 追加でインストールするパッケージ ## {: #additional-package }

以下のパッケージは別途インストールが必要です。

|パッケージ名                       |説明                                     |インストール|
|----------------------------------|-----------------------------------------|------------|
|Axios                             |Vue.js で非同期通信を行うためのプロミスベースのHTTPクライアント|``` npm install axios ```|
|autoprefixer                      |ベンダープレフィックスを付与するプラグイン |``` npm install -D autoprefixer ```|
|openapi-generator                 |Web API仕様からクライアントコードの自動生成|[参照](create-api-client-code.md)|
|postcss                           |CSSの最適化                               |``` npm install -D postcss ```|
|postcss-nesting                   |CSS最適化時にネストするプラグイン          |``` npm install -D postcss-nesting ```|
|stylelint                         |CSS の静的検証ツール                      |[参照](static-verification-and-format.md)|
|stylelint-config-standard         |Stylelint の標準設定|[参照](static-verification-and-format.md)|
|stylelint-config-prettier         |Stylelint の Ptettier 向け設定|[参照](static-verification-and-format.md)|
|stylelint-config-recommended-vue  |Stylelint の .vue ファイル向け推奨設定|[参照](static-verification-and-format.md)|
|stylelint-prettier                |Stylelint と Prettier の連携プラグイン|[参照](static-verification-and-format.md)|
|Tailwind CSS                      |CSSフレームワーク                         |``` npm install -D taiilwindcss ```|
|VeeValidate                       |Vue.js 用のリアルタイムバリデーションコンポーネントライブラリ |[参照](input-validation.md)
|yup                               |JavaScriptでフォームのバリデーションルールを宣言的に記述することのできるライブラリ|[参照](input-validation.md)|
