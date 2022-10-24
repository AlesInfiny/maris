---
title: .NET 編
description: バックエンドで動作する .NET アプリケーションの開発手順を解説します。
---

# ASP.NET Core Web API プロジェクトの構成 {#top}

ASP.NET Core Web API のプロジェクトには、 Open API 仕様書の出力設定と例外ハンドリングのための設定、ログ出力設定をします。

## Open API 仕様書の出力設定 {#open-api-specification-output-configuration}

ブラウザー上で確認できる Web API の仕様書と、クライアントコードを生成するための Open API 仕様書のファイル生成ができるようにプロジェクトを設定します。

### Open API 出力用 NuGet パッケージの追加 {#add-nuget-package-for-generating-open-api}

以下の NuGet パッケージを ASP.NET Core Web API プロジェクトに追加します。

- [NSwag.AspNetCore](https://www.nuget.org/packages/NSwag.AspNetCore)
- [NSwag.MSBuild](https://www.nuget.org/packages/NSwag.MSBuild)
- [Microsoft.AspNetCore.Mvc.NewtonsoftJson](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.NewtonsoftJson)

### NSwag 構成ファイルの追加 {#add-nswag-json}

[nswag.json] ファイルをプロジェクトルートに追加します。
NSwag を用いた実装コードの生成は行わないため、 Open API 仕様書の生成に関する設定のみ実施してください。
[nswag.json] の設定値の詳細は、以下を参照してください。

- [NSwag Configuration Document](https://github.com/RicoSuter/NSwag/wiki/NSwag-Configuration-Document)

??? example "nswag.json の設定例"
    [nswag.json] の設定例を示します。
    コメントのある個所については、プロジェクトのプロファイルに応じて設定を変更してください。

    ```json title="nswag.json"
    {
      "runtime": "Net60",
      "defaultVariables": null,
      "documentGenerator": {
        "aspNetCoreToOpenApi": {
          "project": "AaaSubSystem.Web.csproj", // プロジェクト名を指定
          "msBuildProjectExtensionsPath": null,
          "configuration": "$(Configuration)",
          "runtime": null,
          "targetFramework": null,
          "noBuild": true,
          "msBuildOutputPath": null,
          "verbose": true,
          "workingDirectory": null,
          "requireParametersWithoutDefault": true,
          "apiGroupNames": null,
          "defaultPropertyNameHandling": "Default",
          "defaultReferenceTypeNullHandling": "Null",
          "defaultDictionaryValueReferenceTypeNullHandling": "NotNull",
          "defaultResponseReferenceTypeNullHandling": "NotNull",
          "generateOriginalParameterNames": true,
          "defaultEnumHandling": "Integer",
          "flattenInheritanceHierarchy": false,
          "generateKnownTypes": true,
          "generateEnumMappingDescription": false,
          "generateXmlObjects": false,
          "generateAbstractProperties": false,
          "generateAbstractSchemas": true,
          "ignoreObsoleteProperties": false,
          "allowReferencesWithProperties": false,
          "useXmlDocumentation": true,
          "resolveExternalXmlDocumentation": true,
          "excludedTypeNames": [],
          "serviceHost": "localhost:5001",
          "serviceBasePath": null,
          "serviceSchemes": [
            "https"
          ],
          "infoTitle": "Xxx Web API", // Open API 仕様書のタイトルや説明、バージョンを指定
          "infoDescription": "Xxx の Web API 仕様",
          "infoVersion": "1.0.0",
          "documentTemplate": null,
          "documentProcessorTypes": [],
          "operationProcessorTypes": [],
          "typeNameGeneratorType": null,
          "schemaNameGeneratorType": null,
          "contractResolverType": null,
          "serializerSettingsType": null,
          "useDocumentProvider": false,
          "documentName": "v1",
          "aspNetCoreEnvironment": "Development",
          "createWebHostBuilderMethod": null,
          "startupType": null,
          "allowNullableBodyParameters": true,
          "useHttpAttributeNameAsOperationId": false,
          "output": "XXXXXXXXX-api.json", // 出力するファイル名を指定
          "outputType": "OpenApi3",
          "newLineBehavior": "Auto",
          "assemblyPaths": [],
          "assemblyConfig": null,
          "referencePaths": [],
          "useNuGetCache": false
        }
      },
      "codeGenerators": {}
    }
    ```

### Open API 仕様書ファイルの出力設定 {#setup-for-generate-open-api-specification-file}

Open API 仕様書のファイルがビルド時に生成されるようプロジェクトファイルを設定します。
設定方法の詳細は、以下を参照してください。

- [NSwag.MSBuild](https://github.com/RicoSuter/NSwag/wiki/NSwag.MSBuild)

??? example ".NET 6 の場合のプロジェクトファイル設定例"
    .NET 6 を使用するプロジェクトの場合、プロジェクトファイルには以下のように設定することで、 Open API 仕様書のファイルを出力できます。

    ```xml title="Open API 仕様書のファイルを出力する csproj の設定例"
    <Project Sdk="Microsoft.NET.Sdk.Web">
      <!-- 追加箇所以外は省略 -->
      <Target Name="NSwag" AfterTargets="PostBuildEvent" Condition="'$(Configuration)' == 'Debug'">
        <Exec WorkingDirectory="$(ProjectDir)" EnvironmentVariables="ASPNETCORE_ENVIRONMENT=Development" Command="$(NSwagExe_Net60) run nswag.json /variables:Configuration=$(Configuration)" />
      </Target>

      <PropertyGroup>
        <RunPostBuildEvent>OnBuildSuccess</RunPostBuildEvent>
      </PropertyGroup>
    </Project>
    ```

### ブラウザーで Open API 仕様書を表示する設定 {#setup-for-generate-open-api-specification-gui}

ブラウザー上で確認できる Web API の仕様書の出力設定を変更します。
開発環境でのみ、 Open API v3 の仕様書をブラウザー経由で参照できるように設定します。
設定方法の詳細は、以下を参照してください。

- [AspNetCore Middleware](https://github.com/RicoSuter/NSwag/wiki/AspNetCore-Middleware)

<!-- textlint-disable ja-technical-writing/ja-no-mixed-period -->
??? example "Web API 仕様書をブラウザーから確認できるようにする設定例"
    <!-- textlint-enable ja-technical-writing/ja-no-mixed-period -->

    ブラウザーから Web API 仕様書を確認できるようにするためには、 Open API v3 仕様書の出力設定と、 Web UI の設定が必要です。
    ASP.NET Core Web API プロジェクトの [Program.cs] または [Startup.cs] に、以下のように実装を加えてください。

    ```csharp title="Open API v3 仕様書の出力設定 ( Program.cs )"
    builder.Services.AddOpenApiDocument(config =>
    {
        config.PostProcess = document =>
        {
            document.Info.Version = "1.0.0";
            document.Info.Title = "Xxx Web API";
            document.Info.Description = "Xxx の Web API 仕様";
        };
    });
    ```

    ```csharp title="Web UI の設定 ( Program.cs )"
    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
        app.UseSwaggerUi3();
    }
    ```

## 例外ハンドリングの設定 {#exception-handling-configuration}

サーバー処理内で発生する例外に対処する共通的な設定と実装を追加します。

### 未処理例外のエラー情報を返却するコントローラークラスの作成 {#create-error-controller}

キャッチされなかった例外の情報を返却するために、エラー情報を取得できるコントローラーを追加します。
エラーレスポンスは [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807) ([日本語訳付き](https://tex2e.github.io/rfc-translater/html/rfc7807.html)) に従った形式で返却するようにしましょう。

本番環境ではアプリケーションの内部情報流出を防ぐため、スタックトレースを返却しないようにします。
開発環境ではエラーの詳細を簡単に開発者が把握できるよう、スタックトレースをエラーレスポンスに含めることを検討しましょう。

<!-- textlint-disable ja-technical-writing/ja-no-mixed-period -->
??? example "システムエラーのエラー情報を返却するコントローラー実装例"
    <!-- textlint-enable ja-technical-writing/ja-no-mixed-period -->

    システムエラーのエラー情報を返却するためには、未処理例外の情報を取得し、適切な形式に変換するコントローラー ( この例では `#!csharp ErrorController` ) を作成します。
    このコントローラーは、 [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807) ([日本語訳付き](https://tex2e.github.io/rfc-translater/html/rfc7807.html)) に従ったエラーレスポンスを返却するように実装しましょう。
    `#!csharp ControllerBase.Problem` メソッドを利用すると、 RFC 7807 に準拠したレスポンスを簡単に構築できます。

    開発環境で使用するアクションメソッドと、それ以外の環境で使用するアクションメソッドは分割して定義しましょう。
    開発環境で使用するアクションメソッドでは、スタックトレースをレスポンスに含めるように実装します。

    開発環境で使用するアクションメソッドは、安全のため開発環境以外の場所で呼び出しても動作しないように実装しておきましょう。
    実装例のように、開発環境以外の呼び出しには HTTP 404 を返却するのが最も簡単です。

    ```csharp title="ErrorController.cs"
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    
    namespace AaaSubSystem.Web.Controllers; // 名前空間は適宜変更してください。
    
    /// <summary>
    ///  エラーの情報にアクセスする API コントローラーです。
    ///  このコントローラーは、例外ハンドラーで例外を検知したときに呼び出されることを想定しています。
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // Open API 仕様書にこのコントローラーの情報を追加しないよう制御します。
    public class ErrorController : ControllerBase
    {
        /// <summary>
        ///  開発環境におけるエラー情報取得のためのルートパス ( /error-development ) 。
        /// </summary>
        internal const string DevelopmentErrorRoute = "/error-development"; // Program.cs または Startup.cs から参照するため internal にします。
    
        /// <summary>
        ///  実行環境におけるエラー情報取得のためのルートパス ( /error ) 。
        /// </summary>
        internal const string ErrorRoute = "/error"; // Program.cs または Startup.cs から参照するため internal にします。
    
        /// <summary>
        ///  開発環境でのエラーレスポンスを取得します。
        /// </summary>
        /// <param name="hostEnvironment">環境の情報。</param>
        /// <returns>エラーの詳細情報。</returns>
        [Route(DevelopmentErrorRoute)]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            // 開発環境以外からのアクセスは HTTP 404 を返します。
            if (!hostEnvironment.IsDevelopment())
            {
                return this.NotFound();
            }
    
            var exceptionHandlerFeature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
            return this.Problem(
                detail: exceptionHandlerFeature?.Error.StackTrace,
                title: exceptionHandlerFeature?.Error.Message);
        }
    
        /// <summary>
        ///  実行環境でのエラーレスポンスを取得します。
        /// </summary>
        /// <returns>エラーの情報。</returns>
        [Route(ErrorRoute)]
        public IActionResult HandleError() => this.Problem();
    }
    ```

### 未処理例外発生時の例外ハンドラーを設定 {#setup-exception-handler}

未処理の例外が ASP.NET Core のランタイムまで到達した場合、[先ほど](#create-error-controller)作成したエラー情報を取得できるコントローラーを呼び出すようランタイムを構成します。
開発環境ではスタックトレース込みのエラー情報を返却する処理を登録します。
それ以外の環境ではスタックトレースを返さない処理を登録します。

??? example "例外ハンドラーの登録例"
    [先ほど](#create-error-controller)作成した `#!csharp ErrorController` を例外ハンドラーとして使用するように構成します。
    環境に応じて、登録する例外ハンドラーを適切に設定しましょう。
    ASP.NET Core Web API プロジェクトの [Program.cs] または [Startup.cs] に、以下のように実装を加えてください。

    ```csharp title="例外ハンドラーの設定 ( Program.cs )"
    if (app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler(ErrorController.DevelopmentErrorRoute);
    }
    else
    {
        app.UseExceptionHandler(ErrorController.ErrorRoute);
    }
    ```

## HTTP 400 時のログ出力 {#logging-on-http-400}

Web API から HTTP 400 のレスポンスを返却する際、問題の原因となった入力値の情報をログに記録しましょう。
入力値検証などのエラー情報をサーバー側でロギングでき、障害発生時の追跡性を高めることができます。

??? example "HTTP 400 の場合ログを出力する実装例"
    `#!csharp ILogger` を使用して、 `#!csharp ModelState` の情報をログに出力するよう構成します。
    HTTP レスポンスは、 ASP.NET Core の既定の実装を使って返却するようにします。
    ASP.NET Core Web API プロジェクトの [Program.cs] または [Startup.cs] に、以下のように実装を加えてください。

    ```csharp title="HTTP 400 時のログ出力設定 ( Program.cs )"
    builder.Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            // Bad Request となった場合の処理。
            var builtInFactory = options.InvalidModelStateResponseFactory;
            options.InvalidModelStateResponseFactory = context =>
            {
                // エラーの原因をログに出力。
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("HTTP 要求に誤りがあります。詳細情報: {0} 。", JsonSerializer.Serialize(context.ModelState));

                // ASP.NET Core の既定の実装を使ってレスポンスを返却。
                return builtInFactory(context);
            };
        });
    ```

## HTTP 通信ログの出力 {#configure-http-communication-log}

Web API アプリケーションの入出力は、 HTTP 通信の形式になります。
実際の開発作業では、意図した通りの HTTP リクエスト / レスポンスが送受信できているか確認することがよくあります。
これをサポートするため、開発環境では HTTP 通信ログを出力するよう設定することを推奨します。

設定したら、開発環境の[ログレベルの設定](#configure-log-level)をあわせて実施するようにしてください。

??? example "HTTP 通信ログを出力する実装例"
    HTTP 通信ログの出力は、 ASP.NET Core のミドルウェアを用いて実現します。
    通常開発者がログ出力処理を記述する必要はありません。
    開発環境でのみ HTTP 通信ログが出力されるよう ASP.NET Core ランタイムを構成します。
    ASP.NET Core Web API プロジェクトの [Program.cs] または [Startup.cs] に、以下の 2 つの実装を加えてください。

    ```csharp title="HTTP 通信ログ出力設定 1 ( Program.cs )"
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddHttpLogging(logging =>
        {
            // どのデータをどのくらいの量出力するか設定。
            // 適宜設定値は変更する。
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }
    ```

    ```csharp title="HTTP 通信ログ出力設定 2 ( Program.cs )"
    if (app.Environment.IsDevelopment())
    {
        // HTTP 通信ログを有効にする。
        app.UseHttpLogging();
    }
    ```

## ログレベルの設定 {#configure-log-level}

ソースコード内で `#!csharp ILogger` を用いてログ出力を行っても、ログレベルを正しく構成しない限りログ出力は行われません。
ASP.NET Core Web API プロジェクトを作成した際、一緒に作成される [appsettings.json] および [appsettings.Development.json] ファイルに対してログレベルを設定します。
ログレベルの設定方法については、以下を参照してください。

- [.NET でのログの記録 - ログの構成](https://docs.microsoft.com/ja-JP/dotnet/core/extensions/logging#configure-logging)

ログレベルは、本番環境向けの設定と開発環境向けの設定を別々に管理します。
本番環境向けには原則 Information 以上のログのみを出力するように設定します。
開発環境向けには、アプリケーション内で出力するように設定したログがすべて出力できるように設定します。

??? example "基本のログ構成例"
    本番環境向けのログレベル設定は、 [appsettings.json] に行います。
    Information レベル以上のログを出力するように設定しましょう。
    ただし、 `Microsoft.AspNetCore` のカテゴリについては、 Warning 以上のレベルのみ出力するようにします。

    開発しているアプリケーションから出力するログのログレベルも、明示的に設定しておくことを推奨します。
    例のように、 `LogLLevel` 要素にソリューション名と同名のキーを追加し、値としてログレベルを設定します。
    通常は Information を設定しましょう。

    ```json title="appsettings.json"
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning",
          "AaaSubSystem": "Information"
        }
      }
    }
    ```

    開発環境向けのログレベル設定は [appsettings.Development.json] に行います。
    開発環境では、開発者が設定したデバッグレベルのログも出力できるように設定します。
    例のように、 `LogLLevel` 要素にソリューション名と同名のキーを追加し、 Debug を設定します。

    [HTTP 通信ログ](#configure-http-communication-log)を記録するように設定した場合は、そのログが出力されるよう個別に設定を追加します。
    例のように、 `LogLLevel` 要素に Microsoft.AspNetCore.HttpLogging のキーを追加し、 Information を設定します。

    ```json title="appsettings.Development.json"
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning",
          "Microsoft.AspNetCore.HttpLogging": "Information",
          "AaaSubSystem": "Debug"
        }
      }
    }
    ```
