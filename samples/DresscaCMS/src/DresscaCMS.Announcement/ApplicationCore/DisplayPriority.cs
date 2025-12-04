namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
///  お知らせメッセージの優先度を示す列挙型です。
/// </summary>
public enum DisplayPriority
{
    /// <summary>緊急です。</summary>
    Critical = 1,

    /// <summary>高です。</summary>
    High = 2,

    /// <summary>中です。</summary>
    Medium = 3,

    /// <summary>低です。</summary>
    Low = 4,
}
