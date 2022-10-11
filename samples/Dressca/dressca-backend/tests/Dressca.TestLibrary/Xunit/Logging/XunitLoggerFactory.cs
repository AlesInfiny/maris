using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Dressca.TestLibrary.Xunit.Logging;

/// <summary>
///  xUnit で使用できる <see cref="ILogger{TCategoryName}"/> のファクトリークラスです。
///  テスト対象クラスが <see cref="ILogger{TCategoryName}"/> のオブジェクトを必要とするケースで使用してください。
/// </summary>
/// <example>
///  <para>
///   テスト対象クラスが <see cref="ILogger{TCategoryName}"/> のオブジェクトを必要とする場合、
///   xUnit を用いたテストクラスで、以下のように使用します。
///   <code language="c#">
///    <![CDATA[
///     public class TestClass
///     {
///        private readonly XunitLoggerFactory loggerFactory;
///
///        public TestClass(ITestOutputHelper testOutputHelper)
///            => this.loggerFactory = XunitLoggerFactory.Create(testOutputHelper);
///
///        [Fact]
///        public void TestMethod()
///        {
///            // ILogger<T> のオブジェクトを生成可能。
///            var logger = this.loggerFactory.CreateLogger<TestTarget>();
///            var target = new TestTarget(logger);
///
///            // Do something...
///        }
///     }
///    ]]>
///   </code>
///  </para>
/// </example>
public class XunitLoggerFactory : ILoggerFactory, IDisposable
{
    private LoggerFactory? loggerFactory;
    private bool disposedValue;

    /// <summary>
    ///  <see cref="XunitLoggerFactory"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    private XunitLoggerFactory() => this.loggerFactory = new();

    /// <summary>
    ///  テスト標準出力のためのヘルパーオブジェクトを指定して
    ///  <see cref="XunitLoggerFactory"/> クラスのインスタンスを生成します。
    /// </summary>
    /// <param name="testOutputHelper">テスト標準出力のためのヘルパーオブジェクト。</param>
    /// <returns><see cref="XunitLoggerFactory"/> のオブジェクト。</returns>
    public static XunitLoggerFactory Create(ITestOutputHelper testOutputHelper)
    {
        var instance = new XunitLoggerFactory();
        instance.AddProvider(new XunitLoggerProvider(testOutputHelper));
        return instance;
    }

    /// <inheritdoc/>
    public void AddProvider(ILoggerProvider provider)
    {
        this.ThrowIfDisposed();
        this.loggerFactory.AddProvider(provider);
    }

    /// <summary>
    ///  <typeparamref name="T"/> クラスで使用する
    ///  <see cref="ILogger{TCategoryName}"/> のオブジェクトを生成します。
    /// </summary>
    /// <typeparam name="T">
    ///  ログのカテゴリ名を表す型。
    ///  通常はこの ILogger を使用するクラスの型。
    /// </typeparam>
    /// <returns>ロガー。</returns>
    /// <exception cref="ObjectDisposedException">
    ///  <list type="bullet">
    ///   <item>すでにオブジェクトが破棄されています。</item>
    ///  </list>
    /// </exception>
    public ILogger<T> CreateLogger<T>()
    {
        this.ThrowIfDisposed();
        return this.loggerFactory.CreateLogger<T>();
    }

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName)
    {
        this.ThrowIfDisposed();
        return this.loggerFactory.CreateLogger(categoryName);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///  このオブジェクトを破棄します。
    /// </summary>
    /// <param name="disposing">明示的に破棄する場合は <see langword="true"/> 、そうでない場合は <see langword="false"/> 。</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.loggerFactory?.Dispose();
            }

            this.loggerFactory = null!;
            this.disposedValue = true;
        }
    }

    [MemberNotNull(nameof(loggerFactory))]
    private void ThrowIfDisposed()
    {
        if (this.loggerFactory is null)
        {
            throw new ObjectDisposedException(nameof(XunitLoggerProvider));
        }
    }
}
