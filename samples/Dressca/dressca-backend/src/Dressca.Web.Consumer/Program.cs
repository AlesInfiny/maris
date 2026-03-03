using System.Text.Json;
using Dressca.ApplicationCore;
using Dressca.EfInfrastructure;
using Dressca.Store.Assets.StaticFiles;
using Dressca.Web.Configuration;
using Dressca.Web.Consumer;
using Dressca.Web.Consumer.Baskets;
using Dressca.Web.Consumer.Mapper;
using Dressca.Web.Consumer.Resources;
using Dressca.Web.Controllers;
using Dressca.Web.Extensions;
using Dressca.Web.HealthChecks;
using Dressca.Web.Runtime;
using Maris.Core.Text.Json;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// アプリケーション設定ファイルの定義と型をバインドし、 DataAnnotation による検証を有効化する。
builder.Services
    .AddOptions<WebServerOptions>()
    .BindConfiguration(nameof(WebServerOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// サービスコレクションに CORS を追加する。
builder.Services.AddCors();

// CookiePolicy を DI に登録（他のコードから IOptions<CookiePolicyOptions> で取得可能にする）
builder.Services.AddOptions<CookiePolicyOptions>()
    .Configure<IOptions<WebServerOptions>>((cookiePolicy, webServerOptions) =>
    {
        // アプリケーション全体の Cookie ポリシーを定義する。
        cookiePolicy.HttpOnly = HttpOnlyPolicy.Always;
        cookiePolicy.Secure = CookieSecurePolicy.Always;

        cookiePolicy.MinimumSameSitePolicy =
            webServerOptions.Value.AllowedOrigins.Length > 0
                ? SameSiteMode.None
                : SameSiteMode.Strict;
    });

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<BuyerIdFilterAttribute>();
        if (builder.Environment.IsDevelopment())
        {
            options.Filters.Add<BusinessExceptionDevelopmentFilter>();
        }
        else
        {
            options.Filters.Add<BusinessExceptionFilter>();
        }
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;

        // Bad Request となった場合の処理。
        var builtInFactory = options.InvalidModelStateResponseFactory;
        options.InvalidModelStateResponseFactory = context =>
        {
            // エラーの原因をログに出力。
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation(Events.ReceiveHttpBadRequest, LogMessages.ReceiveHttpBadRequest, JsonSerializer.Serialize(context.ModelState, DefaultJsonSerializerOptions.GetInstance()));

            // ASP.NET Core の既定の実装を使ってレスポンスを返却。
            return builtInFactory(context);
        };
    });

builder.Services.AddOpenApiDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "1.0.0";
        document.Info.Title = "Dressca Consumer Web API";
        document.Info.Description = "Dressca Consumer の Web API 仕様";
        document.Servers.Add(new()
        {
            Description = "ローカル開発用のサーバーです。",
            Url = "https://localhost:5001",
        });
    };
});

builder.Services.AddDresscaEfInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddStaticFileAssetStore();

builder.Services.AddDresscaApplicationCore();

builder.Services.AddDresscaDtoMapper();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });
}

builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApiDescriptionProvider, HealthCheckDescriptionProvider>());

builder.Services.AddHealthChecks()
    .AddDresscaDbContextCheck("DresscaDatabaseHealthCheck");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseHttpLogging();
    app.UseExceptionHandler(ErrorController.DevelopmentErrorRoute);
}
else
{
    app.UseExceptionHandler(ErrorController.ErrorRoute);
}

app.UseHttpsRedirection();

app.UseSecuritySettings();

app.UseStaticFiles();

var options = app.Services.GetRequiredService<IOptions<WebServerOptions>>();

// アプリケーション設定にオリジンの記述がある場合のみ CORS ポリシーを追加する。
if (options.Value.AllowedOrigins.Length > 0)
{
    app.UseCors(policy =>
    {
        // Origins, Methods, Header, Credentials すべての設定が必要（設定しないと CORS が動作しない）
        // レスポンスの Header を フロントエンド側 JavaScript で使用する場合、 WithExposedHeaders も必須
        policy
            .WithOrigins(options.Value.AllowedOrigins)
            .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Location");
    });
}

// DI に登録された CookiePolicyOptions を有効化する。
app.UseCookiePolicy();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks(HealthCheckDescriptionProvider.HealthCheckRelativePath);

app.MapFallbackToFile("/index.html");

app.Run();
