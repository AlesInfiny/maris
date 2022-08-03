# テストの実行

## 単体テスト

### Vitest

Vitest は、Vue.js のブランクプロジェクト作成時にオプションとしてインストールしているため、追加でインストールする必要はありません。

単体テストのコードは ```src\components\__tests__``` に配置します。デフォルトではサンプルの単体テストコードが追加済みです。必要に応じて削除してください。

### Vitest の設定

Vitest は、ビルドツール vite をベースに作成されています。そのため、Vitest のインストール時に vite も併せてインストールされ、vite の設定ファイルである「vite.config.ts」がデフォルトで追加されます。Vitest はこのファイルの設定を参照します。

もし、vite.config.ts の内容を上書きしたい場合は、Vitest 独自の設定ファイル「vitest.config.ts」を作成し、設定値を上書きしてください。詳細は [公式ドキュメント](https://vitest.dev/config/) を参照してください。

### 単体テストの実行

package.json の scripts セクションにタスク「test:unit」がデフォルトで追加されているので、これをコンソールで実行します。

```bash
npm run test:unit
```

```src\components\__tests__``` 内のテストが自動的に実行され、結果がコンソールに表示されます。
