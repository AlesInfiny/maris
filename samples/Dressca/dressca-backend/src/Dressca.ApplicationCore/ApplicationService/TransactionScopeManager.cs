using System.Transactions;

namespace Dressca.ApplicationCore.ApplicationService;

public static class TransactionScopeManager
{
    private static readonly TransactionOptions defaultTransactionOptions = new() { IsolationLevel = IsolationLevel.ReadCommitted };

    public static TransactionScope CreateTransactionScope(
        TransactionScopeOption scopeOption = TransactionScopeOption.RequiresNew,
        TransactionOptions? transactionOptions = null,
        TransactionScopeAsyncFlowOption asyncFlowOption = TransactionScopeAsyncFlowOption.Enabled)
    {
        if (transactionOptions == null)
        {
            transactionOptions = defaultTransactionOptions;
        }

        return new TransactionScope(scopeOption, (TransactionOptions)transactionOptions, asyncFlowOption);
    }
}
