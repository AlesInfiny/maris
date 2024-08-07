namespace Dressca.ApplicationCore.Auth;

/// <summary>
/// 認可ドメインサービス。
/// </summary>
public interface IAuthorizationDomainService
{
    /// <summary>
    /// ログイン中のユーザー名を取得します。
    /// </summary>
    /// <returns>ユーザー名。</returns>
    public string GetLoginUserName();

    /// <summary>
    /// ログイン中のユーザーのロールの一覧を取得します。
    /// </summary>
    /// <returns>ログイン中のユーザーのロール。</returns>
    public IReadOnlyCollection<string> GetLoginUserRoles();

    /// <summary>
    /// ユーザーが特定のロールに属しているか確認します。
    /// </summary>
    /// <param name="role">ロール。</param>
    /// <returns>ロールに属しているかどうか。</returns>
    public bool IsInRole(string role);
}
