using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かご内に指定のカタログアイテムが存在しないことを表す業務例外クラスです。
/// </summary>
public class CatalogItemNotExistingInBasketException : BusinessException
{
    private const string ExceptionId = "catalogItemIdDoesNotExistInBasket";

    /// <summary>
    ///  見つからなかったカタログアイテム Id を指定して
    ///  <see cref="CatalogItemNotExistingInBasketException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogItemIds">見つからなかったカタログアイテム Id 。</param>
    public CatalogItemNotExistingInBasketException(IEnumerable<long> catalogItemIds)
        : base(new BusinessError(ExceptionId, new ErrorMessage(Messages.CatalogItemIdDoesNotExistInBasket, string.Join(",", catalogItemIds))))
    {
    }
}
