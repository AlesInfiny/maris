---
title: 集約エラーハンドラーの実装 （SSR 編）
description: SSR アプリケーション開発における 集約エラーハンドラー の実装方法を解説します。
---

# 集約エラーハンドラーの実装 {#top}

本章では、 SSR ベースの Blazor Server アプリケーションにおける集約エラーハンドラーの構成と実装手順を解説します。
本章の手順を実施することで、 本番環境で Blazor ランタイムおよび .NET ランタイム内でシステム例外が発生した場合、エラーページに遷移する機能が実装されます。

システム例外の処理方針については、[全体処理方式 - 例外処理方針](../../../app-architecture/server-side-rendering/global-function/exception-handling.md) を参照してください。

## 集約エラーハンドリングの全体像 {#overview}

本章では、次の内容を実施します。

- Blazor ランタイム内で発生した未処理例外を表示するコンポーネント（ Error.razor ）を実装します。
- Error.razor これを囲う `ErrorBoundary` をレイアウトします。
- .NET ランタイム（ Blazor 起動前）で発生した例外を表示する Razor Pages ベースのエラーページ （ ServerError.cshtml ）を実装します。
- エントリーポイント（ Program.cs ）で、上記のエラーハンドリングを有効にするための設定をします。

[プロジェクトの作成](./create-project.md) で作成した Fluent Blazor Web アプリのテンプレートから変更すべき点は、以下の通りです。

```text linenums="0"
└ {ApplicationName}.Web
├ Components
│ ├ Layout
│    └ MainLayout.razor　............ 変更します。
│ └ Pages
│   ├ Error.razor ................... 変更します。
│   └ Error.razor.css ............... 追加します。
│ ├ _imports.razor .................. 変更します。
│ └ Pages
|   └ ServerError.cshtml ............ 追加します。
└ Program.cs ........................ 変更します。
```

## レイアウトの変更 {#layout-with-error-boundary-integration}

このセクションでは、アプリケーションの共通レイアウト（`MainLayout.razor`）を変更し、 Blazor ランタイム内の未処理例外を集約して扱う方法を説明します。

MainLayout.razor を次のように修正し、エラー境界を導入します。
このことにより、子コンポーネントで発生した未処理例外をまとめてキャッチし、エラーページの表示やエラー後の回復処理を一元化します。

- MainLayout.razor の `@page` ブロックに ErrorBoundary コンポーネントを追加します。

```html title="MainLayout.razorの変更点（抜粋）" hl_lines="5-12"
    <FluentStack Class="main" Orientation="Orientation.Horizontal" Width="100%">
        <NavMenu />
        <FluentBodyContent Class="body-content">
            <div class="content">
                <ErrorBoundary @ref="errorBoundary">
                    <ChildContent>
                        @Body
                    </ChildContent>
                    <ErrorContent>
                        <Error Exception="@context" />
                    </ErrorContent>
                </ErrorBoundary>
            </div>
        </FluentBodyContent>
    </FluentStack>
    <FluentFooter>
```

- `@code` ブロックを追加します。 `OnParametersSet()` のタイミングで `Recover()` 処理をすることで、エラー状態から通常の状態へ復旧するように仕込んでいます。

```csharp title="MainLayout.razor に追加する @code ブロック"
@code {
    private ErrorBoundary? errorBoundary;

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }
}
```

