using Dressca.EfInfrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Dressca.IntegrationTest;
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            // CIのビルドマシン上で実行する場合はappsettingsを読み込む
            var env = context.HostingEnvironment.EnvironmentName;
            if (env == "IntegrationTest")
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .Build();

                services.AddDresscaEfInfrastructure(config);
            }
        });

        //builder.UseEnvironment("Development");

        //var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //if (env == "Development")
        //{
        //    builder.ConfigureServices(services =>
        //    {
        //        var config = new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
        //            .Build();

        //        services.AddDresscaEfInfrastructure(config);
        //    });
        //}
    }
}
