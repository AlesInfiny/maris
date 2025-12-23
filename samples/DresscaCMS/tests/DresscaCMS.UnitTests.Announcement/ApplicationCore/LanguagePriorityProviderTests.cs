using DresscaCMS.Announcement.ApplicationCore;

namespace DresscaCMS.UnitTests.Announcement;

/// <summary>
/// <see cref="LanguagePriorityProvider"/> のテストクラスです。
/// </summary>
public class LanguagePriorityProviderTests
{
    [Theory]
    [InlineData("ja", 0)]
    [InlineData("en", 1)]
    [InlineData("zh", 2)]
    [InlineData("es", 3)]
    public void GetLanguageOrder_指定した言語コードに応じた優先順位を返す(string code, int expectedOrder)
    {
        // Act
        var result = LanguagePriorityProvider.GetLanguageOrder(code);

        // Assert
        Assert.Equal(expectedOrder, result);
    }

    [Fact]
    public void GetLanguageOrder_存在しない言語コードの場合はMaxValueを返す()
    {
        // Arrange
        var code = "fr"; // フランス語（存在しない言語コード）

        // Act
        var result = LanguagePriorityProvider.GetLanguageOrder(code);

        // Assert
        Assert.Equal(int.MaxValue, result);
    }
}
