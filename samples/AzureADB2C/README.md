<!-- textlint-disable @textlint-rule/require-header-id -->

<!-- cSpell:ignore Validatable -->

# Azure AD B2C による認証サンプル

## このサンプルについて

Azure AD B2C によるユーザー認証の簡単な実装サンプルを提供します。

本サンプルは、クライアントサイドレンダリングアプリケーションにおいて Azure AD B2C を利用する場合のコード例として利用できます。
また、 SPA アプリケーション（ AlesInfiny Maris のアーキテクチャに準拠したアプリケーション）に本サンプルのファイルやコードをコピーすることで、 Azure AD B2C による認証機能を組み込むことができます。

## 前提

本サンプルを動作させるためには、以下が必要です。

- Azure サブスクリプション
- サブスクリプション内、またはサブスクリプション内のリソース グループ内で共同作成者以上のロールが割り当てられている Azure アカウント

Azure サブスクリプションを持っていない場合、 [無料アカウントを作成](https://azure.microsoft.com/ja-jp/free/?WT.mc_id=A261C142F) できます。

## 動作環境

本サンプルは以下の環境で動作確認を行っています。

- .NET 8
- Node.js
- Visual Studio 2022
- Visual Studio Code

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
└ src
　 ├ Dressca.Web
　 │ ├ appsettings.json
　 │ ├ Program.cs
　 │ └ Controllers
　 │ 　 └ UserController.cs
　 └ Dressca.Web.Dto
　 　 └ Users
　 　 　 └ UserResponse.cs
```

### フロントエンドアプリケーションの構成

フロントエンドアプリケーションを構成するファイルやフォルダーのうち、認証機能に関係があるものを以下に示します。

```text
auth-frontend
├ .env.dev
├ main.ts
├ App.vue
└ src
　 ├ api-client
　 │ └ index.ts
　 ├ generated
　 ├ router
　 ├ shared
　 │ └ authentication
　 │ 　 ├ authentication-adb2c.ts
　 │ 　 ├ authentication-config.ts
　 │ 　 └ authentication-guard.ts
　 ├ stores
　 │ ├ authentication
　 │ 　 └ authentication.ts
　 │ └ users
　 │ 　 └ users.ts
　 └ views
```

## サンプルのシナリオ

1. サンプルを起動すると、ブラウザーに SPA のトップ画面が表示されます。
1. トップ画面の「 `ログイン` 」をクリックすると、 Azure AD B2C のサインイン画面がポップアップで表示されます。
1. サインインまたはサインアップが成功すると、ポップアップが閉じ、ユーザー固有の ID （JWT における sub の値）が表示されます。

※本サンプルではサインインとサインアップのシナリオのみ提供しており、ログアウトは存在しません。

## 前提となる OSS ライブラリ

本サンプルでは、バックエンド、フロントエンドアプリケーションそれぞれで OSS を使用しています。

- バックエンドアプリケーション
    - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
- フロントエンドアプリケーション
    - [MSAL.js](https://www.npmjs.com/package/@azure/msal-browser)

その他の使用 OSS は、 AlesInfiny Maris のサンプルアプリケーションに準じます。

## サンプルの動作方法

本サンプルを動作させるには、事前作業として Azure AD B2C のテナントを作成し、アプリケーションを登録する作業が必要です。

### Azure AD B2C テナントの作成

1. [Microsoft のチュートリアル「 Azure AD B2C テナントを作成する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#create-an-azure-ad-b2c-tenant) に従って、 [Azure ポータル](https://portal.azure.com/) にサインインし、 Azure AD B2C テナントを作成します。
    - 「`初期ドメイン名`」をメモします。
1. [Microsoft のチュートリアル「 B2C テナント ディレクトリを選択する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#select-your-b2c-tenant-directory) に従って、 B2C テナントディレクトリに切り替えます。
1. [Microsoft のチュートリアル「 Azure AD B2C をお気に入りとして追加する (省略可能)」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#add-azure-ad-b2c-as-a-favorite-optional) に従って、 Azure ポータル上で「 Azure サービス」から「 Azure AD B2C 」を選択しお気に入りに登録します。

### Azure AD B2C テナントを利用するアプリの登録（バックエンドアプリケーション）

<!-- textlint-disable ja-no-redundant-expression sentence-length -->
1. [Microsoft のチュートリアル「 Azure Active Directory B2C テナントに Web API アプリケーションを追加する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga) に従って、バックエンドアプリケーション用のアプリを Azure AD B2C に登録します。
    - 登録したアプリの名前を、ここでは「 `SampleWebAPI` 」とします。
    - 登録したアプリの `クライアント ID` （アプリケーション ID ）をメモします。
<!-- textlint-enable ja-no-redundant-expression sentence-length -->
1. [Microsoft のチュートリアル「スコープを構成する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga#configure-scopes)に従って、アプリにスコープを追加します。
    - チュートリアルの手順では読み取りと書き込み 2 つのスコープを作成していますが、作成するスコープは 1 つで良いです。
    - 追加したスコープの名前を、ここでは「 `api.read` 」とします。
1. Azure ポータルのお気に入りから「 Azure AD B2C 」を選択します。
1. 「アプリの登録」ブレードを選択し、「すべてのアプリケーション」から「 SampleWebAPI 」を選択します。
1. 「概要」ブレードに表示された「 `アプリケーション ID の URI` 」をメモします。

### Azure AD B2C テナントを利用するアプリの登録（フロントエンドアプリケーション）

1. [Microsoft のチュートリアル「 SPA アプリケーションの登録」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-register-spa#register-the-spa-application) に従って、フロントエンドアプリケーション用のアプリを Azure AD B2C に登録します。
    - 登録したアプリの名前を、ここでは「 `SampleSPA` 」とします。
    - 登録したアプリの `クライアント ID` （アプリケーション ID ）をメモします。
    - 「暗黙的フロー」に関する設定は無視してください。
1. Azure ポータルのお気に入りから「 Azure AD B2C 」を選択します。
1. 「アプリの登録」ブレードを選択し、「すべてのアプリケーション」から「 SampleSPA 」を選択します。
1. 「認証」ブレードを選択し、「シングルページアプリケーション」の「リダイレクト URI」に `http://localhost` を追加します。
1. [Microsoft のチュートリアル「[アクセス許可の付与]」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga#grant-permissions) に従って、 SampleSPA に、前の手順で追加した SampleWebAPI のスコープ「 api.read 」へのアクセス許可を付与します。

### ユーザーフローの作成

1. [Microsoft のチュートリアル「Azure Active Directory B2C でサインアップおよびサインイン フローを設定する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-sign-up-and-sign-in-policy?pivots=b2c-user-flow) に従って、サインアップとサインインユーザーフローを作成します。
    - ここでは追加したサインアップとサインインユーザーフローの名前を「 `signupsignin1` 」とします（ユーザーフローの名前には自動的に『`B2C_1_`』プレフィックスが付与されます）。

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
    "SignUpSignInPolicyId": "B2C_1_signupsignin1"
  }
}
```

#### フロントエンドアプリケーションの設定

1. `auth-frontend\.env.dev` を開きます。
1. 以下のように設定情報を記入します（以下の例では Azure AD B2C の設定以外は省略しています）。

```properties
VITE_ADB2C_B2CPOLICIES_NAMES_SIGNUP_SIGNIN=B2C_1_signupsignin1
VITE_ADB2C_AUTHORITIES_SIGNUP_SIGNIN_AUTHORITY=https://[初期ドメイン名].b2clogin.com/[初期ドメイン名].onmicrosoft.com/B2C_1_signupsignin1
VITE_ADB2C_B2CPOLICIES_AUTHORITYDOMAIN=[初期ドメイン名].b2clogin.com
VITE_ADB2C_SCOPE=[SampleWebAPI のアプリケーション ID の URI]/api.read
VITE_ADB2C_APP_CLIENT_ID=[SampleSPA のクライアント ID]
VITE_ADB2C_APP_URI=http://localhost:5173
```

### 動作確認

1. ターミナルで `auth-frontend` のフォルダーへ移動し、 `npm install` を実行します。
1. Visual Studio で `auth-backend\Dressca.sln` を開きます。
1. `Dressca.Web` を右クリックし「スタートアッププロジェクトに設定」を選択します。
1. ソリューションをデバッグなしで開始します。ブラウザーが起動し、しばらく待つと SPA の初期画面が表示されます。
1. 画面右上の「 `ログイン` 」をクリックします。 Azure AD B2C のサインイン画面がポップアップで表示されます。
1. 「 Sign up now 」リンクをクリックします。
1. 使用可能なメールアドレスを入力し、「 Send verification code 」をクリックします。
1. 上の手順で入力したメールアドレス宛に Verification code が送信されるので、画面に入力して「 Verfiy code 」をクリックします。
1. 画面に新しいパスワード等の必要事項を入力し、「 Create 」をクリックします。
1. サインインが成功し、画面右上に「ユーザー ID 」が表示されれば成功です。以降は入力したメールアドレスとパスワードでサインインできるようになります。

Azure AD B2C に追加したユーザーは、以下の手順で削除できます。

1. Azure ポータルのお気に入りから「 Azure AD B2C 」を選択します。
1. 「ユーザー」ブレードを選択します。
1. 対象のユーザーをチェックし、画面上部から「削除」を選択します。

## アプリケーションへの認証機能の組み込み

本サンプルのコードを既存のアプリケーションへコピーすることで、 Azure AD B2C の認証機能を組み込むことができます。
なお、対象のアプリケーションは AlesInfiny Maris のクライアントサイドレンダリングアプリケーションです。

### バックエンドアプリケーション

1. ASP.NET Core Web API プロジェクトに対して以下の NuGet パッケージをインストールします。
    - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
1. ASP.NET Core Web API プロジェクトの Program.cs に Azure AD B2C の設定を追加します。

```cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args); // （既存のコード）

