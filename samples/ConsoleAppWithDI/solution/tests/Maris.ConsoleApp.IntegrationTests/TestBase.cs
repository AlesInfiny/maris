using Maris.Logging.Testing.Xunit;

namespace Maris.ConsoleApp.IntegrationTests;

public class TestBase
{
    protected TestBase(ITestOutputHelper testOutputHelper)
    {
        ArgumentNullException.ThrowIfNull(testOutputHelper);
        this.LoggerManager = new TestLoggerManager(testOutputHelper);
    }

    protected TestLoggerManager LoggerManager { get; }
}
