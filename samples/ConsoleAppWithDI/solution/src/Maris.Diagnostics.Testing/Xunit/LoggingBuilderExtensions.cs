using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Maris.Diagnostics.Testing.Xunit;

/// <summary>
///  Xunit で使用可能な <see cref="ILogger"/> を登録する処理を提供します。
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    ///  Xunit で使用可能な <see cref="ILogger"/> を登録します。
    /// </summary>
    /// <param name="builder">ロギングプロバイダーを構成するための <see cref="ILoggingBuilder"/> オブジェクト。</param>
    /// <param name="testOutputHelper">テスト標準出力のためのヘルパーオブジェクト。</param>
    /// <returns>構成済みの <see cref="ILoggingBuilder"/> 。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="builder"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="testOutputHelper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public static ILoggingBuilder AddXunitLogger(this ILoggingBuilder builder, ITestOutputHelper testOutputHelper)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(testOutputHelper);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, XunitLoggerProvider>(
            provider => new XunitLoggerProvider(testOutputHelper)));
        return builder;
    }
}
