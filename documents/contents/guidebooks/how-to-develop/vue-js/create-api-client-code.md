# API 仕様書からのクライアントコード生成

サーバー側で公開される Web API は、Open API 仕様書を自動生成しています（詳細は [ASP.NET Core Web API プロジェクトの構成](../dotnet/configure-asp-net-core-web-api-project.md) を参照）。Vue.js アプリケーションでは、Open API Generator を使用して、この Open API 仕様書からクライアントコードを生成します。

## 事前準備 ## {: #preparation }

[Open API 仕様書の出力設定](../dotnet/configure-asp-net-core-web-api-project.md) に示す手順に従って生成した Open API 仕様書をローカルに保存します。ここでは、ファイル名を「dressca-api.json」とします。

### JDK のインストール ## { #install-jdk }

Open API Generator を使用するためには、Java 8 以降のランタイムと、システム環境変数 JAVA_HOME の設定が必要です。Oracle JDK や Eclipse Adoptium など、適当な JDK をインストールし、JAVA_HOME を設定してください。

## インストールと設定 ## {: #install-and-setting }

OpenAPI Generator をインストールします。ターミナルで以下のコマンドを入力します。

```terminal
npm install --D @openapitools/openapi-generator-cli
```

package.json の script セクションにタスクを追加します。

```json
{
  "scripts": {
    "generate-client": "openapi-generator-cli generate -g typescript-axios -i ./dressca-api.json --additional-properties=withSeparateModelsAndApi=true,modelPackage=models,apiPackage=api,supportsES6=true -o ./src/api-client"
  }
}
```

追加したタスクのオプションについて説明します。

ジェネレーターとして typescript-axios を指定します。

``` terminal
-g typescript-axios
```

入力のAPI仕様書として「dressca-api.json」というファイルを指定します。

``` terminal
-i ./dressca-api.json 
```

以下のプロパティを追加します。

- withSeparateModelsAndApi=true：model と API を別クラス・別フォルダーに配置する
- modelPackage=models：model クラスのパッケージ名を「models」に設定する
- apiPackage=api：API クラスのパッケージ名を「api」に設定する
- supportsES6=true：ES6 に準拠したコードを生成する

``` terminal
--additional-properties=withSeparateModelsAndApi=true,modelPackage=models,apiPackage=api,supportsES6=true
```

生成されたコードの出力先を ```./src/api-client``` に設定します。

``` terminal
-o ./src/api-client
```

## クライアントコードの生成 ## {: #create-client-code }

ターミナルで以下のコマンドを実行します。

```terminal
npm run generate-client
```

scripts に追加したタスクの ``` -o ``` に定義した出力先に、クライアントコードが生成されます。
