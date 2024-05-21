using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Runtime;

/// <summary>
/// 403 Forbidden を表す <see cref="StatusCodeResult"/> の派生クラスです。
/// </summary>
public class ForbiddenCodeResult : StatusCodeResult
{
    /// <summary>
    /// <see cref="ForbiddenCodeResult"/> クラスの新しいインタンスを初期化します。
    /// </summary>
    public ForbiddenCodeResult()
        : base(StatusCodes.Status403Forbidden)
    {
    }
}
