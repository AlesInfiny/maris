using Dressca.EfInfrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dressca.IntegrationTest;

public class TransactionalTestDatabaseFixture
{
    private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=Dressca.Eshop.IT;Integrated Security=True;";

    public TransactionalTestDatabaseFixture()
    {
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

    internal DresscaDbContext CreateContext()
        => new DresscaDbContext(
            new DbContextOptionsBuilder<DresscaDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

    [CollectionDefinition("TransactionalTests")]
    public class TransactionalTestsCollection : ICollectionFixture<TransactionalTestDatabaseFixture>
    {
    }
}
