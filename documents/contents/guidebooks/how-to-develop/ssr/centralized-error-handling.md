---
title: 集約エラーハンドラーの実装 （SSR 編）
description: SSR アプリケーション開発における 集約エラーハンドラー の実装方法を解説します。
---

# 集約エラーハンドラーの実装 {#top}

本章では、 SSR ベースの Blazor Server アプリケーションにおける集約エラーハンドラーの構成と実装手順を解説します。
本章の手順を実施することで、 本番環境で Blazor ランタイムおよび .NET ランタイム内でシステム例外が発生した場合、エラーページに遷移する機能が実装されます。

システム例外の処理方針については、[全体処理方式 - 例外処理方針](../../../app-architecture/server-side-rendering/global-function/exception-handling.md) を参照してください。

!!! warning "実施前の注意点"

    本章の手順を実施する前に、Debug ビルド、 Release ビルドの構成でそれぞれアプリケーションをビルドし、正常に起動することを確認してください。本章にはあえてエラーを発生させて、エラーが発生した場合の挙動を確認する手順が含まれるためです。

## 集約エラーハンドリングの全体像 {#overview}

本章では、次の内容を実施します。

- Blazor ランタイム内で発生した未処理例外を表示するコンポーネント（ Error.razor ）を実装します。
- `ErrorBoundary` で `Error.razor` を囲むようにレイアウトします。
- .NET ランタイム（ Blazor 起動前）で発生した例外を表示する Razor Pages ベースのエラーページ （ ServerError.cshtml ）を実装します。
- エントリーポイント（ Program.cs ）で、上記のエラーハンドリングを有効にするための設定をします。

[プロジェクトの作成](./create-project.md) で作成した Fluent Blazor Web アプリのテンプレートから変更すべき点は、以下の通りです。

```text linenums="0"
├ {ApplicationName}.Web
├ Components
│ ├ Layout
│ │  └ MainLayout.razor　............ 変更します。
│ ├ Pages
│ │ ├ Error.razor ................... 変更します。
│ │ └ Error.razor.css ............... 追加します。
│ └ _imports.razor .................. 変更します。
├ Pages
│   └ ServerError.cshtml ............ 追加します。
└ Program.cs ........................ 変更します。
```

## レイアウトの変更 {#layout-with-error-boundary-integration}

このセクションでは、アプリケーションの共通レイアウト（`MainLayout.razor`）を変更し、 Blazor ランタイム内の未処理例外を集約して扱う方法を説明します。

MainLayout.razor を次のように修正し、エラー境界を導入します。
このことにより、子コンポーネントで発生した未処理例外をまとめてキャッチし、エラーページの表示やエラー後の回復処理を一元化します。

- MainLayout.razor のビューに ErrorBoundary コンポーネントを追加します。

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

`ErrorBoundary` コンポーネントの詳細については、[ASP.NET Core Blazor アプリのエラーを処理する - エラー境界 :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/fundamentals/handle-errors#error-boundaries){ target=_blank }を参照してください。

## エラーコンポーネントの設定 {#blazor-error-component-implementation}

このセクションでは、 Blazor ランタイム内の未処理例外をユーザーに表示するためのコンポーネント Error.razor の実装方針を説明します。
開発環境でのデバッグ効率を高めるため、 `Exception` のスタックトレースの情報を表示するように実装します。

Error.razor の実装例を下記に示します。
開発者が扱いやすいように、トップページへのリンク機能とスタックトレースの表示機能を実装しています。
開発環境かつ例外の情報がある場合にのみ、スタックトレースを表示します。

```csharp title="サンプルアプリケーションの Error.razor"
https://github.com/AlesInfiny/maris/blob/main/samples/DresscaCMS/src/DresscaCMS.Web/Components/Pages/Error.razor
```

## エラーページの実装 {#server-error-page-implementation}

.NET ランタイム側で発生した例外を扱うためのエラーページを追加します。
Blazor の起動前にエラーをキャッチする必要があるので、 Razor Components ではなく、  Razor Pages（ .cshtml ）として実装します。 Razor Pages のファイルは、 Razor Components を格納する Pages フォルダーとは異なるプロジェクトルート直下の Pages フォルダー内に必ず作成してください。

!!! warning "注意：Razor Pages と Razor Components"

    Razor Pages はページ指向のアーキテクチャーを採用している一方で、Razor Components はコンポーネント指向のアーキテクチャーを採用しています。そのため、 Razor Pages では、 1つの URL に対して 1つのページ（ .cshtml ）が対応することが想定されています。よって、 Razor Pages を用いる場合、 URL に紐づくファイル名およびパスは Pages フォルダ配下のフォルダー階層によって決定されるので、異なる場所に配置しないよう注意してください。Razor Pages についての詳細な解説は、[Razor ASP.NET Core のページのアーキテクチャと概念 :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/razor-pages){ target=_blank }を参照してください。


    ```text linenums="0"
    ├ {ApplicationName}.Web
    ├ Components
    │ ├ Pages
    │ │ │ Error.razor
    │ │ └ ServerError.cshtml --- NG
    ├ Pages
    └   └ ServerError.cshtml --- OK
    ```

下記にエラーページの実装例を示します。

??? example "エラーページの実装例"

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

```csharp title="Program.cs の変更点（抜粋）" hl_lines="1 8 14 23"
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

// 中略

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddRazorPages(); // Razor Pages の機能一式を DI コンテナに登録します。

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

```csharp title="例外を発生させる Home.razor"
@code {

    protected override void OnParametersSet()
    {

        throw new Exception("エラーページの動作確認");
    }
}
```

アプリケーションの起動と同時に、 Error.razor のページが表示され、内部サーバーエラーを示すメッセージと、例外のスタックトレースが表示されることを確認してください。
確認ができたら、確認用に追加したコードは削除してください。

### .NET ランタイムの例外の確認 {#verify-dotnet-exception}

NavMenu.razor に下記のようにわざと例外を発生させる `@code` ブロックを実装し、アプリケーションを起動します。

```csharp title="例外を発生させる NavMenu.razor" hl_lines="3-6"
@code {

    protected override void OnParametersSet()
    {
        throw new Exception("エラーページの動作確認");
    }

    private bool expanded = true;
}
```

Production 環境での動作を確認するために、プロジェクトファイルの直下でターミナルを開き、下記のコマンドを用いて Release 構成でビルドし、 Production 環境でアプリケーションを立ち上げます。

```powershell title="プロジェクト名を AaaSubSystem.Web に設定した場合の例"  linenums="0"
src\AaaSubSystem.Web> dotnet publish -c Release
src\AaaSubSystem.Web> dotnet .\bin\Release\net10.0\publish\AaaSubSystem.Web.dll --environment Production
```

アプリケーションの起動に成功すると、下記のようなログが出力されるので、表示された URL にアクセスします。

```powershell linenums="0"
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

アクセス直後、[エラーページの実装](#server-error-page-implementation) で追加した、 ServerError.cshtml に遷移することを確認してください。
確認ができたら、 NavMenu.razor へ追加した確認用のコードは削除してください。
