using System.ComponentModel.DataAnnotations;

namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
///  お知らせメッセージの優先度を示す列挙型です。
/// </summary>
public enum DisplayPriority
{
    /// <summary>緊急です。</summary>
    [Display(Name = "緊急")]
    Critical = 1,

    /// <summary>高です。</summary>
    [Display(Name = "高")]
    High = 2,

    /// <summary>中です。</summary>
    [Display(Name = "中")]
    Medium = 3,

    /// <summary>低です。</summary>
    [Display(Name = "低")]
    Low = 4,
}
