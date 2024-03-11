using Dressca.EfInfrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dressca.IntegrationTest;

public class IntegrationTestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private string? connectionString;

    public async Task InitializeDatabaseAsync()
    {
        var configuration = this.Services.GetRequiredService<IConfiguration>();
        this.connectionString = configuration.GetConnectionString("DresscaDbContext");
        using var connection = new SqlConnection(this.connectionString);
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

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? Environments.Development;

        if (!env.Equals(Environments.Development, StringComparison.OrdinalIgnoreCase))
        {
            builder.ConfigureServices(services =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .Build();

                services.AddDresscaEfInfrastructure(config);
            });
        }
    }
}
