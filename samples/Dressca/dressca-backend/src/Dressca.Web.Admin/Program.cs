using System.Text.Json;
using Dressca.ApplicationCore;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Auth;
using Dressca.EfInfrastructure;
using Dressca.Store.Assets.StaticFiles;
using Dressca.Web.Admin.Authorization;
using Dressca.Web.Admin.Controllers;
using Dressca.Web.Admin.HealthChecks;
using Dressca.Web.Admin.Mapper;
using DummyAuthentication.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressMapClientErrors = true;

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

builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenApiDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "1.0.0";
        document.Info.Title = "Dressca Web API";
        document.Info.Description = "Dressca の Web API 仕様";
        document.Servers.Add(new()
        {
            Description = "ローカル開発用のサーバーです。",
            Url = "https://localhost:6001",
        });
    };
});

builder.Services.AddDresscaEfInfrastructure(builder.Configuration);
builder.Services.AddStaticFileAssetStore();
builder.Services.AddDresscaApplicationCore();
builder.Services.AddTransient<CatalogManagementApplicationService>();
builder.Services.AddTransient<IUserStore, UserStore>();
builder.Services.AddDresscaDtoMapper();

if (builder.Environment.IsDevelopment())
{
    // ローカル開発環境用の認証ハンドラーを登録します。
    builder.Services.AddAuthentication("DummyAuthentication")
    .AddScheme<AuthenticationSchemeOptions, DummyAuthenticationHandler>("DummyAuthentication", null);
    builder.Services.AddAuthorization();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks(HealthCheckDescriptionProvider.HealthCheckRelativePath);

app.MapFallbackToFile("/index.html");

app.Run();

/// <summary>
/// 結合テストプロジェクトに公開するための部分クラス。
/// </summary>
public partial class Program { }
