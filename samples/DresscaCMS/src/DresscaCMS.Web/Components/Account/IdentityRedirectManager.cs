using DresscaCMS.Authentication.Infrastructures.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace DresscaCMS.Web.Components.Account;

/// <summary>
/// Identity関連のページリダイレクトを管理するクラス。
/// </summary>
/// <param name="navigationManager">ナビゲーションマネージャー。</param>
internal sealed class IdentityRedirectManager(NavigationManager navigationManager)
{
    /// <summary>
    /// ステータスメッセージを保存するCookie名。
    /// </summary>
    public const string StatusCookieName = "Identity.StatusMessage";

    /// <summary>
    /// ステータスメッセージCookieのビルダー設定。
    /// </summary>
    private static readonly CookieBuilder StatusCookieBuilder = new()
    {
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromSeconds(5),
    };

    /// <summary>
    /// 現在のページパスを取得します。
    /// </summary>
    private string CurrentPath => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

    /// <summary>
    /// 指定されたURIにリダイレクトします。
    /// </summary>
    /// <param name="uri">リダイレクト先のURI。nullの場合は空文字列として扱われます。</param>
    /// <remarks>
    /// オープンリダイレクトを防ぐため、相対URIでない場合は基準相対パスに変換されます。
    /// </remarks>
    public void RedirectTo(string? uri)
    {
        uri ??= string.Empty;

        // オープンリダイレクトを防ぐために、相対URIでない場合は基準相対パスに変換します。
        if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
        {
            uri = navigationManager.ToBaseRelativePath(uri);
        }

        navigationManager.NavigateTo(uri);
    }

    /// <summary>
    /// クエリパラメータを付与して指定されたURIにリダイレクトします。
    /// </summary>
    /// <param name="uri">リダイレクト先のURI。</param>
    /// <param name="queryParameters">追加するクエリパラメータのディクショナリ。</param>
    public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
    {
        var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
        var newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
        this.RedirectTo(newUri);
    }

    /// <summary>
    /// ステータスメッセージをCookieに保存してから指定されたURIにリダイレクトします。
    /// </summary>
    /// <param name="uri">リダイレクト先のURI。</param>
    /// <param name="message">ステータスメッセージ。</param>
    /// <param name="context">HTTPコンテキスト。</param>
    public void RedirectToWithStatus(string uri, string message, HttpContext context)
    {
        context.Response.Cookies.Append(StatusCookieName, message, StatusCookieBuilder.Build(context));
        this.RedirectTo(uri);
    }

    /// <summary>
    /// 現在のページにリダイレクトします。
    /// </summary>
    public void RedirectToCurrentPage() => this.RedirectTo(this.CurrentPath);

    /// <summary>
    /// ステータスメッセージをCookieに保存してから現在のページにリダイレクトします。
    /// </summary>
    /// <param name="message">ステータスメッセージ。</param>
    /// <param name="context">HTTPコンテキスト。</param>
    public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
        => this.RedirectToWithStatus(this.CurrentPath, message, context);

    /// <summary>
    /// 無効なユーザーエラーページにリダイレクトします。
    /// </summary>
    /// <param name="userManager">ユーザーマネージャー。</param>
    /// <param name="context">HTTPコンテキスト。</param>
    public void RedirectToInvalidUser(UserManager<ApplicationUser> userManager, HttpContext context)
        => this.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
}
