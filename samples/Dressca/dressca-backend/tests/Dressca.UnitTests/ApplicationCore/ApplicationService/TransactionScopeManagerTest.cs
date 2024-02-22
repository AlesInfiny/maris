using System.Transactions;
using Dressca.ApplicationCore.ApplicationService;

namespace Dressca.UnitTests.ApplicationCore.ApplicationService;

public class TransactionScopeManagerTest
{
    [Fact]
    public void CreateTransactionScope_インスタンスが取得できる()
    {
        // Arrange & Act
        var scope = TransactionScopeManager.CreateTransactionScope();

        // Assert
        Assert.NotNull(scope);
    }

    [Fact]
    public void CreateTransactionScope_IsolationLevelがReadCommittedに設定されている()
    {
        // Arrange & Act
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            // Assert
            Assert.Equal(IsolationLevel.ReadCommitted, Transaction.Current.IsolationLevel);
            scope.Complete();
        }
    }

    [Fact]
    public void CreateTransactionScope_TransactionScopeOptionがRequiresNewに設定されている()
    {
        // Arrange & Act
        using (var scope1 = TransactionScopeManager.CreateTransactionScope())
        {
            var transaction1 = Transaction.Current;

            // Assert
            using (var scope2 = TransactionScopeManager.CreateTransactionScope())
            {
                Assert.NotEqual(transaction1, Transaction.Current);
                scope2.Complete();
            }

            scope1.Complete();
        }
    }
}
