using Dressca.Authorization;
using Dressca.CatalogManagement.ApplicationCore;
using Dressca.CatalogManagement.ApplicationCore.ApplicationServices;
using Dressca.CatalogManagement.ApplicationCore.RepositoryInterfaces;
using Dressca.CatalogManagement.Infrastructures;
using Dressca.CatalogManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Dressca.CatalogManagement;

/// <summary>
///  <see cref="IServiceCollection" /> の拡張メソッドを提供します。
///  カタログ管理コンテキストの各機能を利用するためのスタートアップ処理が含まれています。
/// </summary>
public static class CatalogManagementServiceCollectionExtensions
{
    private const string ConnectionStringName = nameof(CatalogManagementDbContext);

    /// <summary>
    ///  Dressca.CatalogManagement の各機能を利用するために必要な一連のスタートアップ処理を実行します。
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
    ///   <item><paramref name="configuration"/> に接続文字列が定義されていません。</item>
    ///  </list>
    /// </exception>
    public static IServiceCollection AddDresscaCatalogManagement(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
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
        services.RemoveAll<DbContextOptions<CatalogManagementDbContext>>();
        services.AddDbContext<CatalogManagementDbContext>(option =>
        {
            option.UseSqlServer(connectionString);

            if (env.IsDevelopment())
            {
                option.EnableSensitiveDataLogging();
                option.EnableDetailedErrors();
            }
        });

        // Repositories
        services.AddTransient<ICatalogBrandRepository, EfCatalogBrandRepository>();
        services.AddTransient<ICatalogCategoryRepository, EfCatalogCategoryRepository>();
        services.AddTransient<ICatalogRepository, EfCatalogRepository>();

        // Domain Services
        services.AddTransient<ICatalogDomainService, CatalogDomainService>();

        // Application Services
        services.AddTransient<CatalogApplicationService>();

        // Default
        // 該当する型のサービスが未登録の場合に、デフォルトで使用するサービスを追加します。
        services.TryAddTransient<IUserStore, NotConnectedUserStore>();

        return services;
    }
}
