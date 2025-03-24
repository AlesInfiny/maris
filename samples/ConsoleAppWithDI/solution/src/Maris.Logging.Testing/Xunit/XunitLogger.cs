using Microsoft.Extensions.Logging;
using Xunit;

namespace Maris.Logging.Testing.Xunit;

/// <summary>
///  Xunit で使用可能な <see cref="ILogger"/> の具象クラスです。
/// </summary>
internal class XunitLogger : ILogger
{
    private readonly ITestOutputHelper testOutputHelper;
    private readonly string categoryName;

    /// <summary>
    ///  <see cref="ITestOutputHelper"/> オブジェクトとログのカテゴリ名を指定して
    ///  <see cref="XunitLogger"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="testOutputHelper"><see cref="ITestOutputHelper"/> オブジェクト。</param>
    /// <param name="categoryName">カテゴリ名。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="testOutputHelper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public XunitLogger(ITestOutputHelper testOutputHelper, string categoryName)
    {
        this.testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
        this.categoryName = categoryName;
    }

    /// <inheritdoc/>
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull => NoopDisposable.Instance;

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        this.testOutputHelper.WriteLine($"{logLevel}: {this.categoryName}[{eventId}] {formatter(state, exception)}");
        if (exception != null)
        {
            this.testOutputHelper.WriteLine(exception.ToString());
        }
    }

    private class NoopDisposable : IDisposable
    {
        internal static NoopDisposable Instance { get; } = new();

        public void Dispose()
        {
        }
    }
}
