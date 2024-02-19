---
title: アプリケーションの発行
description: .NET + Vue.js で構成されたアプリケーションの発行手順について解説します。
---

# アプリケーションの発行 {#top}

.NET + Vue.js のアプリケーションを発行する手順について解説します。なお、本手順は単一サーバー構成を前提としています。

## クライアントサイドの設定 {#client-side-settings}

### API エンドポイントの設定 {#api-endpoint-settings}

API エンドポイントの設定は Axios のインスタンス生成時に行います。エンドポイントは一般的にハードコーディングせず、 env ファイルから取得した値を利用します。 AlesInfiny Maris サンプルアプリでは、 API アクセスコードに関する設定を `src/api-client/index.ts` に記述しています。

```typescript title="src/api-client/index.ts"
const axiosInstance = axios.create({
  baseURL: import.meta.env.API_ENDPOINT,
});
```

OpenAPI Generator を利用する際は、 OpenAPI 定義書に記載されているエンドポイントが上書きされてしまいます。そのため、 OpenAPI Generator で生成されたコードをラップしてエンドポイントを上書きする必要があります。

```typescript title="src/api-client/index.ts"
const config = new apiClient.Configuration({});

const axiosInstance = axios.create({});

const wrappedApi = new apiClient.generatedApi(
  config,
  import.meta.env.API_ENDPOINT,
  axiosInstance
);

export default wrappedApi;
```

### 本番用 env ファイルの作成 {#create-env-prod}

本番用の環境変数ファイルを作成します。 `.env.production` や `.env.prod` など本番用と判断できる名前で env ファイルを作成します。前の手順で作成した `import.meta.env.API_ENDPOINT` に対応する値を設定します。

```env title=".env.prod"
API_ENDPOINT=https://api.example.com
```

### 本番ビルドスクリプトの作成 {#create-production-build-script}

本番用の env ファイルを読み込むビルドスクリプトを `package.json` に作成します。 `mode` オプションの値は、前の手順で作成した env ファイルの名前に合わせます。 `.env.prod` の場合、 `mode prod` となります。

```json title="package.json"
{
  "scripts": {
    "build:prod": "vue-tsc --noEmit && vite build -- mode prod"
  }
}
```

## サーバーサイドの設定 {#server-side-settings}

### Program.cs の設定 {#program-cs-settings}

単一サーバーでの運用では、ドメイン名へのリクエストに対してエントリーページとなる静的ファイルを返します。そのため、 `Program.cs` に以下のような設定を追加します。

```csharp title="Program.cs"
app.UseStaticFiles();

...

app.MapFallbackToFile("/index.html");

app.Run();
```

- `app.UseStaticFiles()` : 静的ファイルの提供を有効にします。
- `app.MapFallbackToFile("/index.html")` : ドメイン名へのリクエストに対して `index.html` を返します。リクエストパイプラインの最後に記述する必要があります。

### プロジェクトファイルの設定 {#project-file-settings}

発行する際、最新のクライアントサイドのビルドファイルを含めるために、 `.csproj` ファイルに以下のような設定を追加します。以下のコード例は、クライアントサイドのパスを書き換えれば、そのまま csproj ファイルに追加して使用できます。ただし、 `npm install` と `npm run build:prod` のコマンドは、クライアントサイドのビルドに合わせて変更してください。

```xml title="StartUp.csproj"
<Project Sdk="Microsoft.NET.Sdk.Web">
 <PropertyGroup>
   <SpaRoot>[ClientAppRoot]</SpaRoot>
 </PropertyGroup>

  ...

 <Target Name="PublishRunWebpack" AfterTargets="ComputeFilesToPublish">
   <!-- As part of publishing, ensure the JS resources are freshly built in production mode -->
   <Exec WorkingDirectory="$(SpaRoot)" Command="npm install" />
   <Exec WorkingDirectory="$(SpaRoot)" Command="npm run build:prod" />

   <!-- Include the newly-built files in the publish output -->
   <ItemGroup>
     <DistFiles Include="$(SpaRoot)dist\**; $(SpaRoot)dist-server\**" />
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

`dotnet publish` コマンドや Visual Studio などのツールを利用して、アプリケーションを発行します。発行したファイルをサーバーに配置し、アプリケーションを起動します。
