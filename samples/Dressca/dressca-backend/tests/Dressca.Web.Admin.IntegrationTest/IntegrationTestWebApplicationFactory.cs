using Dressca.AssetsManagement;
using Dressca.CatalogManagement;
using Dressca.Shopping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dressca.Web.Admin.IntegrationTest;

public class IntegrationTestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private const string DefaultTestEnvironment = "LocalIntegrationTest";

    private bool isInitialized = false;
    private string? connectionString;

    public async Task InitializeDatabaseAsync()
    {
        if (!this.isInitialized)
        {
            throw new InvalidOperationException("先に CreateClient メソッドを呼び出してください。");
        }

        string connectionString = string.Empty;
        if (string.IsNullOrEmpty(this.connectionString))
        {
            var configuration = this.Services.GetRequiredService<IConfiguration>();
            connectionString = configuration.GetConnectionString("ShoppingDbContext") ?? throw new InvalidOperationException("ShoppingDbContext の接続文字列が未設定");
        }
        else
        {
            connectionString = this.connectionString;
        }

        using var connection = new SqlConnection(connectionString);
        var command = connection.CreateCommand();
        command.CommandText =
            """
            DELETE FROM [dbo].[BasketItems];
            DELETE FROM [Baskets];
            DELETE FROM [OrderItemAssets];
            DELETE FROM [OrderItems];
            DELETE FROM [Orders];
            """;
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
        this.isInitialized = true;
    }

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
            services.AddDresscaShopping(config, context.HostingEnvironment);
            this.connectionString = config.GetConnectionString("ShoppingDbContext");
        });
    }
}
