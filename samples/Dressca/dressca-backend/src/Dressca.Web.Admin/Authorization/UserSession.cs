namespace Dressca.Web.Admin.Authorization;

/// <summary>
/// ユーザーのセッション情報。
/// </summary>
public class UserSession
{
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public UserSession(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
}


