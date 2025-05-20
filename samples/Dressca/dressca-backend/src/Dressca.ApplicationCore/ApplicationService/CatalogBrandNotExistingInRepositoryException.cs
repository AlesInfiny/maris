using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  リポジトリ内に指定のカタログブランドが存在しないことを表す業務例外クラスです。
/// </summary>
public class CatalogBrandNotExistingInRepositoryException : BusinessException
{
    private const string ErrorCode = "catalogBrandNotFound";

    /// <summary>
    ///  見つからなかったカタログブランド ID を指定して
    ///  <see cref="CatalogBrandNotExistingInRepositoryException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogBrandIds">見つからなかったカタログブランド ID 。</param>
    public CatalogBrandNotExistingInRepositoryException(IEnumerable<long> catalogBrandIds)
        : base(new BusinessError(ErrorCode, new ErrorMessageBuilder(string.Format(Messages.CatalogBrandIdDoesNotExist, string.Join(",", catalogBrandIds)), [..catalogBrandIds.Select(id => id.ToString())])))
    {
    }
}
