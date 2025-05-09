using Maris.ConsoleApp.Hosting;
using Maris.Samples.ApplicationCore;
using Maris.Samples.Cli.Commands;
using Maris.Samples.InMemoryInfrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// 汎用ホストのランタイムを設定します。
var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    // コマンド内で利用するクラス群を DI コンテナーに登録
    services.AddScoped<ProductApplicationService>();
    services.AddScoped<IProductsRepository, ProductsRepository>();

    // コンソールアプリケーションのフレームワークの登録と設定
    services.AddConsoleAppService(
        args,
        options =>
        {
            options.DefaultErrorExitCode = CommandResult.GenericError.ExitCode;
            options.DefaultValidationErrorExitCode = CommandResult.ValidationError.ExitCode;
        });
});

// 設定に基づき、汎用ホストのランタイムを構築します。
var app = builder.Build();

// 汎用ホストを実行します。
await app.RunAsync();
