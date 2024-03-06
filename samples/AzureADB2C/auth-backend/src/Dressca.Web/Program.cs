using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options =>
    {
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Bad Request となった場合の処理。
        var builtInFactory = options.InvalidModelStateResponseFactory;
        options.InvalidModelStateResponseFactory = context =>
        {
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

app.UseStaticFiles();

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
