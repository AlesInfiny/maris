using Dressca.ApplicationCore.Assets;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Dressca.EfInfrastructure.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace Dressca.EfInfrastructure;

/// <summary>
///  <see cref="IServiceCollection" /> の拡張メソッドを提供します。
///  このプロジェクトの各機能を利用するためのスタートアップ処理が含まれています。
/// </summary>
public static class EfInfrastructureServicesExtension
{
    private const string ConnectionStringName = nameof(DresscaDbContext);

    /// <summary>
    ///  Dressca.EfInfrastructure の各機能を利用するために必要な一連のスタートアップ処理を実行します。
    /// </summary>
    /// <param name="services">サービスコレクション。</param>
    /// <param name="configuration">構成設定。</param>
    /// <param name="env">ホスト環境。</param>
    /// <returns>構築済みのサービスコレクション。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="services"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="configuration"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="env"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="configuration"/>　に接続文字列が定義されていません。</item>
    ///  </list>
    /// </exception>
    public static IServiceCollection AddDresscaEfInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(env);

        // Connection Strings
        var connectionString = configuration.GetConnectionString(ConnectionStringName);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                message: string.Format(Messages.NotFoundConnectionString, ConnectionStringName),
                paramName: nameof(configuration));
        }

        // DbContext
        // 結合テスト実行時に設定を差し替えるため、サービス登録をいったん削除してから登録
        services.RemoveAll<DbContextOptions<DresscaDbContext>>();
        services.AddDbContext<DresscaDbContext>(option =>
        {
            option.UseSqlServer(connectionString);

            if (env.IsDevelopment())
            {
                option.EnableSensitiveDataLogging();
                option.EnableDetailedErrors();
            }
        });

        // Repositories
        services.AddTransient<IBasketRepository, EfBasketRepository>();
        services.AddTransient<ICatalogBrandRepository, EfCatalogBrandRepository>();
        services.AddTransient<ICatalogCategoryRepository, EfCatalogCategoryRepository>();
        services.AddTransient<ICatalogRepository, EfCatalogRepository>();
        services.AddTransient<IOrderRepository, EfOrderRepository>();
        services.AddTransient<IAssetRepository, EfAssetRepository>();

        return services;
    }
}
