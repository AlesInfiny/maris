using System.Collections.Concurrent;
using DresscaCMS.Web.Resources;

namespace DresscaCMS.Web.State;

/// <summary>
///  ステートをインメモリで管理するためのインターフェースを表します。
/// </summary>
internal class InMemoryStateStore : IStateStore
{
    private readonly ConcurrentDictionary<string, object> store = new();

    /// <summary>
    ///  <see cref="InMemoryStateStore"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public InMemoryStateStore()
    {
    }

    /// <inheritdoc/>
    public Task ClearAsync()
    {
        this.store.Clear();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<StateResult<T>> GetAsync<T>(string key)
        => this.store.TryGetValue(key, out var value) ?
            Task.FromResult(StateResult<T>.FoundValue(ConvertTo<T>(key, value))) :
            Task.FromResult(StateResult<T>.NotFound());

    /// <inheritdoc/>
    public Task<StateResult<T>> PopAsync<T>(string key)
        => this.store.TryRemove(key, out var value) ?
            Task.FromResult(StateResult<T>.FoundValue(ConvertTo<T>(key, value))) :
            Task.FromResult(StateResult<T>.NotFound());

    /// <inheritdoc/>
    public Task<bool> RemoveAsync(string key)
    {
        var result = this.store.Remove(key, out _);
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task SetAsync<T>(string key, T value)
    {
        this.store[key] = value!;
        return Task.CompletedTask;
    }

    /// <summary>
    ///  指定したオブジェクトを指定した型に変換します。
    /// </summary>
    /// <typeparam name="T">変換後の型。</typeparam>
    /// <param name="key">キー名。</param>
    /// <param name="value">変換する値。</param>
    /// <returns>変換後の値。</returns>
    /// <exception cref="InvalidCastException">
    ///  <list type="bullet">
    ///   <paramref name="value"/> を <typeparamref name="T"/> に変換できません。
    ///  </list>
    /// </exception>
    private static T ConvertTo<T>(string key, object value)
    {
        try
        {
            return (T)value;
        }
        catch (InvalidCastException ex)
        {
            throw new InvalidCastException(
                string.Format(Messages.StoredObjectCannotCastTo, key, value.GetType(), typeof(T)),
                ex);
        }
    }
}
