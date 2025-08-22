<!-- textlint-disable @textlint-rule/require-header-id -->
<!-- markdownlint-disable-file CMD001 -->
<!-- cSpell:ignore onmicrosoft signupsignin -->

# Microsoft Entra External ID による認証サンプル

## このサンプルについて

Microsoft Entra External ID （以降、 Entra External ID ）によるユーザー認証の簡単な実装サンプルを提供します。

本サンプルは、クライアントサイドレンダリングアプリケーションにおいて Entra External ID を利用する場合のコード例として利用できます。

<!-- textlint-disable ja-technical-writing/sentence-length -->

また、 SPA アプリケーション（ AlesInfiny Maris OSS Edition（以降、 AlesInfiny Maris ） のアーキテクチャに準拠したアプリケーション）に本サンプルのファイルやコードをコピーすることで、 Entra External ID による認証機能を組み込むことができます。

<!-- textlint-enable ja-technical-writing/sentence-length -->

## 前提

本サンプルを動作させるためには、以下が必要です。

- Azure サブスクリプション
- サブスクリプションに対してテナント作成者、アプリケーション開発者以上のロールが割り当てられている Azure アカウント

Azure サブスクリプションを持っていない場合、 [無料アカウントを作成](https://azure.microsoft.com/ja-jp/free) できます。

## 動作環境

本サンプルは以下の環境で動作確認を行っています。

- .NET 8
- Node.js v22.17.1
- Visual Studio 2022 17.14.10
- Visual Studio Code 1.102.3

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
│ │ ├ appsettings.json ............. Entra External ID への接続情報を記載する設定ファイル
│ │ ├ Program.cs ................... Web API アプリケーションのエントリーポイント。 Entra External ID による認証を有効化している。
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
  ├ .env.dev .............................. Entra External ID への接続情報を記載する設定ファイル
  ├ env.d.ts .............................. 環境変数の型定義をする TypeScript ファイル
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

本サンプルは、ユーザー認証が必要な Web API に対し、 Entra External ID を利用してその機能を提供します。
本サンプルでは、ユーザー認証が必要な Web API と、認証が不要な Web API の両方を実装しています。
これにより、認証を必要とする Web API を選択して保護できます。
本サンプルのシナリオは以下の通りです。

1. サンプルを起動すると、ブラウザーに SPA のトップ画面が表示されます。
1. 現在時刻を取得する Web API が認証機能なしで呼び出され、トップ画面に表示されます。
1. トップ画面の「 `ログイン` 」をクリックすると、 Entra External ID の `サインイン` 画面がポップアップで表示されます。
1. `サインイン` または `サインアップ` が成功すると、ポップアップが閉じます。
1. 成功した認証情報に基づき、ユーザー固有の ID （JWT における sub の値）を取得する Web API が呼び出され、トップ画面に結果が表示されます。
1. トップ画面の「`更新`」をクリックすると、現在時刻を再度取得します。本 Web API は、引き続き認証機能なしで呼び出されます。
1. トップ画面の「`ログアウト`」をクリックすると、 Entra External ID の `サインアウト` 画面がポップアップで表示されます。
1. `サインアウト` が成功すると、ポップアップが閉じます。

## サンプルで実現している認証フロー

本サンプルでは、 Microsoft 認証ライブラリ（ MSAL ）の使用によって、 [OAuth 2.0 承認コードフロー with PKCE](https://auth0.com/docs/get-started/authentication-and-authorization-flow/authorization-code-flow-with-pkce) を実現しています。

なお、以下の処理はフロントエンドの MSAL.js (JavaScript 用 Microsoft Authentication Library) によって行われます。

- code_verifier の生成・送信
- code_challenge の生成・送信

## 前提となる OSS ライブラリ

本サンプルでは、バックエンド、フロントエンドアプリケーションそれぞれで OSS を使用しています。

- バックエンドアプリケーション
    - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
    - [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer) （※テストプロジェクトで利用）
- フロントエンドアプリケーション
    - [MSAL.js](https://www.npmjs.com/package/@azure/msal-browser)

その他の使用 OSS は、 AlesInfiny Maris のサンプルアプリケーションに準じます。

## サンプルの動作方法

本サンプルをローカルマシンで動作させるには、事前に Entra External ID のテナントを作成し、アプリケーションを登録する作業が必要です。

### Entra External ID テナントの作成

1. [クイック スタート: Azure サブスクリプションを使用して外部テナントを作成する](https://learn.microsoft.com/ja-jp/entra/external-id/customers/quickstart-tenant-setup) に従って、[Microsoft Entra 管理センター](https://entra.microsoft.com/) にサインインし、 Entra External ID の外部テナントを作成します。
    - 「`テナントサブドメイン`」（ドメイン名から `.onmicrosoft.com` を除いた部分）をメモします。

### Entra External ID テナントを利用するアプリの登録（バックエンドアプリケーション）

<!-- textlint-disable ja-technical-writing/sentence-length -->

1. [アプリケーションを Microsoft Entra ID に登録する](https://learn.microsoft.com/ja-jp/entra/identity-platform/quickstart-register-app) に従って、バックエンドアプリケーション用のアプリを Entra External ID に登録します。

    - 登録したアプリの名前を、ここでは「 `SampleWebAPI` 」とします。
  　<!-- textlint-disable @textlint-ja/no-synonyms -->
    - サポートされているアカウントの種類を、「この組織ディレクトリのみに含まれるアカウント」とします。
    <!-- textlint-enable @textlint-ja/no-synonyms -->
    - 登録したアプリの `クライアント ID` （アプリケーション ID ）をメモします。

    <!-- textlint-enable ja-technical-writing/sentence-length -->

1. [委任されたアクセス許可 (スコープ) を追加する](https://learn.microsoft.com/ja-jp/entra/identity-platform/quickstart-web-api-dotnet-protect-app?tabs=aspnet-core#add-delegated-permissions-scopes) に従って、アプリにスコープを追加します。
    - チュートリアルの手順では読み取りと書き込み 2 つのスコープを作成していますが、本サンプルのシナリオでは作成するスコープは 1 つで良いです。
    - 追加したスコープの名前を、ここでは「 `api.read` 」とします。
1. 「API の公開」ブレードを選択し、「 `api.read` 」を選択します。
1. スコープの編集画面で、「同意できるのはだれですか?」で「管理者とユーザー」を選択し、保存します。
1. 表示されている「 `アプリケーション ID の URI` 」をメモします。

### Entra External ID テナントを利用するアプリの登録（フロントエンドアプリケーション）

1. [アプリケーションを Microsoft Entra ID に登録する](https://learn.microsoft.com/ja-jp/entra/identity-platform/quickstart-register-app) に従って、フロントエンドアプリケーション用のアプリを Entra External ID に登録します。
    - 登録したアプリの名前を、ここでは「 `SampleSPA` 」とします。
  　<!-- textlint-disable @textlint-ja/no-synonyms -->
    - サポートされているアカウントの種類を、「この組織ディレクトリのみに含まれるアカウント」とします。
    <!-- textlint-enable @textlint-ja/no-synonyms -->
    - 登録したアプリの `クライアント ID` （アプリケーション ID ）をメモします。
1. 「アプリの登録」ブレードを選択し、「すべてのアプリケーション」から「 SampleSPA 」を選択します。
1. 「認証」ブレードを選択し、「リダイレクト URI の追加」をクリックします。「シングルページアプリケーション」を選択し、リダイレクト URI に「 `http://localhost:5173` 」を設定します。
1. [Web API にアクセスするためのアクセス許可を追加する](https://learn.microsoft.com/ja-jp/entra/identity-platform/quickstart-configure-app-access-web-apis#add-permissions-to-access-your-web-api) に従って、 SampleSPA に、前の手順で追加した SampleWebAPI のスコープ「 `api.read` 」へのアクセス許可を付与します。

### ユーザーフローの作成と割り当て

1. [外部テナント アプリのサインアップおよびサインイン ユーザー フローを作成する](https://learn.microsoft.com/ja-jp/entra/external-id/customers/how-to-user-flow-sign-up-sign-in-customers) に従って、ユーザーフローを作成します。
    - ここでは追加した `サインアップとサインイン` ユーザーフローの名前を「 `signupsignin1` 」とします。
1. [アプリケーションをユーザー フローに追加する](https://learn.microsoft.com/ja-jp/entra/external-id/customers/how-to-user-flow-add-application) に従って、 SampleSPA を signupsignin1 に追加します。

### 設定情報の記入

#### バックエンドアプリケーションの設定

1. `auth-backend\src\Dressca.Web\appsettings.json` を開きます。
1. 以下のように設定情報を記入します（以下の例では Entra External ID の設定以外は省略しています）。

```json
{
    "EntraId": {
      "Instance": "https://[テナントサブドメイン].ciamlogin.com/",
      "TenantId": "[テナントID]",
      "ClientId": "[SampleWebAPI のクライアント ID]"
    }
}
```

#### フロントエンドアプリケーションの設定

1. `auth-frontend\.env.dev` を開きます。
1. 以下のように設定情報を記入します（以下の例では Entra External ID の設定以外は省略しています）。

```properties
VITE_EXTERNAL_ID_AUTHORITY_DOMAIN=https://[テナントサブドメイン].ciamlogin.com/
VITE_EXTERNAL_ID_SCOPE=[SampleWebAPI のアプリケーション ID の URI]/[Web APIに追加したスコープの名前]
VITE_EXTERNAL_ID_APP_CLIENT_ID=[SampleSPA のクライアント ID]
VITE_EXTERNAL_ID_APP_URI=[フロントエンドアプリケーションのベースとなるURL。サンプルの既定では http://localhost:5173]
```

### 動作確認

1. ターミナルで `auth-frontend` のフォルダーへ移動し、 `npm install` を実行します。
1. Visual Studio で `auth-backend\Dressca.sln` を開きます。
1. `Dressca.Web` を右クリックし「スタートアッププロジェクトに設定」を選択します。
1. ソリューションをデバッグなしで開始します。ブラウザーが起動し、しばらく待つと SPA の初期画面が表示されます。
1. 画面上の「 `ログイン` 」をクリックします。 Entra External ID のサインイン画面がポップアップで表示されます。
1. 「アカウントをお持ちでない場合、作成できます」リンクをクリックします。
1. 使用可能なメールアドレスを入力し、「次へ」をクリックします。
1. 上の手順で入力したメールアドレス宛にアカウント確認コードが送信されるので、画面に入力して「次へ」をクリックします。
1. 画面に新しいパスワード等の必要事項を入力し、「次へ」をクリックします。
<!-- textlint-disable @textlint-ja/no-synonyms -->
1. SampleSPA 、 SampleWebAPI からユーザーデータへのアクセス許可を要求されるので「承諾」をクリックします。
<!-- textlint-enable @textlint-ja/no-synonyms -->
1. サインインが成功し、画面上に「ユーザー ID 」が表示されれば成功です。以降は入力したメールアドレスとパスワードでサインインできるようになります。
1. 画面上の「 `ログアウト` 」をクリックします。 Entra External ID のサインアウト画面がポップアップで表示されます。
1. サインアウトするアカウントをクリックします。
1. サインアウトに成功すると、画面上から「ユーザー ID 」 の表示が消え、「 `ログイン` 」が表示されます。

Entra External ID に追加したユーザーは、以下の手順で削除できます。

1. Microsoft Entra 管理センターの「ユーザー」ブレードを選択します。
1. 対象のユーザーをチェックし、画面上部から「削除」を選択します。

### テストの実行

バックエンドアプリケーションの `Dressca.IntegrationTest` には、認証が必要な Web API および認証不要な Web API の両方についての結合テストが実装されています。
Visual Studio で本サンプルのソリューションを開き、 `テストエクスプローラー` ウィンドウからテストを実行してください。

※[設定情報の記入](#設定情報の記入) 前でもテストを実行できます。

## アプリケーションへの認証機能の組み込み

本サンプルのコードを既存のアプリケーションへコピーすることで、 Entra External ID の認証機能を組み込むことができます。
なお、対象のアプリケーションは AlesInfiny Maris のクライアントサイドレンダリングアプリケーションです。

### バックエンドアプリケーション

1. ASP.NET Core Web API プロジェクトに対して以下の NuGet パッケージをインストールします。
   - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
1. ASP.NET Core Web API プロジェクトの Program.cs に Entra External ID の設定を追加します。

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

    // Entra External ID 認証に必要な設定をインジェクションします。
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(
        options =>
        {
            builder.Configuration.Bind("EntraId", options);
            options.TokenValidationParameters.NameClaimType = "name";
        },
        options => { builder.Configuration.Bind("EntraId", options); });

    var app = builder.Build(); // （既存のコード）

    // 認証を有効化します。
    app.UseAuthentication();
    app.UseAuthorization();
    ```

    ※ `app.UseAuthentication` および `app.UserAuthorization` の呼び出し位置は、[ミドルウェアの順序](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0#middleware-order) に従ってください。

    <!-- textlint-disable ja-technical-writing/sentence-length -->

1. [バックエンドアプリケーションの設定](#バックエンドアプリケーションの設定) を参照し、 `auth-backend\src\Dressca.Web\appsettings.json` に記述した Entra External ID の設定を ASP.NET Core Web API プロジェクトの `appsettings.json` へコピーします。

    <!-- textlint-enable ja-technical-writing/sentence-length -->

1. 認証を必要とする Web API に `[Authorize]` 属性を付与します。 `[Authorize]` 属性は Web API Controller クラスにも、個別のアクションメソッドにも付与できます。

    ```csharp
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class ExampleController : ControllerBase
    {
       // 省略
    }
    ```

### フロントエンドアプリケーション

1. VS Code で `auth-frontend` のフォルダーの `auth-frontend.code-workspace` ファイルを開きます。
1. ターミナルで `npm install @azure/msal-browser` を実行し、フロントエンドアプリケーションに MSAL.js をインストールします。
1. `auth-frontend\.env.dev` に記述した Entra External ID の設定をフロントエンドアプリケーションの `.env.dev` にコピーします。
1. `env.d.ts` のインターフェースに、前の手順で `.env.dev` に追加したプロパティを追加します。

    ```typescript
    interface ImportMetaEnv {
      // 認証に関係のないプロパティは省略
      readonly VITE_EXTERNAL_ID_AUTHORITY_DOMAIN: string
      readonly VITE_EXTERNAL_ID_SCOPE: string
      readonly VITE_EXTERNAL_ID_APP_CLIENT_ID: string
      readonly VITE_EXTERNAL_ID_APP_URI: string
    }
    ```

1. `npm run generate-client` を実行し、 Axios のクライアントコードを再生成します。
1. `src\services\authentication` フォルダーを作成し、サンプルの以下のコードをコピーします。
    - authentication-services.ts
    - authentication-config.ts
1. `src\store\authentication` フォルダーを作成し、サンプルの以下のコードをコピーします。
    - authentication.ts
1. 認証が成功したら、認証が必要な Web API リクエストヘッダーに Bearer トークンを付与する必要があります。
    AlesInfiny Maris のサンプルアプリケーション Dressca の場合、 `src\api-client\index.ts` を編集します。

    ```typescript
    import axios from "axios";
    import * as apiClient from "@/generated/api-client";
    import { authenticationService } from '@/services/authentication/authentication-service';

    // その他のコードは省略

    /** api-client の共通の Configuration があればここに定義します。 */
    function createConfig(): apiClient.Configuration {
      const config = new apiClient.Configuration({
        basePath: import.meta.env.VITE_AXIOS_BASE_ENDPOINT_ORIGIN,
      });

      return config;
    }

    async function addTokenAsync(config: apiClient.Configuration) {
      

      // 認証済みの場合、アクセストークンを取得して Configuration に設定します。
      if (await authenticationService.isAuthenticated()) {
        const token = await authenticationService.getTokenAzureADB2C();
        config.accessToken = token;
      }
    }

    export async function getExampleApi(): Promise<apiClient.ExampleApi> {
      const config = createConfig();

      // 認証が必要な API では、addTokenAsync を呼び出します。
      await addTokenAsync(config);
      const exampleApi = new apiClient.ExampleApi(config, '', axiosInstance);
      return exampleApi;
    }

    export async function getServerTimeApi(): Promise<apiClient.ServerTimeApi> {
      const config = createConfig();

      // 認証が不要な API では、addTokenAsync は呼び出しません。
      const serverTimeApi = new apiClient.ServerTimeApi(
        config,
        '',
        axiosInstance
      );
      return serverTimeApi;
    }
    ```

1. `ログイン` 画面へのリンクを含む Vue ファイルの `<script>` セクションにコードを追加します。

    ```vue
    <script setup lang="ts">
    import { authenticationService } from '@/services/authentication/authentication-service'
    import { useAuthenticationStore } from '@/stores/authentication/authentication'
    const authenticationStore = useAuthenticationStore()

    const signIn = async () => {
      await authenticationService.signInAzureADB2C()
    }
    const signOut = async () => {
      await authenticationService.signOutEntraExternalId()
    }
    </script>
    ```

1. `ログイン` 画面、 `ログアウト` 画面へのリンクを以下のように記述します（クリック時に `signIn` メソッド、 `signOut` メソッドが動作すれば `button` である必要はありません）。

    ```vue
    <button v-if="!authenticationStore.isAuthenticated" @click="signIn()">ログイン</button>
    <button v-if="authenticationStore.isAuthenticated" @click="signOut()">ログアウト</button>
    ```

1. `npm install` を実行し、その他のパッケージをインストールします。

### テスト

認証が必要な Web API のテストでは、 Entra External ID 認証で取得できるアクセストークンの代わりに、テストコード内で生成した JWT トークンをヘッダーに追加してリクエストを送信しています。
送信された JWT トークンはテスト用の [JwtBearer 認証](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer.jwtbearerhandler) で検証しています。

1. 結合テスト用プロジェクトに対して以下の NuGet パッケージをインストールします。
   - [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)

1. 結合テスト用プロジェクトに `auth-backend\tests\Dressca.IntegrationTest\ApiTestWebApplicationFactory.cs` をコピーします。

    ```csharp title="ApiTestWebApplicationFactory.cs"
    using System.Text;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    namespace Dressca.IntegrationTest;

    public class ApiTestWebApplicationFactory<TProgram>
     : WebApplicationFactory<TProgram>
     where TProgram : class
    {
     protected override void ConfigureWebHost(IWebHostBuilder builder)
     {
         builder.ConfigureServices(services =>
         {
             // 構成情報を取得します。
             var config = this.GetConfiguration();

             // デフォルトで使用される認証スキームを"Test"に設定します。
             // (本サンプルのProgram.csの設定ではデフォルトの認証スキームに
             // "Bearer"が設定されているため上書きしています。)
             services.AddAuthentication("Test")
             // "Test"スキームでJwtBearer認証を利用します。
             .AddJwtBearer("Test", options =>
             {
                 // リクエストで送信されるJWTの検証内容を設定します。
                 options.TokenValidationParameters =
                 new TokenValidationParameters
                 {
                     // Issuer,Audience,IssuerSigningKeyを検証対象とします。
                     ValidIssuer = config["Jwt:Issuer"],
                     ValidAudience = config["Jwt:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey
                         (Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new NullReferenceException("Jwt:Key"))),
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = false,
                     ValidateIssuerSigningKey = true
                 };
             });
         });
     }

     internal string CreateToken(string userName)
     {
         // JWTの生成
         // 省略
     }

     internal IConfiguration GetConfiguration()
     {
         // テスト用のappsettings.jsonの内容を取得。
         // 省略
     }

    }

    ```

    上記のコードで設定したテスト用の認証機能は、 `[Authorize]` または `[Authorize(AuthenticationSchemes = "Test")]` が付与された Web API にリクエストが送信される際に動作します。

1. 結合テスト用プロジェクトの　`appsettings.IntegrationTest.json` に `auth-backend\tests\Dressca.IntegrationTest\appsettings.json` の内容をコピーします。
<!-- textlint-disable @textlint-ja/no-synonyms -->
1. `IClassFixture<ApiTestWebApplicationFactory>` を実装するテストクラスを作成し、テストコードを追加します。 JWT をヘッダーに付与して API にリクエストを送信することで、認証済みの状態での API アクセスを再現できます。
<!-- textlint-enable @textlint-ja/no-synonyms -->

    ```csharp
    using System.Net;
    using System.Net.Http.Headers;
    using Xunit;

    namespace Dressca.IntegrationTest;

    public class ApiTest(ApiTestWebApplicationFactory<Program> factory)
        : IClassFixture<ApiTestWebApplicationFactory<Program>>
    {
        private readonly ApiTestWebApplicationFactory<Program> factory = factory;

        [Fact]
        public async Task Get_認証必要なAPI_認証成功_UserIDを返す()
        {
            // Arrange
            var client = this.factory.CreateClient();
            // 取得したJWTをBearerトークンに設定します。
            var token = this.factory.CreateToken("testUser");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            // 認証が必要なAPIにリクエストを送信します。
            var response = await client.GetAsync("api/users");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("{\"userId\":\"testUser\"}", result);
        }
    }

    ```

## 参照記事

本サンプルは、以下の記事に基づき作成しました。

### フロントエンドアプリケーションの参照記事

- [Entra External ID を利用した SPA アプリケーションサンプル](https://github.com/Azure-Samples/ms-identity-ciam-javascript-tutorial/tree/main/2-Authorization/0-call-api-vanillajs)

### バックエンドアプリケーションの参照記事

- [クイック スタート: Microsoft ID プラットフォームによって保護されている Web API を呼び出す](https://learn.microsoft.com/ja-jp/entra/identity-platform/quickstart-web-api-dotnet-protect-app?tabs=aspnet-core)
