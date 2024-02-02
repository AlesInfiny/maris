using Maris.Diagnostics.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///  Xunit で使用可能な <see cref="ILogger"/> と、
///  <see cref="FakeLogger"/> を登録する処理を提供します。
/// </summary>
public static class TestLoggerServiceCollectionExtensions
{
    /// <summary>
    ///  テストで使用するログ出力の構成をします。
    /// </summary>
    /// <param name="services">ログ出力の構成を行う <see cref="IServiceCollection"/> 。</param>
    /// <param name="loggerManager">Xunit で利用できる <see cref="ILogger"/> を管理する <see cref="TestLoggerManager"/> 。</param>
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
            builder.AddFakeLogging();
        });

        return services;
    }
}
