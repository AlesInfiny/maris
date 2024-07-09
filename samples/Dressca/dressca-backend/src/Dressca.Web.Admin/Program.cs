using System.Text.Json;
using Dressca.Web.Admin.Controllers;
using Microsoft.AspNetCore.HttpLogging;

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

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseExceptionHandler(ErrorController.DevelopmentErrorRoute);
    app.UseHttpLogging();
}
else
{
    app.UseExceptionHandler(ErrorController.ErrorRoute);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
