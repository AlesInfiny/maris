---
title: CORS 環境の構築
description: CORS （オリジン間リソース共有）環境でのアプリケーションの構築方法を解説します。
---

# CORS 環境の構築 {#top}

## CORS （オリジン間リソース共有）とは {#about-cors}

CORS (Cross-Origin Resource Sharing: オリジン間リソース共有 ) とは、いくつかの HTTP ヘッダーを使用することで、同一オリジンポリシーの制限を回避する仕組みです。

??? note "オリジンとは"

    オリジンとは、 URL のスキーム（プロトコル）、ホスト（ドメイン）、ポート番号の部分を指します（ [Origin (オリジン) - MDN Web Docs 用語集: ウェブ関連用語の定義 | MDN](https://developer.mozilla.org/ja/docs/Glossary/Origin) ）。

    - `https://www.example.com` と `https://www2.example.com` はホスト（ドメイン）部分が異なるので異なるオリジン
    - `https://localhost:4431` と `https://localhost:4432` はポート番号が異なるので異なるオリジン

??? note "同一オリジンポリシーとは"

    ブラウザーは原則として「同一オリジンポリシー」で動作します（ [同一オリジンポリシー - ウェブセキュリティ | MDN](https://developer.mozilla.org/ja/docs/Web/Security/Same-origin_policy)   ）。
    
    > 同一オリジンポリシーは重要なセキュリティの仕組みであり、あるオリジンによって読み込まれた文書やスクリプトが、他のオリジンにあるリソースにアクセスできる方法を制限するものです。

    <!-- textlint-disable ja-technical-writing/sentence-length -->
    
    つまり、 `https://aaa.example.com` から取得したリソース（ HTML 文書や JavaScript ）から、 `https://bbb.example.net` のリソース（ Web API や HTML 文書）には原則としてアクセスできません。
    
    <!-- textlint-enable ja-technical-writing/sentence-length -->

本章で解説する CORS 環境とは、 SPA において、フロントエンドアプリケーションとバックエンドアプリケーションの配置されるサーバーのオリジンが異なる環境を意味します。

CORS についてのその他の詳細は「 [オリジン間リソース共有 (CORS) - HTTP | MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/CORS) 」を参照してください。

## バックエンドアプリケーション（ .NET ） {#backend}

ASP.NET Web API アプリケーションでは、 `Program.cs` で CORS を設定します。 AlesInfiny Maris OSS Edition （以降『 AlesInfiny Maris 』）では、許可するオリジンの一覧をアプリケーション設定ファイル `appSettings.json` から取得します。

### appSettings.json の設定 {#appSettings-json}

```json
"WebServerOptions": {
  "AllowedOrigins": [
    "https://frontend.example.com", "https://dev.frontend.example.net"
  ]
}
```

### 構成オプション用クラスの追加 {#option-settings-class}

`appSettings.json` の内容をコード上で扱いやすくするため、構成オプション用のクラスを作成します。

```csharp
public class WebServerOptions
{
    /// <summary>
    /// 許可するオリジンを取得または設定します。
    /// </summary>
    public string[]? AllowedOrigins { get; set; }
}
```

### Program.cs の実装 {#Program-cs}

ASP.NET Web API では、 CORS に関する設定を `Program.cs` 上で行う必要があります。`CorsServiceCollectionExtensions.AddCors` メソッドを呼び出します。

```csharp title="Program.cs の実装"
// CORS ポリシーの名称です。値は任意ですが、 AddCors メソッドと UseCors メソッドで同じ値にする必要があります。
const string corsPolicyName = "allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// アプリケーション設定ファイルから CORS の設定部分を取得し、構成オプション用クラスに変換します。
WebServerOptions? options = builder.Configuration.GetSection(nameof(WebServerOptions)).Get<WebServerOptions>();
var origins = options != null ? options.AllowedOrigins : null;

// アプリケーション設定ファイルに CORS の設定がある場合のみ、
// CorsServiceCollectionExtensions.AddCors メソッドを呼び出します。
if (origins != null)
{
    builder.Services
        .AddCors(options =>
        {
            options.AddPolicy(
               name: corsPolicyName,
               policy =>
               {
                   // CORS のポリシーを設定します。
                   policy
                       .WithOrigins(origins)
                       .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
                       .AllowAnyHeader()
                       .AllowCredentials()
                       .WithExposedHeaders("Location");
               });
        });
}

// 中略

var app = builder.Build();

// アプリケーション設定ファイルに CORS の設定がある場合のみ、 CORS ミドルウェアを有効にします。
if (origins != null)
{
    app.UseCors(corsPolicyName);
}
```

#### CORS のポリシー設定について {#cors-policy}

コード例「`Program.cs` の実装」の 22-27 行目を以下に抜粋します。

```csharp
policy
    .WithOrigins(origins)
    .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
    .AllowAnyHeader()
    .AllowCredentials()
    .WithExposedHeaders("Location");
```

<!-- textlint-disable ja-technical-writing/ja-no-mixed-period -->

`WithOrigins` メソッド

:   CORS でリソースへのアクセスを許可するオリジンを設定します。 AlesInfiny Maris ではアプリケーション設定ファイルから値を取得します。

`WithMethods` メソッド

:   許可したオリジンが使用可能な HTTP メソッドを設定します。アプリケーションで許可するメソッド名を指定してください。

`AllowAnyHeader` メソッド

:   許可したオリジンに任意の HTTP リクエストヘッダー使用を許可します。クライアントから送信される HTTP リクエストヘッダーを制限したい場合は `WithHeaders` メソッドを使用してください。

`AllowCredentials` メソッド

:   許可したオリジンに Cookie 等の認証情報を送信することを許可します。アプリケーションで Cookie や認証を使用する場合、このメソッドの呼び出しが必要です。

`WithExposedHeaders` メソッド

:   許可したオリジンに対して公開する必要がある HTTP レスポンスヘッダーを設定します。アプリケーションで許可する HTTP レスポンスヘッダー名を指定してください。 `WithExposedHeaders` メソッドで設定していないレスポンスヘッダーはクライアントに公開されません。

<!-- textlint-enabled ja-technical-writing/ja-no-mixed-period -->

### Cookie を使用する場合の注意事項 {#notice}

アプリケーションで Cookie を使用する場合、 `SameSite` 属性に `None` を明示的に指定する必要があります。設定しない場合、別オリジンへ Cookie を送信できません。
なお、 Cookie の仕様上、 `SameSite` 属性に `None` を設定する場合は、必ず `Secure` 属性も併せて設定する必要があります（ [HTTP Cookie の使用 - HTTP | MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Cookies) ）。

> Cookie に SameSite=None が付いた場合は、 Secure 属性も指定することになりました（安全なコンテキストが必要になりました）。

## フロントエンドアプリケーション（ Vue.js ） {#frontend}

フロントエンドアプリケーションでは、 Web API 呼び出し時の HTTP リクエストヘッダーに `Access-Control-Allow-Origin` ヘッダーを追加し、自身のオリジンを設定する必要があります。

### 環境変数ファイルの作成と読み込み {#env-file}

環境変数用 env ファイルを作成し、 `Access-Control-Allow-Origin` ヘッダーに設定するオリジン用の変数を宣言します。
開発用と本番用でオリジンが異なる場合は、開発用の env ファイルと本番用 env ファイルそれぞれに設定します（以下の例では、開発用 env ファイルを `.env.dev` 、本番用 env ファイルを `.env.prod` としています）。

```properties title=".env.dev"
VITE_ACCESS_CONTROL_ALLOW_ORIGIN=https://dev.frontend.example.net
```

```properties title=".env.prod"
VITE_ACCESS_CONTROL_ALLOW_ORIGIN=https://frontend.example.com
```

環境変数を TypeScript 上で使用可能にするため、 `env.d.ts` に項目を追加します。

```typescript
interface ImportMetaEnv {
  readonly VITE_ACCESS_CONTROL_ALLOW_ORIGIN: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
```

### Web API 呼び出し時の HTTP ヘッダーの設定 {#http-request-header}

AlesInfiny Maris では、 Web API 呼び出しを共通化するために `./src/api-client/index.ts` という設定ファイルを作成するので、ここで行います（ [参照](../vue-js/create-api-client-code.md#set-client-code) ）。

```ts title="index.ts"
import axios from 'axios';
import * as apiClient from '@/generated/api-client';

// （中略）

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({
  headers: {
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': import.meta.env
      .VITE_ACCESS_CONTROL_ALLOW_ORIGIN,
  },
  withCredentials: true,
});
```

`headers`

:   `Access-Control-Allow-Origin` ヘッダーに `env.d.ts` の値を設定します。

`withCredentials`

:   CORS 環境でのリクエストが Cookie 、認証ヘッダー、 TLS クライアント証明書などの資格情報を使用して行うべきかを示します。既定値は `false` なので、 `true` を明示的に設定します。
