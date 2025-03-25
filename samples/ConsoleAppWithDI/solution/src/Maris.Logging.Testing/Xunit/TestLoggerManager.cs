using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Xunit;

namespace Maris.Logging.Testing.Xunit;

/// <summary>
///  Xunit で使用できる <see cref="ILogger"/> を管理するクラスです。
///  テスト対象クラスが <see cref="ILogger"/> または
///  <see cref="ILogger{TCategoryName}"/> のオブジェクトを必要とするケースで使用してください。
/// </summary>
/// <example>
///  <para>
///   テスト対象クラスが <see cref="ILogger{TCategoryName}"/> のオブジェクトを必要とする場合、
///   xUnit を用いたテストクラスで、以下のように使用します。
///   <code language="c#">
///    <![CDATA[
///     public class TestClass
///     {
///        private readonly TestLoggerManager loggerManager;
///
///        public TestClass(ITestOutputHelper testOutputHelper)
///            => this.loggerManager = new TestLoggerManager(testOutputHelper);
///
///        [Fact]
///        public void TestMethod()
///        {
///            // ILogger<T> のオブジェクトを生成可能。
///            var logger = this.loggerManager.CreateLogger<TestTarget>();
///            var target = new TestTarget(logger);
///
///            // Do something...
///        }
///     }
///    ]]>
///   </code>
///  </para>
/// </example>
public class TestLoggerManager
{
    private readonly ITestOutputHelper testOutputHelper;
    private readonly LoggerFactory loggerFactory;

    /// <summary>
    ///  <see cref="TestLoggerManager"/> の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="testOutputHelper">テスト標準出力のためのヘルパーオブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="testOutputHelper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public TestLoggerManager(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(this.testOutputHelper));
        this.XunitLoggerProvider = new XunitLoggerProvider(this.testOutputHelper);
        this.FakeLoggerProvider = new FakeLoggerProvider();
        this.loggerFactory = new LoggerFactory([this.XunitLoggerProvider, this.FakeLoggerProvider]);
    }

    /// <summary>
    ///  <see cref="TestLoggerManager"/> を利用して生成した
    ///  <see cref="ILogger"/> から出力したログのレコードを取得します。
    /// </summary>
    public FakeLogCollector LogCollector => this.FakeLoggerProvider.Collector;

    /// <summary>
    ///  <see cref="ILogger"/> の生成に使用する
    ///  Xunit 向けの <see cref="ILoggerProvider"/> を取得します。
    /// </summary>
    internal XunitLoggerProvider XunitLoggerProvider { get; }

    /// <summary>
    ///  <see cref="ILogger"/> の生成に使用する
    ///  <see cref="ILoggerProvider"/> を取得します。
    /// </summary>
    internal FakeLoggerProvider FakeLoggerProvider { get; }

    /// <summary>
    ///  指定した型の名称を使って新しい <see cref="ILogger{T}"/> インスタンスを作成します。
    /// </summary>
    /// <typeparam name="T">型。</typeparam>
    /// <returns>生成した <see cref="ILogger{T}"/> 。</returns>
    public ILogger<T> CreateLogger<T>() => this.loggerFactory.CreateLogger<T>();

    /// <summary>
    ///  指定したカテゴリ名の <see cref="ILogger"/> インスタンスを作成します。
    /// </summary>
    /// <param name="categoryName">ロガーのカテゴリ名。</param>
    /// <returns>生成した <see cref="ILogger"/> 。</returns>
    public ILogger CreateLogger(string categoryName) => this.loggerFactory.CreateLogger(categoryName);

    /// <summary>
    ///  指定した型の名称を使って新しい <see cref="ILogger"/> インスタンスを作成します。
    /// </summary>
    /// <param name="type">型。</param>
    /// <returns>生成した <see cref="ILogger"/> 。</returns>
    public ILogger CreateLogger(Type type) => this.loggerFactory.CreateLogger(type);
}
