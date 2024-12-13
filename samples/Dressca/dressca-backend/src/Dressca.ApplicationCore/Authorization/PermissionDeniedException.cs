using System.Runtime.CompilerServices;
using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.Authorization;

/// <summary>
///  ユーザーにユースケースの実行権限がないことを表す業務例外クラスです。
/// </summary>
public class PermissionDeniedException : BusinessException
{
    private const string ErrorCode = "PermissionDenied";

    /// <summary>
    ///  実行を試みた操作を指定して
    ///  <see cref="PermissionDeniedException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="operationName">実行を試みた操作の名称。</param>
    public PermissionDeniedException([CallerMemberName] string operationName = "")
        : base(new BusinessError(ErrorCode, string.Format(Messages.PermissionDenied, operationName)))
    {
    }
}
