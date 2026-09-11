using Dressca.ApplicationCore.Resources;
using Maris.Core;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  リポジトリ内に指定の陳列品が存在しないことを表す業務例外クラスです。
/// </summary>
public class DisplayItemNotExistingInRepositoryException : BusinessException
{
    private const string ExceptionId = "displayIdNotFound";

    /// <summary>
    ///  見つからなかった陳列品 Id を指定して
    ///  <see cref="DisplayItemNotExistingInRepositoryException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="displayItemIds">見つからなかった陳列品 Id 。</param>
    public DisplayItemNotExistingInRepositoryException(IEnumerable<Guid> displayItemIds)
        : base(new BusinessError(ExceptionId, new ErrorMessage(Messages.DisplayItemIdDoesNotExistInRepository, string.Join(",", displayItemIds))))
    {
    }
}
