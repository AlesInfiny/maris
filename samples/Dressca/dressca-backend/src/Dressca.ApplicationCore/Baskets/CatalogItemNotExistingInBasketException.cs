using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かご内に指定のカタログアイテムが存在しないことを表す業務例外クラスです。
/// </summary>
public class CatalogItemNotExistingInBasketException : BusinessException
{
    private const string ErrorCode = "catalogItemIdDoesNotExistInBasket";

    /// <summary>
    ///  見つからなかったカタログアイテム Id を指定して
    ///  <see cref="CatalogItemNotExistingInBasketException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogItemIds">見つからなかったカタログアイテム Id 。</param>
    public CatalogItemNotExistingInBasketException(IEnumerable<long> catalogItemIds)
        : base(new BusinessError(ErrorCode, new ErrorMessageBuilder(string.Format(Messages.CatalogItemIdDoesNotExistInBasket, string.Join(",", catalogItemIds)), [..catalogItemIds.Select(id => id.ToString())])))
    {
    }
}
