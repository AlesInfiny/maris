namespace Dressca.ApplicationCore.Auth;

/// <summary>
///  ユーザーのセッション情報のインターフェース。
/// </summary>
public interface IUserSession
{
    /// <summary>
    /// ログイン中のユーザー名を取得します。
    /// 未ログインの場合は空文字を返します。
    /// </summary>
    /// <returns>ユーザー名。</returns>
    public string LoginUserName();

    /// <summary>
    /// ログイン中のユーザーのロールを取得します。
    /// 未ログインの場合は空文字を返します。
    /// </summary>
    /// <returns>ロール。</returns>
    public string LoginUserRole();

    /// <summary>
    /// ログイン中のユーザーが指定したロールに属しているかどうか確認します。
    /// 未ログインの場合はfalseを返します。
    /// </summary>
    /// <param name="role">ロール。</param>
    /// <returns>ロールに属するかどうか。</returns>
    public bool IsInRole(string role);
}
