# API 仕様書からのクライアントコード生成

サーバー側で公開される Web API は、Open API 仕様書を自動生成しています（詳細は [ASP.NET Core Web API プロジェクトの構成](../dotnet/configure-asp-net-core-web-api-project.md) を参照）。Vue.js アプリケーションでは、Open API Generator を使用して、この Open API 仕様書からクライアントコードを生成します。

## 事前準備 ## {: #preparation }

[Open API 仕様書の出力設定](../dotnet/configure-asp-net-core-web-api-project.md) に示す手順に従って生成した Open API 仕様書をローカルに保存します。ここでは、ファイル名を「dressca-api.json」とします。

### JDK のインストール ## { #install-jdk }

Open API Generator を使用するためには、Java 8 以降のランタイムと、システム環境変数 JAVA_HOME の設定が必要です。Oracle JDK や Eclipse Adoptium など、適当な JDK をインストールし、JAVA_HOME を設定してください。

## インストールとクライアントコード生成 ## {: #install-and-create-client-code }

1. OpenAPI Generator をインストールします。ターミナルで以下のコマンドを入力します。

```bash
npm install --D @openapitools/openapi-generator-cli
```

1. package.json の script セクションにタスクを追加します。

```json
{
  "scripts": {
    "generate-client": "openapi-generator-cli generate -g typescript-axios -i ./dressca-api.json --additional-properties=withSeparateModelsAndApi=true,modelPackage=models,apiPackage=api,supportsES6=true -o ./src/api-client"
  }
}
```

|オプション|説明|
|-----|-----|
| ``` -g typescript-axios ``` |ジェネレーターとして typescript-axios を指定します。|
| ``` -i ./dressca-api.json ``` |入力のAPI仕様書として「dressca-api.json」というファイルを指定します。|
| ``` --additional-properties=withSeparateModelsAndApi=true,modelPackage=models,apiPackage=api,supportsES6=true ``` |withSeparateModelsAndApi=true：model と API を別クラス・別フォルダーに配置する<br>modelPackage=models：model クラスのパッケージ名を「models」に設定する<br>apiPackage=api：API クラスのパッケージ名を「api」に設定する<br>supportsES6=true：ES6 に準拠したコードを生成する|
| ``` -o ./src/api-client ``` |生成されたコードの出力先を「./src/api-client」に設定します。|

ターミナルで以下のコマンドを実行します。

```cmd
npm run generate-client
```

scripts に追加したタスクの ``` -o ``` に定義した出力先に、クライアントコードが生成されます。
