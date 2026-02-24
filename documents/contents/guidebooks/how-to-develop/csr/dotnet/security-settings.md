---
title: .NET 編（CSR 編）
description: CSR アプリケーションの サーバーサイドで動作する .NET アプリケーションの 開発手順を解説します。
---

# セキュリティの設定 {#top}

以下の目的で、 `Program.cs` 上で HTTP レスポンスヘッダーを設定します。

- [MIME スニッフィング :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/HTTP/Guides/MIME_types#mime_%E3%82%B9%E3%83%8B%E3%83%83%E3%83%95%E3%82%A3%E3%83%B3%E3%82%B0){ target=_blank } の防止
- クリックジャッキングの防止（詳細は [こちら](../../../../app-architecture/security/clickjacking.md) を参照）

`Program.cs` が冗長になることを防止するため、ミドルウェア [^1] を作成します。

??? example "セキュリティ設定を HTTP レスポンスヘッダーに設定するミドルウェア"

    ```csharp title="HttpSecurityHeadersMiddleware.cs"
    https://github.com/AlesInfiny/maris/blob/main/samples/Dressca/dressca-backend/src/Dressca.Web/Extensions/HttpSecurityHeadersMiddleware.cs
    ```

    ```csharp title="HttpSecurityHeadersMiddlewareExtensions.cs"
    https://github.com/AlesInfiny/maris/blob/main/samples/Dressca/dressca-backend/src/Dressca.Web/Extensions/HttpSecurityHeadersMiddlewareExtensions.cs
    ```

作成したミドルウェアを `Program.cs` から呼び出します。

??? example "Program.cs での呼び出し"

    ```csharp title="Program.cs" hl_lines="9-10"
    using Dressca.Web.Extensions; // その他の using は省略

    var builder = WebApplication.CreateBuilder(args);

    // 省略

    var app = builder.Build();

    // 静的ファイル（js/css/画像など）に対するレスポンスでも設定を有効にするため、 app.UseStaticFiles の前に呼び出す
    app.UseSecuritySettings();

    app.UseStaticFiles();

    // その他のミドルウェアの使用は省略

    app.Run();
    ```

HTTP レスポンスヘッダーが以下のように設定されます。

![セキュリティ設定後の HTTP レスポンスヘッダー](../../../../images/guidebooks/how-to-develop/csr/dotnet/security-header.png)

[^1]: ここで言う「ミドルウェア」は、 ASP.NET Core のミドルウェアを指します。 ASP.NET Core のミドルウェアとは、リクエストとレスポンスを処理するために、アプリのパイプラインに組み込まれたソフトウェアのことです（ [詳細 :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/middleware/){target=blank} ）。
