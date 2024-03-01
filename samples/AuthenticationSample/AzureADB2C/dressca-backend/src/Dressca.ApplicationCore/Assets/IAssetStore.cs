namespace Dressca.ApplicationCore.Assets;

/// <summary>
///  アセットのストア。
/// </summary>
public interface IAssetStore
{
    /// <summary>
    ///  指定したアセット情報のストリームを取得します。
    ///  見つからない場合は <see langword="null"/> を返します。
    /// </summary>
    /// <param name="asset">アセット情報。</param>
    /// <returns>ストリーム。見つからない場合は <see langword="null"/> 。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="asset"/> が <see langword="null"/> です。
    /// </exception>
    Stream? GetStream(Asset asset);
}
