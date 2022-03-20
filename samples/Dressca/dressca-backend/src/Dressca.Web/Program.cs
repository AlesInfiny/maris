using System.Text.Json;
using Dressca.ApplicationCore;
using Dressca.EfInfrastructure;
using Dressca.Store.Assets.StaticFiles;
using Dressca.Web.Baskets;
using Dressca.Web.Controllers;
using Dressca.Web.Mapper;
using Dressca.Web.Resources;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options => options.Filters.Add(new BuyerIdFilterAttribute()))
    .ConfigureApiBehaviorOptions(options =>
    {
        // Bad Request となった場合の処理。
        var builtInFactory = options.InvalidModelStateResponseFactory;
        options.InvalidModelStateResponseFactory = context =>
        {
            // エラーの原因をログに出力。
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation(WebMessages.RecieveHttpBadRequest, JsonSerializer.Serialize(context.ModelState));

            // ASP.NET Core の既定の実装を使ってレスポンスを返却。
            return builtInFactory(context);
        };
    });

builder.Services.AddSwaggerDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "1.0.0";
        document.Info.Title = "Dressca Web API";
        document.Info.Description = "Dressca の Web API 仕様";
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
    app.UseHttpLogging();
    app.UseExceptionHandler(ErrorController.DevelopmentErrorRoute);
}
else
{
    app.UseExceptionHandler(ErrorController.ErrorRoute);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
