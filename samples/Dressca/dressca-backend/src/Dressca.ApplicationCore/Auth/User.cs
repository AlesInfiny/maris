namespace Dressca.ApplicationCore.Auth;

/// <summary>
/// ユーザーエンティティ。
/// </summary>
public class User
{
    /// <summary>
    /// コンストラクター。
    /// </summary>
    public User()
    {
    }

    /// <summary>
    /// ID。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ユーザー名。
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// パスワード。
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// ロール。
    /// </summary>
    public string Role { get; set; }
}
