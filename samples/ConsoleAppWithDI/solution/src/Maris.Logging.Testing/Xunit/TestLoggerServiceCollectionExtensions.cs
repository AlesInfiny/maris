using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///  <see cref="TestLoggerManager"/> で管理できる <see cref="ILogger"/> を登録する処理を提供します。
/// </summary>
public static class TestLoggerServiceCollectionExtensions
{
    /// <summary>
    ///  テストで使用するログ出力の構成をします。
    /// </summary>
    /// <param name="services">ログ出力の構成を行う <see cref="IServiceCollection"/> 。</param>
    /// <param name="loggerManager">Xunit のテストクラスで使用する <see cref="TestLoggerManager"/> 。</param>
    /// <returns><see cref="IServiceCollection"/> 。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="services"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="loggerManager"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public static IServiceCollection AddTestLogging(this IServiceCollection services, TestLoggerManager loggerManager)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(loggerManager);
        services.AddLogging(builder =>
        {
            builder.AddXunitLogging(loggerManager);
            builder.AddFakeLogging(loggerManager);
        });

        return services;
    }
}
