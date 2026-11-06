namespace Dressca.Authorization;

/// <summary>
///  未接続のユーザーのセッション情報です。
/// </summary>
public class NotConnectedUserStore : IUserStore
{
    /// <inheritdoc/>
    public string LoginUserName
    {
        get => string.Empty;
    }

    /// <inheritdoc/>
    public string[] LoginUserRoles
    {
        get => [];
    }

    /// <inheritdoc/>
    public bool IsInRole(string role)
    {
        return false;
    }
}
