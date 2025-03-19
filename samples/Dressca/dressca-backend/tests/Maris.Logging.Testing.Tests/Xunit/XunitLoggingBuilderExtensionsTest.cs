using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Maris.Logging.Testing.Tests.Xunit;

public class XunitLoggingBuilderExtensionsTest
{
    [Fact]
    public void AddXunitLogging_BuilderIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ILoggingBuilder? builder = null;
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var loggerManager = new TestLoggerManager(testOutputHelper);

        // Act
        var action = () => XunitLoggingBuilderExtensions.AddXunitLogging(builder!, loggerManager);

        // Assert
        Assert.Throws<ArgumentNullException>("builder", action);
    }

    [Fact]
    public void AddXunitLogging_LoggerManagerIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = Mock.Of<ILoggingBuilder>();
        TestLoggerManager? loggerManager = null;

        // Act
        var action = () => XunitLoggingBuilderExtensions.AddXunitLogging(builder, loggerManager!);

        // Assert
        Assert.Throws<ArgumentNullException>("loggerManager", action);
    }

    [Fact]
    public void AddXunitLogging_ValidInputs_AddsXunitLoggerProviderAsSingleton()
    {
        // Arrange
        var builderMock = new Mock<ILoggingBuilder>();
        var services = new ServiceCollection();
        builderMock.Setup(builder => builder.Services).Returns(services);
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var loggerManager = new TestLoggerManager(testOutputHelper);

        // Act
        var result = XunitLoggingBuilderExtensions.AddXunitLogging(builderMock.Object, loggerManager);

        // Assert
        var serviceDescriptor = Assert.Single(
            services,
            descriptor => descriptor.ImplementationInstance is XunitLoggerProvider);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }
}
