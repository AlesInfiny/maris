﻿using Dressca.ApplicationCore;
using Dressca.EfInfrastructure;
using Dressca.Web.Mapper;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
