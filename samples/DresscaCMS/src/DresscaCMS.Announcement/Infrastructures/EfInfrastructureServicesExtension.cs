using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using DresscaCMS.Announcement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DresscaCMS.Announcement.Infrastructures;

/// <summary>
///  お知らせメッセージに関する Entity Framework Core 関連サービスを登録する拡張メソッドを提供します。
/// </summary>
public static class EfInfrastructureServicesExtension
{
    private const string ConnectionStringName = nameof(AnnouncementDbContext);

    /// <summary>
    ///  お知らせメッセージに関するEntity Framework Core 関連サービスを登録します。
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
    ///   <paramref name="configuration"/> に接続文字列が定義されていません。
    /// </exception>
    public static IServiceCollection AddAnnouncementsEfInfrastructure(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(env);

        // 接続文字列を取得します。
        var connectionString = configuration.GetConnectionString(ConnectionStringName);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                message: string.Format(Messages.NotFoundConnectionString, ConnectionStringName),
                paramName: nameof(configuration));
        }

        // DbContextFactory を登録します。
        services.AddDbContextFactory<AnnouncementDbContext>(options =>
        {
            options.UseSqlServer(connectionString, providerOptions =>
            {
                providerOptions.EnableRetryOnFailure();
            });

            if (env.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // リポジトリを登録
        services.AddScoped<IAnnouncementsRepository, EfAnnouncementsRepository>();

        return services;
    }
}
