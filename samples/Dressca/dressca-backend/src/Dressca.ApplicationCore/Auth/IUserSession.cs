namespace Dressca.ApplicationCore.Auth;

/// <summary>
/// ユーザレポジトリのインターフェース。
/// </summary>
public interface IUserSession
{
    /// <summary>
    /// 指定した認証情報を持つユーザーを検索します。
    /// </summary>
    /// <param name="userName">ユーザー名。</param>
    /// <param name="password">パスワード。</param>
    /// <returns>ユーザー。</returns>
    User? FindUser(string userName, string password);
}
