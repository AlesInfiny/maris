<!-- textlint-disable @textlint-rule/require-header-id -->
<!-- markdownlint-disable-file CMD001 -->
<!-- cSpell:ignore Validatable signupsignin onmicrosoft b2clogin -->

# Azure Active Directory B2C による認証サンプル

> [!WARNING]
> Azure Active Directory B2C の販売（新規購入）は、 2025 年 5 月 1 日をもって終了しています。
> また、[Microsoft の公開情報](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/faq?tabs=app-reg-ga) では、サポート継続期間は **2030 年 5 月** までとなっています。
>
> これに伴い、 **本サンプルについても 2026 年 12 月 31 日** をもってサポートを終了し、
> 今後の機能追加やメンテナンスは予定していません。
> Azure Active Directory B2C を利用した認証の新規導入を考えている場合には、
> 現在推奨されている Microsoft Entra External ID への移行を検討してください。
> AlesInfiny Maris OSS Edition で提供している Microsoft Entra External ID のサンプルについては、
> [こちら](https://github.com/AlesInfiny/maris/tree/main/samples/ExternalIDSampleForSPA) を参照してください。

## このサンプルについて

<!-- textlint-disable ja-technical-writing/sentence-length -->

本サンプルは、 クライアントサイドレンダリングのシングルページアプリケーション（SPA）において、 Azure Active Directory B2C （以降、 Azure AD B2C ）を利用したユーザー認証を実装するためのコード例を提供します。

<!-- textlint-enable ja-technical-writing/sentence-length -->

あわせて本ドキュメントでは、以下について説明します。

- 本サンプルの動作確認手順
- AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）のサンプルアプリケーションである Dressca への組み込み手順

## 前提

本サンプルを動作させるためには、以下が必要です。

- Azure サブスクリプション
- サブスクリプション内、またはサブスクリプション内のリソースグループ内で共同作成者以上のロールが割り当てられている Azure アカウント

Azure サブスクリプションを持っていない場合、 [無料アカウントを作成](https://azure.microsoft.com/ja-jp/free) できます。

## 動作環境

本サンプルは以下の環境で動作確認を行っています。

- .NET 10
- Node.js v24.14.1
- Visual Studio 2026 18.4.1
- Visual Studio Code 1.115.0

## サンプルの構成

本サンプルは、クライアントブラウザー上で動作するフロントエンドアプリケーション (SPA) と、 SPA が呼び出すバックエンドアプリケーション (Web API) によって構成されています。
フォルダー構成は以下のとおりです。

```text
ルートフォルダー
├ auth-backend ....... バックエンドアプリケーションが配置されたフォルダー
├ auth-frontend ...... フロントエンドアプリケーションが配置されたフォルダー
└ README.md .......... このファイル
```

バックエンドアプリケーションは ASP.NET Core Web API 、フロントエンドアプリケーションは Vue.js (TypeScript) で作成されています。
また、 AlesInfiny Maris のサンプルアプリケーション Dressca をベースとしており、フォルダー構造、参照する OSS 、名前空間等は Dressca に準拠しています。

### バックエンドアプリケーションの構成

バックエンドアプリケーションを構成するファイルやフォルダーのうち、認証機能に関係があるものを以下に示します。

```text
auth-backend
├ Dressca.sln
├ src
│ ├ Dressca.Web
│ │ ├ appsettings.json ............. Azure AD B2C への接続情報を記載する設定ファイル
│ │ ├ Program.cs ................... Web API アプリケーションのエントリーポイント。 Azure AD B2C による認証を有効化している。
│ │ └ Controllers
│ │ 　 ├ ServerTimeController.cs ... 認証の必要がない Web API を配置しているコントローラー
│ │ 　 └ UserController.cs ......... 認証が必要な Web API を配置しているコントローラー
│ └ Dressca.Web.Dto ................ Web API の戻り値の型を配置しているプロジェクト
└ tests
  └ Dressca.IntegrationTest ........ 結合テストプロジェクト

```

### フロントエンドアプリケーションの構成

フロントエンドアプリケーションを構成するファイルやフォルダーのうち、認証機能に関係があるものを以下に示します。

```text
auth-frontend
└ app
  ├ .env.dev .............................. 開発環境での Azure AD B2C への接続情報を記載する設定ファイル
  ├ env.d.ts .............................. 環境変数の型定義をする TypeScript ファイル
  ├ redirect.html ......................... Redirect Bridge Page 用の HTML ファイル
  └ src
  　 ├ App.vue ............................ 画面。本サンプルでは画面は App.vue のみ。
  　 ├ api-client
  　 │ ├ __tests__
  　 │ │  └ api-client.spec.ts ............ Web API 呼び出しに関するテストを記述する TypeScript ファイル
  　 │ └ index.ts ......................... Web API 呼び出し時の共通処理を記述する TypeScript ファイル
  　 ├ generated .......................... 自動生成された Axios のコードが配置されるフォルダー
  　 ├ services
  　 │  ├ authentication
  　 │  │ ├ authentication-service.ts ..... 認証（サインイン、トークン取得）を行うサービス
  　 │  │ └ authentication-config.ts ...... 上のコードが使用する設定ファイル
  　 │  ├ server-time
  　 │  │ └ server-time-service.ts ........ 認証の必要がない処理を行うサービス
  　 │  └ user
  　 │    └ user-service.ts ............... 認証の必要がある処理を行うサービス
  　 └ stores
  　 　 ├ authentication
  　 　 │ └ authentication.ts ............. 認証の状態を保持するストア
  　 　 ├ server-time
  　 　 │ └ server-time.ts ................ 認証の必要がない Web API 呼び出しの結果を保持するストア
  　 　 └ user
  　 　 　 └ user.ts ...................... 認証が必要な Web API 呼び出しの結果を保持するストア
```

## サンプルのシナリオ

本サンプルは、ユーザー認証が必要な Web API に対し、 Azure AD B2C を利用してその機能を提供します。
本サンプルでは、ユーザー認証が必要な Web API と、認証が不要な Web API の両方を実装しています。
これにより、認証を必要とする Web API を選択して保護できます。
本サンプルのシナリオは以下の通りです。

1. サンプルを起動すると、ブラウザーに SPA のトップ画面が表示されます。
1. 現在時刻を取得する Web API が認証情報なしで正常に呼び出され、トップ画面に表示されます。
1. ユーザー固有の ID （JWT における sub の値）を取得する Web API が認証情報なしで呼び出され、未認証によるエラーのアラートが表示されます。
1. トップ画面の「 `ログイン` 」をクリックすると、 Azure AD B2C の `サインイン` 画面がポップアップで表示されます。
1. `サインイン` または `サインアップ` が成功すると、ポップアップが閉じます。
1. 成功した認証情報に基づき、再度ユーザー固有の ID （JWT における sub の値）を取得する Web API が呼び出され、トップ画面に結果が表示されます。
1. トップ画面の「`更新`」をクリックすると、現在時刻を再度取得します。本 Web API は、引き続き認証機能なしで呼び出されます。

※本サンプルでは `サインイン` と `サインアップ` のシナリオのみ提供しており、 `サインアウト` は存在しません。

## サンプルで実現している認証フロー

本サンプルでは、 Microsoft 認証ライブラリ（ MSAL ）の使用によって、 [OAuth 2.0 承認コードフロー with PKCE](https://auth0.com/docs/get-started/authentication-and-authorization-flow/authorization-code-flow-with-pkce) を実現しています。

なお、以下の処理はフロントエンドの [MSAL.js](https://www.npmjs.com/package/@azure/msal-browser) (JavaScript 用 Microsoft Authentication Library) によって行われます。

- code_verifier の生成・送信
- code_challenge の生成・送信

また、`MSAL.js v5` 以降、すべての認証フローにおいて MSAL リダイレクトブリッジを実装した専用のリダイレクトページが必要になりました。
これは、 [COOP（Cross-Origin-Opener-Policy）ヘッダー](https://developer.mozilla.org/ja/docs/Web/HTTP/Reference/Headers/Cross-Origin-Opener-Policy) をサポートし、ポップアップウィンドウとメインアプリケーション間の安全な通信を可能にするためです。
そのため、本サンプルでは認証結果の受け取り先として `redirect.html` を用意しています。
詳細については、 [こちら](https://learn.microsoft.com/ja-jp/entra/msal/javascript/browser/login-user#redirecturi-considerations) を参照してください。

## 前提となる OSS ライブラリ

本サンプルでは、バックエンド、フロントエンドアプリケーションそれぞれで OSS を使用しています。

- バックエンドアプリケーション
    - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
    - [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer) （※テストプロジェクトで利用）
- フロントエンドアプリケーション
    - [MSAL.js](https://www.npmjs.com/package/@azure/msal-browser)

その他の使用 OSS は、 AlesInfiny Maris のサンプルアプリケーションに準じます。

## サンプルの動作確認手順

本サンプルをローカルマシンで動作させるには、事前に Azure AD B2C のテナントを作成し、アプリケーションを登録する作業が必要です。

### Azure AD B2C テナントの作成

1. [Microsoft のチュートリアル「 Azure AD B2C テナントを作成する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#create-an-azure-ad-b2c-tenant) に従って、 [Azure ポータル](https://portal.azure.com/) にサインインし、 Azure AD B2C テナントを作成します。
   - 「`初期ドメイン名`」をメモします。
1. [Microsoft のチュートリアル「 B2C テナント ディレクトリを選択する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#select-your-b2c-tenant-directory) に従って、 B2C テナントディレクトリに切り替えます。
1. [Microsoft のチュートリアル「 Azure AD B2C をお気に入りとして追加する (省略可能)」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#add-azure-ad-b2c-as-a-favorite-optional) に従って、 Azure ポータル上で「 Azure サービス」から「 Azure AD B2C 」を選択しお気に入りに登録します。

### Azure AD B2C テナントを利用するアプリの登録（バックエンドアプリケーション）

1. [Microsoft のチュートリアル「 Azure Active Directory B2C テナントに Web API アプリケーションを追加する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga) に従い、バックエンドアプリケーションを Azure AD B2C に登録します。
   - 登録したアプリの名前を、ここでは「 `SampleWebAPI` 」とします。
   - 登録したアプリの `クライアント ID` （アプリケーション ID ）をメモします。
1. [Microsoft のチュートリアル「スコープを構成する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga#configure-scopes) に従って、アプリにスコープを追加します。
   - チュートリアルの手順では読み取りと書き込み 2 つのスコープを作成していますが、本サンプルのシナリオでは作成するスコープは 1 つで良いです。
   - 追加したスコープの名前を、ここでは「 `api.read` 」とします。
1. Azure ポータルのお気に入りから「 Azure AD B2C 」を選択します。
1. 「アプリの登録」ブレードを選択し、「すべてのアプリケーション」から「 SampleWebAPI 」を選択します。
1. 「概要」ブレードに表示された「 `アプリケーション ID の URI` 」をメモします。

### Azure AD B2C テナントを利用するアプリの登録（フロントエンドアプリケーション）

1. [Microsoft のチュートリアル「 SPA アプリケーションの登録」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-register-spa#register-the-spa-application) に従って、フロントエンドアプリケーションを Azure AD B2C に登録します。
   - 登録したアプリの名前を、ここでは「 `SampleSPA` 」とします。
   - 登録したアプリの `クライアント ID` （アプリケーション ID ）をメモします。
   - 「暗黙的フロー」に関する設定は無視してください。
1. Azure ポータルのお気に入りから「 Azure AD B2C 」を選択します。
1. 「アプリの登録」ブレードを選択し、「すべてのアプリケーション」から「 SampleSPA 」を選択します。
1. 「認証」ブレードを選択し、「シングルページアプリケーション」の「リダイレクト URI」に以下の URI を追加します。
   - `http://localhost:5173`
   - `http://localhost:5173/redirect.html`
1. [Microsoft のチュートリアル「[アクセス許可の付与]」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga#grant-permissions) に従い、 SampleSPA に、前の手順で追加した SampleWebAPI のスコープ「 `api.read` 」へのアクセス許可を付与します。

### ユーザーフローの作成

1. [Microsoft のチュートリアル「Azure Active Directory B2C でサインアップおよびサインイン フローを設定する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-sign-up-and-sign-in-policy?pivots=b2c-user-flow) に従って、 `サインアップとサインイン` ユーザーフローを作成します。
   - ここでは追加した `サインアップとサインイン` ユーザーフローの名前を「 `signupsignin1` 」とします（ユーザーフローの名前には自動的に『`B2C_1_`』プレフィックスが付与されます）。

### 設定情報の記入

#### バックエンドアプリケーションの設定

1. `auth-backend\src\Dressca.Web\appsettings.json` を開きます。
1. 以下のように設定情報を記入します（以下の例では Azure AD B2C の設定以外は省略しています）。

    ```json
    {
        "AzureAdB2C": {
            "Instance": "https://[初期ドメイン名].b2clogin.com",
            "ClientId": "[SampleWebAPI のクライアント ID]",
            "Domain": "[初期ドメイン名].onmicrosoft.com",
            "SignUpSignInPolicyId": "[追加した「サインアップとサインインのユーザーフロー」の名前。本サンプルでは B2C_1_signupsignin1]"
        }
    }
    ```

#### フロントエンドアプリケーションの設定

1. `auth-frontend\.env.dev` を開きます。
1. 以下のように設定情報を記入します（以下の例では Azure AD B2C の設定以外は省略しています）。

    ```properties
    VITE_ADB2C_USER_FLOW_SIGN_IN=[B2C_1_[『サインアップとサインイン』のユーザフロー名]。 本サンプルでは B2C_1_signupsignin1]
    VITE_ADB2C_SIGN_IN_URI=https://[初期ドメイン名].b2clogin.com/[初期ドメイン名].onmicrosoft.com/B2C_1_[『サインアップとサインイン』のユーザフロー名]
    VITE_ADB2C_AUTHORITY_DOMAIN=[初期ドメイン名].b2clogin.com
    VITE_ADB2C_SCOPE=[APIの公開で設定したApplication ID URI]/[Web APIに追加したスコープの名前]
    VITE_ADB2C_APP_CLIENT_ID=[クライアントアプリケーションのクライアントID]
    VITE_ADB2C_REDIRECT_URI=[クライアントアプリケーションのリダイレクトURI。本サンプルの既定では http://localhost:5173/redirect.html]
    ```

### 動作確認

1. ターミナルで `auth-frontend` のフォルダーへ移動し、 `npm install` を実行します。
1. Visual Studio で `auth-backend\Dressca.sln` を開きます。
1. `Dressca.Web` を右クリックし「スタートアッププロジェクトに設定」を選択します。
1. ソリューションをデバッグなしで開始します。ブラウザーが起動し、しばらく待つと SPA の初期画面が表示されます。
1. 画面の「 `ログイン` 」をクリックします。 Azure AD B2C のサインイン画面がポップアップで表示されます。
1. 「 Sign up now 」リンクをクリックします。
1. 使用可能なメールアドレスを入力し、「 Send verification code 」をクリックします。
1. 上の手順で入力したメールアドレス宛に Verification code が送信されるので、画面に入力して「 Verify code 」をクリックします。
1. 画面に新しいパスワード等の必要事項を入力し、「 Create 」をクリックします。
1. `サインイン` が成功し、画面上に「ユーザー ID 」が表示されれば成功です。以降は入力したメールアドレスとパスワードで `サインイン` できるようになります。

Azure AD B2C に追加したユーザーは、以下の手順で削除できます。

1. Azure ポータルのお気に入りから「 Azure AD B2C 」を選択します。
1. 「ユーザー」ブレードを選択します。
1. 対象のユーザーをチェックし、画面上部から「削除」を選択します。

### テストの実行

バックエンドアプリケーションの `Dressca.IntegrationTest` には、認証が必要な Web API および認証不要な Web API の両方についての結合テストが実装されています。
Visual Studio で本サンプルのソリューションを開き、 `テストエクスプローラー` ウィンドウからテストを実行してください。

※[設定情報の記入](#設定情報の記入) 前でもテストを実行できます。

## Dressca アプリケーションへの認証機能の組み込み手順

本サンプルのコード例を既存のアプリケーションへコピーすることで、 Azure AD B2C の認証機能を組み込むことができます。
本章ではそのコード例を AlesInfiny Maris のサンプルアプリケーションである Dressca アプリケーションに組み込む方法を、具体的な手順として説明します。

### バックエンドアプリケーション

1. ASP.NET Core Web API プロジェクトに対して以下の NuGet パッケージをインストールします。
   - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
1. ASP.NET Core Web API プロジェクトの Program.cs に Azure AD B2C の設定を追加します。

    ```csharp
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Identity.Web;
    using NSwag;
    using NSwag.Generation.Processors.Security;

    var builder = WebApplication.CreateBuilder(args); // （既存のコード）

    builder.Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            // Web API アクションメソッドにおいてエラーの型を指定しなかったときに
            // 自動的に ProblemDetails へ変換されることを抑止します。
            options.SuppressMapClientErrors = true;
        });

    // Open API ドキュメントの security scheme を有効化します。
    builder.Services.AddOpenApiDocument(config =>
    {
        config.AddSecurity("Bearer", new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            Description = "この API は Bearer トークンによる認証が必要です。",
        });
        config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
    });

    // Azure AD B2C 認証に必要な設定をインジェクションします。
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(
        options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options);
            options.TokenValidationParameters.NameClaimType = "name";
        },
        options => { builder.Configuration.Bind("AzureAdB2C", options); });

    var app = builder.Build(); // （既存のコード）

    // 認証を有効化します。
    app.UseAuthentication();
    app.UseAuthorization();
    ```

    ※ `app.UseAuthentication` および `app.UserAuthorization` の呼び出し位置は、[ミドルウェアの順序](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/middleware/#middleware-order) に従ってください。

    <!-- textlint-disable ja-technical-writing/sentence-length -->

1. [バックエンドアプリケーションの設定](#バックエンドアプリケーションの設定) を参照し、 `auth-backend\src\Dressca.Web\appsettings.json` に記述した Azure AD B2C の設定を ASP.NET Core Web API プロジェクトの `appsettings.json` へコピーします。

    <!-- textlint-enable ja-technical-writing/sentence-length -->

1. 認証を必要とする Web API に `[Authorize]` 属性を付与します。 `[Authorize]` 属性は Web API Controller クラスにも、個別のアクションメソッドにも付与できます。
本例では、 OrdersController.cs に対して設定した例を示します。

    ```csharp
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class OrdersController : ControllerBase
    {
       // 省略
    }
    ```

### フロントエンドアプリケーション

以下、 Dressca アプリケーション (Consumer) のフロントエンドアプリケーションに認証機能を適用する手順を示します。

1. VS Code で `dressca-frontend` のフォルダーの `dressca-frontend.code-workspace` ファイルを開きます。
1. ターミナルで `consumer` フォルダーに移動し、 `npm install @azure/msal-browser` を順に実行し、フロントエンドアプリケーションに MSAL.js をインストールします。
1. 本サンプルの以下のファイルを `consumer` フォルダーにコピーします。
    - redirect.html
1. 本サンプルの `.env.dev` に記述した Azure AD B2C の設定をフロントエンドアプリケーションの `.env.dev` にコピーします。
1. `env.d.ts` のインターフェースに、前の手順で `.env.dev` に追加したプロパティを追加します。

    ```diff
    interface ImportMetaEnv {
      readonly VITE_NO_ASSET_URL: string
      readonly VITE_ASSET_URL: string
      readonly VITE_AXIOS_BASE_ENDPOINT_ORIGIN: string
      readonly VITE_PROXY_ENDPOINT_ORIGIN: string
    + readonly VITE_ADB2C_USER_FLOW_SIGN_IN: string
    + readonly VITE_ADB2C_SIGN_IN_URI: string
    + readonly VITE_ADB2C_AUTHORITY_DOMAIN: string
    + readonly VITE_ADB2C_SCOPE: string
    + readonly VITE_ADB2C_APP_CLIENT_ID: string
    + readonly VITE_ADB2C_REDIRECT_URI: string
    }
    ```

1. `npm run generate-client` を実行し、 Axios のクライアントコードを再生成します。
1. `src\services\authentication` フォルダーで、本サンプルの以下のコードをコピー・差し替えします。
    - authentication-service.ts
    - authentication-config.ts
1. `src\stores\authentication\authentication.ts` を本サンプルのコードに差し替えます。
1. 認証が成功したら、認証が必要な Web API リクエストヘッダーに Bearer トークンを付与する必要があります。
   本例では、 OrdersApi アクセス時に Bearer トークンを付与する例を示します。
   `src\api-client\index.ts` を以下のように編集します。

    ```typescript
    import { authenticationService } from '@/services/authentication/authentication-service'
    // その他のコードは省略
    async function addToken(config: apiClient.Configuration): Promise<void> {
      const { isAuthenticated, getToken } = authenticationService()
      if (isAuthenticated()) {
        const token = await getToken()
        config.accessToken = token
      }
    }

    async function ordersApi() {
      const config = createConfig()
      // 認証が必要な API では、addToken を呼び出します。
      await addToken(config)
      const ordersApi = new apiClient.OrdersApi(config, '', axiosInstance)
      return ordersApi
    }
    ```

1. `src\App.vue` に対して、 `<script>` セクションに以下のコードを追加します。

    ```typescript
    import { useLogger } from './composables/use-logger'
    import { useCustomErrorHandler } from '@/shared/error-handler/custom-error-handler'
    import { BrowserAuthError } from '@azure/msal-browser'

    const { signIn, signOut, isAuthenticated } = authenticationService()
    const logger = useLogger()
    const handleErrorAsync = useCustomErrorHandler()

    const signInButtonClicked = async () => {
      try {
        await signIn()
      } catch (error) {
        // ポップアップ画面をユーザーが×ボタンで閉じると、 BrowserAuthError が発生します。
        if (error instanceof BrowserAuthError) {
          // 認証途中でポップアップを閉じることはよくあるユースケースなので、ユーザーには特に通知しません。
          await handleErrorAsync(error, () => {
            logger.info('ユーザーが認証処理を中断しました。')
          })
        } else {
          await handleErrorAsync(error, () => {
            window.alert('Azure AD B2C での認証に失敗しました。')
          })
        }
      }
    }
    ```

1. `src\App.vue` に対して、 `<template>` セクションのボタンを以下のように差し替えます。

   ```html
    <header>
      <nav
        aria-label="Jump links"
        class="py-5 text-lg font-medium text-gray-900 shadow-xs ring-1 ring-gray-900/5"
      >
        <div class="mx-auto flex justify-between px-4 md:px-24 lg:px-24">
          <div>
            <router-link class="text-2xl" to="/"> Dressca </router-link>
          </div>
          <div class="flex gap-5 sm:gap-5 lg:gap-12">
            <router-link to="/basket">
              <ShoppingCartIcon class="h-8 w-8 text-amber-600" />
            </router-link>
            <button v-if="!isAuthenticated()" @click="signInButtonClicked">ログイン</button>
          </div>
        </div>
      </nav>
    </header>
   ```
  
1. `src\views\authentication\LoginView.vue` は Azure AD B2C の LoginPopup ウィンドウに切り替わるため削除します。
1. `src\shared\authentication\authentication-guard.ts` はログインページではなく Azure AD B2C の LoginPopup を表示させるように変更します。

    ```typescript
    // その他のコードは省略
    import { authenticationService } from '@/services/authentication/authentication-service'
    if (to.meta.requiresAuth && !authenticationStore.isAuthenticated) {
      try {
        await authenticationService().signIn()
      } catch {
        return false
      }
    }
    ```

1. `src\router\index.ts` から、 `authenticationRoutes` を削除します。

1. BrowserAuthError が発生した場合は、エラーページに遷移させないように `src\shared\error-handler\custom-error-handler.ts` に以下を追加します。

    ```diff
    import { BrowserAuthError } from '@azure/msal-browser'

    export function useCustomErrorHandler(): handleErrorAsyncFunction {
    const { t } = i18n.global
    const handleErrorAsync = async (
      error: unknown,
      callback: MaybeAsyncFunction<void>,
      handlingHttpError: MaybeAsyncUnaryFunction<HttpError, void> | null = null,
      handlingUnauthorizedError: MaybeAsyncFunction<void> | null = null,
      handlingNetworkError: MaybeAsyncFunction<void> | null = null,
      handlingServerError: MaybeAsyncFunction<void> | null = null,
    ) => {
      const logger = useLogger()
      const unhandledErrorEventBus = useEventBus(unhandledErrorEventKey)
      const unauthorizedErrorEventBus = useEventBus(unauthorizedErrorEventKey)
      // ハンドリングできるエラーの場合はコールバックを実行します。
    + if (error instanceof BrowserAuthError) {
    +   await callback()
    +   return
    + }
      if (error instanceof CustomErrorBase) {
        logger.error(JSON.stringify(error.toJSON()))
        await callback()
        if (error instanceof HttpError) {
          // 業務処理で発生した HttpError を処理します。
          if (handlingHttpError) {
            await handlingHttpError(error)
          }
          // その他のコードは省略
          ...
        }
      }
    }
    ```

## 参照記事

本サンプルは、以下の記事に基づき作成しました。

### フロントエンドアプリケーションの参照記事

- [Azure AD B2C を利用した SPA アプリケーションサンプル](https://github.com/Azure-Samples/ms-identity-b2c-javascript-spa/tree/main)

### バックエンドアプリケーションの参照記事

- [Azure AD B2C を使用して独自の Web API で認証を有効にする](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/enable-authentication-web-api?tabs=csharpclient)
