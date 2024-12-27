namespace Dressca.UnitTests.SystemCommon;

public class StringExtentionsTest
{
    [Theory]
    [InlineData("\r\nLine1Line2", "Line1Line2")] // CRとLFを含む、先頭
    [InlineData("Line1\rLine2", "Line1Line2")] // CRのみを含む、中間
    [InlineData("Line1Line2\n", "Line1Line2")] // LFのみを含む、末尾
    public void RemoveNewLines_改行文字があれば取り除かれる(string input, string expected)
    {
        // Arrange

        // Act
        var actual = input.RemoveNewLines();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RemoveNewLines_改行文字なし_変化なし()
    {
        // Arrange
        var target = "Line1Line2";

        // Act
        var actual = target.RemoveNewLines();

        // Assert
        Assert.Equal(target, actual);
    }

    [Fact]
    public void RemoveNewLines_null_nullを返却()
    {
        // Arrange
        string? target = null;

        // Act
        var actual = target.RemoveNewLines();

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void RemoveNewLines_空文字_変化なし()
    {
        // Arrange
        var target = string.Empty;

        // Act
        var actual = target.RemoveNewLines();

        // Assert
        Assert.Equal(target, actual);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("　")]
    public void RemoveNewLines_空白文字_変化なし(string target)
    {
        // Arrange

        // Act
        var actual = target.RemoveNewLines();

        // Assert
        Assert.Equal(target, actual);
    }

    [Theory]
    [InlineData("\r\n")]
    [InlineData("\r")]
    [InlineData("\n")]
    public void RemoveNewLines_改行コードのみ_取り除かれて空文字になる(string target)
    {
        // Arrange
        var expected = string.Empty;

        // Act
        var actual = target.RemoveNewLines();

        // Assert
        Assert.Equal(expected, actual);
    }
}
