using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  リポジトリ内に指定のカタログブランドが存在しないことを表す業務例外クラスです。
/// </summary>
public class CatalogBrandNotExistingInRepositoryException : BusinessException
{
    private const string ErrorCode = "CatalogBrandNotExistingInRepository";

    /// <summary>
    ///  見つからなかったカタログブランド Id を指定して
    ///  <see cref="CatalogItemNotExistingInRepositoryException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogBrandIds">見つからなかったカタログブランド Id 。</param>
    public CatalogBrandNotExistingInRepositoryException(IEnumerable<long> catalogBrandIds)
        : base(new BusinessError(ErrorCode, string.Format(Messages.CatalogBrandIdDoesNotExist, string.Join(",", catalogBrandIds))))
    {
    }
}
