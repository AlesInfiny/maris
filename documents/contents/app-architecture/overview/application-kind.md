---
title: 概要編
description: AlesInfiny Maris OSS Edition を利用することで構築できる アプリケーションの概要を説明します。
---

# 構築できるアプリケーション形態 {#top}

AlesInfiny Maris OSS Edition を利用することで構築できるアプリケーションの概要を、アプリケーション形態ごとに説明します。

## Web アプリケーション（クライアントサイドレンダリング） {#web-application-client-side-rendering}

HTML をクライアント側 JavaScript でレンダリングする方式の Web アプリケーションです。
画面初期表示時にはコンパイル済みの静的ファイルをダウンロードして、 JavaScript で動的に画面をレンダリングします。
業務データの取得、更新などの処理を行う際は、 Web API 経由でサーバー側の業務ロジックを呼び出します。

![クライアントサイドレンダリング](../../images/app-architecture/overview/client-side-rendering-light.png#only-light){ loading=lazy }
![クライアントサイドレンダリング](../../images/app-architecture/overview/client-side-rendering-dark.png#only-dark){ loading=lazy }

## Web アプリケーション（サーバーサイドレンダリング） {#web-application-server-side-rendering}

サーバーサイドで構築した HTML をクライアント側のブラウザーでレンダリングする方式の Web アプリケーションです。

![サーバーサイドレンダリング](../../images/app-architecture/overview/server-side-rendering-light.png#only-light){ loading=lazy }
![サーバーサイドレンダリング](../../images/app-architecture/overview/server-side-rendering-dark.png#only-dark){ loading=lazy }

## コンソールアプリケーション {#console-application}

クライアント端末で動作するネイティブアプリケーションです。コマンドラインからプログラム名と引数を指定して起動し、処理結果はコマンドラインに文字列で出力されます。バッチ処理の作成に適した構成です。

![コンソールアプリケーション](../../images/app-architecture/overview/console-application-light.png#only-light){ loading=lazy }
![コンソールアプリケーション](../../images/app-architecture/overview/console-application-dark.png#only-dark){ loading=lazy }
