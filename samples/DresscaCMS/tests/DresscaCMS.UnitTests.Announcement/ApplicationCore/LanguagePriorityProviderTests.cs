using DresscaCMS.Announcement.ApplicationCore;

namespace DresscaCMS.UnitTests.Announcement;

/// <summary>
/// <see cref="LanguagePriorityProvider"/> のテストクラスです。
/// </summary>
public class LanguagePriorityProviderTests
{
    [Fact]
    public void GetLanguageOrderMap_デフォルトの優先順位を返す()
    {
        // Act
        var result = LanguagePriorityProvider.GetLanguageOrderMap();

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Equal(0, result["ja"]);
        Assert.Equal(1, result["en"]);
        Assert.Equal(2, result["zh"]);
        Assert.Equal(3, result["es"]);
    }
}
