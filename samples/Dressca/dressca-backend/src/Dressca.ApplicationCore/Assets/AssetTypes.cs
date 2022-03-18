namespace Dressca.ApplicationCore.Assets;

/// <summary>
///  アセットタイプを管理します。
/// </summary>
public static class AssetTypes
{
    /// <summary>
    ///  PNG 形式のアセットであることを表します。
    /// </summary>
    public const string Png = "png";

    private static readonly HashSet<string> SupportedAssetTypes = new() { Png };

    /// <summary>
    ///  指定したアセットタイプがサポートされているかどうか示す値を取得します。
    /// </summary>
    /// <param name="assetType">アセットタイプ。</param>
    /// <returns>サポートされている場合は <see langword="true"/> 、サポートされていない場合は <see langword="false"/> 。</returns>
    public static bool IsSupportedAssetType(string? assetType)
    {
        if (string.IsNullOrWhiteSpace(assetType))
        {
            return false;
        }

        return SupportedAssetTypes.Contains(assetType);
    }
}
