using System.Text.Json;
using Dressca.ApplicationCore;
using Dressca.ApplicationCore.Authorization;
using Dressca.EfInfrastructure;
using Dressca.Store.Assets.StaticFiles;
using Dressca.Web.Admin;
using Dressca.Web.Admin.Authorization;
using Dressca.Web.Admin.Mapper;
using Dressca.Web.Admin.Resources;
using Dressca.Web.Authorization;
using Dressca.Web.Configuration;
using Dressca.Web.Controllers;
using Dressca.Web.HealthChecks;
using Dressca.Web.Runtime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// アプリケーション設定ファイルの定義と型をバインドし、 DataAnnotation による検証を有効化します。
builder.Services
    .AddOptions<WebServerOptions>()
    .BindConfiguration(nameof(WebServerOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// サービスコレクションに CORS を追加します。
builder.Services.AddCors();

builder.Services
    .AddControllers(options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.Filters.Add<BusinessExceptionDevelopmentFilter>();
            options.Filters.Add<DbUpdateConcurrencyExceptionDevelopmentFilter>();
        }
        else
        {
            options.Filters.Add<BusinessExceptionFilter>();
            options.Filters.Add<DbUpdateConcurrencyExceptionFilter>();
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
            logger.LogInformation(Events.ReceiveHttpBadRequest, LogMessages.ReceiveHttpBadRequest, JsonSerializer.Serialize(context.ModelState));

            // ASP.NET Core の既定の実装を使ってレスポンスを返却。
            return builtInFactory(context);
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenApiDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "1.0.0";
        document.Info.Title = "Dressca Admin Web API";
        document.Info.Description = "Dressca Admin の Web API 仕様";
        document.Servers.Add(new()
        {
            Description = "ローカル開発用のサーバーです。",
            Url = "https://localhost:6001",
        });
    };
});

builder.Services.AddDresscaEfInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddStaticFileAssetStore();
builder.Services.AddTransient<IUserStore, UserStore>();
builder.Services.AddDresscaApplicationCore();
builder.Services.AddDresscaDtoMapper();

if (builder.Environment.IsDevelopment())
{
    // ローカル開発環境用の認証ハンドラーを登録します。
    _ = builder.Services.AddAuthentication("DummyAuthentication")
    .AddScheme<AuthenticationSchemeOptions, DummyAuthenticationHandler>("DummyAuthentication", null);

    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });
}
else if (builder.Environment.IsProduction())
{
    // 本番環境用の認証ハンドラーを登録します。
}

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Policies.RequireAdminRole, policy => policy.RequireRole(Roles.Admin));

builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler, StatusCodeMapAuthorizationMiddlewareResultHandler>();

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

app.UseStaticFiles();

var options = app.Services.GetRequiredService<IOptions<WebServerOptions>>();

// アプリケーション設定にオリジンの記述がある場合のみ CORS ポリシーを追加します。
if (options.Value.AllowedOrigins.Length > 0)
{
    app.UseCors(policy =>
    {
        // Origins, Methods, Header, Credentials すべての設定が必要です。（設定しないと CORS が動作しません。）
        // レスポンスの Header を フロントエンド側 JavaScript で使用する場合、 WithExposedHeaders も必須です。
        policy
            .WithOrigins(options.Value.AllowedOrigins)
            .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Location");
    });
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks(HealthCheckDescriptionProvider.HealthCheckRelativePath);

app.MapFallbackToFile("/index.html");

app.Run();
