namespace DresscaCMS.Announcement.ApplicationCore;

    /// <summary>
    /// 操作種別を表す列挙型です。
    /// </summary>
    public enum OperationType
{
    /// <summary>未定義です。</summary>
    None = 0,

    /// <summary>作成です。</summary>
    Create = 1,

    /// <summary>更新です。</summary>
    Update = 2,

    /// <summary>削除です。</summary>
    Delete = 3,
}
