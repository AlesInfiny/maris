---
title: .NET 編（CSR 編）
description: CSR アプリケーションの サーバーサイドで動作する .NET アプリケーションの 開発手順を解説します。
---

# ASP.NET Core with Vite プロジェクトの構成 {#top}

ASP.NET Core with Vite プロジェクトには、フロントエンドアプリケーションとの連携に関する設定をします。

## ASP.NET Core Web API の設定 {#setup-asp-net-core-web-api}

ASP.NET Core with Vite プロジェクトは、 ASP.NET Core Web API プロジェクトと同じ Web API 向け構成を組みこみます。
[ASP.NET Core Web API プロジェクトの構成](./configure-asp-net-core-web-api-project.md) を参照し、設定してください。

## フロントエンドアプリケーションの配置変更 {#change-frontend-app-placement}

### フロントエンドアプリケーションのフォルダーをプロジェクト内から移動または削除する {#move-or-delete-client-app-folder}

![フォルダー構造](../../../../images/guidebooks/how-to-develop/csr/dotnet/root-folders-light.png#only-light){ loading=lazy align=right }
![フォルダー構造](../../../../images/guidebooks/how-to-develop/csr/dotnet/root-folders-dark.png#only-dark){ loading=lazy align=right }

Vue.js などの JavaScript フレームワークを用いたフロントエンドアプリケーションは、バックエンドアプリケーションと階層構造の異なる場所に配置することを推奨します。
ASP.NET Core with Vite プロジェクトを作成すると、プロジェクトルートにフロントエンドアプリケーションを配置するための [ClientApp] フォルダーが生成されます。
このフォルダーを [backend] フォルダーと同じ階層に移動し、 [frontend] にフォルダー名を変更します[^1]。

以降、図のような階層となっていることを前提に、設定値を変更します。
フォルダー名や階層構造が異なる場合は、適宜読み替えてください。

### フロントエンドアプリケーションの配置設定 {#configure-spa-root-property}

ASP.NET Core with Vite プロジェクトを実行したとき、適切なフロントエンドアプリケーションが実行できるように、プロジェクトファイルの設定値を変更します。

ASP.NET Core with Vite プロジェクトのプロジェクトファイルを開き、 `#!xml <SpaRoot>` プロパティの値を変更します。
上記の例のような配置の場合、 [AaaSubSystem.Web.csproj] から、フロントエンドアプリケーションを配置したルートフォルダー [frontend] に対する相対パスを設定します。
続けて、`#!xml <SpaRootWorkspace>`  プロパティを追加し、プロジェクトフォルダー [workspace-name] に対する相対パスを設定します。

```xml title="ASP.NET Core with Vite プロジェクト ( AaaSubSystem.Web.csproj )"
<SpaRoot>..\..\..\frontend\</SpaRoot>
<SpaWorkspace>$(SpaRoot)workspace-name\</SpaWorkspace>
```

### フロントエンドアプリケーションの参照削除 {#remove-frontend-folder-reference}

上記のフォルダー移動と設定変更を行うと、 ASP.NET Core with Vite プロジェクトにフロントエンドアプリケーションへの参照設定を残す必要がなくなります。
プロジェクトファイル内の以下の設定を削除します。

```xml title="ASP.NET Core with Vite プロジェクトから削除する設定 ( AaaSubSystem.Web.csproj )"
<ItemGroup>
    <!-- Don't publish the SPA source files, but do show them in the project files list -->
    <Content Remove="$(SpaRoot)**" />
    <None Remove="$(SpaRoot)**" />
    <None Include="$(SpaRoot)**" Exclude="$(SpaRoot)node_modules\**" />
</ItemGroup>
```

## フロントエンドアプリケーションの起動設定 {#setup-frontend-application-launch-command}

ASP.NET Core with Vite プロジェクトのプロジェクトファイルには、フロントエンドアプリケーションを実行するためのコマンドを設定できます。
この設定は、 Visual Studio で ASP.NET Core with Vite プロジェクトを実行したとき、 Web サーバーの起動完了後に自動実行されます。
これにより、 SPA の実行を Visual Studio の操作のみで行うことができます。

既定では、 `npm start` コマンドが設定されています。
フロントエンドアプリケーションの実装にあわせて、起動コマンドを設定してください。
設定箇所は ASP.NET Core with Vite プロジェクトのプロジェクトファイル内、 `#!xml <SpaProxyLaunchCommand>` プロパティです。

```xml title="フロントエンドアプリケーションの起動コマンドの設定例 ( AaaSubSystem.Web.csproj )"
<SpaProxyLaunchCommand>npm run dev:workspace-name</SpaProxyLaunchCommand>
```

[^1]: フォルダー名は適宜変更してください。
      フロントエンドアプリケーションを別途ゼロから開発する場合は、 [ClientApp] フォルダーを削除してしまってもかまいません。
