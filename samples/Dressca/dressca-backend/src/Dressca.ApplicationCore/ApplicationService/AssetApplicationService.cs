﻿using Dressca.ApplicationCore.Assets;
using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  アセット情報に関するビジネスユースケースを実現するアプリケーションサービスです。
/// </summary>
public class AssetApplicationService
{
    private readonly ILogger<AssetApplicationService> logger;
    private readonly IAssetRepository repository;
    private readonly IAssetStore store;

    /// <summary>
    ///  <see cref="AssetApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="repository">アセットリポジトリ。</param>
    /// <param name="store">アセットストア。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="repository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="store"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public AssetApplicationService(
        IAssetRepository repository,
        IAssetStore store,
        ILogger<AssetApplicationService> logger)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  指定したアセットコードのアセット情報とストリームを取得します。
    /// </summary>
    /// <param name="assetCode">アセットコード。</param>
    /// <returns>アセット情報とそのストリーム。</returns>
    /// <exception cref="AssetNotFoundException">
    ///  アセット情報が見つかりませんでした。
    /// </exception>
    public AssetStreamInfo GetAssetStreamInfo(string assetCode)
    {
        this.logger.LogDebug(Events.DebugEvent, Messages.AssetApplicationService_GetAssetStreamInfoStart, assetCode);

        Asset? asset;
        Stream? stream;

        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            asset = this.repository.Find(assetCode);
            if (asset == null)
            {
                throw new AssetNotFoundException(assetCode);
            }

            stream = this.store.GetStream(asset);
            if (stream == null)
            {
                throw new AssetNotFoundException(assetCode);
            }

            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, Messages.AssetApplicationService_GetAssetStreamInfoEnd, assetCode);
        return new(asset, stream);
    }

    /// <summary>
    ///  アセット情報とそのストリームの情報。
    /// </summary>
    /// <param name="Asset">アセット情報。</param>
    /// <param name="Stream">ストリーム。</param>
    public record AssetStreamInfo(Asset Asset, Stream Stream);
}