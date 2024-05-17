using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Runtime;

/// <summary>
/// 403 Forbidden を表す <see cref="StatusCodeResult"/> の派生クラスです。
/// </summary>
public class ForbiddenObjectResult : StatusCodeResult
{
    /// <summary>
    /// <see cref="ForbiddenObjectResult"/> クラスの新しいインタンスを初期化します。
    /// </summary>
    public ForbiddenObjectResult()
        : base(StatusCodes.Status403Forbidden)
    {
    }
}
