using System.Transactions;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  <see cref="TransactionScope"/> に関する共通処理を提供します。
/// </summary>
internal static class TransactionScopeManager
{
    private static readonly TransactionOptions DefaultTransactionOptions = new() { IsolationLevel = IsolationLevel.ReadCommitted };

    /// <summary>
    ///  <see cref="TransactionScope"/> のインスタンスを生成します。
    /// </summary>
    /// <param name="scopeOption">トランザクションスコープの追加情報。</param>
    /// <param name="transactionOptions">トランザクション動作を指定する追加情報。</param>
    /// <param name="asyncFlowOption">トランザクションが非同期呼び出し間で流れるかどうかを示す値。</param>
    /// <returns><see cref="TransactionScope"/> のインスタンス。</returns>
    public static TransactionScope CreateTransactionScope(
        TransactionScopeOption scopeOption = TransactionScopeOption.RequiresNew,
        TransactionOptions? transactionOptions = null,
        TransactionScopeAsyncFlowOption asyncFlowOption = TransactionScopeAsyncFlowOption.Enabled)
    {
        if (transactionOptions == null)
        {
            transactionOptions = DefaultTransactionOptions;
        }

        return new TransactionScope(scopeOption, (TransactionOptions)transactionOptions, asyncFlowOption);
    }
}
