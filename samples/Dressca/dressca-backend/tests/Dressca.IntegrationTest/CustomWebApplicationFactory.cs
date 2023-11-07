using System.Data.Common;
using Dressca.EfInfrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dressca.IntegrationTest;
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //var dbContextDescriptor = services.SingleOrDefault(
            //    d => d.ServiceType ==
            //        typeof(DbContextOptions<DresscaDbContext>));

            //services.Remove(dbContextDescriptor);

            //var dbConnectionDescriptor = services.SingleOrDefault(
            //    d => d.ServiceType ==
            //        typeof(DbConnection));

            //services.Remove(dbConnectionDescriptor);

            var fileDirectory = Environment.GetEnvironmentVariable("appSettingsFileDirectory");
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileDirectory, optional: true, reloadOnChange: true)
                .Build();


            services.AddDresscaEfInfrastructure(config);

            //var dbContext = services.BuildServiceProvider().GetRequiredService<DbContext>();
            //dbContext.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development");

    }
}
