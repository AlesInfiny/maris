using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文のチェックアウト処理開始時に買い物かごが存在しないことを表す業務例外クラスです。
/// </summary>
public class NullBasketOnCheckoutException : BusinessException
{
    private const string ErrorCode = "basketIsNullOnCheckout";

    /// <summary>
    ///  <see cref="NullBasketOnCheckoutException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public NullBasketOnCheckoutException()
        : base(new BusinessError(ErrorCode, Messages.BasketIsNullOnCheckout))
    {
    }
}
