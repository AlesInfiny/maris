# 事前準備

## ローカル開発環境の構築

ローカル開発環境の構築について [ローカル開発環境の構築](../local-environment/index.md) を参照し、最低限必要なソフトウェアをインストールしてください。

## Vue.js での開発に必要なソフトウェアの追加インストール

### JDK のインストール

サーバー側で公開される Web API は、Open API 仕様書を自動生成しています（詳細は [ASP.NET Core Web API プロジェクトの構成](../dotnet/configure-asp-net-core-web-api-project.md) を参照）。Vue.js アプリケーションでは、Open API Generator を使用して、この Open API 仕様書からクライアントコードを生成しています。

Open API Generator を使用するためには、Java 8 以降のランタイムと、システム環境変数 JAVA_HOME の設定が必要です。Oracle JDK や Eclipse Adoptium など、適当な JDK をインストールし、JAVA_HOME を設定してください。
