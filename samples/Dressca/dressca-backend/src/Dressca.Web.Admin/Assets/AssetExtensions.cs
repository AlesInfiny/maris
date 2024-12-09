using Dressca.ApplicationCore.Assets;
using Dressca.Web.Admin.Resources;

namespace Dressca.Web.Admin.Assets;

/// <summary>
///  <see cref="Asset"/> の拡張メソッドを提供します。
/// </summary>
public static class AssetExtensions
{
    /// <summary>
    ///  アセットタイプから Content-Type に変換します。
    /// </summary>
    /// <param name="asset">アセット。</param>
    /// <returns>Content-Type の名称。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="asset"/> が <see langword="null"/> です。
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///  指定したアセットのアセットタイプは Content-Type に変換できません。
    /// </exception>
    public static string GetContentType(this Asset asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        return asset.AssetType switch
        {
            AssetTypes.Png => ContentTypes.ImagePng,
            _ => throw new NotSupportedException(string.Format(Messages.CannotConvertAssetTypeToContentType, asset.AssetType)),
        };
    }

    private static class ContentTypes
    {
        internal const string ImagePng = "image/png";
    }
}
