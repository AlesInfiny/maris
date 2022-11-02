---
title: Vue.js 開発手順
description: Vue.js を用いたクライアントサイドアプリケーションの開発手順を説明します。
---

<!-- cSpell:ignore openapi -->

# Open API 仕様書からのクライアントコード生成 {#top}

サーバー側で公開される Web API は、 Open API 仕様書を自動生成しています（詳細は [ASP.NET Core Web API プロジェクトの構成](../dotnet/configure-asp-net-core-web-api-project/) を参照）。 Vue.js アプリケーションでは、 Open API Generator を使用して、この Open API 仕様書からクライアントコードを生成します。

## 事前準備 {#preparation}

[Open API 仕様書の出力設定](../dotnet/configure-asp-net-core-web-api-project.md#open-api-specification-output-configuration) に示す手順に従って生成した Open API 仕様書をローカルに保存します。ここでは、ファイル名を「dressca-api.json」とします。

### JDK のインストール {#install-jdk}

Open API Generator を使用するためには、 Java 8 以降のランタイムと、システム環境変数 JAVA_HOME の設定が必要です。 Oracle JDK や Eclipse Adoptium など、適当な JDK をインストールし、 JAVA_HOME を設定してください。

## Axios {#axios}

### Axios のインストール {#install-axios}

```terminal
npm install axios
```

### Axios の設定 {#settings-axios}

`./src/config/axios.config.ts` というファイルを作成し、以下のように記述します。

```typescript title="axios.config.ts"
import axios from 'axios';

axios.defaults.baseURL = `作成済みの Web API の URL`;
```

- `axios.defaults.baseURL` ：Web API のベース URL を設定します。

作成したファイルを読み込むため、 main.ts に import を記述します。

```typescript title="main.ts"
import '@/config/axios.config';
```

## Open API Generator {#open-api-generator}

### Open API Generator のインストール {#install-open-api-generator}

Open API Generator をインストールします。ターミナルで以下のコマンドを入力します。

```terminal
npm install -D @openapitools/openapi-generator-cli
```

### Open API Generator の設定 {#settings-open-api-generator}

package.json の scripts セクションにタスクを追加します。

```json title="package.json"
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

入力の API 仕様書として `./dressca-api.json` というファイルを指定します。

``` terminal
-i ./dressca-api.json 
```

以下のプロパティを追加します。

- `withSeparateModelsAndApi=true` ：model と API を別クラス・別フォルダーに配置する
- `modelPackage=models：model` ：クラスのパッケージ名を「models」に設定する
- `apiPackage=api` ：API クラスのパッケージ名を「api」に設定する
- `supportsES6=true` ：ES6 に準拠したコードを生成する

``` terminal
--additional-properties=withSeparateModelsAndApi=true,modelPackage=models,apiPackage=api,supportsES6=true
```

生成されたコードの出力先を `./src/api-client` に設定します。

``` terminal
-o ./src/api-client
```

## クライアントコードの生成 {#create-client-code}

ターミナルで以下のコマンドを実行します。

```terminal
npm run generate-client
```

オプション ` -o ` に定義した出力先へ、クライアントコードが生成されます。
