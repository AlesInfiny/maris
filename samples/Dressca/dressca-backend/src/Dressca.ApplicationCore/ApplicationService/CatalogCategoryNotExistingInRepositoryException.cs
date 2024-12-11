using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  リポジトリ内に指定のカタログブランドが存在しないことを表す業務例外クラスです。
/// </summary>
public class CatalogCategoryNotExistingInRepositoryException : BusinessException
{
    private const string ErrorCode = "CatalogCategoryNotExistingInRepository";

    /// <summary>
    ///  見つからなかったカタログカテゴリ Id を指定して
    ///  <see cref="CatalogItemNotExistingInRepositoryException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogCategoryIds">見つからなかったカタログカテゴリ Id 。</param>
    public CatalogCategoryNotExistingInRepositoryException(IEnumerable<long> catalogCategoryIds)
        : base(new BusinessError(ErrorCode, string.Format(Messages.CatalogCategoryIdDoesNotExist, string.Join(",", catalogCategoryIds))))
    {
    }
}
