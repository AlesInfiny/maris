using Maris.Logging.Testing.Xunit;
using Xunit.Abstractions;

namespace Maris.Logging.Testing.Tests.Xunit;

public class XunitLoggerProviderTest
{
    [Fact]
    public void Constructor_ThrowsException_WhenTestOutputHelperIsNull()
    {
        // Arrange
        ITestOutputHelper? testOutputHelper = null;

        // Act
        var action = () => new XunitLoggerProvider(testOutputHelper!);

        // Assert
        Assert.Throws<ArgumentNullException>("testOutputHelper", action);
    }

    [Fact]
    public void CreateLogger_ReturnsXunitLogger()
    {
        // Arrange
        var outputHelperMock = Mock.Of<ITestOutputHelper>();
        var provider = new XunitLoggerProvider(outputHelperMock);
        var categoryName = "TestCategory";

        // Act
        var logger = provider.CreateLogger(categoryName);

        // Assert
        Assert.NotNull(logger);
        Assert.IsType<XunitLogger>(logger);
    }

    [Fact]
    public void Dispose_DisposesTestOutputHelper()
    {
        // Arrange
        var outputHelperMock = Mock.Of<ITestOutputHelper>();
        var provider = new XunitLoggerProvider(outputHelperMock);

        // Act
        provider.Dispose();

        // Assert
        outputHelperMock.Verify(h => h.Dispose(), Times.Once);
    }
}
