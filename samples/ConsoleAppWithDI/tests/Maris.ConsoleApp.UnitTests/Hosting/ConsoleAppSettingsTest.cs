using Maris.ConsoleApp.Hosting;

namespace Maris.ConsoleApp.UnitTests.Hosting;

public class ConsoleAppSettingsTest
{
    [Fact]
    public void DefaultErrorExitCode_既定値の確認()
    {
        // Arrange
        var settings = new ConsoleAppSettings();

        // Act
        var exitCode = settings.DefaultErrorExitCode;

        // Assert
        Assert.Equal(int.MaxValue, exitCode);
    }

    [Fact]
    public void DefaultValidationErrorExitCode_既定値の確認()
    {
        // Arrange
        var settings = new ConsoleAppSettings();

        // Act
        var exitCode = settings.DefaultValidationErrorExitCode;

        // Assert
        Assert.Equal(int.MinValue, exitCode);
    }
}
