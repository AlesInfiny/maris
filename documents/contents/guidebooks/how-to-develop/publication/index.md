---
title: アプリケーションの 発行
description: .NET + Vue.js で構成されたアプリケーションの 発行手順について解説します。
---

<!-- cspell:ignore wwwroot -->

# アプリケーションの発行 {#top}

.NET + Vue.js のアプリケーションを発行する手順について解説します。なお、本手順は Web サーバーと AP サーバーを統合する構成を前提としています。

![Web サーバーと AP サーバーを統合する構成](../../../images/guidebooks/how-to-develop/publication/integrated-server-constructure-light.png#only-light){ loading=lazy }
![Web サーバーと AP サーバーを統合する構成](../../../images/guidebooks/how-to-develop/publication/integrated-server-constructure-dark.png#only-dark){ loading=lazy }

## フロントエンドの設定 {#frontend-settings}

### API エンドポイントの設定 {#api-endpoint-settings}

API エンドポイントの設定は Axios のインスタンス生成時に行います。エンドポイントは一般的にハードコーディングせず、 env ファイルから取得した値を利用します。 AlesInfiny Maris OSS Edition サンプルアプリでは、 API アクセスコードに関する設定を `src/api-client/index.ts` に記述しています。

```typescript title="src/api-client/index.ts"
const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_ENDPOINT,
});
```

??? info "Vite の環境変数"

    Vite で環境変数を利用するためには、環境変数名の前に `VITE_` を付ける必要があります。例えば、 `API_ENDPOINT` という環境変数を利用する場合、 `VITE_API_ENDPOINT` という名前で設定します。

※ `vite.config.ts` に `server.proxy` の設定をしている場合は [vite.config の設定値の解説](../vue-js/project-settings.md#vite-config) の「API エンドポイントを設定する際の注意点」も併せて参照してください。

### 本番用 env ファイルの作成 {#create-env-prod}

本番用の環境変数ファイルを作成します。 `.env.production` や `.env.prod` など本番用と判断できる名前で env ファイルを作成します。前の手順で作成した `import.meta.env.API_ENDPOINT` に対応する値を設定します。

```env title=".env.prod"
VITE_API_ENDPOINT=https://www.example.com
```

`VITE_` で始まる環境変数に対して TypeScript で型定義をしたい場合、 `env.d.ts` に以下のような設定を追加します。

```typescript title="env.d.ts"
/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_API_ENDPOINT: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
```

### 本番ビルドスクリプトの作成 {#create-production-build-script}

本番用の env ファイルを読み込むビルドスクリプトを `package.json` に作成します。 `mode` オプションの値は、前の手順で作成した env ファイルの名前に合わせます。 `.env.prod` の場合、 `mode prod` となります。

```json title="package.json（ワークスペース）"
{
  "scripts": {
    "build:prod": "run-p type-check build-only:prod --print-label",
    "build-only:prod": "vite build --mode prod",
    "type-check": "vue-tsc --build --force"
  }
}
```

ルートプロジェクトの `package.json` で、上記のスクリプトを呼び出すよう設定します。

```json title="package.json（ルート）"
{
  "scripts": {
    "build:prod:workspace-name": "run build:prod -w workspace-name",
  }
}
```

## バックエンドの設定 {#backend-settings}

### Program.cs の設定 {#program-cs-settings}

Web サーバーと AP サーバーを統合するサーバーでの運用では、ドメイン名へのリクエストに対してエントリーページとなる静的ファイルを返します。そのため、 `Program.cs` に以下のような設定を追加します。

```csharp title="Program.cs" hl_lines="8 22"
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
```

- `app.UseStaticFiles()` : 静的ファイルの提供を有効にします。
- `app.MapFallbackToFile("/index.html")` : ドメイン名へのリクエストに対して `index.html` を返します。リクエストパイプラインの最後に記述する必要があります。

### プロジェクトファイルの設定 {#project-file-settings}

発行する際、最新のフロントエンドのビルドファイルを含めるために、 Web アプリケーションのプロジェクトファイルに以下のような設定を追加します。以下のコード例は、フロントエンドのパスを書き換えれば、そのまま csproj ファイルに追加して使用できます。ただし、 `npm ci` と `npm run build:prod` のコマンドは、フロントエンドのビルドに合わせて変更してください。

```xml title="StartUp.csproj"
<Project Sdk="Microsoft.NET.Sdk.Web">
 <PropertyGroup>
   <SpaRoot>..\..\..\frontend\</SpaRoot>
   <SpaWorkspace>$(SpaRoot)workspace-name</SpaWorkspace>
 </PropertyGroup>

  ...

 <Target Name="PublishRunWebpack" AfterTargets="ComputeFilesToPublish">
   <!-- As part of publishing, ensure the JS resources are freshly built in production mode -->
   <Exec WorkingDirectory="$(SpaRoot)" Command="npm ci" />
   <Exec WorkingDirectory="$(SpaRoot)" Command="npm run build:prod:workspace-name" />

   <!-- Include the newly-built files in the publish output -->
   <ItemGroup>
     <DistFiles Include="$(SpaWorkspace)dist\**; $(SpaWorkspace)dist-server\**" />
     <ResolvedFileToPublish Include="@(DistFiles->'%(FullPath)')" Exclude="@(ResolvedFileToPublish)">
       <RelativePath>wwwroot\%(RecursiveDir)%(FileName)%(Extension)</RelativePath>
       <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
       <ExcludeFromSingleFile>true</ExcludeFromSingleFile>
     </ResolvedFileToPublish>
   </ItemGroup>
 </Target>

</Project>
```

### アプリケーションの発行 {#publish-application}

`dotnet publish` コマンドや Visual Studio などのツールを利用して、アプリケーションを発行します。 `publish` フォルダー直下にバックエンドのビルドアーティファクトと `wwwroot` フォルダーにフロントエンドの静的ファイル群が出力されます。発行したファイルをサーバーに配置し、アプリケーションを起動します。

![dotnet publish の出力ファイル](../../../images/guidebooks/how-to-develop/publication/published-folders-light.png#only-light){ loading=lazy }
![dotnet publish の出力ファイル](../../../images/guidebooks/how-to-develop/publication/published-folders-dark.png#only-dark){ loading=lazy }
