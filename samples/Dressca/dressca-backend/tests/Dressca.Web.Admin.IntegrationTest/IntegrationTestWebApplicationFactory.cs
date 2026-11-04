using Dressca.AssetsManagement;
using Dressca.CatalogManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dressca.Web.Admin.IntegrationTest;

public class IntegrationTestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private const string DefaultTestEnvironment = "LocalIntegrationTest";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // 開発用のデータベースを書き換えないよう、既定では結合テスト専用のデータベースに接続します。
        var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? DefaultTestEnvironment;

        builder.ConfigureServices((context, services) =>
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddDresscaAssetsManagement(config, context.HostingEnvironment);
            services.AddDresscaCatalogManagement(config, context.HostingEnvironment);
        });
    }
}
