using System.Security.Claims;
using Dressca.ApplicationCore.Authorization;

namespace Dressca.Web.Admin.Authorization;

/// <summary>
/// ユーザーのセッション情報を表すクラスです。
/// </summary>
public class UserStore : IUserStore
{
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    ///   <see cref="UserStore"/>のインスタンスを初期化します。
    /// </summary>
    /// <param name="httpContextAccessor"><see cref="HttpContext"/>の情報にアクセスするためのクラス。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="httpContextAccessor"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public UserStore(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc/>
    public string LoginUserName
    {
        get
        {
            if (this.IsAuthenticated())
            {
                return this.httpContextAccessor.HttpContext?.User.Identity?.Name ?? string.Empty;
            }

            return string.Empty;
        }
    }

    /// <inheritdoc/>
    public string[] LoginUserRoles
    {
        get
        {
            if (this.IsAuthenticated())
            {
                return this.httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray() ?? [];
            }

            return [];
        }
    }

    /// <inheritdoc/>
    public bool IsInRole(string role)
    {
        if (this.IsAuthenticated())
        {
            return this.httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
        }

        return false;
    }

    /// <summary>
    ///   ユーザーがログイン済みかどうかを表す真理値を取得します。
    /// </summary>
    /// <returns>ユーザーがログイン済みかどうかを表す真理値。
    /// ログイン済みならば <see langword="true"/> 、そうでなければ <see langword="false"/> 。
    /// </returns>
    private bool IsAuthenticated()
    {
        return this.httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
