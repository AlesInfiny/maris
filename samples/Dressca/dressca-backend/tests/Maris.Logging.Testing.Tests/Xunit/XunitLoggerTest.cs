using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Maris.Logging.Testing.Tests.Xunit;

public class XunitLoggerTest
{
    public static IEnumerable<object[]> LogLevels
        => Enum.GetValues<LogLevel>().Select(logLevel => new object[] { logLevel });

    [Fact]
    public void Constructor_ThrowsException_WhenTestOutputHelperIsNull()
    {
        // Arrange
        ITestOutputHelper? testOutputHelper = null;
        string category = "TestCategory";

        // Act
        var action = () => new XunitLogger(testOutputHelper!, category);

        // Assert
        Assert.Throws<ArgumentNullException>("testOutputHelper", action);
    }

    [Theory]
    [MemberData(nameof(LogLevels))]
    public void IsEnabled_ReturnsTrue_ForAllLogLevels(LogLevel logLevel)
    {
        // Arrange
        var outputHelperMock = new Mock<ITestOutputHelper>();
        var logger = new XunitLogger(outputHelperMock.Object, "TestCategory");

        // Act
        var isEnabled = logger.IsEnabled(logLevel);

        // Assert
        Assert.True(isEnabled);
    }

    [Fact]
    public void Log_WritesToOutputHelper()
    {
        // Arrange
        var outputHelperMock = new Mock<ITestOutputHelper>();
        var categoryName = "TestCategory";
        var logger = new XunitLogger(outputHelperMock.Object, categoryName);
        var logLevel = LogLevel.Information;
        var eventId = new EventId(1);
        var state = "TestState";
        Exception? exception = null;
        var formatter = (string state, Exception? exception) => state.ToString();

        // Act
        logger.Log(logLevel, eventId, state, exception, formatter);

        // Assert
        outputHelperMock.Verify(h => h.WriteLine(It.IsAny<string>()), Times.Once);
        outputHelperMock.Verify(
            h => h.WriteLine($"{logLevel}: {categoryName}[{eventId}] {formatter(state, exception)}"), Times.Once);
    }

    [Fact]
    public void Log_WritesExceptionToOutputHelper_WhenExceptionIsNotNull()
    {
        // Arrange
        var outputHelperMock = new Mock<ITestOutputHelper>();
        var categoryName = "TestCategory";
        var logger = new XunitLogger(outputHelperMock.Object, categoryName);
        var logLevel = LogLevel.Information;
        var eventId = new EventId(1);
        var state = "TestState";
        var exception = new Exception("TestException");
        var formatter = (string state, Exception? exception) => state.ToString();

        // Act
        logger.Log(logLevel, eventId, state, exception, formatter);

        // Assert
        outputHelperMock.Verify(h => h.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        outputHelperMock.Verify(
            h => h.WriteLine($"{logLevel}: {categoryName}[{eventId}] {formatter(state, exception)}"), Times.Once);
        outputHelperMock.Verify(
            h => h.WriteLine(exception.ToString()), Times.Once);
    }
}
