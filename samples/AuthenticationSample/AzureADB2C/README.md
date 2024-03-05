<!-- textlint-disable @textlint-rule/require-header-id -->

<!-- cSpell:ignore Validatable -->

# Azure AD B2C による認証サンプル

## このサンプルについて

Azure AD B2C によるユーザー認証の簡単な実装サンプルを提供します。

本サンプルは、クライアントサイドレンダリングアプリケーションにおいて Azure AD B2C を利用する場合のコード例として利用することができます。
また、 AlesInfiny Maris のサンプルアプリケーションに本サンプルのファイルやコードをコピーすることで、 Azure AD B2C による認証機能を組み込むことができます。

## 前提

本サンプルを動作させるためには、以下が必要です。

- Azure サブスクリプション
- サブスクリプション内、またはサブスクリプション内のリソース グループ内で共同作成者以上のロールが割り当てられている Azure アカウント

Azure サブスクリプションを持っていない場合、 [無料アカウントを作成](https://azure.microsoft.com/ja-jp/free/?WT.mc_id=A261C142F) することができます。

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

## 前提となる OSS ライブラリ

本サンプルでは、バックエンド、フロントエンドアプリケーションそれぞれで OSS を使用しています。

- バックエンドアプリケーション
  - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
  - [Microsoft.IdentityModel.JsonWebTokens](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens)
- フロントエンドアプリケーション
  - [MSAL.js](https://www.npmjs.com/package/@azure/msal-browser)
  - [Vue.js](https://ja.vuejs.org/)
  - [Vite](https://ja.vitejs.dev/)
  - [Axios](https://github.com/axios/axios)

## 使用方法

### Azure AD B2C テナントの作成

1. [Microsoft のチュートリアル「Azure AD B2C テナントを作成する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#create-an-azure-ad-b2c-tenant) に従って、 [Azure Portal](https://portal.azure.com/) にサインインし、Azure AD B2C テナントを作成します。
    - 「`初期ドメイン名`」をメモします。
1. [Microsoft のチュートリアル「B2C テナント ディレクトリを選択する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#select-your-b2c-tenant-directory) に従って、 B2C テナントディレクトリに切り替えます。
1. [Microsoft のチュートリアル「Azure AD B2C をお気に入りとして追加する (省略可能)」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#add-azure-ad-b2c-as-a-favorite-optional) に示す手順に従って、Azure ポータル上で「 Azure サービス」から「 Azure AD B2C 」を選択しお気に入りに登録します。

### Azure AD B2C テナントを利用するアプリの登録（バックエンドアプリケーション）

1. [Microsoft のチュートリアル「Azure Active Directory B2C テナントに Web API アプリケーションを追加する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga) に従って、バックエンドアプリケーション用のアプリを Azure AD B2C に登録します。
    - 登録したアプリの名前を、ここでは「 `AlesInfinyMarisWebAPI` 」とします。
    - 登録したアプリの `クライアント ID` （アプリケーション ID ）をメモします。
1. [Microsoft のチュートリアル「スコープを構成する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga#configure-scopes)に従って、アプリにスコープを追加します。
    - チュートリアルの手順では読み取りと書き込み 2 つのスコープを作成していますが、作成するスコープは 1 つで良いです。
    - 追加したスコープの名前を、ここでは「 `api.read` 」とします。
1. Azure ポータルのお気に入りから「 Azure AD B2C 」を選択します。
1. 「アプリの登録」ブレードを選択し、「すべてのアプリケーション」から「 AlesInfinyMarisWebAPI 」を選択します。
1. 「概要」ブレードに表示された「 `アプリケーション ID の URI` 」をメモします。

### Azure AD B2C テナントを利用するアプリの登録（フロントエンドアプリケーション）

1. [Microsoft のチュートリアル「SPA アプリケーションの登録」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-register-spa#register-the-spa-application) に従って、フロントエンドアプリケーション用のアプリを Azure AD B2C に登録します。
    - 登録したアプリの名前を、ここでは「 `AlesInfinyMarisSPA` 」とします。
    - 登録したアプリの `クライアント ID` （アプリケーション ID ）をメモします。
    - 「暗黙的フロー」に関する設定は無視してください。
1. Azure ポータルのお気に入りから「 Azure AD B2C 」を選択します。
1. 「アプリの登録」ブレードを選択し、「すべてのアプリケーション」から「 AlesInfinyMarisSPA 」を選択します。
1. 「認証」ブレードを選択し、「シングルページアプリケーション」の「リダイレクト URI」に `http://localhost` を追加します。
1. [Microsoft のチュートリアル「[アクセス許可の付与]」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga#grant-permissions) に従って、 AlesInfinyMarisSPA に、前の手順で追加した AlesInfinyMarisWebAPI のスコープ「 api.read 」へのアクセス許可を付与します。

### ユーザーフローの作成

1. [Microsoft のチュートリアル「Azure Active Directory B2C でサインアップおよびサインイン フローを設定する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-sign-up-and-sign-in-policy?pivots=b2c-user-flow) に従って、サインアップとサインインユーザーフローを作成します。
    - ここでは追加したサインアップとサインインユーザーフローの名前を「 `signupsignin1` 」とします（ユーザーフローの名前には自動的に『B2C_1_』プレフィックスが付与されます）。

### 設定情報の記入

#### バックエンドアプリケーションの設定

1. `auth-backend\src\Dressca.Web\appsettings.json` を開きます。
1. 以下のように設定情報を記入します（以下の例では Azure AD B2C の設定以外は省略しています）。

```json
{
  "AzureAdB2C": {
    "Instance": "https://[初期ドメイン名].b2clogin.com",
    "ClientId": "[AlesInfinyMarisWebAPI のクライアント ID]",
    "Domain": "[初期ドメイン名].onmicrosoft.com",
    "SignUpSignInPolicyId": "B2C_1_signupsignin1",
  },
}
```

#### フロントエンドアプリケーションの設定

1. `auth-frontend\.env.dev` を開きます。
1. 以下のように設定情報を記入します（以下の例では Azure AD B2C の設定以外は省略しています）。

```properties
VITE_ADB2C_B2CPOLICIES_NAMES_SIGNUP_SIGNIN=B2C_1_signupsignin1
VITE_ADB2C_AUTHORITIES_SIGNUP_SIGNIN_AUTHORITY=https://[初期ドメイン名].b2clogin.com/[ドメイン名]/B2C_1_signupsignin1
VITE_ADB2C_B2CPOLICIES_AUTHORITYDOMAIN=[初期ドメイン名].b2clogin.com
VITE_ADB2C_SCOPE=[AlesInfinyMarisWebAPI のアプリケーション ID の URI]/api.read
VITE_ADB2C_APP_CLIENT_ID=[AlesInfinyMarisSPA のクライアント ID]
VITE_ADB2C_APP_URI=http://localhost:5173
```

### 動作確認

1. Visual Studio で `auth-backend\Dressca.sln` を開きます。
1. `Dressca.Web` を右クリックし「スタートアッププロジェクトに設定」を選択します。
1. ソリューションをデバッグなしで開始します。ブラウザーが起動し、しばらく待つと SPA の初期画面が表示されます。
1. 画面右上の「ログイン」をクリックします。 Azure AD B2C のログイン画面がポップアップで表示されます。
1. 「 Sign up now 」リンクをクリックします。
1. 使用可能なメールアドレスを入力し、「 Send verification code 」をクリックします。
1. 上の手順で入力したメールアドレス宛に Verification code が送信されるので、画面に入力して「 Verfiy code 」をクリックします。
1. 画面に新しいパスワード等の必要事項を入力し、「 Create 」をクリックします。
1. ログインが成功し、画面右上に「ユーザー ID 」が表示されれば成功です。以降は登録したメールアドレスとパスワードでログインできるようになります。

## AlesInfiny Maris サンプル（ Dressca ）への認証機能の組み込み

### バックエンドアプリケーション

1. `Dressca.Web` に対して以下の NuGet パッケージをインストールします。
    - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
    - [Microsoft.IdentityModel.JsonWebTokens](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens)
2. `Dressca\dressca-backend\src\Dressca.Web\Program.cs` に Azure AD B2C の設定を追加します。

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

// 認証機能の利用を有効化
app.UseAuthentication();
```

3. `auth-backend\src\Dressca.Web\Controllers\UsersController.cs` を `Dressca\dressca-backend\src\Dressca.Web\Controllers\` へコピーします。
4. `auth-backend\src\Dressca.Web.Dto\Users\UserResponse.cs` を `Dressca\dressca-backend\src\Dressca.Web.Dto\Users\` へコピーします。
5. `auth-backend\src\Dressca.Web\appsettings.json` に記述した Azure AD B2C の設定を `Dressca\dressca-backend\src\Dressca.Web\appsettings.json` へコピーします。
6. ソリューションをビルドします。

### フロントエンドアプリケーション

1. `npm run generate-client` を実行し、 Axios のクライアントコードを再生成します。
1. `npm install @azure/msal-browser` を実行し、 MSAL.js をインストールします。
