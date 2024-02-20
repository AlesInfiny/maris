﻿using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  リポジトリ内に指定のカタログアイテムが存在しないことを表す業務例外クラスです。
/// </summary>
public class CatalogItemNotExistingInRepositoryException : BusinessException
{
    private const string ErrorCode = "CatalogItemNotExistingInRepository";

    /// <summary>
    ///  見つからなかったカタログアイテム Id を指定して
    ///  <see cref="CatalogItemNotExistingInRepositoryException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogItemIds">見つからなかったカタログアイテム Id 。</param>
    public CatalogItemNotExistingInRepositoryException(IEnumerable<long> catalogItemIds)
        : base(new BusinessError(ErrorCode, string.Format(Messages.CatalogItemIdDoesNotExistInBasket, string.Join(",", catalogItemIds))))
    {
    }
}
