using System.Diagnostics.CodeAnalysis;
using DresscaCMS.Announcement.ApplicationCore.ApplicationServices;
using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using DresscaCMS.Announcement.Infrastructures;
using DresscaCMS.Announcement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DresscaCMS.Announcement;

/// <summary>
///  お知らせメッセージに関連するサービスを追加する拡張メソッドを提供します。
/// </summary>
[SuppressMessage(
    category: "StyleCop.CSharp.ReadabilityRules",
    checkId: "SA1101:PrefixLocalCallsWithThis",
    Justification = "StyleCop bug. see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3954")]
public static class AnnouncementServiceCollectionExtensions
{
    private const string ConnectionStringName = nameof(AnnouncementDbContext);

    extension(IServiceCollection services)
    {

        /// <summary>
        ///  お知らせメッセージに関連するサービス一式を構成します。
        /// </summary>
        /// <param name="configuration">アプリケーション構成情報。</param>
        /// <param name="env">ホスト環境情報。</param>
        /// <returns>サービスコレクション。</returns>
        public IServiceCollection AddAnnouncementsServices(
            IConfiguration configuration,
            IHostEnvironment env)
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
                    // TransactionScope を使用するため、開発環境では再試行戦略を無効化します。
                    // 本番環境では接続の再試行のみを有効化します。
                    if (!env.IsDevelopment())
                    {
                        providerOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }
                });

                if (env.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            // リポジトリを登録します。
            services.AddScoped<IAnnouncementsRepository, EfAnnouncementsRepository>();

            // アプリケーションサービスを登録します。
            services.AddScoped<AnnouncementsApplicationService>();

            return services;
        }
    }
}
