---
title: CORS 環境の構築 （CSR 編）
description: CORS （オリジン間リソース共有）環境での アプリケーションの構築方法を解説します。
---

# CORS 環境の構築 {#top}

## CORS （オリジン間リソース共有）とは {#about-cors}

CORS とは、いくつかの HTTP ヘッダーを使用することで、同一オリジンポリシーの制限を回避する仕組みです。

??? note "オリジンとは"

    オリジンとは、 URL のスキーム（プロトコル）、ホスト（ドメイン）、ポート番号の部分を指します（ [Origin (オリジン) - MDN Web Docs 用語集: ウェブ関連用語の定義 | MDN :material-open-in-new:](https://developer.mozilla.org/ja/docs/Glossary/Origin){ target=_blank } ）。

    - `http://localhost` と `https://localhost` はスキームが異なるので異なるオリジン
    - `https://www.example.com` と `https://www2.example.com` はホスト部分が異なるので異なるオリジン
    - `https://localhost:4431` と `https://localhost:4432` はポート番号が異なるので異なるオリジン

??? note "同一オリジンポリシーとは"

    ブラウザーは原則として「同一オリジンポリシー」で動作します（ [同一オリジンポリシー - ウェブセキュリティ | MDN :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/Security/Same-origin_policy){ target=_blank }   ）。
    
    > 同一オリジンポリシーは重要なセキュリティの仕組みであり、あるオリジンによって読み込まれた文書やスクリプトが、他のオリジンにあるリソースにアクセスできる方法を制限するものです。

    <!-- textlint-disable ja-technical-writing/sentence-length -->
    
    つまり、 `https://aaa.example.com` から取得したリソース（ HTML 文書や JavaScript ）から、 `https://bbb.example.net` のリソース（ Web API や HTML 文書）には原則としてアクセスできません。
    
    <!-- textlint-enable ja-technical-writing/sentence-length -->

本章で解説する CORS 環境とは、 CSR アプリケーションにおいて、フロントエンドアプリケーションとバックエンドアプリケーションの配置されるサーバーのオリジンが異なる環境を意味します。 CORS 環境で CSR アプリケーションを構築する場合、いくつかの考慮や追加の実装が必要です。

CORS の仕組みの詳細は「 [オリジン間リソース共有 (CORS) - HTTP | MDN :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/HTTP/CORS){ target=_blank } 」を参照してください。

## バックエンドアプリケーション（ .NET ） {#backend}

ASP.NET Core Web API アプリケーションでは、 `Program.cs` で CORS に関するポリシーを設定します。
AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）では、許可するオリジンの一覧をアプリケーション設定ファイル `appSettings.json` から取得します。

### 許可するオリジンの追加 {#appSettings-json}

許可するオリジンの一覧をアプリケーション設定ファイル `appSettings.json` に記述します。

```json title="appSettings.json"
"WebServerOptions": {
  "AllowedOrigins": [
    "https://frontend.example.com", "https://sub.frontend.example.com"
  ]
}
```

なお、開発時にのみ使用する設定は `appSettings.Development.json` に記述します。

```json title="appSettings.Development.json"
"WebServerOptions": {
  "AllowedOrigins": [
    "https://dev.frontend.example.net"
  ]
}
```

### 構成オプション用クラスの追加 {#option-settings-class}

`appSettings.json` の内容をコード上で扱いやすくするため、構成オプション用のクラスを作成します。クラス名、プロパティ名、プロパティの型は `appSettings.json` に追加した名称と一致させる必要があります。

```csharp
public class WebServerOptions
{
    /// <summary>
    /// 許可するオリジンを取得または設定します。
    /// </summary>
    public string[] AllowedOrigins { get; set; } = [];
}
```

### CORS ポリシーの設定 {#program-cs}

ASP.NET Core Web API では、 CORS に関する設定を `Program.cs` 上で行う必要があります。`builder.Services.AddCors` メソッドで CORS を有効化し、 `app.UseCors` メソッドでポリシーを追加して CORS ミドルウェアを有効化します。

```csharp title="Program.cs"
var builder = WebApplication.CreateBuilder(args);

// アプリケーション設定ファイルから CORS の設定部分を取得し、サービスコンテナーに追加します。
// さらに DataAnnotation による検証を有効化します。
builder.Services
    .AddOptions<WebServerOptions>()
    .BindConfiguration(nameof(WebServerOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// CORS の使用を宣言します。
builder.Services.AddCors();

// 中略

var app = builder.Build();

// サービスコンテナーに追加したアプリケーション設定を取得します。
var options = app.Services.GetRequiredService<IOptions<WebServerOptions>>();

// アプリケーション設定ファイルにオリジンが記述されている場合のみ CORS ミドルウェアを有効化します。
if (options.Value.AllowedOrigins.Length > 0)
{
    app.UseCors(policy =>
    {
        policy
            .WithOrigins(options.Value.AllowedOrigins)
            .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Location");
    });
}
```

#### CORS のポリシー設定について {#cors-policy}

上のコード例「 `Program.cs` 」の 26-31 行目を以下に抜粋します。

```csharp
policy
    .WithOrigins(options.Value.AllowedOrigins)
    .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
    .AllowAnyHeader()
    .AllowCredentials()
    .WithExposedHeaders("Location");
```

<!-- textlint-disable ja-technical-writing/ja-no-mixed-period -->

`WithOrigins` メソッド

:   CORS でリソースへのアクセスを許可するオリジンを設定します。 AlesInfiny Maris ではアプリケーション設定ファイルから値を取得して引数に渡します。

`WithMethods` メソッド

<!-- textlint-disable ja-technical-writing/sentence-length -->

:   許可したオリジンのクライアントが使用可能な HTTP メソッドを設定します。アプリケーションで許可する HTTP メソッド名を指定してください。なお、 CORS 環境の場合プリフライトリクエストが使用する `OPTIONS` は必須です。詳細は [Preflight request (プリフライトリクエスト) - MDN Web Docs 用語集: ウェブ関連用語の定義 | MDN :material-open-in-new:](https://developer.mozilla.org/ja/docs/Glossary/Preflight_request){ target=_blank } を参照してください。

<!-- textlint-enable ja-technical-writing/sentence-length -->

`AllowAnyHeader` メソッド

:   許可したオリジンのクライアントに任意の HTTP リクエストヘッダー使用を許可します。クライアントから送信される HTTP リクエストヘッダーを制限したい場合は代わりに `WithHeaders` メソッドを使用してください。

`AllowCredentials` メソッド

:   許可したオリジンのクライアントに Cookie 等の認証情報を送信することを許可します。アプリケーションで Cookie や認証を使用する場合、このメソッドの呼び出しが必要です。

`WithExposedHeaders` メソッド

:   許可したオリジンのクライアントに対して公開する必要がある HTTP レスポンスヘッダーを設定します。アプリケーションで許可する HTTP レスポンスヘッダー名を指定してください。 `WithExposedHeaders` メソッドで設定していないレスポンスヘッダーはクライアントに公開されません。

<!-- textlint-enable ja-technical-writing/ja-no-mixed-period -->

!!! danger "Cookie を使用する場合の注意事項"

    CORS 環境においてアプリケーションで Cookie を使用する場合、 `SameSite` 属性に `None` を明示的に指定する必要があります。設定しない場合、別オリジンへ Cookie を送信できません。
    なお、 Cookie の仕様上 `SameSite` 属性に `None` を設定する場合は必ず `Secure` 属性も設定する必要があります（ [HTTP Cookie の使用 - HTTP | MDN :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/HTTP/Cookies){ target=_blank } ）。
    
    > Cookie に SameSite=None が付いた場合は、 Secure 属性も指定することになりました（安全なコンテキストが必要になりました）。

## フロントエンドアプリケーション（ Vue.js ） {#frontend}

### Web API 呼び出し時の HTTP ヘッダーの設定 {#http-request-header}

AlesInfiny Maris では Web API 呼び出しの共通処理用に `./src/api-client/index.ts` という設定ファイルを作成する（ [参照](../vue-js/create-api-client-code.md#set-client-code) ）ので、ここで HTTP ヘッダーを設定します。

```typescript title="index.ts" hl_lines="11"
import axios from 'axios'
import * as apiClient from '@/generated/api-client'

// （中略）

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
})

const exampleApi = new apiClient.ExampleApi(createConfig(), '', axiosInstance)

export { exampleApi }
```

<!-- textlint-disable @textlint-ja/no-synonyms -->

`withCredentials: true` （ 11 行目）

<!-- textlint-enable @textlint-ja/no-synonyms -->

:   CORS 環境でのリクエストが Cookie 、認証ヘッダー、 TLS クライアント証明書などの資格情報を使用して行われるべきかを示します。既定値は `false` なので、 `true` を明示的に設定します。
