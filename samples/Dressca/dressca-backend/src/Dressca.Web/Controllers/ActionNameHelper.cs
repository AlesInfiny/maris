namespace Dressca.Web.Controllers;

/// <summary>
///  アクション名を解決するヘルパーメソッドです。
/// </summary>
public static class ActionNameHelper
{
    /// <summary>
    ///  非同期メソッドのアクション名を解決します。
    ///  "Async" で終わるメソッド名から "Async" を削除した文字列を返却します。
    /// </summary>
    /// <param name="asyncActionMethodName">"Async" を接尾辞に持つ非同期メソッドの名前。</param>
    /// <returns>アクション名。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="asyncActionMethodName"/> が <see langword="null"/> です。
    /// </exception>
    public static string GetAsyncActionName(string asyncActionMethodName)
    {
        ArgumentNullException.ThrowIfNull(asyncActionMethodName);
        const string asyncSuffix = "Async";
        if (asyncActionMethodName.EndsWith(asyncSuffix))
        {
            return asyncActionMethodName[..^asyncSuffix.Length];
        }
        else
        {
            return asyncActionMethodName;
        }
    }
}
