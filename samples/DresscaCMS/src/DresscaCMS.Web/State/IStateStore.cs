namespace DresscaCMS.Web.State;

/// <summary>
///  ステートを管理するためのインターフェースを表します。
/// </summary>
public interface IStateStore
{
    /// <summary>
    ///  このオブジェクトの保持しているすべてのステートを削除します。
    /// </summary>
    /// <returns>タスク。</returns>
    Task ClearAsync();

    /// <summary>
    ///  指定したキーのステートを削除せずに取得します。
    /// </summary>
    /// <typeparam name="T">ステートの型。</typeparam>
    /// <param name="key">ステートのキー。</param>
    /// <returns>ステートの取得結果。</returns>
    /// <exception cref="InvalidCastException">
    ///  <list type="bullet">
    ///   <item>保存されているステートを、 <typeparamref name="T"/> にキャストできません。</item>
    ///  </list>
    /// </exception>
    Task<StateResult<T>> GetAsync<T>(string key);

    /// <summary>
    ///  指定したキーのステートを取得し、削除します。
    /// </summary>
    /// <typeparam name="T">ステートの型。</typeparam>
    /// <param name="key">ステートのキー。</param>
    /// <returns>ステートの取得結果。</returns>
    /// <exception cref="InvalidCastException">
    ///  <list type="bullet">
    ///   <item>保存されているステートを、 <typeparamref name="T"/> にキャストできません。</item>
    ///  </list>
    /// </exception>
    Task<StateResult<T>> PopAsync<T>(string key);

    /// <summary>
    ///  指定したキーのステートを削除します。
    /// </summary>
    /// <param name="key">ステートのキー。</param>
    /// <returns>削除できた場合は <see langword="true"/> 、削除できない場合は <see langword="false"/> 。</returns>
    Task<bool> RemoveAsync(string key);

    /// <summary>
    ///  指定したキーでステートを保存します。
    /// </summary>
    /// <typeparam name="T">ステートの型。</typeparam>
    /// <param name="key">ステートのキー。</param>
    /// <param name="value">ステートの値。</param>
    /// <returns>タスク。</returns>
    Task SetAsync<T>(string key, T value);
}
