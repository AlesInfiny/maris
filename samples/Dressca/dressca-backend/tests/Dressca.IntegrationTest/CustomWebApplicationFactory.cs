using System.Data.Common;
using Dressca.EfInfrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dressca.IntegrationTest;
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<DresscaDbContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            services.AddDbContext<DresscaDbContext>( options =>
            {
                options.UseNpgsql("Host=localhost;Database=dressca_eshop;Username=postgres;Password=postgres");
            });

            var dbContext = services.BuildServiceProvider().GetRequiredService<DresscaDbContext>();
            dbContext.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development");
    }
}
