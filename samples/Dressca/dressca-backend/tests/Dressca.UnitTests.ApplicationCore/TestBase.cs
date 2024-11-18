﻿using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore;

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

    protected ILogger CreateTestLogger(Type type)
        => this.loggerManager.CreateLogger(type);

    protected ILogger CreateTestLogger(string categoryName)
        => this.loggerManager.CreateLogger(categoryName);
}
