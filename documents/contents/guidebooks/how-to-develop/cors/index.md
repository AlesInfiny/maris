---
title: CORS 環境の構築
description: CORS （オリジン間リソース共有）環境でのアプリケーションの構築方法を解説します。
---

# CORS 環境の構築 {#top}

## CORS （オリジン間リソース共有）とは {#about-cors}

オリジンとは、 URL のスキーム（プロトコル）、ホスト（ドメイン）、ポート番号の部分を指します（ [Origin (オリジン) - MDN Web Docs 用語集: ウェブ関連用語の定義 | MDN](https://developer.mozilla.org/ja/docs/Glossary/Origin) ）。

- `https://www.example.com` と `https://www2.example.com` はホスト（ドメイン）部分が異なるので異なるオリジン
- `https://localhost:4431` と `https://localhost:4432` はポート番号が異なるので異なるオリジン

ブラウザーは原則として「同一オリジンポリシー」で動作します。

> 同一オリジンポリシーは重要なセキュリティの仕組みであり、あるオリジンによって読み込まれた文書やスクリプトが、他のオリジンにあるリソースにアクセスできる方法を制限するものです。
> [同一オリジンポリシー - ウェブセキュリティ | MDN](https://developer.mozilla.org/ja/docs/Web/Security/Same-origin_policy)

原則としてブラウザーは同一オリジンポリシー、 CORS （オリジン間リソース共有）とは、そのルールをちょっと緩めるもの

SPA においては、フロントエンドアプリケーションとバックエンドアプリケーションの配置されるサーバーのオリジンが異なる環境を意味する

## バックエンドアプリケーション（ .NET ） {#backend}

### appSettings.json の設定 {#appSettings-json}

### 構成オプション用クラスの追加 {#option-settings-class}

### Program.cs の設定 {#Program-cs}

### 注意点 {#notice}

SameSite=None, Secure, HttpOnly

## フロントエンドアプリケーション（ Vue.js ） {#frontend}

Web API 呼び出し時のヘッダーに `Access-Control-Allow-Origin` ヘッダーを追加する
