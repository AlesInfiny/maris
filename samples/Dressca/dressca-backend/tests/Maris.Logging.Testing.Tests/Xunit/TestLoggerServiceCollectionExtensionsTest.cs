using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Maris.Logging.Testing.Tests.Xunit;

public class TestLoggerServiceCollectionExtensionsTest
{
    [Fact]
    public void AddTestLogging_ServicesIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection? services = null;
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var loggerManager = new TestLoggerManager(testOutputHelper);

        // Act
        var action = () => TestLoggerServiceCollectionExtensions.AddTestLogging(services!, loggerManager);

        // Assert
        Assert.Throws<ArgumentNullException>("services", action);
    }

    [Fact]
    public void AddTestLogging_LoggerManagerIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        TestLoggerManager? loggerManager = null;

        // Act
        var action = () => TestLoggerServiceCollectionExtensions.AddTestLogging(services, loggerManager!);

        // Assert
        Assert.Throws<ArgumentNullException>("loggerManager", action);
    }

    [Fact]
    public void AddTestLogging_ValidInputs_AddsLoggerAndTwoLoggerProviders()
    {
        // Arrange
        var services = new ServiceCollection();
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var loggerManager = new TestLoggerManager(testOutputHelper);

        // Act
        var result = TestLoggerServiceCollectionExtensions.AddTestLogging(services, loggerManager);

        // Assert
        Assert.Contains(result, service => service.ServiceType == typeof(ILogger<>));
        Assert.Contains(result, descriptor => descriptor.ImplementationInstance is XunitLoggerProvider);
        Assert.Contains(result, descriptor => descriptor.ImplementationInstance is FakeLoggerProvider);
    }
}
