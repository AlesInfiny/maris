using Bunit;
using DresscaCMS.Web.Components.Pages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace DresscaCMS.UnitTests.Web.Components.Pages;

public class ErrorTest : BunitContext
{
    [Fact]
    public void Developmentモードの時スタックトレースが表示できる()
    {
        // Arrange
        this.SetupEnvironment("Development");
        var exception = new InvalidOperationException("Test exception");

        // Act
        var renderedComponent = this.Render<Error>(
            parameters => parameters.Add(p => p.Exception, exception));

        // Assert
        var stackTraceBox = renderedComponent.Find("div.stack-trace-box");
        stackTraceBox.MarkupMatches(
            """
            <div class="stack-trace-box">
              <fluent-card>
                <h3 >スタックトレース</h3>
                <pre >System.InvalidOperationException: Test exception</pre>
              </fluent-card>
            </div>
            """);
    }

    [Fact]
    public void Productionモードの時スタックトレースは表示されない()
    {
        // Arrange
        this.SetupEnvironment("Production");
        var exception = new Exception("Test");

        // Act
        var renderedComponent = this.Render<Error>(
            parameters => parameters.Add(p => p.Exception, exception));

        // Assert
        Assert.Empty(renderedComponent.FindAll("div.stack-trace-box"));
    }

    [Fact]
    public void パラメーターに例外が設定されていなければスタックトレースは表示されない()
    {
        // Arrange
        this.SetupEnvironment("Development");

        // Act
        var renderedComponent = this.Render<Error>();

        // Assert
        Assert.Empty(renderedComponent.FindAll("div.stack-trace-box"));
    }

    private void SetupEnvironment(string environment)
    {
        var envMock = new Moq.Mock<IWebHostEnvironment>();
        envMock.Setup(e => e.EnvironmentName).Returns(environment);
        this.JSInterop.Mode = JSRuntimeMode.Loose;

        this.Services.AddSingleton<IWebHostEnvironment>(envMock.Object);
        this.Services.AddSingleton<LibraryConfiguration>();
    }
}
