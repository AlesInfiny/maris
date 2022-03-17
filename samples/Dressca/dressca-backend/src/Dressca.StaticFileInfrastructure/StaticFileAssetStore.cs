using Dressca.ApplicationCore.Assets;
using Dressca.StaticFileInfrastructure.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.StaticFileInfrastructure;

/// <summary>
///  静的ファイルを取り扱うアセットのストアの実装です。
/// </summary>
internal class StaticFileAssetStore : IAssetStore
{
    private readonly ILogger<StaticFileAssetStore> logger;
    private readonly string basePath;

    /// <summary>
    ///  <see cref="StaticFileAssetStore"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー。</param>
    /// <param name="basePath">Images ディレクトリが存在するパス。未指定時はこのクラスのアセンブリが存在するディレクトリ。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="logger"/> が <see langword="null"/> です。
    /// </exception>
    public StaticFileAssetStore(ILogger<StaticFileAssetStore> logger, string? basePath = null)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.basePath = string.IsNullOrEmpty(basePath)
            ? (Path.GetDirectoryName(typeof(StaticFileAssetStore).Assembly.Location) ?? string.Empty)
            : basePath;
    }

    /// <inheritdoc/>
    public Stream? GetStream(Asset asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        this.logger.LogDebug(StaticFileInfrastructureMessages.StaticFileAssetStore_GetStreamStart, asset.AssetCode);
        var filePath = this.GetFilePath(asset);
        if (File.Exists(filePath))
        {
            this.logger.LogDebug(StaticFileInfrastructureMessages.StaticFileAssetStore_GetStreamEnd, asset.AssetCode, filePath);
            return new FileStream(filePath, FileMode.Open);
        }
        else
        {
            this.logger.LogInformation(StaticFileInfrastructureMessages.FileNotFound, asset.AssetCode, filePath);
            return null;
        }
    }

    private string GetFilePath(Asset asset)
    {
        const string imagePath = "Images";
        return Path.Combine(this.basePath, imagePath, asset.AssetCode);
    }
}
