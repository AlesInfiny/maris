---
title: CORS 環境の構築
description: CORS （オリジン間リソース共有）環境でのアプリケーションの構築方法を解説します。
---

# CORS 環境の構築 {#top}

## CORS （オリジン間リソース共有）とは {#about-cors}

オリジンとは、 URL のスキーム（プロトコル）、ホスト（ドメイン）、ポート番号の部分を指します（ [Origin (オリジン) - MDN Web Docs 用語集: ウェブ関連用語の定義 | MDN](https://developer.mozilla.org/ja/docs/Glossary/Origin) ）。

- `https://www.example.com` と `https://www2.example.com` はホスト（ドメイン）部分が異なるので異なるオリジン
- `https://localhost:4431` と `https://localhost:4432` はポート番号が異なるので異なるオリジン

ブラウザーは原則として「同一オリジンポリシー」で動作します（ [同一オリジンポリシー - ウェブセキュリティ | MDN](https://developer.mozilla.org/ja/docs/Web/Security/Same-origin_policy)   ）。

> 同一オリジンポリシーは重要なセキュリティの仕組みであり、あるオリジンによって読み込まれた文書やスクリプトが、他のオリジンにあるリソースにアクセスできる方法を制限するものです。

<!-- textlint-disable ja-technical-writing/sentence-length -->

つまり、 `https://aaa.example.com` から取得したリソース（ HTML 文書や JavaScript ）から、 `https://bbb.example.net` のリソース（ Web API や HTML 文書）には原則としてアクセスできません。

<!-- textlint-enable ja-technical-writing/sentence-length -->

![同一オリジンポリシーのイメージ](../../../images/guidebooks/how-to-develop/cors/sop-image-light.png#only-light){ loading=lazy }
![同一オリジンポリシーのイメージ](../../../images/guidebooks/how-to-develop/cors/sop-image-dark.png#only-dark){ loading=lazy }

CORS (Cross-Origin Resource Sharing: オリジン間リソース共有 ) とは、いくつかの HTTP ヘッダーを使用することで、同一オリジンポリシーの制限を回避する仕組みです。

本章で解説する CORS 環境とは、 SPA において、フロントエンドアプリケーションとバックエンドアプリケーションの配置されるサーバーのオリジンが異なる環境を意味します。

## CORS の仕組み {#structure-of-cors}

- シンプルリクエストの場合、

![CORSの仕組み](../../../images/guidebooks/how-to-develop/cors/cors-image-light.png#only-light){ loading-lazy }
![CORSの仕組み](../../../images/guidebooks/how-to-develop/cors/cors-image-dark.png#only-dark){ loading-lazy }

CORS についてのその他の詳細は「 [オリジン間リソース共有 (CORS) - HTTP | MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/CORS) 」を参照してください。

## バックエンドアプリケーション（ .NET ） {#backend}

ASP.NET Web API アプリケーションでは、 `Program.cs` で CORS を設定します。 AlesInfiny Maris では、許可するオリジンの一覧をアプリケーション設定ファイル `appSettings.json` から取得します。

### appSettings.json の設定 {#appSettings-json}

```json
  "WebServerOptions": {
    "AllowedOrigins": [
      "https://frontend.example.com"
    ]
  }
```

### 構成オプション用クラスの追加 {#option-settings-class}

```csharp
public class WebServerOptions
{
    /// <summary>
    /// 許可するオリジンを取得または設定します。
    /// </summary>
    public string[]? AllowedOrigins { get; set; }
}
```

### Program.cs の設定 {#Program-cs}

```csharp
const string corsPolicyName = "allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

WebServerOptions? options = builder.Configuration.GetSection(nameof(WebServerOptions)).Get<WebServerOptions>();
var origins = options != null ? options.AllowedOrigins : null;

if (origins != null)
{
    builder.Services
        .AddCors(options =>
        {
            options.AddPolicy(
               name: corsPolicyName,
               policy =>
               {
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

if (origins != null)
{
    app.UseCors(corsPolicyName);
}
```

### 注意点 {#notice}

SameSite=None, Secure, HttpOnly

## フロントエンドアプリケーション（ Vue.js ） {#frontend}

Web API 呼び出し時のヘッダーに `Access-Control-Allow-Origin` ヘッダーを追加する。
