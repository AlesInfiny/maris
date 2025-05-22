using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.Assets;

/// <summary>
///  アセットが存在しないことを表す例外クラスです。
/// </summary>
public class AssetNotFoundException : BusinessException
{
    private const string ExceptionId = "assetNotFound";

    /// <summary>
    ///  見つからなかったアセットコードを指定して
    ///  <see cref="AssetNotFoundException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="assetCode">見つからなかった買い物かご Id 。</param>
    public AssetNotFoundException(string assetCode)
        : base(new BusinessError(ExceptionId, new ErrorMessage(Messages.AssetNotFound, assetCode)))
    {
    }
}
