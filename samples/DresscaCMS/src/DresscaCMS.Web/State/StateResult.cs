namespace DresscaCMS.Web.State;

/// <summary>
///  ステートの取得結果を表します。
/// </summary>
/// <typeparam name="T">ステートの型。</typeparam>
public record StateResult<T>
{
    /// <summary>
    ///  ステートが見つかったかどうかを示す値を返します。
    /// </summary>
    /// <value>
    ///  ステートが見つかった場合は <see langword="true"/> 、見つからなかった場合は <see langword="false"/> 。
    /// </value>
    public bool Found { get; private init; }

    /// <summary>
    ///  ステートの値を返します。
    /// </summary>
    /// <value>
    ///  ステートが見つからなかった場合は <typeparamref name="T"/> の既定値を返します。
    /// </value>
    public T? Value { get; private init; }

    /// <summary>
    ///  ステートが見つからなかったことを示す <see cref="StateResult{T}"/> の新しいインスタンスを返します。
    /// </summary>
    /// <returns>ステートが見つからなかったことを示す <see cref="StateResult{T}"/> のインスタンス。</returns>
    public static StateResult<T> NotFound() => new() { Found = false, Value = default };

    /// <summary>
    ///  ステートが見つかったことを示す <see cref="StateResult{T}"/> の新しいインスタンスを返します。
    /// </summary>
    /// <param name="value">見つかったステートの値。</param>
    /// <returns>ステートが見つかったことを示す <see cref="StateResult{T}"/> のインスタンス。</returns>
    public static StateResult<T> FoundValue(T? value) => new() { Found = true, Value = value };
}
