using Microsoft.EntityFrameworkCore;

namespace Dressca.IntegrationTest;

public static class DatabaseHelper
{
    public static void ClearTransactionTable(DbContext dbContext)
    {
        dbContext.Database.EnsureCreated();
        dbContext.Database.ExecuteSql($"DELETE FROM [BasketItems];");
        dbContext.Database.ExecuteSql($"DELETE FROM [Baskets];");
        dbContext.Database.ExecuteSql($"DELETE FROM [OrderItemAssets];");
        dbContext.Database.ExecuteSql($"DELETE FROM [OrderItems];");
        dbContext.Database.ExecuteSql($"DELETE FROM [Orders];");
    }
}
