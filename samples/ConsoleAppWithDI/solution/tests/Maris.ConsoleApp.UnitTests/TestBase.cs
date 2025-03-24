using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Maris.ConsoleApp.UnitTests;

public class TestBase
{
    protected TestBase(ITestOutputHelper testOutputHelper)
    {
        ArgumentNullException.ThrowIfNull(testOutputHelper);
        this.LoggerManager = new TestLoggerManager(testOutputHelper);
    }

    protected TestLoggerManager LoggerManager { get; }

    protected FakeLogCollector LogCollector => this.LoggerManager.LogCollector;

    protected ILogger<T> CreateTestLogger<T>()
        => this.LoggerManager.CreateLogger<T>();
}
