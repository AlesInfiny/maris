namespace Dressca.ApplicationCore.Assets;

/// <summary>
///  アセットリポジトリ。
/// </summary>
public interface IAssetRepository
{
    /// <summary>
    ///  指定したアセットコードの情報を取得します。
    ///  存在しない場合は <see langword="null"/> を返します。
    /// </summary>
    /// <param name="assetCode">アセットコード。</param>
    /// <returns>アセット情報。</returns>
    Asset? Find(string? assetCode);
}
