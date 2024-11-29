namespace Dressca.ApplicationCore.Authorization;

/// <summary>
/// 未接続のユーザーのセッション情報です。
/// </summary>
public class NotConnectedUserStore : IUserStore
{
    /// <inheritdoc/>
    public string LoginUserName()
    {
        return string.Empty;
    }

    /// <inheritdoc/>
    public string LoginUserRole()
    {
        return string.Empty;
    }

    /// <inheritdoc/>
    public bool IsInRole(string role)
    {
        return false;
    }
}
