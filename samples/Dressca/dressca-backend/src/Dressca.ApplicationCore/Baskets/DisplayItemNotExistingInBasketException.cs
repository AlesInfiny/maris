using Dressca.ApplicationCore.Resources;
using Maris.Core;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かご内に指定の陳列品が存在しないことを表す業務例外クラスです。
/// </summary>
public class DisplayItemNotExistingInBasketException : BusinessException
{
    private const string ExceptionId = "displayItemIdDoesNotExistInBasket";

    /// <summary>
    ///  見つからなかった陳列品 Id を指定して
    ///  <see cref="DisplayItemNotExistingInBasketException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="displayItemIds">見つからなかった陳列品 Id 。</param>
    public DisplayItemNotExistingInBasketException(IEnumerable<Guid> displayItemIds)
        : base(new BusinessError(ExceptionId, new ErrorMessage(Messages.DisplayItemIdDoesNotExistInBasket, string.Join(",", displayItemIds))))
    {
    }
}
