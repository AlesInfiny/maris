using Dressca.ApplicationCore.Resources;
using Maris.Core;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文のチェックアウト処理開始時に買い物かごが空であることを表す業務例外クラスです。
/// </summary>
public class EmptyBasketOnCheckoutException : BusinessException
{
    private const string ExceptionId = "basketIsEmptyOnCheckout";

    /// <summary>
    ///  <see cref="EmptyBasketOnCheckoutException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public EmptyBasketOnCheckoutException()
        : base(new BusinessError(ExceptionId, new ErrorMessage(Messages.BasketIsEmptyOnCheckout)))
    {
    }
}
