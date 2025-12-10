using DresscaCMS.Web.State;

namespace DresscaCMS.UnitTests.Web.State;

public class InMemoryStateStoreTest
{
    [Fact]
    public async Task ClearAsync_全ての値が削除されること()
    {
        // Arrange
        var store = CreateStore();
        await store.SetAsync("key1", 1);
        await store.SetAsync("key2", 2);

        // Act
        await store.ClearAsync();
        var result1 = await store.GetAsync<int>("key1");
        var result2 = await store.GetAsync<int>("key2");

        // Assert
        Assert.False(result1.Found);
        Assert.False(result2.Found);
    }

    [Fact]
    public async Task GetAsync_存在しないキーの場合NotFoundとなること()
    {
        // Arrange
        var store = CreateStore();

        // Act
        var result = await store.GetAsync<string>("no-such-key");

        // Assert
        Assert.False(result.Found);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task GetAsync_値が削除されずに取得できること()
    {
        // Arrange
        var store = CreateStore();
        var key = "peek-key";
        var value = 123;
        await store.SetAsync(key, value);

        // Act
        var result1 = await store.GetAsync<int>(key);
        var result2 = await store.GetAsync<int>(key);

        // Assert
        Assert.True(result1.Found);
        Assert.True(result2.Found);
        Assert.Equal(value, result1.Value);
        Assert.Equal(value, result2.Value);
    }

    [Fact]
    public async Task GetAsync_保存した型と異なる型で取得しようとした場合例外が発生すること()
    {
        // Arrange
        var store = CreateStore();
        var key = "type-mismatch-key";
        await store.SetAsync(key, 123);

        // Act
        var action = async () =>
        {
            await store.GetAsync<string>(key);
        };

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidCastException>(action);
        Assert.Equal("キー type-mismatch-key のオブジェクトは System.Int32 です。指定された型 System.String にキャストできません。", exception.Message);
    }

    [Fact]
    public async Task PopAsync_存在しないキーの場合NotFoundとなること()
    {
        // Arrange
        var store = CreateStore();

        // Act
        var result = await store.PopAsync<string>("no-such-key");

        // Assert
        Assert.False(result.Found);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task PopAsync_値が取得でき削除されること()
    {
        // Arrange
        var store = CreateStore();
        var key = "pop-key";
        var value = 456;
        await store.SetAsync(key, value);

        // Act
        var popResult = await store.PopAsync<int>(key);
        var peekResult = await store.GetAsync<int>(key);

        // Assert
        Assert.True(popResult.Found);
        Assert.Equal(value, popResult.Value);
        Assert.False(peekResult.Found);
    }

    [Fact]
    public async Task PopAsync_保存した型と異なる型で取得しようとした場合例外が発生すること()
    {
        // Arrange
        var store = CreateStore();
        var key = "type-mismatch-key-pop";
        await store.SetAsync(key, 123);

        // Act
        var action = async () =>
        {
            await store.PopAsync<string>(key);
        };

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidCastException>(action);
        Assert.Equal("キー type-mismatch-key-pop のオブジェクトは System.Int32 です。指定された型 System.String にキャストできません。", exception.Message);
    }

    [Fact]
    public async Task RemoveAsync_保存したキーを削除できること()
    {
        // Arrange
        var store = CreateStore();
        var key = "remove-key";
        await store.SetAsync(key, "value");

        // Act
        var removed = await store.RemoveAsync(key);
        var result = await store.GetAsync<string>(key);

        // Assert
        Assert.True(removed);
        Assert.False(result.Found);
    }

    [Fact]
    public async Task RemoveAsync_存在しないキーを削除しようとした場合falseが返ること()
    {
        // Arrange
        var store = CreateStore();
        var key = "not-exist-key";

        // Act
        var removed = await store.RemoveAsync(key);

        // Assert
        Assert.False(removed);
    }

    [Fact]
    public async Task SetAsync_値を保存できること()
    {
        // Arrange
        var store = CreateStore();
        var key = "test-key";
        var value = "test-value";

        // Act
        await store.SetAsync(key, value);
        var result = await store.GetAsync<string>(key);

        // Assert
        Assert.True(result.Found);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public async Task SetAsync_同じキーを指定すると値を上書き保存できること()
    {
        // Arrange
        var store = CreateStore();
        var key = "test-key";
        var value1 = "test-value1";
        var value2 = "test-value2";

        // Act
        await store.SetAsync(key, value1);
        var result1 = await store.GetAsync<string>(key);
        await store.SetAsync(key, value2);
        var result2 = await store.GetAsync<string>(key);

        // Assert
        Assert.True(result1.Found);
        Assert.Equal(value1, result1.Value);
        Assert.True(result2.Found);
        Assert.Equal(value2, result2.Value);
    }

    private static InMemoryStateStore CreateStore() => new();
}