`ErrorBoundary` コンポーネントの詳細については、[ASP.NET Core Blazor アプリのエラーを処理する - エラー境界 :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/fundamentals/handle-errors?view=aspnetcore-10.0#error-boundaries){ target=_blank }を参照してください。

## エラーコンポーネントの設定 {#blazor-error-component-implementation}

このセクションでは、 Blazor ランタイム内の未処理例外をユーザーに表示するためのコンポーネント Error.razor の実装方針を説明します。
開発環境でのデバッグ効率を高めるため、 `Exception` のスタックトレースの情報を表示するように実装します。

Error.razor を次のように変更してください。

- `@pages` ブロックを次のように変更します。下記の実装では、開発者が扱いやすいように、トップページへのリンク機能とスタックトレースの表示機能を実装しています。

```html title="Error.razorの変更点（抜粋）" hl_lines="3-27"
@page "/Error"
@using System.Diagnostics
@using System.Diagnostics.CodeAnalysis

<PageTitle>内部サーバーエラー</PageTitle>

<h1 class="text-danger">内部サーバーエラー</h1>
<p>
    申し訳ありません。処理中に予期しないエラーが発生しました。<br />
    時間をおいて再度アクセスしてください。問題が解決しない場合は、管理者へお問い合わせください。
</p>

<div>
    <FluentAnchor Href="/" Appearance="Appearance.Hypertext">トップページに戻る</FluentAnchor>
</div>

@if (this.ShowStackTrace)
{
    <div class="stack-trace-box">
        <FluentCard>
            <h3>スタックトレース</h3>
            <pre>
                @this.Exception.ToString()
            </pre>
        </FluentCard>
    </div>
}
```

- `@code` ブロックを次の内容で置き換えます。
開発環境かつ例外の情報がある場合にのみ、スタックトレースを表示します。

```csharp
@code{
    [Parameter]
    public Exception? Exception { get; set; }

    // ASP.NET Core では、 IWebHostEnvironment は常に登録されており、
    // null になることはないため、 null 非許容にはしていません。
    [Inject]
    private IWebHostEnvironment Environment { get; set; } = default!;

    [MemberNotNullWhen(true, nameof(this.Exception))]
    private bool ShowStackTrace => this.Environment.IsDevelopment() && this.Exception is not null;
}
```

必要に応じて、 Error.razor.css を作成してスタイルを変更します。
下記に例を示します。こだわりがなければそのまま利用して構いません。

??? example "スタイルの設定例"

    ```css title="Error.razor.css の設定例"
    div.stack-trace-box {
        margin-top: 30px;
    }
    ```

## エラーページの実装 {#server-error-page-implementation}

.NET ランタイム側で発生した例外を扱うためのエラーページを追加します。
Blazor の起動前にエラーをキャッチする必要があるので、 Razor コンポーネントではなく、  Razor Pages（ .cshtml ）として実装します。
両者を区別するため、 Razor コンポーネントを格納する Pages フォルダーとは異なる Pages フォルダーに作成してください。

下記にエラーページの実装例を示します。

??? example "エラーページの設定例"

    ```html title="サンプルアプリケーションの ServerError.cshtml"
    @page "/ServerError"
    @{
        Layout = null;
    }
    <!DOCTYPE html>
    <html lang="ja">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width,initial-scale=1" />
        <title>内部サーバーエラー</title>
        <meta name="robots" content="noindex" />
        <style>
            body {
                margin: 0;
                font-family: "Segoe UI", system-ui, sans-serif;
                background: #f5f5f5;
                display: flex;
                align-items: center;
                justify-content: center;
                min-height: 100vh;
            }

            .container {
                width: 100%;
                max-width: 600px;
                box-sizing: border-box;
                padding: 32px 40px;
                background: #fff;
                border-radius: 12px;
                box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            }

            h1 {
                margin: 0 0 16px;
                font-size: 32px;
                font-weight: 600;
            }

            p {
                margin: 0 0 24px;
                line-height: 1.6;
            }

            a { color: #004578; text-decoration: none; }
            a:hover { text-decoration: underline; }

            .footer {
                font-size: 18px;
                color: #666;
                margin-top: 16px;
            }
        </style>
    </head>
    <body>
        <div class="container">
            <h1>サーバーで問題が発生しています。</h1>
            <p>
                申し訳ありません。処理中に予期しないエラーが発生しました。<br />
                時間をおいて再度アクセスしてください。問題が解決しない場合は、管理者へお問い合わせください。
            </p>
            <a href="/">トップへ戻る</a>
            <div class="footer">&copy; @(DateTime.UtcNow.Year) Dressca CMS</div>
        </div>
    </body>
    </html>
    ```

## エントリーポイントの設定 {#entrypoint-configuration}

Razor Pages として追加した ServerError.cshtml を動作させるため、 エントリーポイントで Razor Pages 関連の設定をします。
Program.cs を下記のように変更します。

```csharp title="Program.cs の変更点（抜粋）" hl_lines="1 8 10-13 19 28"
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

// 中略

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddRazorPages(); // Razor Pages の機能一式を DI コンテナに登録します。

if (builder.Environment.IsDevelopment()) // 開発環境用の設定です。
{
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration); // 静的アセットの読み込みを構成します。
}

// 中略

if (!app.Environment.IsDevelopment()) // 本番環境用の設定です。
{
    app.UseExceptionHandler("/ServerError", createScopeForErrors: true);　// ServerError.cshtml のパスを設定します。

    app.UseHsts();
}

// 中略

app.MapStaticAssets();

app.MapRazorPages(); // 追加した Razor Pages （ ServerError.cshtml ）をルーティングに登録します。

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
```

## 動作確認 {#verification}

### Blazor ランタイムの例外の確認 {#verify-blazor-exception}

Home.razor に下記のようにわざと例外を発生させる `@code` ブロックを実装し、アプリケーションを起動します。

```csharp
@code {

    protected override void OnParametersSet()
    {

        throw new Exception("エラーページの動作確認");
    }
}
```

アプリケーションの起動と同時に、 Error.razor のページが表示され、内部サーバーエラーを示すメッセージと、例外のスタックトレースが表示されることを確認してください。

### .NET ランタイムの例外の確認 {#verify-dotnet-exception}

ブラウザーのアドレスバーから /ServerError に遷移し、 エラー画面が表示されることを確認してください。
