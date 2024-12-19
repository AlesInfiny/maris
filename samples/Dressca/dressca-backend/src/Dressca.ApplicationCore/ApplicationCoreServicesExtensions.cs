using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Authorization;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dressca.ApplicationCore;

/// <summary>
///  <see cref="IServiceCollection" /> の拡張メソッドを提供します。
///  このプロジェクトの各機能を利用するためのスタートアップ処理が含まれています。
/// </summary>
public static class ApplicationCoreServicesExtensions
{
    /// <summary>
    ///  Dressca.ApplicationCore の各機能を利用するために必要な一連のスタートアップ処理を実行します。
    /// </summary>
    /// <param name="services">サービスコレクション。</param>
    /// <returns>構築済みのサービスコレクション。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="services"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public static IServiceCollection AddDresscaApplicationCore(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Domain Services
        services.AddTransient<ICatalogDomainService, CatalogDomainService>();

        // Factory
        services.AddTransient<IOrderFactory, OrderFactory>();

        // Application Services
        services.AddTransient<ShoppingApplicationService>();
        services.AddTransient<OrderApplicationService>();
        services.AddTransient<CatalogApplicationService>();
        services.AddTransient<AssetApplicationService>();

        // Default
        // 該当する型のサービスが未登録の場合に、デフォルトで使用するサービスを追加します。
        services.TryAddTransient<IUserStore, NotConnectedUserStore>();

        return services;
    }
}
