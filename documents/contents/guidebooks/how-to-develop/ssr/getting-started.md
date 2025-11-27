---
title: 事前準備 （SSR 編）
description: SSR アプリケーション開発に必要な事前準備を解説します。
---

# 事前準備 {#top}

各種クラウドサービスや、外部の API など、ローカルマシン内に構築できないものを除き、原則としてローカル PC 内でアプリケーションの実行が完結できるように開発環境を構築しましょう。

## ローカル開発環境の構築 {#build-local-environment}

ローカル開発環境の構築について「[ローカル開発環境の構築](./local-development.md)」を参照し、最低限必要なソフトウェアをインストールしてください。

## .NET アプリケーションの開発に必要なソフトウェアの追加インストール {#additional-installation-for-dotnet-app-development}

### SQL Server を使用する場合 {#sql-server}

ローカル開発環境で使用する SQL Server は、原則として SQL Server Express LocalDB ( 以降 LocalDB ) を使用します。
LocalDB は SQL Server の様々なエディションのインストーラーに付属しているほか、 Visual Studio のインストーラーにも付属しています。
「[ローカル開発環境の構築](./local-development.md)」の手順を実行すれば、インストールされます。

!!! note "ローカル開発環境で使用する SQL Server のエディション"
    SQL Server LocalDB を利用すると、コンピューターリソースを節約しながら、データベースを用いた開発ができます。
    しかし、いくつかの [機能制限 :material-open-in-new:](https://learn.microsoft.com/ja-jp/sql/database-engine/configure-windows/sql-server-express-localdb#restrictions){ target=_blank } があります。
    これらの機能を用いて開発したい場合は、 SQL Server Developer Edition の利用を検討してください。
    各エディションの比較については [こちら :material-open-in-new:](https://learn.microsoft.com/ja-jp/sql/sql-server/editions-and-components-of-sql-server-2019){ target=_blank } を参照してください。

LocalDB に直接アクセスする場合は、 Visual Studio の [SQL Server オブジェクト エクスプローラー] ウィンドウを利用してください。
このウィンドウで機能不足の場合は、「 [SQL Server Management Studio :material-open-in-new:](https://learn.microsoft.com/ja-jp/sql/ssms/download-sql-server-management-studio-ssms){ target=_blank } 」を別途インストールしてください。