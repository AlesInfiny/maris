using Dressca.Web.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Identity.Web;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

const string corsPolicyName = "allowSpecificOrigins";

var section = builder.Configuration.GetSection(nameof(WebServerOptions));
builder.Services.Configure<WebServerOptions>(section);
var options = section.Get<WebServerOptions>();
var origins = options != null ? options.AllowedOrigins : null;

if (origins != null && origins.Length > 0)
{
    builder.Services
        .AddCors(options =>
        {
            options.AddPolicy(
               name: corsPolicyName,
               policy =>
               {
                   // Origins, Methods, Header, Credentials すべての設定が必要（設定しないと CORS が動作しない）
                   policy
                       .WithOrigins(origins)
                       .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
                       .AllowAnyHeader()
                       .AllowCredentials();
               });
        });
}

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
        document.Info.Title = "Azure AD B2C ユーザー認証";
        document.Info.Description = "Azure AD B2Cを利用したユーザー認証機能を提供するサンプルアプリケーションです。";
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

// Azure AD B2C の設定をインジェクション
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(
    options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);
        options.TokenValidationParameters.NameClaimType = "name";
    },
    options => { builder.Configuration.Bind("AzureAdB2C", options); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseHttpLogging();
}

app.UseHttpsRedirection();

if (origins != null && origins.Length > 0)
{
    app.UseCors(corsPolicyName);
}

// Azure AD B2C での認証を有効にするために追加
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
