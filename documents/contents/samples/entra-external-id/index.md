---
title: Entra External ID を利用して ユーザーを認証する
description: Entra External ID による認証を利用するためのサンプルと、 その使い方を解説します。
---

# Azure Active Directory B2C による認証を利用する {#top}

## 概要 {#about-this-sample}

Microsoft Entra External ID （以降、 Entra External ID ） によるユーザー認証の簡単な実装サンプルを提供します。

本サンプルは、クライアントサイドレンダリングアプリケーションにおいて Entra External ID を利用する場合のコード例として利用できます。

<!-- textlint-disable ja-technical-writing/sentence-length -->

また、 SPA アプリケーション（ AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）のアーキテクチャに準拠したアプリケーション）に本サンプルのファイルやコードをコピーすることで、 Entra External ID による認証機能を組み込むことができます。

<!-- textlint-enable ja-technical-writing/sentence-length -->

## 本サンプルを利用するための前提 {#prerequisites}

本サンプルを動作させるためには、以下が必要です。

- Azure サブスクリプション
- サブスクリプション内、またはサブスクリプション内のリソース グループ内で共同作成者以上のロールが割り当てられている Azure アカウント

## 本サンプルを利用する前の準備 {#preparations}

本サンプルを動作させるまでの流れは以下のとおりです。

1. Entra External ID テナントを作成する
1. Entra External ID テナントを利用するアプリを登録する
1. ユーザーフローを作成する
1. ユーザーフローにアプリケーションを追加する
1. 本サンプルの設定ファイルに各手順で作成した設定内容を記入する
1. 本サンプルを動作させる

具体的な手順は、[サンプルアプリケーション](#download) に付属する README.md を参照してください。

## 本サンプルで利用する OSS {#oss-libraries}

本サンプルでは以下の OSS ライブラリを使用しています。
他の OSS ライブラリについては、 [サンプルアプリケーションをダウンロード](#download) して確認してください。

- フロントエンド
    - [MSAL.js :material-open-in-new:](https://www.npmjs.com/package/@azure/msal-browser){ target=_blank }
- バックエンド
    - [Microsoft.Identity.Web :material-open-in-new:](https://www.nuget.org/packages/Microsoft.Identity.Web){ target=_blank }

## 本サンプルを利用する際の検討事項 {#consideration}

- [MSAL.js で提供される秘密情報のキャッシュ保存先](./entra-external-id-consideration.md)

## ダウンロード {#download}

サンプルアプリケーションと詳細な解説は以下からダウンロードできます。

- [サンプルアプリケーションのダウンロード](../downloads/entra-external-id-auth.zip)
