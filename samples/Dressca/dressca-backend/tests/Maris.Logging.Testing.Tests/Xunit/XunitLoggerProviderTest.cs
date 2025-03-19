using System.Reflection;
using Maris.Logging.Testing.Xunit;

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
    public void CreateLogger_ThrowsException_WhenObjectDisposed()
    {
        // Arrange
        var outputHelper = Mock.Of<ITestOutputHelper>();
        var provider = new XunitLoggerProvider(outputHelper);
        provider.Dispose();

        // Act
        var action = () => provider.CreateLogger("TestCategory");

        // Assert
        var exception = Assert.Throws<ObjectDisposedException>(action);
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
        Assert.True(IsDisposed(provider));
    }

    private static bool? IsDisposed(XunitLoggerProvider provider)
    {
        var providerType = provider.GetType();
        var disposedField = providerType.GetField("disposed", BindingFlags.Instance | BindingFlags.NonPublic);
        var value = disposedField?.GetValue(provider);
        return (bool?)value;
    }
}
