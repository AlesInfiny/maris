namespace Dressca.ApplicationCore.Auth;

/// <summary>
///  ユーザーのセッション情報のインターフェース。
/// </summary>
public interface IUserSession
{
    /// <summary>
    /// ログイン中のユーザー名を取得します。
    /// </summary>
    /// <returns>ユーザー名。</returns>
    public string LoginUserName();

    /// <summary>
    /// ログイン中のユーザーのロールを取得します。
    /// </summary>
    /// <returns>ロールのリスト。</returns>
    public IReadOnlyCollection<string> LoginUserRoles();

    /// <summary>
    /// ログイン中のユーザーが指定したロールに属しているかどうか確認します。
    /// </summary>
    /// <param name="role">ロール。</param>
    /// <returns>ロールに属するかどうか。</returns>
    public bool IsInRole(string role);
}
