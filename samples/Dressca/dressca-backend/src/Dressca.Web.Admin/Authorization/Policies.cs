namespace Dressca.Web.Admin.Authorization;

/// <summary>
///  ポリシーを管理するための静的クラスです。
/// </summary>
internal static class Policies
{
    /// <summary>
    ///  管理者ロールが必要であるというポリシーを示す文字列です。
    /// </summary>
    internal const string RequireAdminRole = "RequireAdminRole";
}
