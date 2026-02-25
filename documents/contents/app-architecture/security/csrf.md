---
title: アプリケーション セキュリティ編
description: アプリケーションセキュリティを 担保するための方針を説明します。
---

# CSRF （クロスサイトリクエストフォージェリ） {#top}

## CSRF とは {#what-is-csrf}

<!-- textlint-disable ja-technical-writing/sentence-length -->

[安全なウェブサイトの作り方 - 1.6 CSRF（クロスサイト・リクエスト・フォージェリ） | 情報セキュリティ | IPA 独立行政法人 情報処理推進機構 :material-open-in-new:](https://www.ipa.go.jp/security/vuln/websecurity/csrf.html){ target=_blank } より CSRF の定義を以下に引用します。

<!-- textlint-enable ja-technical-writing/sentence-length -->

<!-- textlint-disable -->

> ウェブサイトの中には、サービスの提供に際しログイン機能を設けているものがあります。ここで、ログインした利用者からのリクエストについて、その利用者が意図したリクエストであるかどうかを識別する仕組みを持たないウェブサイトは、外部サイトを経由した悪意のあるリクエストを受け入れてしまう場合があります。このようなウェブサイトにログインした利用者は、悪意のある人が用意した罠により、利用者が予期しない処理を実行させられてしまう可能性があります。このような問題を「CSRF（Cross-Site Request Forgeries／クロスサイト・リクエスト・フォージェリ）の脆弱性」と呼び、これを悪用した攻撃を、「CSRF攻撃」と呼びます。

<!-- textlint-enable -->

## AlesInfiny Maris OSS Edition での CSRF 対策 {#measures-against-csrf}

CSR アプリケーションと SSR アプリケーションで対策が異なります。

- CSR アプリケーションでは、プリフライトリクエストによる Origin ヘッダーの検証を対策とします。
- SSR アプリケーションでは、 CSRF トークンの付与を対策とします。

以下、それぞれのアプリケーションでの対策について説明します。

### CSR アプリケーションでの対策 {#csr-application}

ブラウザーは原則として、悪意のある Web サイトなど異なるオリジン間でリクエストをブロックするために [同一オリジンポリシー :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/Security/Same-origin_policy){ target=_blank } で動作します。

同一オリジンポリシーでは、異なるオリジンの Web サイトに対し「リクエストを送ることはできるが、その結果の読み取りはできない」ことを規定しています。
つまり、結果の読み取りができないだけでリクエスト自体は送られてしまい、データの改ざんといった処理が実行されてしまう危険性があることを表しています。

<!-- textlint-disable ja-technical-writing/sentence-length -->
そのため AlesInfiny Maris OSS Edition （以下 AlesInfiny Maris）では、異なるオリジンに配置された悪意のある Web サイトからの「データを更新するリクエストを事前にブロックする」ことで CSRF 攻撃に対策します。
<!-- textlint-enable ja-technical-writing/sentence-length -->

上記に基づき、原則として以下の方針をとります。

#### プリフライトリクエストによる Origin ヘッダーの検証 {#verification-of-origin-header}

<!-- textlint-disable ja-technical-writing/sentence-length -->

Web API へのリクエスト受信時に [Origin ヘッダー :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/HTTP/Reference/Headers/Origin){ target=_blank } を検証することで、異なるオリジン上の Web サイトからのリクエストを処理が実行される前にブロックします。

具体的には、以下のように異なるオリジンからのリクエストをあらかじめブロックするために [プリフライトリクエスト :material-open-in-new:](https://developer.mozilla.org/ja/docs/Glossary/Preflight_request){ target=_blank } によって Origin ヘッダーを検証します。

<!-- textlint-enable ja-technical-writing/sentence-length -->

![プリフライトリクエスト](../../images/app-architecture/security/preflight-request-light.png#only-light){ loading=lazy }
![プリフライトリクエスト](../../images/app-architecture/security/preflight-request-dark.png#only-dark){ loading=lazy }

正しいオリジンからのリクエストの場合にはプリフライトリクエストの結果として `204` のレスポンスが返却されるため、メインリクエストを正常に送ることができます。
しかし、異なるオリジンからのリクエストの場合はプリフライトリクエストの結果として `403` のレスポンスが返却されるため、悪意のあるメインリクエストが送られることを事前にブロックできます。

#### 単純リクエストにおける更新系処理の実行禁止 {#prohibition-of-update-operations-on-get-requests}

[単純リクエスト :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/HTTP/Guides/CORS#%E5%8D%98%E7%B4%94%E3%83%AA%E3%82%AF%E3%82%A8%E3%82%B9%E3%83%88){ target=_blank } では、 プリフライトリクエストが発生しません。

そのため、単純リクエストに対しては Origin ヘッダーの検証が行われず、単純リクエストに更新系の処理が含まれていると CSRF 攻撃の対象になってしまいます。

よって、単純リクエストには更新系の処理を含まないように Web API を設計する必要があります。

#### Cookie の属性付与 {#granting-cookie}

Cookie を使用する際には、悪意のあるサイトで Cookie にアクセスされたり Cookie が異なるドメインに送られたりすることで、 Cookie を利用したデータの更新が不正に行われることを防止する必要があります。
Cookie に属性が設定されていない場合ブラウザー側で `SameSite = Lax` として扱われますが、 CSRF 対策を実現するために以下の属性を付与します。

- `HttpOnly` を設定し、 JavaScript から Cookie へアクセスできないようにする。
- アプリケーションがクロスオリジンの構成をとる場合は、別オリジンとの通信を許可するために `SameSite = None` を設定することが強制される。そのため、 `Secure` を必ず設定して HTTPS プロトコル上の暗号化されたリクエストのみで Cookie を送るようにする（ [参照 :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/HTTP/Cookies){ target=_blank } ）。
- アプリケーションが同一オリジンの構成をとる場合は、認証情報などのセキュリティを格納する必要がある際には `SameSite = Strict` を設定する。

これらにより、 CSRF 対策を実現します。

??? info "その他の CSRF 対策の方法"

    [後述](#ssr-application) のとおり、Open Web Application Security Project (OWASP) では、 CSRF 対策のベストプラクティスとして上記以外の方法も提唱しています。

    - CSRF トークンの付与（ [後述](#ssr-application) ）
    - カスタムヘッダーの付与
    
        固有のヘッダーを付与することで、全てのリクエストがクロスオリジンのプリフライトリクエストの対象となります。
        これにより、固有のヘッダーが存在しないオリジンからのリクエストはブロックされます。

    AlesInfiny Maris での対策に加えてこれらの方法を導入することで、多段階のより厳重な CSRF 対策を実現できます。
    セキュリティ要件やビジネスニーズに応じてこれらの対策を追加で実装するかを検討してください。

#### CSR アプリケーションの実装 {#csr-application-programming}

バックエンドアプリケーションを ASP.NET Core Web API で構築する場合、各方針に対して以下のように実装します。

- Origin ヘッダーの検証

    [こちら](#verification-of-origin-header) で説明した通り、ブラウザーが発行するプリフライトリクエストの Origin ヘッダーをバックエンドアプリケーションで検証し、その結果をレスポンスとして返却します。
    ブラウザーは、そのレスポンスをもとにメインリクエストをバックエンドアプリケーションに送るかどうかを決定します。
    <!-- textlint-disable @textlint-ja/no-synonyms -->
    なお、フロントエンドアプリケーションとバックエンドアプリケーションのオリジンの構成によって検証の設定が異なるため、注意が必要です。
    <!-- textlint-enable @textlint-ja/no-synonyms -->

    - クロスオリジンで構成されている場合

        アプリケーションがクロスオリジンで構成されている場合、バックエンドアプリケーションはフロントエンドアプリケーションのオリジンからのリクエストは許可するよう設定する必要があります。
        そのため、 Spring Security を利用してフロントエンドアプリケーションのオリジンからのリクエストを許可するよう CORS の設定を変更します。
        実装方法については、[こちら](../../guidebooks/how-to-develop/csr/cors/index.md) を参照してください。

    - 同一オリジンで構成されている場合

        バックエンドアプリケーションで CORS に関する設定が実装されていないと、ブラウザーがプリフライトリクエストを発行した際のレスポンスはエラーを返却します。
        そのため、ブラウザーは異なるオリジンからのリクエストと判断してバックエンドアプリケーションへリクエストが送られることをブロックします。
        よって、アプリケーションが同一オリジンで構成されている場合、バックエンドアプリケーションで CORS を設定する必要はありません。

- Cookie の属性付与

    アプリケーション構成設定の値をもとに Cookie の属性を付与します。

### SSR アプリケーションでの対策 {#ssr-application}

OWASP では、 CSRF 対策のベストプラクティスとして Origin ヘッダーの検証以外の方法も提唱しています。
詳細は [こちら :material-open-in-new:](https://cheatsheetseries.owasp.org/cheatsheets/Cross-Site_Request_Forgery_Prevention_Cheat_Sheet.html){ target=_blank } を参照してください。

SSR アプリケーションでは、 OWASP が提唱する方法のうち、 CSRF トークンの付与を使用します。

- CSRF トークンの付与

    フロントエンドアプリケーションがリクエストを発行する際に、バックエンドアプリケーションでそのリクエスト内のトークンの存在と有効性を検証することで、正しいクライアントからのリクエストであることを保証する方法です。

これは、 Blazor が CSRF トークンの付与・検証を実現する機能を提供しており、それを利用することにより最小限のコードで CSRF 対策を実現できるためです。

#### SSR アプリケーションの実装 {#ssr-application-programming}

SSR アプリケーションでは、 Blazor が提供する Antiforgery サービスを使用します。

Blazor Web アプリでは `Program.cs` 上で `AppRazorComponent` が呼び出されると、自動的に Antiforgery サービスが追加されます。
Antiforgery ミドルウェアを有効化するには、 [`UseAntiforgery` :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.builder.antiforgeryapplicationbuilderextensions.useantiforgery){ target=_blank } を呼び出す必要があります。

`UseAntiforgery` の呼び出しは、 Visual Studio のプロジェクトテンプレートを使用して Blazor Web アプリを作成した場合、既定で `Program.cs` に追加されます。

なお、`UseAntiforgery` の呼び出しは、 `UseAuthentication` と `UseAuthorization` の間に置く必要があります。
後から `UseAuthentication` や `UseAuthorization` を追加する場合は、呼び出し位置に注意してください。

```csharp title="Program.cs での Antiforgery 有効化" hl_lines="14"
var builder = WebApplication.CreateBuilder(args);

// 省略

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// その他のミドルウェア有効化は省略

app.UseAuthentication();

app.UseAntiforgery();

app.UseAuthorization();

app.Run();
```

<!-- textlint-disable ja-technical-writing/sentence-length -->
Antiforgery サービスおよびミドルウェアを有効にした状態で、 [`EditForm` に基づく入力フォーム :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/forms){ target=_blank } を使用すれば、 Antiforgery トークンの生成と検証が自動的に行われます。
<!-- textlint-enable ja-technical-writing/sentence-length -->
追加のコードは必要ありません。

```razor title="Antiforgery が既定で有効な EditForm" hl_lines="5-13"
@page "/starship-2"
@using System.ComponentModel.DataAnnotations
@inject ILogger<Starship2> Logger

<EditForm Model="Model" OnValidSubmit="Submit" FormName="Starship2">
    <DataAnnotationsValidator />
    <ValidationSummary />
    <label>
        Identifier: 
        <InputText @bind-Value="Model!.Id" />
    </label>
    <button type="submit">Submit</button>
</EditForm>

@code {
    [SupplyParameterFromForm]
    private Starship? Model { get; set; }

    protected override void OnInitialized() => Model ??= new();

    private void Submit() => Logger.LogInformation("Id = {Id}", Model?.Id);

    public class Starship
    {
        [Required]
        [StringLength(10, ErrorMessage = "Id is too long.")]
        public string? Id { get; set; }
    }
}
```

なお、 `EditForm` ではなく `form` タグを使用する場合は、`<form>` ～ `</form>` 内に `<AntiforgeryToken />` タグを含める必要があります。

```html title="form タグを使用する場合の Antiforgery 有効化" hl_lines="2"
<form method="post" @onsubmit="Submit" @formname="starshipForm">
    <AntiforgeryToken />
    <input id="send" type="submit" value="Send" />
</form>
```
