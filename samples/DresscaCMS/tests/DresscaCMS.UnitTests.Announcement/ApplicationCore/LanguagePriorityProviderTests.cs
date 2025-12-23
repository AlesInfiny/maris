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

    [Fact]
    public void IsSupportedLanguage_不正な言語コードが含まれる場合はfalseを返す()
    {
        // Arrange
        var codes = new[] { "ja", "en", "fr" }; // "fr" はサポートされていない言語コード

        // Act
        var result = LanguagePriorityProvider.AreAllSupportedLanguages(codes);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSupportedLanguage_すべての言語コードがサポートされている場合はtrueを返す()
    {
        // Arrange
        var codes = new[] { "ja", "en", "zh" };

        // Act
        var result = LanguagePriorityProvider.AreAllSupportedLanguages(codes);

        // Assert
        Assert.True(result);
    }
}
