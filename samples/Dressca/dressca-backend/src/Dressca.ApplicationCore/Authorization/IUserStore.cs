namespace Dressca.ApplicationCore.Authorization;

/// <summary>
///  ユーザーのセッション情報のインターフェースです。
/// </summary>
public interface IUserStore
{
    /// <summary>
    ///  ログイン中のユーザー名を取得します。
    ///  未接続または未ログインの場合は空文字を返します。
    /// </summary>
    /// <returns>ユーザー名。</returns>
    public string LoginUserName { get; }

    /// <summary>
    ///  ログイン中のユーザーのロールを取得します。
    ///  未接続または未ログインの場合は空の配列を返します。
    /// </summary>
    /// <returns>ロール。</returns>
    public string[] LoginUserRoles { get; }

    /// <summary>
    ///  ログイン中のユーザーが指定したロールに属しているかどうか確認します。
    ///  未接続または未ログインの場合は <see langword="false"/> を返します。
    /// </summary>
    /// <param name="role">ロール。</param>
    /// <returns>ロールに属するかどうか。</returns>
    public bool IsInRole(string role);
}
