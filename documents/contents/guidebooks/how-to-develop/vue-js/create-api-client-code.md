# API 仕様書からのクライアントコード生成

## 事前準備

1. [Open API 仕様書の出力設定](../dotnet/configure-asp-net-core-web-api-project.md) に示す手順に従って生成した Open API 仕様書をローカルに保存します。ここでは、ファイル名をサンプルアプリケーションで使用しているファイルと同じ「dressca-api.json」とします。

1. OpenAPI Generator の実行には Java 8 以降のランタイムと、システム環境変数 JAVA_HOME が必要です。ローカル環境に JDK をインストールしてください。

## インストールとクライアントコード生成

1. OpenAPI Generator をインストールします。ターミナルで以下のコマンドを入力します。

```bash
npm install --save-dev @openapitools/openapi-generator-cli
```

1. package.json の script セクションにタスクを追加します。なお、ここで追加しているオプションはサンプルアプリケーションでの設定値です。

```json
{
  "scripts": {
    "generate-client": "openapi-generator-cli generate -g typescript-axios -i ./dressca-api.json --additional-properties=withSeparateModelsAndApi=true,modelPackage=models,apiPackage=api,supportsES6=true -o ./src/api-client"
  }
}
```

1. ターミナルで以下のコマンドを実行します。

```cmd
npm run generate-client
```

1. scripts に追加したタスクの ``` -o ``` に定義した出力先に、クライアントコードが生成されます。
