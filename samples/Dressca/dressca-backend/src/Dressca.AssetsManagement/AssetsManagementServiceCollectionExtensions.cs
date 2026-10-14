using Dressca.AssetsManagement.ApplicationCore;
using Dressca.AssetsManagement.ApplicationCore.ApplicationServices;
using Dressca.AssetsManagement.ApplicationCore.RepositoryInterfaces;
using Dressca.AssetsManagement.Infrastructures;
using Dressca.AssetsManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Dressca.AssetsManagement;

/// <summary>
///  <see cref="IServiceCollection" /> の拡張メソッドを提供します。
///  アセット管理コンテキストの各機能を利用するためのスタートアップ処理が含まれています。
/// </summary>
public static class AssetsManagementServiceCollectionExtensions
{
    private const string ConnectionStringName = nameof(AssetsManagementDbContext);

    /// <summary>
    ///  Dressca.AssetsManagement の各機能を利用するために必要な一連のスタートアップ処理を実行します。
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
    public static IServiceCollection AddDresscaAssetsManagement(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
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
        services.RemoveAll<DbContextOptions<AssetsManagementDbContext>>();
        services.AddDbContext<AssetsManagementDbContext>(option =>
        {
            option.UseSqlServer(connectionString);

            if (env.IsDevelopment())
            {
                option.EnableSensitiveDataLogging();
                option.EnableDetailedErrors();
            }
        });

        // Repositories
        services.AddTransient<IAssetRepository, EfAssetRepository>();

        // Stores
        services.AddTransient<IAssetStore, StaticFileAssetStore>();

        // Application Services
        services.AddTransient<AssetApplicationService>();

        return services;
    }
}