// Azure AD B2C 認証に必要な設定をインジェクション
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(
    options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);
        options.TokenValidationParameters.NameClaimType = "name";
    },
    options => { builder.Configuration.Bind("AzureAdB2C", options); });

var app = builder.Build(); // （既存のコード）

// 認証を有効化
app.UseAuthentication();
app.UseAuthorization();
```

※ `app.UseAuthentication` および `app.UserAuthorization` の呼び出し位置は、[ミドルウェアの順序](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0#middleware-order) に従ってください。
<!-- textlint-disable ja-no-redundant-expression ja-technical-writing/sentence-length -->
1. `auth-backend\src\Dressca.Web\appsettings.json` に記述した Azure AD B2C の設定を ASP.NET Core Web API プロジェクトの `appsettings.json` へコピーします。
<!-- textlint-enable ja-no-redundant-expression ja-technical-writing/sentence-length -->
1. 認証を必要とする Web API に `[Authorize]` 属性を付与します。 `[Authorize]` 属性は Web API Controller クラスにも、個別の Controller メソッドにも付与できます

```cs
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class OrdersController : ControllerBase
{
   // 省略
}
```

### フロントエンドアプリケーション

1. ターミナルで `npm install @azure/msal-browser` を実行し、フロントエンドアプリケーションに MSAL.js をインストールします。
1. `auth-frontend\.env.dev` に記述した Azure AD B2C の設定をフロントエンドアプリケーションの `.env.dev` にコピーします。
1. `env.d.ts` のインターフェースに、前の手順で `.env.dev` に追加したプロパティを追加します。

```ts
interface ImportMetaEnv {
  // 認証に関係のないプロパティは省略
  readonly VITE_ADB2C_B2CPOLICIES_NAMES_SIGNUP_SIGNIN: string;
  readonly VITE_ADB2C_AUTHORITIES_SIGNUP_SIGNIN_AUTHORITY: string;
  readonly VITE_ADB2C_B2CPOLICIES_AUTHORITYDOMAIN: string;
  readonly VITE_ADB2C_SCOPE: string;
  readonly VITE_ADB2C_APP_CLIENT_ID: string;
  readonly VITE_ADB2C_APP_URI: string;
}
```

1. `src\shared\authentication` フォルダーを作成し、サンプルの以下のコードをコピーします。
    - authentication-adb2c.ts
    - authentication-config.ts
1. `src\store\authentication` フォルダーを作成し、サンプルの以下のコードをコピーします。
    - authentication.ts

1. `src\main.ts` に MSAL.js を使用するコードを追加します。

```ts
import { msalInstance } from "@/shared/authentication/authentication-config";

