using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Maris.Logging.Testing.Tests.Xunit;

public class FakeLoggingBuilderExtensionsTest
{
    [Fact]
    public void AddFakeLogging_BuilderIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ILoggingBuilder? builder = null;
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var loggerManager = new TestLoggerManager(testOutputHelper);

        // Act
        var action = () => FakeLoggingBuilderExtensions.AddFakeLogging(builder!, loggerManager);

        // Assert
        Assert.Throws<ArgumentNullException>("builder", action);
    }

    [Fact]
    public void AddFakeLogging_LoggerManagerIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = Mock.Of<ILoggingBuilder>();
        TestLoggerManager? loggerManager = null;

        // Act
        var action = () => FakeLoggingBuilderExtensions.AddFakeLogging(builder, loggerManager!);

        // Assert
        Assert.Throws<ArgumentNullException>("loggerManager", action);
    }

    [Fact]
    public void AddFakeLogging_ValidInputs_AddsFakeLoggerProviderAsSingleton()
    {
        // Arrange
        var builderMock = new Mock<ILoggingBuilder>();
        var services = new ServiceCollection();
        builderMock.Setup(builder => builder.Services).Returns(services);
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var loggerManager = new TestLoggerManager(testOutputHelper);

        // Act
        var result = FakeLoggingBuilderExtensions.AddFakeLogging(builderMock.Object, loggerManager);

        // Assert
        var serviceDescriptor = Assert.Single(
            services,
            descriptor => descriptor.ImplementationInstance is FakeLoggerProvider);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }
}
