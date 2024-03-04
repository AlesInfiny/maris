<!-- textlint-disable @textlint-rule/require-header-id -->

<!-- cSpell:ignore Validatable -->

# Azure AD B2C による認証サンプル

## このサンプルについて

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

本サンプルは、クライアントブラウザー上で動作するフロントエンドアプリケーション（ SPA ）と、 SPA が呼び出すバックエンドアプリケーション（ Web API ）によって構成されています。
フォルダー構成は以下のとおりです。

```text
ルートフォルダー
├ auth-backend ....... バックエンドアプリケーションが配置されたフォルダー
├ auth-frontend ...... フロントエンドアプリケーションが配置されたフォルダー
└ Readme.md .......... このファイル
```

バックエンドアプリケーションは ASP.NET Core Web API 、フロントエンドアプリケーションは Vue.js (TypeScript) で作成されています。

## 前提となる OSS ライブラリ

本サンプルでは、バックエンド、フロントエンドアプリケーションそれぞれで OSS を使用しています。

- バックエンド
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
1. [Microsoft のチュートリアル「B2C テナント ディレクトリを選択する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-create-tenant#select-your-b2c-tenant-directory) に従って、 B2C テナントディレクトリに切り替えます。

### Azure AD B2C テナントを利用するアプリの登録（バックエンドアプリケーション）

1. [Microsoft のチュートリアル「Azure Active Directory B2C テナントに Web API アプリケーションを追加する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-web-api-application?tabs=app-reg-ga) に従って、バックエンドアプリケーション用のアプリを Azure AD B2C に登録します。

### Azure AD B2C テナントを利用するアプリの登録（フロントエンドアプリケーション）

1. [Microsoft のチュートリアル「SPA アプリケーションの登録」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/tutorial-register-spa#register-the-spa-application) に従って、フロントエンドアプリケーション用のアプリを Azure AD B2C に登録します。

### ユーザーフローの作成

1. [Microsoft のチュートリアル「Azure Active Directory B2C でサインアップおよびサインイン フローを設定する」](https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-sign-up-and-sign-in-policy?pivots=b2c-user-flow) に従って、サインアップとサインインユーザーフローを作成します。

### 設定情報の記入

### 動作確認
