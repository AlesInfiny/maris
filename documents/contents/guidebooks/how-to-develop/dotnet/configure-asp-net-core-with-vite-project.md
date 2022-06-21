# ASP.NET Core with Vite プロジェクトの構成

ASP.NET Core with Vite プロジェクトには、フロントエンドのアプリケーションとの連携に関する設定を行います。

## ASP.NET Core Web API の設定 {: #setup-asp-net-core-web-api }

ASP.NET Core with Vite プロジェクトは、 ASP.NET Core Web API プロジェクトと同等の Web API 向け構成を組みこみます。
[ASP.NET Core Web API プロジェクトの構成](./configure-asp-net-core-web-api-project.md) を参照し、設定を実施してください。

## フロントエンドアプリケーションの配置変更 {: #change-frontend-app-placement }

### フロントエンドアプリケーションのディレクトリをプロジェクト内から移動または削除する {: #move-or-delete-client-app-directory }

![ディレクトリ構造](../../../images/guidebooks/how-to-develop/dotnet/root-directories-light.png#only-light){ loading=lazy align=right }
![ディレクトリ構造](../../../images/guidebooks/how-to-develop/dotnet/root-directories-dark.png#only-dark){ loading=lazy align=right }

Vue.js などの JavaScript フレームワークを用いたフロントエンドアプリケーションは、バックエンドアプリケーションと階層構造の異なる場所に配置することを推奨します。
ASP.NET Core with Vite プロジェクトを作成すると、プロジェクトルートにフロントエンドアプリケーションを配置するための [ClientApp] ディレクトリが生成されます。
このディレクトリを [backend] ディレクトリと同じ階層に移動し、 [frontend] にディレクトリ名を変更します[^1]。

以降、図のような階層となっていることを前提に、設定値の変更を行います。
ディレクトリ名や階層構造が異なる場合は、適宜読み替えてください。

### フロントエンドアプリケーションの配置設定 {: #configure-spa-root-property }

ASP.NET Core with Vite プロジェクトを実行したとき、適切なフロントエンドアプリケーションが実行できるように、プロジェクトファイルの設定値を変更します。

ASP.NET Core with Vite プロジェクトのプロジェクトファイルを開き、 `#!xml <SpaRoot>` プロパティの値を変更します。
上述の例のような配置の場合、ASP.NET Core with Vite プロジェクトである [AaaSubSystem.Web] から、フロントエンドアプリケーションを配置したルートディレクトリ [frontend] に対する相対パスを設定します。

```xml title="ASP.NET Core with Vite プロジェクト ( AaaSubSystem.Web.csproj )"
<SpaRoot>..\..\..\frontend\</SpaRoot>
```

### フロントエンドアプリケーションの参照削除 {: #remove-frontend-directory-reference }

上記のディレクトリ移動と設定変更を行うと、 ASP.NET Core with Vite プロジェクト内にフロントエンドアプリケーションの参照を残す必要がなくなります。
プロジェクトファイル内の以下の設定を削除します。

```xml title="ASP.NET Core with Vite プロジェクトから削除する設定 ( AaaSubSystem.Web.csproj )"
<ItemGroup>
    <!-- Don't publish the SPA source files, but do show them in the project files list -->
    <Content Remove="$(SpaRoot)**" />
    <None Remove="$(SpaRoot)**" />
    <None Include="$(SpaRoot)**" Exclude="$(SpaRoot)node_modules\**" />
</ItemGroup>
```

## フロントエンドアプリケーションの起動設定 {: #setup-frontend-application-launch-command }

ASP.NET Core with Vite プロジェクトのプロジェクトファイルには、フロントエンドアプリケーションを実行するためのコマンドを設定できます。
この設定は、 Visual Studio でバックエンドアプリケーションを実行したとき、 Web サーバーが起動した後に読み込まれて自動実行されます。
これにより、 SPA の実行を Visual Studio のみで行うことができます。

既定では、 `npm start` コマンドが設定されています。
フロントエンドアプリケーションの実装にあわせて、起動コマンドを設定してください。
設定箇所は ASP.NET Core with Vite プロジェクトのプロジェクトファイル内、 `#!xml <SpaProxyLaunchCommand>` プロパティです。

```xml title="フロントエンドアプリケーションの起動コマンドを設定例 ( AaaSubSystem.Web.csproj )"
<SpaProxyLaunchCommand>npm run dev</SpaProxyLaunchCommand>
```

[^1]: ディレクトリ名は適宜変更してください。
      フロントエンドアプリケーションを別途ゼロから開発する場合は、 [ClientApp] ディレクトリを削除してしまってもかまいません。
