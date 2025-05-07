using Dressca.EfInfrastructure;
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
            connectionString = configuration.GetConnectionString("DresscaDbContext") ?? throw new InvalidOperationException("DresscaDbContext の接続文字列が未設定");
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
        var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? Environments.Development;

        if (!env.Equals(Environments.Development, StringComparison.OrdinalIgnoreCase))
        {
            builder.ConfigureServices((context, services) =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .Build();

                services.AddDresscaEfInfrastructure(config, context.HostingEnvironment);
                this.connectionString = config.GetConnectionString("DresscaDbContext");
            });
        }
    }
}
