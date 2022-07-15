# API 定義からのクライアントコード生成

1. [Open API 仕様書の出力設定](../dotnet/configure-asp-net-core-web-api-project.md/#open-api-specification-output-configuration) に示す手順に従って生成した Open API 仕様書をローカルに保存します。ここでは、ファイル名をサンプルアプリケーション名と同じ「dressca-api.json」とします。

1. OpenAPI Generator をインストールします。ターミナルで以下のコマンドを入力します。

```cmd
npm install -D @openapitools/openapi-generator-cli
```

1. package.json の script セクションにタスクを追加します。なお、ここで追加しているオプションはサンプルアプリケーションでの設定値です。

```json
{
  "scripts": {
    "generate-client": "openapi-generator-cli generate -g typescript-axios -i dressca-api.json --additional-properties=withSeparateModelsAndApi=true,modelPackage=models,apiPackage=api,supportsES6=true -o ./src/api-client"
  }
}
```

1. ターミナルで以下のコマンドを実行します。

```cmd
npm run generate-client
```
