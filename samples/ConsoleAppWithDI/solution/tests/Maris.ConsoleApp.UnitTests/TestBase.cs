using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Xunit.Abstractions;

namespace Maris.ConsoleApp.UnitTests;

public class TestBase
{
    private readonly TestLoggerManager loggerManager;

    protected TestBase(ITestOutputHelper testOutputHelper)
    {
        ArgumentNullException.ThrowIfNull(testOutputHelper);
        this.loggerManager = new TestLoggerManager(testOutputHelper);
    }

    protected FakeLogCollector LogCollector => this.loggerManager.LogCollector;

    protected ILogger<T> CreateTestLogger<T>()
        => this.loggerManager.CreateLogger<T>();
}
