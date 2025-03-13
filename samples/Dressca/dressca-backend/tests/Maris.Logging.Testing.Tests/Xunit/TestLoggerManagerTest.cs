using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;

namespace Maris.Logging.Testing.Tests.Xunit;

public class TestLoggerManagerTest
{
    [Fact]
    public void Constructor_ThrowsException_WhenTestOutputHelperIsNull()
    {
        // Arrange
        ITestOutputHelper? testOutputHelper = null;

        // Act
        var action = () => new TestLoggerManager(testOutputHelper!);

        // Assert
        Assert.Throws<ArgumentNullException>("testOutputHelper", action);
    }

    [Fact]
    public void CreateLogger_ValidType_LoggerInstance()
    {
        // Arrange
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var manager = new TestLoggerManager(testOutputHelper);

        // Act
        var logger = manager.CreateLogger<TestLoggerManagerTest>();

        // Assert
        Assert.NotNull(logger);
        Assert.IsType<Logger<TestLoggerManagerTest>>(logger);
    }

    [Fact]
    public void CreateLogger_ValidCategory_LogRecord()
    {
        // Arrange
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var manager = new TestLoggerManager(testOutputHelper);
        var categoryName = "TestCategory";
        var message = "Test message";

        // Act (カテゴリー名を検証するため、実際にログ出力して確認)
        var logger = manager.CreateLogger(categoryName);
        logger.LogDebug(message);

        // Assert
        Assert.NotNull(logger);
        Assert.Equal(categoryName, manager.LogCollector.LatestRecord.Category);
        Assert.Equal(message, manager.LogCollector.LatestRecord.Message);
    }

    [Fact]
    public void CreateLogger_ValidCategoryOfFullName_LogRecord()
    {
        // Arrange
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var manager = new TestLoggerManager(testOutputHelper);
        var type = this.GetType();
        var message = "Test message";

        // Act (カテゴリー名を検証するため、実際にログ出力して確認)
        var logger = manager.CreateLogger(type);
        logger.LogDebug(message);

        // Assert
        Assert.NotNull(logger);
        Assert.Equal(type.FullName, manager.LogCollector.LatestRecord.Category);
        Assert.Equal(message, manager.LogCollector.LatestRecord.Message);
    }

    [Fact]
    public void LogCollector_CanGetLoggedRecords_Single()
    {
        // Arrange
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var manager = new TestLoggerManager(testOutputHelper);
        var logger = manager.CreateLogger<TestLoggerManagerTest>();
        var eventId = new EventId(100);
        var exception = new InvalidOperationException();
        var message = "Test message";
        logger.LogDebug(eventId, exception, message);

        // Act
        var collector = manager.LogCollector;

        // Assert
        Assert.NotNull(collector);
        Assert.Single(collector.GetSnapshot());
        Assert.Equal(eventId, collector.LatestRecord.Id);
        Assert.Equal(exception, collector.LatestRecord.Exception);
        Assert.Equal(message, collector.LatestRecord.Message);
    }

    [Fact]
    public void LogCollector_CanGetLoggedRecords_Double()
    {
        // Arrange
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var manager = new TestLoggerManager(testOutputHelper);
        var logger = manager.CreateLogger<TestLoggerManagerTest>();
        var eventId = new EventId(100);
        var exception = new InvalidOperationException();
        var message1 = "Test message 1";
        var message2 = "Test message 2";
        logger.LogError(eventId, exception, message1);
        logger.LogTrace(message2);

        // Act
        var collector = manager.LogCollector;

        // Assert
        Assert.NotNull(collector);
        Assert.Collection(
            collector.GetSnapshot(),
            firstRecord =>
            {
                Assert.Equal(LogLevel.Error, firstRecord.Level);
                Assert.Equal(eventId, firstRecord.Id);
                Assert.Equal(exception, firstRecord.Exception);
                Assert.Equal(message1, firstRecord.Message);
            },
            secondRecord =>
            {
                Assert.Equal(LogLevel.Trace, secondRecord.Level);
                Assert.Equal(default, secondRecord.Id);
                Assert.Null(secondRecord.Exception);
                Assert.Equal(message2, secondRecord.Message);
            });
    }

    [Fact]
    public void LogCollector_CanOutputStructuredLog()
    {
        // Arrange
        var testOutputHelper = Mock.Of<ITestOutputHelper>();
        var manager = new TestLoggerManager(testOutputHelper);
        var logger = manager.CreateLogger<TestLoggerManagerTest>();
        var eventId = new EventId(100);
        var message = "Structured Log Message : {param}";
        var param = "dummy";
        logger.LogDebug(eventId, message, param);

        // Act
        var collector = manager.LogCollector;

        // Assert
        Assert.NotNull(collector);
        Assert.Single(collector.GetSnapshot());
        Assert.Equal(eventId, collector.LatestRecord.Id);
        Assert.Equal($"Structured Log Message : {param}", collector.LatestRecord.Message);
        Assert.Equal(nameof(param), collector.LatestRecord.StructuredState![0].Key);
        Assert.Equal(param, collector.LatestRecord.StructuredState![0].Value);
        Assert.Equal("{OriginalFormat}", collector.LatestRecord.StructuredState![1].Key);
        Assert.Equal(message, collector.LatestRecord.StructuredState![1].Value);
    }
}
