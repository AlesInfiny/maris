using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging.Testing;

namespace Microsoft.Extensions.Logging;

/// <summary>
///  <see cref="TestLoggerManager"/> で使用可能な <see cref="FakeLoggerProvider"/> を登録する処理を提供します。
/// </summary>
public static class FakeLoggingBuilderExtensions
{
    /// <summary>
    ///  <paramref name="builder"/> に <see cref="FakeLoggerProvider"/> を追加します。
    /// </summary>
    /// <param name="builder"><see cref="FakeLoggerProvider"/> 追加する <see cref="ILoggingBuilder"/> 。</param>
    /// <param name="loggerManager"><see cref="FakeLoggerProvider"/> を管理する <see cref="TestLoggerManager"/> 。</param>
    /// <returns><see cref="ILoggingBuilder"/> 。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="builder"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="loggerManager"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public static ILoggingBuilder AddFakeLogging(this ILoggingBuilder builder, TestLoggerManager loggerManager)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(loggerManager);
        builder.AddProvider(loggerManager.FakeLoggerProvider);
        return builder;
    }
}