const app = createApp(App); // 既存のコード

app.use(msalInstance);

app.mount("#app"); // 既存のコード
```

1. 認証が成功した場合、以降の Web API リクエストヘッダーに Bearer トークンを付与する必要があります。
   AlesInfiny Maris のサンプルアプリケーション Dressca の場合、 `src\api-client\index.ts` を編集します。

```ts
import { useAuthenticationStore } from "@/stores/authentication/authentication";

// その他のコードは省略

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({
  headers: {
    "Content-Type": "application/json",
  },
});

// interceptor を使用してすべてのリクエストに共通処理を追加
axiosInstance.interceptors.request.use(
  async (config: InternalAxiosRequestConfig) => {
    const store = useAuthenticationStore();
    if (store.isAuthenticated) {
      await store.getToken();
      const token = store.accessToken;
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  }
);
```

1. `ログイン` 画面へのリンクを含む Vue ファイルの `<script>` セクションにコードを追加します。

```ts
<script setup lang="ts">
import { useAuthenticationStore } from '@/stores/authentication/authentication';

const authenticationStore = useAuthenticationStore();
const isAuthenticated = () => {
  return authenticationStore.isAuthenticated;
};
const signIn = async () => {
  await authenticationStore.signIn();

  if (authenticationStore.isAuthenticated) {
    // サインインが成功した場合の処理をここに記述します。
  }
};
</script>
```

1. `ログイン` 画面へのリンクを以下のように記述します（クリック時に `signIn` メソッドが動作すれば `button` である必要はありません）。

```html
<button v-if="!isAuthenticated()" @click="signIn()">ログイン</button>
```
