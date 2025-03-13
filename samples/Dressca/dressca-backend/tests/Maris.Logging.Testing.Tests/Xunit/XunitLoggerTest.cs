using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;

namespace Maris.Logging.Testing.Tests.Xunit;

public class XunitLoggerTest
{
    public static TheoryData<LogLevel> LogLevels
    {
        get
        {
            var data = new TheoryData<LogLevel>();
            foreach (var logLevel in Enum.GetValues<LogLevel>())
            {
                data.Add(logLevel);
            }

            return data;
        }
    }

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
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var logger = new XunitLogger(testOutputHelper, "TestCategory");

        // Act
        var isEnabled = logger.IsEnabled(logLevel);

        // Assert
        Assert.True(isEnabled);
    }

    [Fact]
    public void Log_WritesToOutputHelper()
    {
        // Arrange
        var testOutputHelperMock = new Mock<ITestOutputHelper>();
        var categoryName = "TestCategory";
        var logger = new XunitLogger(testOutputHelperMock.Object, categoryName);
        var logLevel = LogLevel.Information;
        var eventId = new EventId(1);
        var state = "TestState";
        Exception? exception = null;
        var formatter = (string state, Exception? exception) => state.ToString();

        // Act
        logger.Log(logLevel, eventId, state, exception, formatter);

        // Assert
        testOutputHelperMock.Verify(h => h.WriteLine(It.IsAny<string>()), Times.Once);
        testOutputHelperMock.Verify(
            h => h.WriteLine($"{logLevel}: {categoryName}[{eventId}] {formatter(state, exception)}"), Times.Once);
    }

    [Fact]
    public void Log_WritesExceptionToOutputHelper_WhenExceptionIsNotNull()
    {
        // Arrange
        var testOutputHelperMock = new Mock<ITestOutputHelper>();
        var categoryName = "TestCategory";
        var logger = new XunitLogger(testOutputHelperMock.Object, categoryName);
        var logLevel = LogLevel.Information;
        var eventId = new EventId(1);
        var state = "TestState";
        var exception = new Exception("TestException");
        var formatter = (string state, Exception? exception) => state.ToString();

        // Act
        logger.Log(logLevel, eventId, state, exception, formatter);

        // Assert
        testOutputHelperMock.Verify(h => h.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        testOutputHelperMock.Verify(
            h => h.WriteLine($"{logLevel}: {categoryName}[{eventId}] {formatter(state, exception)}"), Times.Once);
        testOutputHelperMock.Verify(
            h => h.WriteLine(exception.ToString()), Times.Once);
    }
}
