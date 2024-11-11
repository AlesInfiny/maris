using Maris.Logging.Testing.Xunit;

namespace Microsoft.Extensions.Logging;

/// <summary>
///  Xunit で使用可能な <see cref="ILogger"/> を登録する処理を提供します。
/// </summary>
public static class XunitLoggingBuilderExtensions
{
    /// <summary>
    ///  <paramref name="builder"/> に Xunit の <see cref="ILoggerProvider"/> を追加します。
    /// </summary>
    /// <param name="builder">Xunit の <see cref="ILoggerProvider"/> 追加する <see cref="ILoggingBuilder"/> 。</param>
    /// <param name="loggerManager">Xunit で利用できる <see cref="ILogger"/> を管理する <see cref="TestLoggerManager"/> 。</param>
    /// <returns><see cref="ILoggingBuilder"/> 。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="builder"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="loggerManager"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public static ILoggingBuilder AddXunitLogging(this ILoggingBuilder builder, TestLoggerManager loggerManager)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(loggerManager);
        builder.AddProvider(loggerManager.XunitLoggerProvider);
        return builder;
    }
}
