using System.Diagnostics.CodeAnalysis;
using DresscaCMS.Authentication.Infrastructures;
using DresscaCMS.Authentication.Infrastructures.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DresscaCMS.Authentication;

/// <summary>
///  認証に関連するサービスを追加する拡張メソッドを提供します。
/// </summary>
[SuppressMessage(
    category: "StyleCop.CSharp.ReadabilityRules",
    checkId: "SA1101:PrefixLocalCallsWithThis",
    Justification = "StyleCop bug. see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3954")]
public static class AuthenticationServiceCollectionExtensions
{
    private const string ConnectionStringName = nameof(AuthenticationDbContext);

    extension(IServiceCollection services)
    {
        /// <summary>
        ///  認証に関連するサービス一式を構成します。
        /// </summary>
        /// <param name="configuration">アプリケーション構成情報。</param>
        /// <param name="env">ホスト環境情報。</param>
        /// <returns>サービスコレクション。</returns>
        public IServiceCollection AddAuthenticationServices(
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
                    message: $"接続文字列 '{ConnectionStringName}' が見つかりません。",
                    paramName: nameof(configuration));
            }

            // DbContext を登録します。
            services.AddDbContextFactory<AuthenticationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, providerOptions =>
                {
                    // TransactionScope を使用する場合に備えて、開発環境では再試行戦略を無効化します。
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

            // Cookie ベースの認証を設定します。
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

            // ASP.NET Core Identity を登録します。
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                if (!env.IsDevelopment())
                {
                    options.SignIn.RequireConfirmedAccount = true;
                }

                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
