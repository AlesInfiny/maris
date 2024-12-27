namespace Dressca.UnitTests.SystemCommon;

public class StringExtentionsTest
{
    [Fact]
    public void RemoveNewLines_null_ArgumentNullExceptionが発生する()
    {
        // Arrange
        string? target = null;

        // Act
        var action = () => StringExtentions.RemoveNewLines(target);

        // Assert
        Assert.Throws<ArgumentNullException>("target", action);
    }

    [Theory]
    [InlineData("Line1\r\nLine2", "Line1Line2")] // CRとLFを含む場合
    [InlineData("Line1\rLine2", "Line1Line2")] // CRのみを含む場合
    [InlineData("Line1\nLine2", "Line1Line2")] // LFのみを含む場合
    [InlineData("", "")] // 空文字の場合
    [InlineData("Line1Line2", "Line1Line2")] // 改行文字を含まない場合
    public void RemoveNewLines_改行文字があれば取り除かれる(string input, string expected)
    {
        // Arrange

        // Act
        var actual = input.RemoveNewLines();

        // Assert
        Assert.Equal(expected, actual);
    }
}
