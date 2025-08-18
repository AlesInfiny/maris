using Dressca.Web.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

// アプリケーション設定ファイルの定義と型をバインドし、 DataAnnotation による検証を有効化する。
builder.Services
    .AddOptions<WebServerOptions>()
    .BindConfiguration(nameof(WebServerOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// サービスコレクションに CORS を追加する。
builder.Services.AddCors();

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

builder.Services.AddOpenApiDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "1.0.0";
        document.Info.Title = "Entra External ID ユーザー認証";
        document.Info.Description = "Entra External IDを利用したユーザー認証機能を提供するサンプルアプリケーションです。";
        document.Servers.Add(new()
        {
            Description = "ローカル開発用のサーバーです。",
            Url = "https://localhost:5001",
        });
    };

    // Open API ドキュメントの security scheme を有効化
    config.AddSecurity("Bearer", new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Description = "この API は Bearer トークンによる認証が必要です。",
    });
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });
}

// Entra External ID の設定をインジェクション
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(
    options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.TokenValidationParameters.NameClaimType = "name";
    },
    options => { builder.Configuration.Bind("AzureAd", options); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseHttpLogging();
}

app.UseHttpsRedirection();

var options = app.Services.GetRequiredService<IOptions<WebServerOptions>>();

// アプリケーション設定にオリジンの記述がある場合のみ CORS ポリシーを追加する。
if (options.Value.AllowedOrigins.Length > 0)
{
    app.UseCors(policy =>
    {
        policy
            .WithOrigins(options.Value.AllowedOrigins)
            .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
            .AllowAnyHeader()
            .AllowCredentials();
    });
}

// Entra External ID での認証を有効にするために追加
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

/// <summary>
///  結合テストプロジェクト用の部分クラス。
/// </summary>
public partial class Program
{
}
