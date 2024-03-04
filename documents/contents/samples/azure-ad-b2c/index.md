---
title: Azure AD B2C での認証の利用
description: Azure AD B2C による認証を利用するためのサンプルと、その使い方を解説します。
---

# Azure AD B2C による認証を利用する {#top}

## 概要 {#about-this-sample}

Azure AD B2C によるユーザー認証の簡単な実装サンプルを提供します。

本サンプルは、クライアントサイドレンダリングアプリケーションにおいて Azure AD B2C を利用する場合のコード例として利用することができます。
また、 [AlesInfiny Maris のサンプルアプリケーション](../downloads/dressca.zip) に本サンプルのファイルやコードをコピーすることで、 Azure AD B2C による認証機能を組み込むことができます。

## 本サンプルを利用するための前提 {#prerequisites}

本サンプルを動作させるためには、以下が必要です。

- Azure サブスクリプション
- サブスクリプション内、またはサブスクリプション内のリソース グループ内で共同作成者以上のロールが割り当てられている Azure アカウント

## 本サンプルを利用する前の準備 {#preparations}

本サンプルを動作させるまでの流れは以下のとおりです。

1. Azure AD B2C テナントを作成する
1. Azure AD B2C テナントを利用するアプリを登録する
1. ユーザーフローを作成する
1. 本サンプルの設定ファイルに各手順で作成した設定内容を記入する
1. 本サンプルを動作させる

具体的な手順は、[サンプルアプリケーション](#download) に付属する README.md を参照してください。

## 本サンプルで利用する OSS {#oss-libraries}

本サンプルでは以下の OSS ライブラリを使用しています。
他の OSS ライブラリについては、 [サンプルアプリケーションをダウンロード](#download) して確認してください。

- フロントエンド
  - [MSAL.js](https://www.npmjs.com/package/@azure/msal-browser)
- バックエンド
  - [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web)
  - [Microsoft.IdentityModel.JsonWebTokens](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens)

## ダウンロード {#download}

サンプルアプリケーションと詳細な解説は以下からダウンロードできます。

- [サンプルアプリケーションのダウンロード](../downloads/azure-ad-b2c.zip)
