using Dressca.EfInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Dressca.IntegrationTest;

public class TransactionalTestDatabaseFixture
{
    private string connectionString;

    public TransactionalTestDatabaseFixture()
    {
        this.connectionString = this.GetConnectionString();

        using (var context = this.CreateContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            this.Cleanup();
        }
    }

    public void Cleanup()
    {
        using var context = this.CreateContext();
        context.Orders.RemoveRange(context.Orders);
        context.SaveChanges();
    }

    public string GetConnectionString()
    {
        var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? Environments.Development;

        if (!env.Equals(Environments.Development, StringComparison.OrdinalIgnoreCase))
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .Build();
            return config.GetConnectionString("DresscaTransactionalTestDbContext") ?? throw new ArgumentNullException("DresscaTransactionalDbContext");
        }
        else
        {
            const string localConnectionString = @"Server=(localdb)\mssqllocaldb;Database=Dressca.Eshop.IT;Integrated Security=True;";
            return localConnectionString;
        }
    }

    internal DresscaDbContext CreateContext()
            => new DresscaDbContext(
                new DbContextOptionsBuilder<DresscaDbContext>()
                    .UseSqlServer(this.connectionString)
                    .Options);

    [CollectionDefinition("TransactionalTests")]
    public class TransactionalTestsCollection : ICollectionFixture<TransactionalTestDatabaseFixture>
    {
    }
}
