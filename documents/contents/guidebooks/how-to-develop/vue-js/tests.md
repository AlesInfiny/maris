# テストの実行

## 単体テスト ## {: #unit-test }

### Vitest ### {: #vitest }

Vitest は、Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

単体テストのコードは ```src\components\__tests__``` に配置します。デフォルトではサンプルの単体テストコードが追加済みです。必要に応じて削除してください。

### Vitest の設定 ### {: #vitest-setting }

Vitest は、ビルドツール vite をベースに作成されています。そのため、Vitest のインストール時に vite も併せてインストールされ、vite の設定ファイルである「vite.config.ts」がデフォルトで追加されます。Vitest はこのファイルの設定を参照します。

もし、vite.config.ts の内容を上書きしたい場合は、Vitest 独自の設定ファイル「vitest.config.ts」を作成し、設定値を上書きしてください。詳細は [公式ドキュメント](https://vitest.dev/config/) を参照してください。

### 単体テストのコード ### {: #unit-test-code }

サンプルアプリケーションを参考にしてください。

### 単体テストの実行 ### {: #do-unit-test }

package.json の scripts セクションにタスク「test:unit」がデフォルトで追加されているので、これをコンソールで実行します（必要に応じて ```npm run build``` でアプリケーションをビルドします）。

```bash
npm run build
npm run test:unit
```

```src\components\__tests__``` 内のテストが自動的に実行され、結果がコンソールに表示されます。

## E2E (End-to-End) テスト ## {: #e2e-test }

E2E テストとは、アプリケーションで利用されるコンポーネントやレイヤーを全て結合した状態で、レイヤーの初めから終わりまで（End-to-End）行うテストです。

### Cypress ### {: #cypress }

Cypress は Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

### アプリケーションのビルドと Cypress 管理コンソールの起動 ### {: #build-application-and-serve-cypress-console }

package.json の scripts セクションにタスク「test:e2e」がデフォルトで追加されているので、これをコンソールで実行します（必要に応じて ```npm run build``` でアプリケーションをビルドします）。

```bash
npm run build
npm run test:e2e
```

上掲のコマンドは、Cypress の管理コンソール画面を起動すると同時に、アプリケーションを実行します。

Windows でセキュリティの警告が表示された場合は、「通信を許可する」にチェックを入れて「アクセスを許可する」をクリックします。

### E2E テストの作成と実行 ### {: #create-and-do-e2e-test }

E2E テストは Cypress の管理コンソール画面上で作成・実行します。詳しい作成方法は、[Cypress の公式ドキュメント](https://docs.cypress.io/guides/end-to-end-testing/writing-your-first-end-to-end-test#What-you-ll-learn) を参照してください。
また、E2E テストの実行方法についても、同様に[Cypress の公式ドキュメント](https://docs.cypress.io/guides/end-to-end-testing/testing-your-app) を参照してください。
