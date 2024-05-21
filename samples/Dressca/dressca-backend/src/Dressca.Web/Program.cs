using System.Text.Json;
using Dressca.ApplicationCore;
using Dressca.EfInfrastructure;
using Dressca.Store.Assets.StaticFiles;
using Dressca.SystemCommon.Text.Json;
using Dressca.Web;
using Dressca.Web.Baskets;
using Dressca.Web.Controllers;
using Dressca.Web.HealthChecks;
using Dressca.Web.Mapper;
using Dressca.Web.Resources;
using Dressca.Web.Runtime;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;

const string corsPolicyName = "allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var section = builder.Configuration.GetSection("UserSettings:AllowedOrigins");
var origins = section != null ? section.Get<string[]>() : null;

if (origins != null)
{
    builder.Services
        .AddCors(options =>
        {
            options.AddPolicy(
               name: corsPolicyName,
               policy =>
               {
                   // Origins, Methods, Header, Credentials すべての設定が必要（設定しないと CORS が動作しない）
                   // レスポンスの Header を使用する場合、 WithExposedHeaders も必須
                   policy
                       .WithOrigins(origins)
                       .WithMethods("POST", "GET", "OPTIONS", "HEAD", "DELETE", "PUT")
                       .AllowAnyHeader()
                       .AllowCredentials()
                       .WithExposedHeaders("Location");
               });
        });
}

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
            options.Filters.Add<OriginVerificationResourceFilter>();
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
        document.Info.Title = "Dressca Web API";
        document.Info.Description = "Dressca の Web API 仕様";
        document.Servers.Add(new()
        {
            Description = "ローカル開発用のサーバーです。",
            Url = "https://localhost:5001",
        });
    };
});

builder.Services.AddDresscaEfInfrastructure(builder.Configuration);

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

app.UseStaticFiles();

if (origins != null)
{
    app.UseCors(corsPolicyName);
}

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks(HealthCheckDescriptionProvider.HealthCheckRelativePath);

app.MapFallbackToFile("/index.html");

app.Run();

/// <summary>
///  結合テストプロジェクト用の部分クラス。
/// </summary>
public partial class Program
{
}
