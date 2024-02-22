using System.Transactions;
using Dressca.ApplicationCore.ApplicationService;

namespace Dressca.UnitTests.ApplicationCore.ApplicationService;

public class TransactionScopeManagerTest
{
    [Fact]
    public void CreateTransactionScope_既定値を取得_IsolationLevelがReadCommittedに設定されている()
    {
        // Arrange & Act
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            // Assert
            Assert.NotNull(Transaction.Current);
            Assert.Equal(IsolationLevel.ReadCommitted, Transaction.Current.IsolationLevel);
            scope.Complete();
        }
    }

    [Fact]
    public void CreateTransactionScope_既定値を取得_TransactionScopeOptionがRequiresNewに設定されている()
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

    [Fact]
    public void CreateTransactionScope_トランザクション分離レベルを設定する_TransactionのIsolationLevelが設定した値になる()
    {
        // Arrange & Act
        using (var scope = TransactionScopeManager.CreateTransactionScope(
            transactionOptions: new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
        {
            // Assert
            Assert.NotNull(Transaction.Current);
            Assert.Equal(IsolationLevel.Snapshot, Transaction.Current.IsolationLevel);
            scope.Complete();
        }
    }

    [Fact]
    public void CreateTransactionScope_scopeOptionをRequiredに設定する_TransactionScopeOptionがRequiredに設定されている()
    {
        // Arrange & Act
        using (var scope1 = TransactionScopeManager.CreateTransactionScope())
        {
            var transaction1 = Transaction.Current;

            // Assert
            using (var scope2 = TransactionScopeManager.CreateTransactionScope(scopeOption: TransactionScopeOption.Required))
            {
                Assert.Equal(transaction1, Transaction.Current);
                scope2.Complete();
            }

            scope1.Complete();
        }
    }
}
