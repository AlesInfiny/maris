using Dressca.ApplicationCore.Assets;
using Microsoft.Extensions.DependencyInjection;

namespace Dressca.Store.Assets.StaticFiles;

/// <summary>
///  <see cref="IServiceCollection" /> の拡張メソッドを提供します。
///  このプロジェクトの各機能を利用するためのスタートアップ処理が含まれています。
/// </summary>
public static class StaticFileAssetStoreServicesExtension
{
    /// <summary>
    ///  Dressca.Store.Assets.StaticFiles の各機能を利用するために必要な一連のスタートアップ処理を実行します。
    /// </summary>
    /// <param name="services">サービスコレクション。</param>
    /// <returns>構築済みのサービスコレクション。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="services"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public static IServiceCollection AddStaticFileAssetStore(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Stores
        services.AddTransient<IAssetStore, StaticFileAssetStore>();

        return services;
    }
}
