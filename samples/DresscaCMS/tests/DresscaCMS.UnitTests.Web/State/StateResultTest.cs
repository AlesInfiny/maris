using DresscaCMS.Web.State;

namespace DresscaCMS.UnitTests.Web.State;

public class StateResultTest
{
    [Fact]
    public void NotFound_見つからなかったことを示すオブジェクトを取得できる()
    {
        // Act
        var result = StateResult<int>.NotFound();

        // Assert
        Assert.False(result.Found);
    }

    [Fact]
    public void NotFound_Tが値型の場合ValueがTの既定値に設定される()
    {
        // Act
        var result = StateResult<int>.NotFound();

        // Assert
        Assert.Equal(default(int), result.Value);
    }

    [Fact]
    public void NotFound_Tが参照型の場合Valueがnullに設定される()
    {
        // Act
        var result = StateResult<string>.NotFound();

        // Assert
        Assert.Null(result.Value);
    }

    [Fact]
    public void FoundValue_ShouldReturnStateResultWithFoundTrue()
    {
        // Arrange
        var expectedValue = 42;

        // Act
        var result = StateResult<int>.FoundValue(expectedValue);

        // Assert
        Assert.True(result.Found);
        Assert.Equal(expectedValue, result.Value);
    }

    [Fact]
    public void FoundValue_見つかったことを示すオブジェクトを取得できる()
    {
        // Act
        var result = StateResult<string>.FoundValue(null);

        // Assert
        Assert.True(result.Found);
    }

    [Fact]
    public void FoundValue_Valueとしてnullを指定できる()
    {
        // Act
        var result = StateResult<string>.FoundValue(null);

        // Assert
        Assert.Null(result.Value);
    }

    [Fact]
    public void FoundValue_Valueとして値型のオブジェクトを指定できる()
    {
        // Arrange
        var dateTimeOffsetValue = DateTimeOffset.Now;

        // Act
        var result = StateResult<DateTimeOffset>.FoundValue(dateTimeOffsetValue);

        // Assert
        Assert.Equal(dateTimeOffsetValue, result.Value);
    }

    [Fact]
    public void FoundValue_Valueとして参照型のオブジェクトを指定できる()
    {
        // Arrange
        var stringValue = "Test";

        // Act
        var result = StateResult<string>.FoundValue(stringValue);

        // Assert
        Assert.Equal(stringValue, result.Value);
    }

    [Fact]
    public void FoundValue_Valueにdynamic型のオブジェクトを指定できる()
    {
        // Arrange
        var expectedValue = new { Id = 1, Name = "Test" };

        // Act
        var result = StateResult<dynamic>.FoundValue(expectedValue);

        // Assert
        Assert.Equal(expectedValue, result.Value);
    }
}
