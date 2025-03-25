using Microsoft.Extensions.Logging;
using Xunit;

namespace Maris.Logging.Testing.Xunit;

/// <summary>
/// <see cref="Microsoft.Extensions.Logging"/> と xUnit のテスト出力を統合するための <see cref="ILoggerProvider"/> です.
/// </summary>
internal class XunitLoggerProvider : ILoggerProvider, IDisposable
{
    private ITestOutputHelper? testOutputHelper;
    private bool disposedValue;

    /// <summary>
    ///  テスト標準出力のためのヘルパーオブジェクトを指定して
    ///  <see cref="XunitLoggerProvider"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="testOutputHelper">テスト標準出力のためのヘルパーオブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="testOutputHelper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    internal XunitLoggerProvider(ITestOutputHelper testOutputHelper)
        => this.testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName)
    {
        ObjectDisposedException.ThrowIf(this.testOutputHelper is null, this);
        return new XunitLogger(this.testOutputHelper, categoryName);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///  オブジェクトを破棄します。
    /// </summary>
    /// <param name="disposing">明示的に破棄する場合は <see langword="true"/> 、そうでない場合は <see langword="false"/> 。</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            this.testOutputHelper = null;
            this.disposedValue = true;
        }
    }
}
